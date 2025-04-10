﻿
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Presentation.DTOs.InformeDTOs;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using AvaluoAPI.Utilities;
using AvaluoAPI.Utilities.JWT;
using MapsterMapper;
using StackExchange.Redis;


namespace AvaluoAPI.Domain.Services.RubricasService
{
    public class RubricaService : IRubricaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHandler _fileHandler;
        private readonly PdfHelper _pdfHelper;
        private readonly IintecService _intecService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IResumenRedisService _redisService;

        public RubricaService(IUnitOfWork unitOfWork, IintecService intecService, FileHandler fileHandler, IJwtService jwtService, IMapper mapper, IResumenRedisService redisService, PdfHelper pdfHelper)
        {
            _jwtService = jwtService;
            _fileHandler = fileHandler;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _intecService = intecService;
            _redisService = redisService;
            _pdfHelper = pdfHelper;


        }

        public async Task CompleteRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras)
        {
            var rubrica = await _unitOfWork.Rubricas.FindAsync(r => r.Id == rubricaDTO.Id);
            var estadoRubricaCompletada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");

            rubrica.IdEstado = estadoRubricaCompletada.Id;
            rubrica.IdMetodoEvaluacion = rubricaDTO.MetodoEvaluacion;
            rubrica.Comentario = rubricaDTO.Comentario;
            rubrica.Problematica = rubricaDTO.Problematica;
            rubrica.Solucion = rubricaDTO.Solucion;
            rubrica.EvaluacionesFormativas = rubricaDTO.EvaluacionesFormativas;
            rubrica.Estrategias = rubricaDTO.Estrategias;
            rubrica.Evidencia = rubricaDTO.Evidencia;
            rubrica.FechaCompletado = DateTime.Now;

           
             

            // Continuamos con el proceso normal de base de datos
            var resumenes = await PrepareResumenesForInsert(rubricaDTO.Resumenes, rubrica.Id);
            var evidencias = await PrepareEvidenciasForInsert(evidenciasExtras, rubrica.Año, rubrica.Periodo, rubrica.Id);
            await Task.WhenAll(
                _unitOfWork.Rubricas.Update(rubrica),
                _unitOfWork.Evidencias.AddRangeAsync(evidencias),
                _unitOfWork.Resumenes.AddRangeAsync(resumenes),
                _redisService.SaveResumenListAsync($"rubrica:{rubrica.Id}:resumenes", rubricaDTO.Resumenes)
            );
            _unitOfWork.SaveChanges();
        }

        public async Task<PaginatedResult<RubricaViewModel>> GetAllRubricas(int? idSO = null, List<int>? carreras = null, List<int>? estado = null, int? idAsignatura = null, int? page = null, int? recordsPerPage = null)
        {
            return await _unitOfWork.Rubricas.GetRubricasFiltered(idSO, carreras, estado, idAsignatura, page, recordsPerPage);
        }

        public async Task DesactivateRubricas()
        {
            
            var entregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Entregada");
            var noEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "No entregada");
            var activo = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activoEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            var asignaturas = await _unitOfWork.Rubricas.ObtenerIdAsignaturasPorEstadoAsync(activoEntregado.Id);
            var rubricasActivasEntregadas = await _unitOfWork.Rubricas.FindAllAsync(r => r.IdEstado == activoEntregado.Id);
            var rubricasActivasNoEntregadas = await _unitOfWork.Rubricas.GetAllIncluding<Rubrica>(r => r.IdEstado == activo.Id, r => r.Asignatura, r => r.Profesor);
            (int trimestre, int año) = PeriodoExtensions.ObtenerTrimestreActual();
               
            foreach (var rubrica in rubricasActivasEntregadas) rubrica.IdEstado = entregado.Id;
            foreach (var rubrica in rubricasActivasNoEntregadas) rubrica.IdEstado = noEntregado.Id;



            await Task.WhenAll(
                _unitOfWork.Rubricas.UpdateRangeAsync(rubricasActivasNoEntregadas),
                _unitOfWork.Rubricas.UpdateRangeAsync(rubricasActivasEntregadas),
                _unitOfWork.Desempeños.InsertDesempeños(asignaturas, año, trimestre.ToString(), activoEntregado.Id),
                _unitOfWork.HistorialIncumplimientos.InsertIncumplimientos(rubricasActivasNoEntregadas)
            );
                

               
            _unitOfWork.SaveChanges();

              
            foreach (int idAsignatura in asignaturas)
            {
                    
                var SO = await _unitOfWork.Desempeños.ObtenerIdSOPorAsignaturasAsync(año, trimestre.ToString(), idAsignatura);
                   
                foreach (int idSO in SO)
                {
                        
                    await GenerarInforme(año, trimestre.ToString(), idAsignatura, idSO);
                       
                }
            }

                
            
            
        }
        private async Task GenerarInforme(int? año, string? periodo, int? idAsignatura, int? idSO)
        {
            try
            {
                if (!año.HasValue || string.IsNullOrWhiteSpace(periodo) || !idAsignatura.HasValue)
                    throw new ArgumentException("Parámetros requeridos no proporcionados.");

                var informes = await _unitOfWork.Desempeños.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);
                if (informes == null || !informes.Any())
                    throw new Exception("No hay datos disponibles para el informe.");

                string añoStr = año?.ToString() ?? "Desconocido";
                var rutaBuilder = new RutaInformeBuilder("Desempeño", añoStr);
                string fileName = $"Informe_Desempeño_{añoStr}_{periodo}_{idAsignatura}_{idSO ?? 0}";

                string pdfPath = await _pdfHelper.GenerarYGuardarPdfAsync(
                    "Informes/InformeDesempeño",
                    informes,
                    rutaBuilder,
                    fileName
                ).ConfigureAwait(false);
                pdfPath = pdfPath.Replace("\\", "/");

                var tipoDesempeño = await _unitOfWork.TiposInformes.GetTipoInformeByDescripcionAsync("desempeño")
                    ?? throw new Exception("Tipo de informe no encontrado.");

                foreach (var informe in informes)
                {
                    var idCarreras = await _unitOfWork.AsignaturasCarreras.GetCarrerasIdsByAsignaturaId(informe.IdAsignatura);
                    if (!idCarreras.Any())
                        throw new Exception($"No se encontraron carreras para la asignatura {informe.IdAsignatura}");

                    foreach (var idCarrera in idCarreras)
                    {
                        var informeDTO = new InformeDTO
                        {
                            Nombre = Path.GetFileName(pdfPath),
                            Ruta = pdfPath,
                            IdTipo = tipoDesempeño.Id,
                            IdCarrera = idCarrera,
                            Año = informe.Año,
                            Trimestre = Convert.ToChar(informe.Trimestre),
                            Periodo = "trimestral"
                        };

                        var informeEntity = _mapper.Map<Informe>(informeDTO);
                        informeEntity.FechaCreacion = DateTime.UtcNow;
                        await _unitOfWork.Informes.AddAsync(informeEntity);
                    }
                }

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("La rubrica cerro correctamente pero no se pudo crear el informe", ex);
            }
        }

        public async Task EditRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras)
        {
            var rubrica = await _unitOfWork.Rubricas.FindAsync(r => r.Id == rubricaDTO.Id);

            var estadoRubricaCompletada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            if (rubrica.IdEstado != estadoRubricaCompletada.Id)
            {
                throw new Exception("No se puede editar una rubrica sin completar");
            }

            rubrica.UltimaEdicion = DateTime.Now;
            rubrica.IdMetodoEvaluacion = rubricaDTO.MetodoEvaluacion;
            rubrica.Comentario = rubricaDTO.Comentario;
            rubrica.Problematica = rubricaDTO.Problematica;
            rubrica.Solucion = rubricaDTO.Solucion;
            rubrica.EvaluacionesFormativas = rubricaDTO.EvaluacionesFormativas;
            rubrica.Estrategias = rubricaDTO.Estrategias;
            rubrica.Evidencia = rubricaDTO.Evidencia;

            
            var evidenciasBeforeUpdate = await _unitOfWork.Evidencias.FindAllAsync(r => r.IdRubrica == rubrica.Id);
            var resumenes = await PrepareResumenesForUpdate(rubricaDTO.Resumenes, rubrica.Id);
            var evidencias = await PrepareEvidenciasForInsert(evidenciasExtras, rubrica.Año, rubrica.Periodo, rubrica.Id);

            await Task.WhenAll(
                _unitOfWork.Rubricas.Update(rubrica),
                _redisService.SaveResumenListAsync($"rubrica:{rubrica.Id}:resumenes", rubricaDTO.Resumenes),
                _unitOfWork.Evidencias.AddRangeAsync(evidencias),
                _unitOfWork.Resumenes.UpdateRangeAsync(resumenes)
            );

            _unitOfWork.SaveChanges();
        }

        public async Task InsertRubricas()
        {
            // 1. Obtener datos iniciales (igual que antes)
            var profesoresTask = _intecService.GetProfesores();
            var asignaturasConCompetenciasTask = _unitOfWork.MapaCompetencias.GetAsignaturasConCompetencias();
            var estadoTask = _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            (int trimestre, int año) = PeriodoExtensions.ObtenerTrimestreActual();

            var profesores = await profesoresTask;
            var asignaturasConCompetencias = await asignaturasConCompetenciasTask;
            var estado = await estadoTask;

            // 2. Crear las rúbricas pero sin insertarlas aún
            var rubricasParaInsertar = new List<(Rubrica Rubrica, int IdAsignatura)>();

            foreach (var profesor in profesores)
            {
                var Profesor = await _unitOfWork.Usuarios.FindAsync(p => p.Email == profesor.Email);
                if (Profesor == null) continue;

                foreach (var seccion in profesor.Secciones!)
                {
                    var asignaturaConCompetencias = asignaturasConCompetencias
                        .FirstOrDefault(a => $"{a.Codigo} - {a.Nombre}" == seccion.Asignatura);

                    if (asignaturaConCompetencias != null && asignaturaConCompetencias.Competencias != null)
                    {
                        foreach (var competencia in asignaturaConCompetencias.Competencias)
                        {
                            var rubrica = new Rubrica
                            {
                                IdSO = competencia.Id,
                                IdProfesor = Profesor.Id,
                                IdAsignatura = asignaturaConCompetencias.Id,
                                IdEstado = estado.Id,
                                Año = año,
                                IdMetodoEvaluacion = 1, // Por defecto
                                Periodo = trimestre.ToString(),
                                Seccion = seccion.Numero,
                                CantEstudiantes = seccion.Estudiantes.Count,
                            };

                            // Guardamos la rúbrica junto con el ID de asignatura para uso posterior
                            rubricasParaInsertar.Add((rubrica, asignaturaConCompetencias.Id));
                        }
                    }
                }
            }

            // 3. Insertar las rúbricas y obtener sus IDs
            await _unitOfWork.Rubricas.AddRangeAsync(rubricasParaInsertar.Select(r => r.Rubrica));
            _unitOfWork.SaveChanges(); // Importante: usar SaveChangesAsync para obtener los IDs

            // 4. Crear los registros para la tabla puente
            var carreraRubricaRegistros = new List<CarreraRubrica>();

            foreach (var (rubrica, idAsignatura) in rubricasParaInsertar)
            {
                // Obtener las carreras asociadas a la asignatura
                var carrerasAsignatura = await _unitOfWork.AsignaturasCarreras.GetCarrerasIdsByAsignaturaId(idAsignatura);

                foreach (var carrera in carrerasAsignatura)
                {
                    carreraRubricaRegistros.Add(new CarreraRubrica
                    {
                        IdRubrica = rubrica.Id, // Ahora tenemos el ID porque ya se insertó
                        IdCarrera = carrera,
                    });
                }
            }

            // 5. Insertar los registros de la tabla puente
            await _unitOfWork.CarrerasRubricas.AddRangeAsync(carreraRubricaRegistros);
             _unitOfWork.SaveChanges();
        }

        private async Task<List<Evidencia>> PrepareEvidenciasForInsert(List<IFormFile> evidenciasExtras, int año, string periodo, int idRubrica)
        {
            var evidencias = new List<Evidencia>();

            if (evidenciasExtras != null)
            {
                foreach (var evidencia in evidenciasExtras)
                {
                    (bool exitoso, string mensaje, string ruta, string nombre) =
                            await _fileHandler.Upload(
                                evidencia,
                                new List<string> { ".pdf", ".doc", ".xlsx", ".rar", ".zip", ".txt", ".ppt" },
                                new RutaEvaluacionBuilder(
                                    año.ToString(),
                                    periodo,
                                    true),
                                nombre => $"evidencia_{idRubrica}_{nombre}");




                    evidencias.Add(new Evidencia
                    {
                        IdRubrica = idRubrica,
                        Nombre = nombre,
                        Ruta = ruta,
                    });
                }
            }

            return evidencias;
        }
        private async Task<List<Resumen>> PrepareResumenesForInsert(List<ResumenDTO> resumenes, int idRubrica)
        {
            var resumenList = new List<Resumen>();
            foreach (var resumen in resumenes)
            {
                // Contar estudiantes por categoría
             

                resumenList.Add(new Resumen
                {
                    IdRubrica = idRubrica,
                    IdPI = resumen.IdPI,
                    CantPrincipiante = resumen.Estudiantes.Count(e => e.Calificacion == 2),
                    CantDesarrollo = resumen.Estudiantes.Count(e => e.Calificacion == 1),
                    CantSatisfactorio = resumen.Estudiantes.Count(e => e.Calificacion == 3),
                    CantExperto = resumen.Estudiantes.Count(e => e.Calificacion == 4)
                });
            }
            return resumenList;
        }

        private async Task<List<Resumen>> PrepareResumenesForUpdate(List<ResumenDTO> resumenesDTO, int idRubrica)
        {
            var resumenes = await _unitOfWork.Resumenes.FindAllAsync(r => r.IdRubrica == idRubrica);

            foreach (var resumen in resumenes)
            {
                var resumenDTO = resumenesDTO.FirstOrDefault(r => r.IdPI == resumen.IdPI)!;
                resumen.CantDesarrollo = resumenDTO.Estudiantes.Count(e => e.Calificacion == 1);
                resumen.CantPrincipiante = resumenDTO.Estudiantes.Count(e => e.Calificacion == 2);
                resumen.CantSatisfactorio = resumenDTO.Estudiantes.Count(e => e.Calificacion == 3);
                resumen.CantExperto = resumenDTO.Estudiantes.Count(e => e.Calificacion == 4);

            }

            return resumenes;
        }
        public async Task<(DateTime inicio, DateTime cierre)> GetFechasCriticas()
        {
            var activa = await _unitOfWork.Estados.GetEstadoByTablaName("Configuracion", "Activa");
            var config = await _unitOfWork.Configuraciones.FindAsync(c => c.IdEstado == activa.Id);
            if (config == null)
            {
                throw new NullReferenceException("No hay una configuración activa");
            }
            return (config.FechaInicio, config.FechaCierre);
        }

        public async Task<PaginatedResult<RubricaViewModel>> GetRubricasBySupervisor(int? page, int? recordsPerPage)
        {
            int id = int.Parse(_jwtService.GetClaimValue("Id")!);
            var activaSinEntrega = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activaEntregada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            var supervisor = await _unitOfWork.Usuarios.FindAsync(u=> u.Id == id);
            var carrerasDelSupervisor = await _unitOfWork.ProfesoresCarreras.GetProfesorWithCarreras(id);

            var rubricas = await _unitOfWork.Rubricas.GetRubricasFiltered(idSO: supervisor.IdSO,carrerasIds: carrerasDelSupervisor.CarrerasIds, estadosIds: new List<int>{ activaEntregada.Id, activaSinEntrega.Id}, page: page, recordsPerPage: recordsPerPage);

            return rubricas;
        }

        public async Task<List<SeccionRubricasViewModel>> GetProfesorSecciones()
        {
            string id = _jwtService.GetClaimValue("Id")!;
            var activo = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activoEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            var secciones = await _unitOfWork.Rubricas.GetProfesorSeccionesWithRubricas(int.Parse(id), activo.Id, activoEntregado.Id);

            foreach (var seccion in secciones)
            {
                var profesores = await _intecService.GetProfesores(seccion.Seccion, seccion.Asignatura);
                var estudiantes = _mapper.Map<List<EstudianteDTO>>(profesores[0].Secciones[0].Estudiantes);
                seccion.Estudiantes = estudiantes;

                if (profesores.Count > 0 && profesores[0].Secciones?.Count > 0 && seccion.Rubricas != null)
                {
                    foreach (var rubrica in seccion.Rubricas)
                    {
                        // Intentar recuperar los resumenes desde Redis
                        string claveRedis = $"rubrica:{rubrica.Id}:resumenes";
                        var resumenesFromRedis = await _redisService.GetResumenListAsync(claveRedis);

                        if (resumenesFromRedis != null && resumenesFromRedis.Count > 0)
                        {
                            // Si hay datos en Redis, usamos esos
                            rubrica.Resumenes = _mapper.Map<List<ResumenViewModelMixed>>(resumenesFromRedis);
                        }
                        else if (rubrica.Resumenes != null)
                        {
                            // Si no hay datos en Redis, asignar estudiantes a los resumenes existentes
                            foreach (var resumen in rubrica.Resumenes)
                            {
                                resumen.Estudiantes = estudiantes;
                            }
                        }
                    }
                }
            }

            return secciones;
        }
    }
}

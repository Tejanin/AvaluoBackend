
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using AvaluoAPI.Utilities;
using AvaluoAPI.Utilities.JWT;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public class RubricaService : IRubricaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHandler _fileHandler;
        private readonly IintecService _intecService;
        private readonly IJwtService _jwtService;
        private TokenConfig _tokens;
        private IHttpContextAccessor _httpContextAccessor;
        public RubricaService(IUnitOfWork unitOfWork, IintecService intecService, FileHandler fileHandler, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _jwtService = jwtService;
            _fileHandler = fileHandler;
            _unitOfWork = unitOfWork;
            _intecService = intecService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CompleteRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras)
        {
            var rubrica = await _unitOfWork.Rubricas.FindAsync(r => r.Id == rubricaDTO.Id);
            var estadoRubricaCompletada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");


            rubrica.IdEstado = estadoRubricaCompletada.Id;
            rubrica.Comentario = rubricaDTO.Comentario;
            rubrica.Problematica = rubricaDTO.Problematica;
            rubrica.Solucion = rubricaDTO.Solucion;
            rubrica.EvaluacionesFormativas = rubricaDTO.EvaluacionesFormativas;
            rubrica.Estrategias = rubricaDTO.Estrategias;
            rubrica.Evidencia = rubricaDTO.Evidencia;
            rubrica.IdMetodoEvaluacion = rubricaDTO.MetodoEvaluacion;
            rubrica.FechaCompletado = DateTime.Now;



            var resumenes = await PrepareResumenesForInsert(rubricaDTO.Resumenes, rubrica.Id);
            var evidencias = await PrepareEvidenciasForInsert(evidenciasExtras, rubrica.Año, rubrica.Periodo, rubrica.Id);




            await Task.WhenAll(
                _unitOfWork.Rubricas.Update(rubrica),
                _unitOfWork.Evidencias.AddRangeAsync(evidencias),
                _unitOfWork.Resumenes.AddRangeAsync(resumenes)
             );
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<RubricaViewModel>> GetAllRubricas(int? idSO = null, List<int>? carrerasIds = null, int? idEstado = null, int? idAsignatura = null)
        {
            return await _unitOfWork.Rubricas.GetRubricasFiltered(idSO,carrerasIds,idEstado,idAsignatura);
        }
        public async Task DesactivateRubricas()
        {

            var entregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Entregada");
            var noEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "No entregada");
            var activo = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activoEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");

            // Siento que hay algo que no estoy tomando en cuenta, pero no se que es
            var asignaturas = await _unitOfWork.Rubricas.ObtenerIdAsignaturasPorEstadoAsync(activoEntregado.Id);
            var rubricasActivasEntregadas = await _unitOfWork.Rubricas.FindAllAsync(r => r.IdEstado == activoEntregado.Id);
            var rubricasActivasNoEntregadas = await _unitOfWork.Rubricas.GetAllIncluding<Rubrica>(r => r.IdEstado == activo.Id, r => r.Asignatura, r => r.Profesor);
            string trimestre = rubricasActivasEntregadas.FirstOrDefault()!.Periodo;
            int año = rubricasActivasEntregadas.FirstOrDefault()!.Año;

            foreach (var rubrica in rubricasActivasEntregadas)
            {
                rubrica.IdEstado = entregado.Id;
            }

            foreach (var rubrica in rubricasActivasNoEntregadas)
            {
                rubrica.IdEstado = noEntregado.Id;
            }

            await Task.WhenAll(
                _unitOfWork.Desempeños.InsertDesempeños(asignaturas, año, trimestre, activoEntregado.Id),
                _unitOfWork.Rubricas.UpdateRangeAsync(rubricasActivasNoEntregadas),
                _unitOfWork.Rubricas.UpdateRangeAsync(rubricasActivasEntregadas),
                _unitOfWork.HistorialIncumplimientos.InsertIncumplimientos(rubricasActivasNoEntregadas)
            );

           _unitOfWork.SaveChanges();

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
                        .FirstOrDefault(a => a.Codigo == seccion.Asignatura);

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
                resumenList.Add(new Resumen
                {
                    IdRubrica = idRubrica,
                    IdPI = resumen.IdPI,
                    CantDesarrollo = resumen.CantDesarrollo,
                    CantPrincipiante = resumen.CantPrincipiante,
                    CantSatisfactorio = resumen.CantSatisfactorio,
                    CantExperto = resumen.CantExperto,
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
                resumen.CantDesarrollo = resumenDTO.CantDesarrollo;
                resumen.CantPrincipiante = resumenDTO.CantPrincipiante;
                resumen.CantSatisfactorio = resumenDTO.CantSatisfactorio;
                resumen.CantExperto = resumenDTO.CantExperto;

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

        public async Task<IEnumerable<RubricaViewModel>> GetRubricasBySupervisor()
        {
            string id = _jwtService.GetClaimValue("Id")!;
            var activaSinEntrega = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activaEntregada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            var carrerasDelSupervisor = await _unitOfWork.ProfesoresCarreras.GetProfesorWithCarreras(int.Parse(id));

            var rubricas = await _unitOfWork.Rubricas.GetRubricasFiltered(carrerasDelSupervisor.IdSO,carrerasDelSupervisor.CarrerasIds);

            throw new NotImplementedException();
        }

        public async Task<List<SeccionRubricasViewModel>> GetProfesorSecciones()
        {
            string id = _jwtService.GetClaimValue("Id")!;
            var activo = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activoEntregado = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            var secciones = await _unitOfWork.Rubricas.GetProfesorSeccionesWithRubricas(int.Parse(id), activo.Id, activoEntregado.Id);
            return secciones; 
        }
    }
}

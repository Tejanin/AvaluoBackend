
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public class RubricaService : IRubricaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHandler _fileHandler;  
        private readonly IintecService _intecService;
        public RubricaService(IUnitOfWork unitOfWork, IintecService intecService, FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            _unitOfWork = unitOfWork;
            _intecService = intecService;
        }

        public async Task CompleteRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras)
        {
            var rubrica = await _unitOfWork.Rubricas.FindAsync(r => r.Id == rubricaDTO.Id);
            var estadoRubricaCompletada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");

            rubrica.UltimaEdicion = rubrica.IdEstado == estadoRubricaCompletada.Id ? DateTime.Now : rubrica.UltimaEdicion;
            rubrica.IdEstado = estadoRubricaCompletada.Id;
            rubrica.Comentario = rubricaDTO.Comentario;
            rubrica.Problematica = rubricaDTO.Problematica;
            rubrica.Solucion = rubricaDTO.Solucion;
            rubrica.EvaluacionesFormativas = rubricaDTO.EvaluacionesFormativas;
            rubrica.Estrategias = rubricaDTO.Estrategias;
            rubrica.Evidencia = rubricaDTO.Evidencia;
            rubrica.FechaCompletado = DateTime.Now;



            var resumenes =await PrepareResumenes(rubricaDTO.Resumenes, rubrica.Id);
            var evidencias =await PrepareEvidencias(evidenciasExtras, rubrica.Año, rubrica.Periodo, rubrica.Id);
            
            


            await Task.WhenAll(
                _unitOfWork.Rubricas.Update(rubrica),
                _unitOfWork.Evidencias.AddRangeAsync(evidencias),
                _unitOfWork.Resumenes.AddRangeAsync(resumenes)
             );
            _unitOfWork.SaveChanges();
        }

        public Task DesactivateRubricas()
        {
            throw new NotImplementedException();
        }

        public async Task InsertRubricas()
        {
            var profesoresTask = _intecService.GetProfesores();
            var asignaturasConCompetenciasTask = _unitOfWork.MapaCompetencias.GetAsignaturasConCompetencias();
            var estadoTask = _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            (int trimestre, int año) = PeriodoExtensions.ObtenerTrimestreActual();

            // Esperar a que se completen las tareas asíncronas
            var profesores = await profesoresTask;
            var asignaturasConCompetencias = await asignaturasConCompetenciasTask;
            var estado = await estadoTask;

            var rubricas = new List<Rubrica>();

            // Iterar por cada sección
            foreach (var profesor in profesores)
            {

                var Profesor = await _unitOfWork.Usuarios.FindAsync(p => p.Email == profesor.Email);

                if(Profesor == null)
                {
                    continue;
                }

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

                            rubricas.Add(rubrica);
                        }
                    }
                }
                    
                    
            }

            await _unitOfWork.Rubricas.AddRangeAsync(rubricas);
           _unitOfWork.SaveChanges();
        }

        private async Task<List<Evidencia>> PrepareEvidencias(List<IFormFile> evidenciasExtras,int año, string periodo, int idRubrica)
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
        private async Task<List<Resumen>> PrepareResumenes(List<ResumenDTO> resumenes, int idRubrica)
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
    }
}

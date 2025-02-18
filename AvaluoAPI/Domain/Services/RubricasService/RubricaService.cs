
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public class RubricaService : IRubricaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IintecService _intecService;
        public RubricaService(IUnitOfWork unitOfWork, IintecService intecService)
        {
            _unitOfWork = unitOfWork;
            _intecService = intecService;
        }
        public async Task<IEnumerable<AsignaturaConCompetenciasViewModel>> InsertRubricas()
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

            return asignaturasConCompetencias;
        }
    }
}

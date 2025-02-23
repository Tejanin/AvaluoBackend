

using AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TiposCompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoMetodoEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.MapaCompetenciaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ResumenRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EvidenciaRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AulaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasCarrerasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.CarrerasRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.ConfiguracionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ProfesorCarreraRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.CarreraRubricaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturaCarreraRepositories;



namespace Avaluo.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IMetodoEvaluacionRepository MetodoEvaluacion { get; }
        IProfesorCarreraRepository ProfesoresCarreras { get; }
        ITipoInformeRepository TiposInformes { get; }
        ITipoCompetenciaRepository TiposCompetencias { get; }
        IEdificioRespository Edificios { get; }

        ICarreraRubricaRepository CarrerasRubricas { get; }
        IConfiguracionRepository Configuraciones { get; }

        ICompetenciaRepository Competencias { get; }
        ISOEvaluacionRepository SOEvaluaciones { get; }
        IMapaCompetenciaRepository MapaCompetencias { get; }
        IRubricaRepository Rubricas { get; }
        IEstadoRepository Estados { get; }
        IEvidenciaRepository Evidencias { get; }
        IResumenRepository Resumenes { get; }

        IAsignaturaCarreraRepository AsignaturasCarreras { get; }

        IAsignaturaRepository Asignaturas { get;}
        IAreaRepository Areas { get;}
        IAulaRepository Aulas { get; }
        ICarreraRepository Carreras { get; }
        IAsignaturaCarreraRepository AsignaturasCarreras { get; }


        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        void SaveChanges();
        Task RollbackTransactionAsync();
    }
}

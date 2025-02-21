

using AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TiposCompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoMetodoEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.MapaCompetenciaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ResumenRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EvidenciaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AulaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories;

namespace Avaluo.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IMetodoEvaluacionRepository MetodoEvaluacion { get; }
        ITipoInformeRepository TiposInformes { get; }
        ITipoCompetenciaRepository TiposCompetencias { get; }
        IEdificioRespository Edificios { get; }

        ICompetenciaRepository Competencias { get; }
        ISOEvaluacionRepository SOEvaluaciones { get; }
        IMapaCompetenciaRepository MapaCompetencias { get; }
        IRubricaRepository Rubricas { get; }
        IEstadoRepository Estados { get; }
        IEvidenciaRepository Evidencias { get; }
        IResumenRepository Resumenes { get; }
        IAulaRepository Aulas { get; }


        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        void SaveChanges();
        Task RollbackTransactionAsync();
    }
}

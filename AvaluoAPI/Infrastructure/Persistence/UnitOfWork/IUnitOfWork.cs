
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

namespace Avaluo.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IMetodoEvaluacionRepository MetodoEvaluacion { get; }
        ITipoInformeRepository TiposInformes { get; }
        ITipoCompetenciaRepository TiposCompetencias { get; }
        ICompetenciaRepository Competencias { get; }
        IEstadoRepository Estados { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        void SaveChanges();
        Task RollbackTransactionAsync();
    }
}

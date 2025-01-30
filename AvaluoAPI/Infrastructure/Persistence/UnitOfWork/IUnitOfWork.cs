﻿using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
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
        ITipoInformeRepository TiposInformes { get; }


        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

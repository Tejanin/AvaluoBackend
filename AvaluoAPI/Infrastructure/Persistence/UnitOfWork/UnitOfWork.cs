using Avaluo.Infrastructure.Data;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AvaluoDbContext _context;
        public UnitOfWork(AvaluoDbContext context)
        {
            _context = context;
            TiposInformes = new TipoInformeRepository(_context);
            Usuarios = new UsuarioRepository(_context);

        }

        // props

        public IUsuarioRepository Usuarios { get; private set; }
        public ITipoInformeRepository TiposInformes { get; private set; }

        // methods
        public Task BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

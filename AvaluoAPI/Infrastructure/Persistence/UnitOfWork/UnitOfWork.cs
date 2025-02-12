using Avaluo.Infrastructure.Data;
using AvaluoAPI.Domain.Services.EdificioService;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories;
using Microsoft.EntityFrameworkCore.Storage;
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
        private readonly DapperContext _dapperContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(AvaluoDbContext context, DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
            _context = context;
            TiposInformes = new TipoInformeRepository(_context);
            TiposCompetencias = new TipoCompetenciaRepository(_context);
            Usuarios = new UsuarioRepository(_context, _dapperContext);
            Edificios = new EdificioRepository(_context, _dapperContext);

        }

        // props

        public IUsuarioRepository Usuarios { get; private set; }
        public ITipoInformeRepository TiposInformes { get; private set; }
        public ITipoCompetenciaRepository TiposCompetencias { get; private set; }
        public IEdificioRespository Edificios { get; private set; }

        // methods
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already started.");

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        // Commit Transaction: Realiza el commit de la transacción
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction not started.");

            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            await _transaction.CommitAsync();  // Realiza el commit de la transacción.
        }

        // Rollback Transaction: Revertir los cambios en caso de error
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction not started.");

            await _transaction.RollbackAsync(); // Revierte los cambios de la transacción.
        }

        // Save Changes: Guardar cambios en la base de datos
        public async Task<int> SaveChangesAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction not started.");

            return await _context.SaveChangesAsync(); // Guarda los cambios en EF Core.
        }

        // Dispose: Libera los recursos
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

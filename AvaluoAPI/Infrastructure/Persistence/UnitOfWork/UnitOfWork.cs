
using Avaluo.Infrastructure.Data;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TiposCompetenciasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.TipoMetodoEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.MapaCompetenciaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ResumenRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EvidenciaRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreasRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AulaRepositories;

using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasCarrerasRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.CarrerasRepositories;

using System.Drawing.Printing;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ConfiguracionRepositories;
using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ProfesorCarreraRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.CarreraRubricaRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.HistorialIncumplimientoRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.InformesRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.IDesempeñoRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.RolRepositories;




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
            Configuraciones = new ConfiguracionRepository(_context);
            SOEvaluaciones = new SOEvaluacionRepository(_context, _dapperContext);
            MapaCompetencias = new MapaCompetenciaRepository(_context, _dapperContext);
            Rubricas = new RubricaRepository(_context, _dapperContext);
            TiposInformes = new TipoInformeRepository(_context);
            TiposCompetencias = new TipoCompetenciaRepository(_context);
            Competencias = new CompetenciaRepository(_context, _dapperContext);
            Estados = new EstadoRepository(_context);
            Usuarios = new UsuarioRepository(_context, _dapperContext);

            Edificios = new EdificioRepository(_context, _dapperContext);

            CarrerasRubricas = new CarreraRubricaRepository(_context);
            Edificios = new EdificioRepository(_context, _dapperContext);

            HistorialIncumplimientos = new HistorialIncumplimientoRepository(_context, _dapperContext);
            Resumenes = new ResumenRepository(_context, _dapperContext);
            Evidencias = new EvidenciaRepository(_context);
            MetodoEvaluacion = new MetodoEvaluacionRepository(_context);
            Asignaturas = new AsignaturaRepository(_context, _dapperContext);

            Informes = new InformeRepository(_context, _dapperContext);
            Areas = new AreaRepository(_context,_dapperContext);
            ProfesoresCarreras = new ProfesorCarreraRepository(_context,_dapperContext);

            Desempeños = new DesempeñoRepository(_context, _dapperContext);
            Aulas = new AulaRepository(_context, _dapperContext);
            Carreras = new CarreraRepository(_context, _dapperContext);
            AsignaturasCarreras = new AsignaturaCarreraRepository(_context, _dapperContext);
            Roles = new RolRepository(_context, _dapperContext);
        }

        // props

        public IUsuarioRepository Usuarios { get; private set; }
        public IProfesorCarreraRepository ProfesoresCarreras { get; private set; }
        public ITipoInformeRepository TiposInformes { get; private set; }
        public ITipoCompetenciaRepository TiposCompetencias { get; private set; }
        public ICarreraRubricaRepository CarrerasRubricas { get; private set; }
        public IEdificioRespository Edificios { get; private set; }
        public IConfiguracionRepository Configuraciones { get; private set; }
        public ICompetenciaRepository Competencias { get; private set; }
        public IEstadoRepository Estados { get; private set; }
        public IRubricaRepository Rubricas { get; private set; }
        public IEvidenciaRepository Evidencias { get; private set; }
        public IResumenRepository Resumenes { get; private set; }
        public ISOEvaluacionRepository SOEvaluaciones { get; private set; }
        public IMapaCompetenciaRepository MapaCompetencias { get; private set; }
        public IMetodoEvaluacionRepository MetodoEvaluacion { get; private set; }
        public IInformeRepository Informes { get; private set; }
        public IHistorialIncumplimientoRepository HistorialIncumplimientos { get; private set; }
        public IAsignaturaCarreraRepository AsignaturasCarreras { get; private set; }
        public IDesempeñoRepository Desempeños { get; private set; }
        public IAsignaturaRepository Asignaturas { get; private set; }
        public IAreaRepository Areas { get; private set; }
        public IRolRepository Roles { get; private set; }

        public IAulaRepository Aulas { get; private set; }
        public ICarreraRepository Carreras { get; private set; }
        


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

using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mapster;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories
{
    public class CompetenciaRepository : Repository<Competencia>, ICompetenciaRepository
    {
        private readonly DapperContext _dapperContext;

        public CompetenciaRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }


        public async Task<IEnumerable<CompetenciaViewModel>> GetAllCompetencias()
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
        SELECT 
            c.Id, 
            c.Nombre, 
            c.Descripcion, 
            c.FechaCreacion, 
            c.UltimaEdicion,
            tc.Nombre AS TipoCompetencia, 
            e.Descripcion AS Estado
        FROM competencia c
        LEFT JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id  
        LEFT JOIN estado e ON c.Id_Estado = e.Id";

            var competencias = await connection.QueryAsync<CompetenciaViewModel>(query);

            return competencias.ToList();
        }





        public async Task<CompetenciaViewModel> GetCompetenciaById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
        SELECT 
            c.Id, 
            c.Nombre, 
            c.Descripcion, 
            c.FechaCreacion, 
            c.UltimaEdicion,
            tc.Nombre AS TipoCompetencia, 
            e.Descripcion AS Estado
        FROM competencia c
        LEFT JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id  
        LEFT JOIN estado e ON c.Id_Estado = e.Id
        WHERE c.Id = @Id";

            var parametros = new { Id = id };

            var competencia = await connection.QueryFirstOrDefaultAsync<CompetenciaViewModel>(query, parametros);

            return competencia;
        }

    }
}

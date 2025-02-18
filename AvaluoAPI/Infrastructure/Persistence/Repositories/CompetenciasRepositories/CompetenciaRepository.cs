﻿using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using System.Data;

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

        public async Task<CompetenciaViewModel?> GetCompetenciaById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
        SELECT 
            c.Id, 
            c.Nombre, 
            c.Acron, 
            c.Titulo, 
            c.DescripcionES, 
            c.DescripcionEN, 
            c.FechaCreacion, 
            c.UltimaEdicion,

            tc.Id,
            tc.Nombre,
            tc.FechaCreacion,
            tc.UltimaEdicion,

            e.Id,
            e.Descripcion,
            e.IdTabla
        FROM competencia c
        LEFT JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id  
        LEFT JOIN estado e ON c.Id_Estado = e.Id
        WHERE c.Id = @Id";

            var parametros = new { Id = id };

            var competencia = await connection.QueryAsync<CompetenciaViewModel, TipoCompetenciaViewModel, EstadoViewModel, CompetenciaViewModel>(
                query,
                (competencia, tipoCompetencia, estado) =>
                {
                    competencia.TipoCompetencia = tipoCompetencia;
                    competencia.Estado = estado;
                    return competencia;
                },
                parametros,
                splitOn: "Id" 
            );

            return competencia.FirstOrDefault();
        }
        public async Task<PaginatedResult<CompetenciaViewModel>> GetCompetencias(
         string? nombre,
         string? acron,
         string? titulo,
         int? idTipo,
         int? idEstado,
         int? page,
         int? recordsPerPage)
           {   
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
            SELECT COUNT(*)
            FROM competencia c
            LEFT JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id  
            LEFT JOIN estado e ON c.Id_Estado = e.Id
            WHERE (@Nombre IS NULL OR c.Nombre LIKE '%' + @Nombre + '%')
            AND (@Acron IS NULL OR c.Acron LIKE '%' + @Acron + '%')
            AND (@Titulo IS NULL OR c.Titulo LIKE '%' + @Titulo + '%')
            AND (@IdTipo IS NULL OR c.Id_Tipo = @IdTipo)
            AND (@IdEstado IS NULL OR c.Id_Estado = @IdEstado)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                Nombre = nombre,
                Acron = acron,
                Titulo = titulo,
                IdTipo = idTipo,
                IdEstado = idEstado
            });

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;

            int currentPage = page.HasValue && page > 0 ? page.Value : 1;

            int offset = (currentPage - 1) * currentRecordsPerPage;

            var query = $@"
            SELECT 
                c.Id, 
                c.Nombre, 
                c.Acron, 
                c.Titulo, 
                c.DescripcionES, 
                c.DescripcionEN, 
                c.FechaCreacion, 
                c.UltimaEdicion,

                tc.Id,
                tc.Nombre,
                tc.FechaCreacion,
                tc.UltimaEdicion,

                e.Id,
                e.Descripcion,
                e.IdTabla
            FROM competencia c
            LEFT JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id  
            LEFT JOIN estado e ON c.Id_Estado = e.Id
            WHERE (@Nombre IS NULL OR c.Nombre LIKE '%' + @Nombre + '%')
            AND (@Acron IS NULL OR c.Acron LIKE '%' + @Acron + '%')
            AND (@Titulo IS NULL OR c.Titulo LIKE '%' + @Titulo + '%')
            AND (@IdTipo IS NULL OR c.Id_Tipo = @IdTipo)
            AND (@IdEstado IS NULL OR c.Id_Estado = @IdEstado)
            ORDER BY c.Id
            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var parametros = new
            {
                Nombre = nombre,
                Acron = acron,
                Titulo = titulo,
                IdTipo = idTipo,
                IdEstado = idEstado,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            };

            var competenciasDictionary = new Dictionary<int, CompetenciaViewModel>();

            var competencias = await connection.QueryAsync<CompetenciaViewModel, TipoCompetenciaViewModel, EstadoViewModel, CompetenciaViewModel>(
                query,
                (competencia, tipoCompetencia, estado) =>
                {
                    if (!competenciasDictionary.TryGetValue(competencia.Id, out var competenciaEntry))
                    {
                        competenciaEntry = competencia;
                        competenciaEntry.TipoCompetencia = tipoCompetencia;
                        competenciaEntry.Estado = estado;
                        competenciasDictionary.Add(competencia.Id, competenciaEntry);
                    }

                    return competenciaEntry;
                },
                parametros,
                splitOn: "Id"
            );
            return new PaginatedResult<CompetenciaViewModel>(competenciasDictionary.Values, currentPage, currentRecordsPerPage, totalRecords);
        }
    }
}

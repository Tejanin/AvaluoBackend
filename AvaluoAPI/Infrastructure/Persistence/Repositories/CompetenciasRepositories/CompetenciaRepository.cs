using Avaluo.Infrastructure.Data;
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


            if (totalRecords == 0)
            {
                return new PaginatedResult<CompetenciaViewModel>(Enumerable.Empty<CompetenciaViewModel>(), 1, 0, 0);
            }

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
        public async Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetMapaCompetencias(int idCarrera, int? idTipoCompetencia)
        {
            using var connection = _dapperContext.CreateConnection();

            const string sql = @"
            SELECT 
                a.Id,
                a.Codigo,
                a.Nombre,

                ea.Id, 
                ea.IdTabla,
                ea.Descripcion,

                c.Id,
                c.Nombre,
                c.Acron,

                ec.Id,
                ec.IdTabla,
                ec.Descripcion
            FROM asignatura_carrera ac
            INNER JOIN asignaturas a ON ac.Id_Asignatura = a.Id
            INNER JOIN estado ea ON a.IdEstado = ea.Id
            LEFT JOIN mapa_competencias mc ON a.Id = mc.Id_Asignatura
            LEFT JOIN competencia c ON mc.Id_Competencia = c.Id
            LEFT JOIN estado ec ON mc.Id_Estado = ec.Id
            WHERE ac.Id_Carrera = @IdCarrera
            AND (@IdTipoCompetencia IS NULL OR c.Id_Tipo = @IdTipoCompetencia)
            ORDER BY a.Codigo";

            var asignaturasDict = new Dictionary<int, AsignaturaConCompetenciasViewModel>();

            await connection.QueryAsync<AsignaturaConCompetenciasViewModel, EstadoViewModel, CompetenciaResumenViewModel, EstadoViewModel, AsignaturaConCompetenciasViewModel>(
                sql,
                (asignatura, estadoAsignatura, competencia, estadoCompetencia) =>
                {
                    if (!asignaturasDict.TryGetValue(asignatura.Id, out var asignaturaEntry))
                    {
                        asignaturaEntry = asignatura;
                        asignaturaEntry.Estado = estadoAsignatura;
                        asignaturaEntry.Competencias = new List<CompetenciaResumenViewModel>();
                        asignaturasDict.Add(asignatura.Id, asignaturaEntry);
                    }

                    if (competencia != null)
                    {
                        competencia.Estado = estadoCompetencia;
                        asignaturaEntry.Competencias.Add(competencia);
                    }

                    return asignaturaEntry;
                },
                new { IdCarrera = idCarrera, IdTipoCompetencia = idTipoCompetencia },
                splitOn: "Id,Id,Id,Id");

            return asignaturasDict.Values;
        }


        public async Task<bool> UpdateEstadoMapaCompetencia(int idAsignatura, int idCompetencia, int idNuevoEstado)
        {
            using var connection = _dapperContext.CreateConnection();

            const string checkSql = @"
            SELECT COUNT(*) 
            FROM mapa_competencias
            WHERE Id_Asignatura = @IdAsignatura AND Id_Competencia = @IdCompetencia";

            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new
            {
                IdAsignatura = idAsignatura,
                IdCompetencia = idCompetencia
            });

            if (exists == 0)
            {
                throw new KeyNotFoundException("No existe un registro en mapa de competencias con la asignatura y competencia proporcionadas.");
            }

            const string updateSql = @"
            UPDATE mapa_competencias
            SET Id_Estado = @IdNuevoEstado
            WHERE Id_Asignatura = @IdAsignatura AND Id_Competencia = @IdCompetencia";

            var affectedRows = await connection.ExecuteAsync(updateSql, new
            {
                IdAsignatura = idAsignatura,
                IdCompetencia = idCompetencia,
                IdNuevoEstado = idNuevoEstado
            });

            return affectedRows > 0;
        }

    }
}

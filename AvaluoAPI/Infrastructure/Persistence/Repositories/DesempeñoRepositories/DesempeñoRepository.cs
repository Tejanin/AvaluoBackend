using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Dapper;
using Microsoft.Graph.Models;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.IDesempeñoRepositories
{
    public class DesempeñoRepository: Repository<Desempeno>, IDesempeñoRepository
    {
        private readonly DapperContext _dapperContext;
        public DesempeñoRepository(AvaluoDbContext context, DapperContext dapperContext): base(context)
        {
            _dapperContext= dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task InsertDesempeños( List<int> asignaturas, int año ,string periodo, int estado)
        {
            var desempeños = new List<Desempeno>();
            foreach (int id in asignaturas)
            {
                 var dtos = await ObtenerResumenPorAsignaturaEstadoAsync(id, estado, año, periodo);

                foreach (var d in dtos) 
                {
                    var desempeño = new Desempeno
                    {
                        IdAsignatura = d.IdAsignatura,
                        IdSO = d.IdSO,
                        IdPI = d.IdPI,
                        Año = año,
                        Trimestre = periodo[0],
                        Satisfactorio = GetDesempeño(d).satisfactorio,
                        Porcentaje = GetDesempeño(d).porcentaje
                    };
                    desempeños.Add(desempeño);
                }
            }
            await _context.Set<Desempeno>().AddRangeAsync(desempeños);

        }

        private async Task<List<DesempeñoDTO>> ObtenerResumenPorAsignaturaEstadoAsync(int idAsignatura, int idEstado, int año, string periodo)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                            SELECT 
                                r.IdAsignatura,
                                r.IdSO,
                                rs.Id_PI AS IdPI,
                                SUM(rs.CantExperto) AS CantExperto,
                                SUM(rs.CantSatisfactorio) AS CantSatisfactorio,
                                SUM(rs.CantPrincipiante) AS CantPrincipiante,
                                SUM(rs.CantDesarrollo) AS CantDesarrollo
                            FROM rubricas r
                            LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica
                            WHERE r.IdAsignatura = @IdAsignatura 
                            AND r.IdEstado = @IdEstado
                            AND r.Año = @Año
                            AND r.Periodo = @Periodo
                            GROUP BY r.IdAsignatura, r.IdSO, rs.Id_PI;";

            var parameters = new
            {
                IdAsignatura = idAsignatura,
                IdEstado = idEstado,
                Año = año,
                Periodo = periodo
            };

            var resultado = await connection.QueryAsync<DesempeñoDTO>(query, parameters);

            return resultado.ToList();
        }


        private (bool satisfactorio, decimal porcentaje) GetDesempeño(DesempeñoDTO resumen)
        {
            int total = resumen.CantDesarrollo + resumen.CantExperto + resumen.CantPrincipiante + resumen.CantSatisfactorio;
            decimal porcentaje = (decimal)(resumen.CantSatisfactorio + resumen.CantExperto) / total;
            bool satisfactorio = porcentaje >= 0.8m;

            return (satisfactorio, porcentaje);
        }

        public async Task<List<int>> ObtenerIdSOPorAsignaturasAsync(int año, string periodo, int idAsignatura)
        {
            using var connection = _dapperContext.CreateConnection();
            string query = "SELECT DISTINCT Id_SO FROM desempeno WHERE Id_Asignatura = @IdAsignatura AND Trimestre = @Trimestre AND Año = @Año;";
            var asignaturas = await connection.QueryAsync<int>(query, new
            {
                IdAsignatura = idAsignatura,
                Trimestre = periodo,
                Año = año
            });
            return asignaturas.ToList();
        }

        public async Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(
    int? año = null, string? periodo = null, int? idAsignatura = null, int? idSO = null)
        {
            using var connection = _dapperContext.CreateConnection();

            const string sql = @"
    SELECT 
    d.Id_Asignatura,
    a.Codigo AS CodigoAsignatura,
    a.Nombre AS NombreAsignatura,

    d.Id_SO,
    so.Nombre AS NombreSO,
    so.DescripcionES AS DescripcionSO,

    d.IdPI,
    pi.DescripcionES AS DescripcionPI,

    d.Año,
    d.Trimestre,

    -- Total de estudiantes evaluados
    ISNULL(SUM(rs.CantExperto) + SUM(rs.CantSatisfactorio) + SUM(rs.CantPrincipiante) + SUM(rs.CantDesarrollo), 0) AS TotalEstudiantes,

    -- Cantidad de estudiantes por nivel
    ISNULL(SUM(rs.CantExperto), 0) AS CantExperto,
    ISNULL(SUM(rs.CantSatisfactorio), 0) AS CantSatisfactorio,
    ISNULL(SUM(rs.CantPrincipiante), 0) AS CantPrincipiante,
    ISNULL(SUM(rs.CantDesarrollo), 0) AS CantDesarrollo,

    -- Porcentaje de satisfactorios
    d.Porcentaje AS PorcentajeSatisfactorio,

    -- Si el PI es satisfactorio
    d.Satisfactorio AS EsSatisfactorio

FROM Desempeno d
INNER JOIN asignaturas a ON d.Id_Asignatura = a.Id
INNER JOIN competencia so ON d.Id_SO = so.Id
INNER JOIN PI pi ON d.IdPI = pi.Id
INNER JOIN rubricas r ON d.Id_Asignatura = r.IdAsignatura 
LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica AND d.IdPI = rs.Id_PI

WHERE (@Año IS NULL OR d.Año = @Año)
AND (@Trimestre IS NULL OR d.Trimestre = LEFT(@Trimestre, 1))
AND (@IdAsignatura IS NULL OR d.Id_Asignatura = @IdAsignatura)
AND (@IdSO IS NULL OR d.Id_SO = @IdSO) 

GROUP BY 
    d.Id_Asignatura, a.Codigo, a.Nombre, 
    d.Id_SO, so.Nombre, so.DescripcionES, 
    d.IdPI, pi.Id, pi.DescripcionES, 
    d.Año, d.Trimestre, d.Porcentaje, d.Satisfactorio

ORDER BY d.Año DESC, d.Trimestre DESC, a.Codigo, so.Nombre, pi.Id;

    ";

            var datosPlano = await connection.QueryAsync<dynamic>(sql, new
            {
                Año = año,
                Trimestre = periodo,
                IdAsignatura = idAsignatura,
                IdSO = idSO
            });

            var informeDict = new Dictionary<int, InformeDesempeñoViewModel>();

            foreach (var fila in datosPlano)
            {
                int idAsignaturaFila = fila.Id_Asignatura;
                if (!informeDict.TryGetValue(idAsignaturaFila, out InformeDesempeñoViewModel? informe))
                {
                    informe = new InformeDesempeñoViewModel
                    {
                        IdAsignatura = idAsignaturaFila,
                        CodigoAsignatura = fila.CodigoAsignatura,
                        NombreAsignatura = fila.NombreAsignatura,
                        Año = fila.Año,
                        Trimestre = fila.Trimestre,
                        TotalEstudiantes = 0,
                        StudentOutcomes = new List<StudentOutcomeViewModel>()
                    };

                    informeDict[idAsignaturaFila] = informe;
                }

                int idSoFila = fila.Id_SO;
                var soExistente = informe.StudentOutcomes.FirstOrDefault(so => so.IdSO == idSoFila);
                if (soExistente == null)
                {
                    soExistente = new StudentOutcomeViewModel
                    {
                        IdSO = idSoFila,
                        NombreSO = fila.NombreSO,
                        DescripcionSO = fila.DescripcionSO,
                        PerformanceIndicators = new List<PerformanceIndicatorViewModel>()
                    };
                    informe.StudentOutcomes.Add(soExistente);
                }

                soExistente.PerformanceIndicators.Add(new PerformanceIndicatorViewModel
                {
                    IdPI = fila.IdPI,
                    DescripcionPI = fila.DescripcionPI,
                    CantExperto = fila.CantExperto,
                    CantSatisfactorio = fila.CantSatisfactorio,
                    CantPrincipiante = fila.CantPrincipiante,
                    CantDesarrollo = fila.CantDesarrollo,
                    PorcentajeSatisfactorio = fila.PorcentajeSatisfactorio,
                    EsSatisfactorio = fila.EsSatisfactorio
                });

                informe.TotalEstudiantes += fila.CantExperto + fila.CantSatisfactorio + fila.CantPrincipiante + fila.CantDesarrollo;
            }

            return informeDict.Values;
        }



    }
}

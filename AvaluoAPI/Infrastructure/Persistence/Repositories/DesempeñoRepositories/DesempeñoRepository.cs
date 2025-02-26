using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Dapper;

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
            var satisfactorio = resumen.CantSatisfactorio / total >= 0.8;
            var porcentaje = resumen.CantSatisfactorio / total;
            return (satisfactorio, porcentaje);
        }
    }
}

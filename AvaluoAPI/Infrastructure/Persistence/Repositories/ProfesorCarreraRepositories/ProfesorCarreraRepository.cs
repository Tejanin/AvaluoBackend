using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain;
using AvaluoAPI.Infrastructure.Data.Contexts;

using Dapper;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ProfesorCarreraRepositories
{
    public class ProfesorCarreraRepository: Repository<ProfesorCarrera>, IProfesorCarreraRepository
    {
        private readonly DapperContext _dapperContext;
        public ProfesorCarreraRepository(AvaluoDbContext context, DapperContext dapperContext):base(context)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<ProfesorCarrerasResult> GetProfesorWithCarreras(int profesorId)
        {
            using var connection = _dapperContext.CreateConnection();

            const string query = @"
                                    SELECT 
                                        u.Id as IdProfesor,
                                        u.IdSO,
                                        c.Id as CarreraId
                                    FROM usuario u
                                    LEFT JOIN profesor_carrera pc ON u.Id = pc.Profesor_Id
                                    LEFT JOIN carreras c ON pc.Carrera_Id = c.Id
                                    WHERE u.Id = @Id";

            var result = new ProfesorCarrerasResult();
            var carrerasIds = new List<int>();

            await connection.QueryAsync<ProfesorCarrerasResult, int?, ProfesorCarrerasResult>(
                query,
                (profesor, carreraId) =>
                {
                    if (result.IdProfesor == 0)
                    {
                        result.IdProfesor = profesor.IdProfesor;
                        result.IdSO = profesor.IdSO;
                    }

                    if (carreraId.HasValue)
                    {
                        result.CarrerasIds.Add(carreraId.Value);
                    }

                    return result;
                },
                new { Id = profesorId },
                splitOn: "CarreraId");

            return result;
        }
    }
}

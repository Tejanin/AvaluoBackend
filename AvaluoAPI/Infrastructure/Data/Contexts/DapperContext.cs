using Microsoft.Data.SqlClient;
using System.Data;

namespace AvaluoAPI.Infrastructure.Data.Contexts
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

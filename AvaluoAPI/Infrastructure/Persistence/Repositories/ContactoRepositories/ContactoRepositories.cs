using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ContactoRepositories
{
    public class ContactoRepository : Repository<Contacto>, IContactoRepository
    {
        private readonly DapperContext _dapperContext;

        public ContactoRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<ContactoViewModel>> GetContactosByUserId(int userId)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
             SELECT 
                 c.Id, 
                 c.NumeroContacto, 
                 c.Id_Usuario
             FROM contacto c
            WHERE c.Id_Usuario = @UserId";

            return await connection.QueryAsync<ContactoViewModel>(query, new { UserId = userId });
        }

        public async Task<ContactoViewModel?> GetContactoById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
             SELECT 
                 c.Id, 
                 c.NumeroContacto, 
                 c.Id_Usuario
             FROM contacto c
            WHERE c.Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<ContactoViewModel>(query, new { Id = id });
        }
    }
}

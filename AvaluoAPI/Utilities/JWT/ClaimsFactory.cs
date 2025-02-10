using Avaluo.Infrastructure.Data.Models;
using System.Security.Claims;

namespace AvaluoAPI.Utilities.JWT
{
    public interface IClaimsFactory
    {
        IEnumerable<Claim> CreateClaims(Usuario user, bool includeSOClaim);
    }

    // Implementación del factory
    public class ClaimsFactory : IClaimsFactory
    {
        public IEnumerable<Claim> CreateClaims(Usuario user, bool includeSOClaim)
        {
            var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("Name", user.Nombre),
            new Claim("Lname", user.Apellido)
        };

            if (includeSOClaim)
            {
                claims.Add(new Claim("SO", "true"));
            }

            return claims;
        }
    }
}

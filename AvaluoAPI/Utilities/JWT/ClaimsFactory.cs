using Avaluo.Infrastructure.Data.Models;
using System.Security.Claims;

namespace AvaluoAPI.Utilities.JWT
{
    public interface IClaimsFactory
    {
        List<Claim> CreateClaims(Usuario user,string? carrera, bool includeSOClaim= false, bool includeCarreraClaim =false,bool includeAreaClaim = false);
    }

    // Implementación del factory
    public class ClaimsFactory : IClaimsFactory
    {
        public List<Claim> CreateClaims(Usuario user, string? carrera, bool includeSOClaim = false, bool includeCarreraClaim = false, bool includeAreaClaim = false)
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
                claims.Add(new Claim("SO", user.IdSO.ToString()));
            }

            if (includeCarreraClaim && carrera != null)
            {
                claims.Add(new Claim("Carrera", carrera));
            }

            if (includeAreaClaim)
            {
                claims.Add(new Claim("Area", user.IdArea.ToString()));
            }

            return claims;
        }

    }
}

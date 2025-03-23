using Avaluo.Infrastructure.Data.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AvaluoAPI.Utilities.JWT
{
    public class TokenConfig
    {
        public string JwtToken { get; set; } = null!;
        // Podemos mantener esta propiedad para compatibilidad, pero ya no la usaremos como cookie
        public string PermissionToken { get; set; }
    }

    public class JwtUserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
    }

    public class UserPermissions
    {
        public int RolId { get; set; }
        public string RolDescripcion { get; set; } = null!;
        public Dictionary<string, bool> Permisos { get; set; } = null!;
    }

    public interface IJwtService
    {
        TokenConfig GenerateAuthTokens(Usuario user, Rol rol, bool includeSOClaim = false);
        TokenConfig GenerateEmailToken(Usuario user);
        bool ValidateToken(string token);
        ClaimsPrincipal? GetClaimsPrincipal(string token);
        UserPermissions? ValidatePermissionCookie(string permissionToken);
        string? GetClaimValue(string claim);
        void BlacklistToken(string token);
        bool IsTokenBlacklisted(string token);
        List<string> GetPermissionsFromToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IDataProtector _protector;
        private readonly IClaimsFactory _claimsFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HashSet<string> _blacklistedTokens = new();

        public JwtService(
        IConfiguration configuration,
        IDataProtectionProvider dataProtectionProvider,
        IHttpContextAccessor httpContextAccessor,
        IClaimsFactory claimsFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _key = configuration["Jwt:Key"]!;
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            _protector = dataProtectionProvider.CreateProtector("Permissions");
            _claimsFactory = claimsFactory;
        }

        public TokenConfig GenerateAuthTokens(Usuario user, Rol rol, bool includeSOClaim = false)
        {
            // JWT con info no sensitiva
            var jwtClaims = _claimsFactory.CreateClaims(user, includeSOClaim);

            // Crear diccionario con todos los permisos

            if(rol != null)
            {

            }
            var permissionsDict = new Dictionary<string, bool>
            {
                { "EsProfesor", rol.EsProfesor },
                { "EsSupervisor", rol.EsSupervisor },
                { "EsCoordinadorArea", rol.EsCoordinadorArea },
                { "EsCoordinadorCarrera", rol.EsCoordinadorCarrera },
                { "EsAdmin", rol.EsAdmin },
                { "EsAux", rol.EsAux },
                { "VerInformes", rol.VerInformes },
                { "VerListaDeRubricas", rol.VerListaDeRubricas },
                { "ConfigurarFechas", rol.ConfigurarFechas },
                { "VerManejoCurriculum", rol.VerManejoCurriculum }
            };

            // Añadir información del rol
            jwtClaims.Add(new Claim("rol_id", rol.Id.ToString()));
            jwtClaims.Add(new Claim("rol_descripcion", rol.Descripcion ?? ""));

            // Añadir permisos como claims individuales (solo los que son true)
            foreach (var permission in permissionsDict)
            {
                if (permission.Value)
                {
                    jwtClaims.Add(new Claim("permission", permission.Key));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _issuer,
                _audience,
                jwtClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // Mantenemos el token de permisos por compatibilidad, pero no lo usaremos como cookie
            var permissions = new UserPermissions
            {
                RolId = rol.Id,
                RolDescripcion = rol.Descripcion!,
                Permisos = permissionsDict
            };

            var permissionJson = JsonSerializer.Serialize(permissions);
            var protectedPermissions = _protector.Protect(permissionJson);

            return new TokenConfig
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                PermissionToken = protectedPermissions
            };
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal? GetClaimsPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string? GetClaimValue(string claim)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(claim);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("No se pudo obtener el ID del usuario");
            }

            return userIdClaim.Value;
        }

        public UserPermissions? ValidatePermissionCookie(string permissionToken)
        {
            try
            {
                var unprotectedJson = _protector.Unprotect(permissionToken);
                return JsonSerializer.Deserialize<UserPermissions>(unprotectedJson);
            }
            catch
            {
                return null;
            }
        }

        public void BlacklistToken(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }

        // Implementación del método para obtener permisos del token JWT
        public List<string> GetPermissionsFromToken(string token)
        {
            try
            {
                var principal = GetClaimsPrincipal(token);
                if (principal == null)
                {
                    return new List<string>();
                }

                // Obtener todos los claims de tipo "permission"
                var permissionClaims = principal.FindAll("permission");
                return permissionClaims.Select(c => c.Value).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo permisos del token: {ex.Message}");
                return new List<string>();
            }
        }

        public TokenConfig GenerateEmailToken(Usuario user)
        {
            var jwtClaims = _claimsFactory.CreateClaims(user, false);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _issuer,
                _audience,
                jwtClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new TokenConfig
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };
        }
           
    }
}
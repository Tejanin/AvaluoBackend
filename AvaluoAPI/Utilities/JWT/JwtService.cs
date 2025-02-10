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
        public string PermissionToken { get; set; } = null!;
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
        TokenConfig GenerateTokens(Usuario user, Rol rol, bool includeSOClaim = false);
        bool ValidateToken(string token);
        ClaimsPrincipal? GetClaimsPrincipal(string token);
        UserPermissions? ValidatePermissionCookie(string permissionToken);
        string? GetClaimValue(string token, string claimType);
    }

    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IDataProtector _protector;
        private readonly IClaimsFactory _claimsFactory;

        public JwtService(
        IConfiguration configuration,
        IDataProtectionProvider dataProtectionProvider,
        IClaimsFactory claimsFactory)
        {
            _key = configuration["Jwt:Key"]!;
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            _protector = dataProtectionProvider.CreateProtector("Permissions");
            _claimsFactory = claimsFactory;
        }

        public TokenConfig GenerateTokens(Usuario user, Rol rol, bool includeSOClaim = false)
        {
            // JWT con info no sensitiva
            var jwtClaims = _claimsFactory.CreateClaims(user, includeSOClaim);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _issuer,
                _audience,
                jwtClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // Token de permisos encriptado
            var permissions = new UserPermissions
            {
                RolId = rol.Id,
                RolDescripcion = rol.Descripcion!,
                Permisos = new Dictionary<string, bool>
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
                }
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

        public string? GetClaimValue(string token, string claimType)
        {
            var principal = GetClaimsPrincipal(token);
            if (principal == null) return null;

            return principal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
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
    }
}

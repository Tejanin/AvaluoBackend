using AvaluoAPI.Utilities;

namespace AvaluoAPI.Application.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwtService;

        public AuthMiddleware(RequestDelegate next, IJwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var permissionCookie = context.Request.Cookies["X-Permissions"];

            if (!string.IsNullOrEmpty(authHeader) && !string.IsNullOrEmpty(permissionCookie))
            {
                var jwt = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : authHeader;

                if (_jwtService.ValidateToken(jwt))
                {
                    var principal = _jwtService.GetClaimsPrincipal(jwt);
                    var permissions = _jwtService.ValidatePermissionCookie(permissionCookie);

                    if (principal != null && permissions != null)
                    {
                        context.User = principal;
                        context.Items["UserPermissions"] = permissions;
                    }
                }
            }

            await _next(context);
        }
    }

    // Extensión para facilitar el registro del middleware
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}

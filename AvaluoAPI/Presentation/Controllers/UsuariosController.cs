using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Domain.Services.UsuariosService;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Utilities.JWT;
using Microsoft.AspNetCore.Mvc;




namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        public UsuariosController(IUsuarioService usuarioService, IJwtService jwtService, IEmailService emailService)
        {
            _emailService = emailService;
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }


        [HttpGet]
        public async Task<ActionResult> Get(int? estado, int? area, int? rol, int? page, int? recordsPerPage)
        {
            return Ok(new { message = "Operación exitosa", data = await _usuarioService.GetAll(estado, area, rol, page, recordsPerPage) });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(new { message = "Operación exitosa", data = await _usuarioService.GetById(id) });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });
            }

            await _usuarioService.Register(usuarioDTO);
            return CreatedAtAction(nameof(Post),
                new { id = usuarioDTO.Id },
                new { message = "Usuario creado exitosamente", data = usuarioDTO });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ModifyUsuarioDTO usuarioDTO)
        {
            await _usuarioService.Update(id, usuarioDTO);
            return Accepted(new { message = "Usuario actualizado exitosamente", data = usuarioDTO });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var tokens = await _usuarioService.Login(loginDTO);

            Response.Cookies.Append("X-Permissions", tokens.PermissionToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Login exitoso", data = new { token = tokens.JwtToken } });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                _jwtService.BlacklistToken(token);
            }

            Response.Cookies.Delete("X-Permissions");
            return Ok(new { message = "Sesión cerrada exitosamente" });
        }

        [HttpPut("desactivate/{id}")]
        public async Task<ActionResult> Desactivate([FromBody] int id)
        {
            await _usuarioService.Desactivate(id);
            return Accepted(new { message = "Usuario desactivado exitosamente" });
        }

        [HttpPut("activate/{id}")]
        public async Task<ActionResult> Activate([FromBody] int id)
        {
            await _usuarioService.Activate(id);
            return Accepted(new { message = "Usuario activado exitosamente" });
        }

        [HttpPost("registerManyUsers")]
        public async Task<IActionResult> RegisterRange(List<UsuarioDTO> usuarios)
        {
            await _usuarioService.RegisterRange(usuarios);
            return Created();
        }

        [HttpGet("test-permissions")]
        public IActionResult TestPermissions()
        {
            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var cookies = Request.Cookies.ToDictionary(c => c.Key, c => c.Value);
            var jwt = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var permissionCookie = Request.Cookies["X-Permissions"];

            return Ok(new
            {
                message = "Permisos obtenidos exitosamente",
                data = new
                {
                    Headers = headers,
                    Cookies = cookies,
                    HasJwt = !string.IsNullOrEmpty(jwt),
                    HasPermissionCookie = !string.IsNullOrEmpty(permissionCookie),
                    RawJwt = jwt,
                    RawPermissionCookie = permissionCookie
                }
            });
        }

        [HttpPut("change-pfp/{id}")]
        public async Task<ActionResult> ChangePfp(int id, IFormFile file)
        {
            await _usuarioService.UpdatePfp(id, file);
            return Accepted(new { message = "Foto de perfil actualizada exitosamente" });
        }

        [HttpPut("change-cv/{id}")]
        public async Task<ActionResult> ChangeCv(int id, IFormFile file)
        {
            await _usuarioService.UpdateCv(id, file);
            return Accepted(new { message = "CV actualizado exitosamente" });
        }

        [HttpPost("request-password-change")]
        public async Task<ActionResult> RequestPasswordChange([FromBody] string email)
        {
            return Ok(new { message = "Solicitud de cambio de contraseña enviada" });
        }

        [HttpPut("change-password-byadmin")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            await _usuarioService.ChangePassword(changePasswordDTO);
            return Accepted(new { message = "Contraseña actualizada exitosamente" });
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] string newPassword)
        {
            await _usuarioService.ChangePassword(newPassword);
            return Accepted(new { message = "Contraseña actualizada exitosamente" });
        }

        [HttpGet("testMocks")]
        public async Task<ActionResult> Test(string? seccion, string? asignatura)
        {
            var mock = new INTECServiceMock();
            var evidencias = await mock.GetProfesores(seccion, asignatura);
            return Ok(new { message = "Datos de prueba obtenidos exitosamente", data = evidencias });
        }


        [HttpGet("testFehcas")]
        public async Task<ActionResult> TestFechas()
        {
            var trimestre = Trimestre.NoviembreEnero;
            var (inicio, fin) = trimestre.GetFechas(2024);

            return Ok(new { message = "Datos de prueba obtenidos exitosamente", data = $"Trimestre actual: {inicio:d} - {fin:d}" });
        }
    }
}

using AvaluoAPI.Domain.Services.UsuariosService;
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
        public UsuariosController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int? estado, int? area, int? rol)
        {
            return Ok(await _usuarioService.GetAll(estado,area,rol));
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _usuarioService.GetById(id));
        }

        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioDTO usuarioDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _usuarioService.Register(usuarioDTO);
            return CreatedAtAction(nameof(Post), new { id = usuarioDTO.Id }, usuarioDTO);
        }

        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            return Ok(await _usuarioService.Login(loginDTO));
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            return Ok("logout");
        }

        [HttpPut("desactivate/{id}")]
        public async Task<ActionResult> Desactivate(int id)
        {
            await _usuarioService.Desactivate(id);
            return Accepted("usuario desactivado");
        }

        [HttpPut("activate/{id}")]
        public async Task<ActionResult> Activate(int id)
        {
            await _usuarioService.Activate(id);
            return Accepted("usuario activado");
        }

        [HttpGet("test-permissions")]
        public IActionResult TestPermissions()
        {
            // Debug: Ver todos los headers
            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            // Debug: Ver todas las cookies
            var cookies = Request.Cookies.ToDictionary(c => c.Key, c => c.Value);

            var jwt = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var permissionCookie = Request.Cookies["X-Permissions"];

            return Ok(new
            {
                Headers = headers,
                Cookies = cookies,
                HasJwt = !string.IsNullOrEmpty(jwt),
                HasPermissionCookie = !string.IsNullOrEmpty(permissionCookie),
                RawJwt = jwt,
                RawPermissionCookie = permissionCookie
            });
        }


    }
}

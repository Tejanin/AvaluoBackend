using AvaluoAPI.Domain.Services.UsuariosService;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;



namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult Post([FromBody] UsuarioDTO usuarioDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _usuarioService.Register(usuarioDTO);
            return CreatedAtAction(nameof(Post), new { id = usuarioDTO.Id }, usuarioDTO);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

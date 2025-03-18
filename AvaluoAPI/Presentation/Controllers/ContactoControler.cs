using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.ContactoDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var contactos = await _unitOfWork.Contactos.GetContactosByUserId(userId);
            return Ok(contactos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactoDTO contactoDTO)
        {
            var contacto = new Contacto
            {
                NumeroContacto = contactoDTO.NumeroContacto,
                IdUsuario = contactoDTO.IdUsuario
            };

            await _unitOfWork.Contactos.AddAsync(contacto);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(GetByUserId), new { userId = contacto.IdUsuario }, contacto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContactoDTO contactoDTO)
        {
            var contacto = await _unitOfWork.Contactos.GetByIdAsync(id);
            if (contacto == null)
                return NotFound("Contacto no encontrado");

            contacto.NumeroContacto = contactoDTO.NumeroContacto;
            await _unitOfWork.Contactos.Update(contacto);
            _unitOfWork.SaveChanges();

            return Ok(new { mensaje = "Contacto actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contacto = await _unitOfWork.Contactos.GetByIdAsync(id);
            if (contacto == null)
                return NotFound("Contacto no encontrado");

            _unitOfWork.Contactos.Delete(contacto);
            _unitOfWork.SaveChanges();

            return Ok(new { mensaje = "Contacto eliminado correctamente." });
        }
    }
}

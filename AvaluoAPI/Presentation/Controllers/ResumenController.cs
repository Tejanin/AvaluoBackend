using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ResumenController : ControllerBase
{
    private readonly IResumenRedisService _redisService;
    private const string CLAVE_RESUMENES = "resumenes:todos";

    public ResumenController(IResumenRedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resumenes = await _redisService.GetResumenListAsync(CLAVE_RESUMENES);
        return Ok(resumenes);
    }

    [HttpGet("{idPI}")]
    public async Task<IActionResult> GetById(int idPI)
    {
        var resumenes = await _redisService.GetResumenListAsync(CLAVE_RESUMENES);
        var resumen = resumenes.Find(r => r.IdPI == idPI);

        if (resumen == null)
            return NotFound($"No se encontró resumen con IdPI: {idPI}");

        return Ok(resumen);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResumenDTO resumen)
    {
        var resumenes = await _redisService.GetResumenListAsync(CLAVE_RESUMENES);

        // Verificar si ya existe un resumen con el mismo IdPI
        if (resumenes.Exists(r => r.IdPI == resumen.IdPI))
            return BadRequest($"Ya existe un resumen con IdPI: {resumen.IdPI}");

        resumenes.Add(resumen);
        bool result = await _redisService.SaveResumenListAsync(CLAVE_RESUMENES, resumenes);

        if (!result)
            return StatusCode(500, "Error al guardar en Redis");

        return CreatedAtAction(nameof(GetById), new { idPI = resumen.IdPI }, resumen);
    }

    [HttpPut("{idPI}")]
    public async Task<IActionResult> Update(int idPI, ResumenDTO resumen)
    {
        if (idPI != resumen.IdPI)
            return BadRequest("El IdPI en la URL no coincide con el del objeto");

        var resumenes = await _redisService.GetResumenListAsync(CLAVE_RESUMENES);
        int index = resumenes.FindIndex(r => r.IdPI == idPI);

        if (index == -1)
            return NotFound($"No se encontró resumen con IdPI: {idPI}");

        resumenes[index] = resumen;
        bool result = await _redisService.SaveResumenListAsync(CLAVE_RESUMENES, resumenes);

        if (!result)
            return StatusCode(500, "Error al actualizar en Redis");

        return Ok(resumen);
    }

    [HttpPatch("calificacion/{idPI}/{matricula}/{calificacion}")]
    public async Task<IActionResult> UpdateCalificacion(int idPI, string matricula, int calificacion)
    {
        if (calificacion < 1 || calificacion > 5)
            return BadRequest("La calificación debe estar entre 1 y 5");

        bool result = await _redisService.UpdateEstudianteCalificacionAsync(
            CLAVE_RESUMENES, idPI, matricula, calificacion);

        if (!result)
            return NotFound("No se encontró el resumen o estudiante especificado");

        return Ok(new { message = "Calificación actualizada correctamente" });
    }

    [HttpDelete("{idPI}")]
    public async Task<IActionResult> Delete(int idPI)
    {
        var resumenes = await _redisService.GetResumenListAsync(CLAVE_RESUMENES);
        int index = resumenes.FindIndex(r => r.IdPI == idPI);

        if (index == -1)
            return NotFound($"No se encontró resumen con IdPI: {idPI}");

        resumenes.RemoveAt(index);
        bool result = await _redisService.SaveResumenListAsync(CLAVE_RESUMENES, resumenes);

        if (!result)
            return StatusCode(500, "Error al eliminar de Redis");

        return NoContent();
    }
}
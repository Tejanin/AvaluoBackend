using AvaluoAPI.Domain;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisTestController : ControllerBase
    {
        private readonly IResumenRedisService _redisService;

        public RedisTestController(IResumenRedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpGet("Prueba")]

        public IActionResult Get() 
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect("localhost:6379");
                var db = redis.GetDatabase();
                bool pingSuccess = db.Ping().TotalSeconds < 3;
               return Ok($"Conexión a Redis exitosa: {pingSuccess}");
            }
            catch (Exception ex)
            {
                return Ok($"Error de conexión: {ex.Message}");
            }
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // 1. Crear datos de prueba
                var estudiantes = new List<EstudianteDTO>
                {
                    new EstudianteDTO { Matricula = "TEST001", Nombre = "Estudiante", Apellido = "De Prueba", Calificacion = 5 }
                };

                var resumen = new ResumenDTO
                {
                    IdPI = 999,
                    Estudiantes = estudiantes
                };

                var resumenes = new List<ResumenDTO> { resumen };

                // 2. Guardar en Redis
                string testKey = "test:connection:" + DateTime.Now.Ticks;
                bool saveResult = await _redisService.SaveResumenListAsync(testKey, resumenes);

                if (!saveResult)
                {
                    return StatusCode(500, new { success = false, message = "Error al guardar en Redis" });
                }

                // 3. Leer de Redis
                var retrievedData = await _redisService.GetResumenListAsync(testKey);

                if (retrievedData == null || retrievedData.Count == 0)
                {
                    return StatusCode(500, new { success = false, message = "Se guardó en Redis pero no se pudo recuperar" });
                }

                // 4. Limpiar datos de prueba
                await _redisService.DeleteResumenAsync(testKey);

                // 5. Devolver resultado exitoso con información
                return Ok(new
                {
                    success = true,
                    message = "Conexión a Redis exitosa",
                    details = new
                    {
                        savedKey = testKey,
                        savedData = resumen,
                        retrievedData = retrievedData[0]
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al conectar con Redis",
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { success = true, message = "Controlador Redis funcionando", timestamp = DateTime.Now });
        }

        [HttpPost("save-test-data")]
        public async Task<IActionResult> SaveTestData()
        {
            try
            {
                // Crear datos de prueba más extensos
                List<ResumenDTO> resumenes = new List<ResumenDTO>();

                for (int i = 1; i <= 3; i++)
                {
                    var estudiantes = new List<EstudianteDTO>();

                    for (int j = 1; j <= 3; j++)
                    {
                        estudiantes.Add(new EstudianteDTO
                        {
                            Matricula = $"TEST{i}{j:D2}",
                            Nombre = $"Nombre{j}",
                            Apellido = $"Apellido{j}",
                            Calificacion = j % 5 + 1
                        });
                    }

                    resumenes.Add(new ResumenDTO
                    {
                        IdPI = i,
                        Estudiantes = estudiantes
                    });
                }

                // Guardar en Redis con una clave fácil de recordar
                string testKey = "rubrica:test:resumenes";
                bool saveResult = await _redisService.SaveResumenListAsync(testKey, resumenes);

                return Ok(new
                {
                    success = saveResult,
                    message = saveResult ? "Datos de prueba guardados correctamente" : "Error al guardar datos de prueba",
                    key = testKey,
                    count = resumenes.Count,
                    data = resumenes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("get-test-data")]
        public async Task<IActionResult> GetTestData()
        {
            try
            {
                string testKey = "rubrica:test:resumenes";
                var resumenes = await _redisService.GetResumenListAsync(testKey);

                if (resumenes == null || resumenes.Count == 0)
                {
                    return NotFound(new { success = false, message = "No se encontraron datos de prueba" });
                }

                return Ok(new
                {
                    success = true,
                    count = resumenes.Count,
                    data = resumenes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
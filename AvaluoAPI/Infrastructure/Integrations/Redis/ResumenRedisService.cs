using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using StackExchange.Redis;

public interface IResumenRedisService
{
    Task<bool> SaveResumenAsync(string key, ResumenDTO resumen);
    Task<bool> SaveResumenListAsync(string key, List<ResumenDTO> resumenes);
    Task<ResumenDTO> GetResumenAsync(string key);
    Task<List<ResumenDTO>> GetResumenListAsync(string key);
    Task<bool> UpdateResumenAsync(string key, ResumenDTO resumen);
    Task<bool> UpdateEstudianteCalificacionAsync(string resumenKey, int idPI, string matricula, int nuevaCalificacion);
    Task<bool> DeleteResumenAsync(string key);
    Task<bool> FlushAllAsync();
}

public class ResumenRedisService : IResumenRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public ResumenRedisService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
    {
        _cache = cache;
        _connectionMultiplexer = connectionMultiplexer;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            SlidingExpiration = TimeSpan.FromMinutes(60)
        };
    }

    public async Task<bool> SaveResumenAsync(string key, ResumenDTO resumen)
    {
        try
        {
            string serializedResumen = JsonSerializer.Serialize(resumen);
            await _cache.SetStringAsync(key, serializedResumen, _options);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> FlushAllAsync()
    {
        try
        {
            // Obtener el servidor Redis
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

            // Ejecutar el comando FLUSHDB para borrar todos los datos de la base de datos actual
            await server.ExecuteAsync("FLUSHDB");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al limpiar Redis: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> SaveResumenListAsync(string key, List<ResumenDTO> resumenes)
    {
        try
        {
            string serializedResumenes = JsonSerializer.Serialize(resumenes);
            await _cache.SetStringAsync(key, serializedResumenes, _options);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ResumenDTO> GetResumenAsync(string key)
    {
        string cachedResumen = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(cachedResumen))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ResumenDTO>(cachedResumen);
    }

    public async Task<List<ResumenDTO>> GetResumenListAsync(string key)
    {
        string cachedResumenes = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(cachedResumenes))
        {
            return new List<ResumenDTO>();
        }

        return JsonSerializer.Deserialize<List<ResumenDTO>>(cachedResumenes);
    }

    public async Task<bool> UpdateResumenAsync(string key, ResumenDTO resumen)
    {
        var lista = await GetResumenListAsync(key);

        // Si estamos actualizando un solo resumen específico por su propia clave
        if (await GetResumenAsync(key) != null)
        {
            return await SaveResumenAsync(key, resumen);
        }

        // Si estamos actualizando un resumen dentro de una lista
        var index = lista.FindIndex(r => r.IdPI == resumen.IdPI);
        if (index != -1)
        {
            lista[index] = resumen;
            return await SaveResumenListAsync(key, lista);
        }

        return false;
    }

    public async Task<bool> UpdateEstudianteCalificacionAsync(string resumenKey, int idPI, string matricula, int nuevaCalificacion)
    {
        var resumenes = await GetResumenListAsync(resumenKey);
        if (resumenes == null || resumenes.Count == 0)
        {
            return false;
        }

        var resumen = resumenes.FirstOrDefault(r => r.IdPI == idPI);
        if (resumen == null)
        {
            return false;
        }

        var estudiante = resumen.Estudiantes.FirstOrDefault(e => e.Matricula == matricula);
        if (estudiante == null)
        {
            return false;
        }

        estudiante.Calificacion = nuevaCalificacion;
        return await SaveResumenListAsync(resumenKey, resumenes);
    }

    public async Task<bool> DeleteResumenAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
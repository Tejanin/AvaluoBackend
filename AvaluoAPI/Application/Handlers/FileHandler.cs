using AvaluoAPI.Utilities;


namespace AvaluoAPI.Application.Handlers
{
    public class FileHandler
    {
        private readonly long MaxSize = 1024 * 1024 * 10;
        private readonly string BasePath;

        private readonly IWebHostEnvironment _hostEnvironment;
        public FileHandler(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            string desktopPath =_hostEnvironment.WebRootPath;
            string baseFolder = "AvaluoFiles";

            // Convertimos las backslashes a forward slashes para evitar problemas con JSON
            BasePath = Path.Combine(desktopPath, baseFolder).Replace("\\", "/");

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            
        }

        public async Task<(bool exitoso, string mensaje, string ruta, string nombreArchivo)> Upload(
                IFormFile archivo,
                List<string> extensionesValidas,
                IRutaBuilder rutaBuilder,
                Func<string, string> generadorNombreArchivo = null)
        {
            try
            {
                var validacionArchivo = ValidarArchivo(archivo, extensionesValidas);
                if (!validacionArchivo.exitoso)
                    return (false, validacionArchivo.mensaje, null, null);

                string nombreArchivo = GenerarNombreArchivo(archivo, generadorNombreArchivo);
                var carpetaDestino = rutaBuilder.Construir();
                var (rutaDirectorio, rutaCompleta) = GenerarRutas(carpetaDestino, nombreArchivo);
                var validacionDirectorio = ValidarYCrearDirectorio(rutaDirectorio);
                if (!validacionDirectorio.exitoso)
                    return (false, validacionDirectorio.mensaje, null, null);

                var manejoArchivoDuplicado = ManejarArchivoDuplicado(rutaCompleta, nombreArchivo);
                rutaCompleta = manejoArchivoDuplicado.rutaFinal;
                nombreArchivo = manejoArchivoDuplicado.nombreFinal;

                await GuardarArchivo(archivo, rutaCompleta);
                string rutaRelativa = Path.Combine(carpetaDestino, nombreArchivo);

                return (true, "Archivo subido exitosamente", rutaCompleta, nombreArchivo);
            }
            catch (Exception ex)
            {
                return (false, $"Error al subir el archivo: {ex.Message}", null, null);
            }
        }

        private (bool exitoso, string mensaje) ValidarArchivo(IFormFile archivo, List<string> extensionesValidas)
        {
            if (archivo == null || archivo.Length == 0)
                return (false, "No se ha seleccionado ningún archivo");

            string extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

            if (!extensionesValidas.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return (false, $"Tipo de archivo no válido. Extensiones permitidas: {string.Join(", ", extensionesValidas)}");

            if (archivo.Length > MaxSize)
                return (false, $"El archivo excede el tamaño máximo permitido de 10MB");

            return (true, string.Empty);
        }

        private string GenerarNombreArchivo(IFormFile archivo, Func<string, string> generadorNombreArchivo)
        {
            string extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

            if (generadorNombreArchivo != null)
            {
                return generadorNombreArchivo(archivo.FileName) + extension;
            }

            return $"{Path.GetFileNameWithoutExtension(archivo.FileName)}_{DateTime.Now:yyyyMMddHHmmss}";
        }

        private (string rutaDirectorio, string rutaCompleta) GenerarRutas(string carpetaDestino, string nombreArchivo)
        {
            string rutaDirectorio = Path.Combine(BasePath, carpetaDestino);
            string rutaCompleta = Path.Combine(rutaDirectorio, nombreArchivo);
            return (rutaDirectorio, rutaCompleta);
        }

        private (bool exitoso, string mensaje) ValidarYCrearDirectorio(string rutaDirectorio)
        {
            if (!Directory.Exists(rutaDirectorio))
            {
                try
                {
                    Directory.CreateDirectory(rutaDirectorio);
                }
                catch (UnauthorizedAccessException)
                {
                    return (false, "No tiene permisos para crear el directorio");
                }
                catch (Exception ex)
                {
                    return (false, $"Error al crear el directorio: {ex.Message}");
                }
            }
            return (true, string.Empty);
        }

        private (string rutaFinal, string nombreFinal) ManejarArchivoDuplicado(string rutaOriginal, string nombreOriginal)
        {
            if (!File.Exists(rutaOriginal))
                return (rutaOriginal, nombreOriginal);

            string directorio = Path.GetDirectoryName(rutaOriginal);
            string extension = Path.GetExtension(nombreOriginal);
            string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreOriginal);

            int contador = 1;
            string nuevoNombre;
            string nuevaRuta;

            do
            {
                nuevoNombre = $"{nombreSinExtension}_{contador}{extension}";
                nuevaRuta = Path.Combine(directorio, nuevoNombre);
                contador++;
            } while (File.Exists(nuevaRuta));

            return (nuevaRuta, nuevoNombre);
        }

        private async Task GuardarArchivo(IFormFile archivo, string rutaCompleta)
        {
            using var stream = new FileStream(rutaCompleta, FileMode.Create);
            await archivo.CopyToAsync(stream);
        }
    }
}

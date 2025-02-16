using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Infrastructure.Integrations.Moodle.Mock;
using AvaluoAPI.Infrastructure.Integrations.Moodle.Models;

namespace AvaluoAPI.Infrastructure.Integrations.Moodle
{

    public interface IMoodleService
    {
        Task<ListaDeEvidenciasMoodleModel?> GetEvidenciasByAsignaturaAndSeccion(string codigoAsignatura, string seccion);
        Task<IEnumerable<ListaDeEvidenciasMoodleModel>> GetAllEvidencias();
        
    }
    public class MoodleServiceMock : IMoodleService
    {
        private readonly List<ListaDeEvidenciasMoodleModel> _evidencias;

        public MoodleServiceMock()
        {
            var mock = new MoodleMock();
            _evidencias = mock.GetMockData(); // Aquí generamos los datos mock al inicializar
        }

        public async Task<ListaDeEvidenciasMoodleModel?> GetEvidenciasByAsignaturaAndSeccion(string codigoAsignatura, string seccion)
        {
            // Simulamos una operación asíncrona
            await Task.Delay(100);

            return _evidencias.FirstOrDefault(e =>
                e.CodigoAsignatura == codigoAsignatura &&
                e.Seccion == seccion);
        }

        public async Task<IEnumerable<ListaDeEvidenciasMoodleModel>> GetAllEvidencias()
        {
            await Task.Delay(100); // Simulamos latencia
            return _evidencias;
        }
    }
}

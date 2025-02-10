namespace AvaluoAPI.Utilities
{
    public interface IRutaBuilder
    {
        string Construir();
    }

    // Para Syllabus y Programas
    public class RutaAsignaturaBuilder : IRutaBuilder
    {
        private readonly string _codigoAsignatura;
        private readonly string _tipoDocumento; // "Syllabus" o "Programa"

        public RutaAsignaturaBuilder(string codigoAsignatura, string tipoDocumento)
        {
            _codigoAsignatura = codigoAsignatura ?? throw new ArgumentNullException(nameof(codigoAsignatura));
            _tipoDocumento = tipoDocumento ?? throw new ArgumentNullException(nameof(tipoDocumento));
        }

        public string Construir()
        {
            return Path.Combine("asignaturas", _codigoAsignatura, _tipoDocumento);
        }
    }

    // Para archivos de usuario
    public class RutaUsuarioBuilder : IRutaBuilder
    {
        private readonly string _identificadorUsuario;

        public RutaUsuarioBuilder(string identificadorUsuario)
        {
            _identificadorUsuario = identificadorUsuario ?? throw new ArgumentNullException(nameof(identificadorUsuario));
        }

        public string Construir()
        {
            return Path.Combine("usuarios", _identificadorUsuario);
        }
    }

    // Para informes
    public class RutaInformeBuilder : IRutaBuilder
    {
        private readonly string _tipoReporte;
        private readonly string _año;

        public RutaInformeBuilder(string tipoReporte, string año)
        {
            _tipoReporte = tipoReporte ?? throw new ArgumentNullException(nameof(tipoReporte));
            _año = año ?? throw new ArgumentNullException(nameof(año));
        }

        public string Construir()
        {
            return Path.Combine("informes", _tipoReporte, _año);
        }
    }

    // Para evaluaciones
    public class RutaEvaluacionBuilder : IRutaBuilder
    {
        private readonly string _año;
        private readonly string _trimestre;
        private readonly bool _esEvidencia;

        public RutaEvaluacionBuilder(string año, string trimestre, bool esEvidencia = false)
        {
            _año = año ?? throw new ArgumentNullException(nameof(año));
            _trimestre = trimestre ?? throw new ArgumentNullException(nameof(trimestre));
            _esEvidencia = esEvidencia;
        }

        public string Construir()
        {
            var partes = new List<string> { "evaluaciones", _año, _trimestre };
            if (_esEvidencia)
            {
                partes.Add("Evidencias");
            }
            return Path.Combine(partes.ToArray());
        }
    }
}

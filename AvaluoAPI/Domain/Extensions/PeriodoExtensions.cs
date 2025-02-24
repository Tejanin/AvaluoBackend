namespace AvaluoAPI.Domain.Helper
{
    public enum Trimestre
    {
        FebreroAbril = 1,
        MayoJulio = 2,
        AgostoOctubre = 3,
        NoviembreEnero = 4,
    
    }

    public static class PeriodoExtensions
    {
        public static (DateTime FechaInicio, DateTime FechaFin) GetFechasAñoUniversitario()
        {
            var (_, añoUniversitario) = ObtenerTrimestreActual();
            return (new DateTime(añoUniversitario, 2, 1), new DateTime(añoUniversitario + 1, 1, 31));
        }

        public static (DateTime FechaInicio, DateTime FechaFin) GetFechasTrimestreActual()
        {
            var (trimestreActual, añoUniversitario) = ObtenerTrimestreActual();
            var trimestre = (Trimestre)trimestreActual;

            return trimestre switch
            {
                Trimestre.FebreroAbril => (new DateTime(añoUniversitario, 2, 1), new DateTime(añoUniversitario, 4, 30)),
                Trimestre.MayoJulio => (new DateTime(añoUniversitario, 5, 1), new DateTime(añoUniversitario, 7, 31)),
                Trimestre.AgostoOctubre => (new DateTime(añoUniversitario, 8, 1), new DateTime(añoUniversitario, 10, 31)),
                Trimestre.NoviembreEnero => (new DateTime(añoUniversitario, 11, 1), new DateTime(añoUniversitario + 1, 1, 31)),
                
                _ => throw new ArgumentException("Trimestre no válido")
            };
        }

        public static (DateTime FechaInicio, DateTime FechaFin) GetFechas(this Trimestre trimestre)
        {
            var (_, añoUniversitario) = ObtenerTrimestreActual();
            return GetFechas(trimestre, añoUniversitario);
        }

        // Sobrecarga que permite especificar el año
        public static (DateTime FechaInicio, DateTime FechaFin) GetFechas(this Trimestre trimestre, int añoUniversitario)
        {
            return trimestre switch
            {
                Trimestre.FebreroAbril => (new DateTime(añoUniversitario, 2, 1), new DateTime(añoUniversitario, 4, 30)),
                Trimestre.MayoJulio => (new DateTime(añoUniversitario, 5, 1), new DateTime(añoUniversitario, 7, 31)),
                Trimestre.AgostoOctubre => (new DateTime(añoUniversitario, 8, 1), new DateTime(añoUniversitario, 10, 31)),
                Trimestre.NoviembreEnero => (new DateTime(añoUniversitario, 11, 1), new DateTime(añoUniversitario + 1, 1, 31)),
                
                _ => throw new ArgumentException("Trimestre no válido")
            };
        }


        public static (int Trimestre, int AñoUniversitario) ObtenerTrimestreActual()
        {
            var fechaActual = DateTime.Now;
            var mes = fechaActual.Month;

            // Determinar el año universitario
            int añoUniversitario = mes == 1 ? fechaActual.Year - 1 : fechaActual.Year;

            // Determinar el trimestre
            int trimestre = mes switch
            {
                2 or 3 or 4 => (int)Trimestre.FebreroAbril,
                5 or 6 or 7 => (int)Trimestre.MayoJulio,
                8 or 9 or 10 => (int)Trimestre.AgostoOctubre,
                11 or 12 or 1 => (int)Trimestre.NoviembreEnero,
                _ => throw new ArgumentException("Mes no válido")
            };

            return (trimestre, añoUniversitario);
        }

        public static bool EstaEnPeriodoActual(this Trimestre trimestre, int añoUniversitario)
        {
            var (trimestreActual, añoActual) = ObtenerTrimestreActual();
            return trimestre == (Trimestre)trimestreActual && añoUniversitario == añoActual;
        }

        public static string GetNombreTrimestre(string trimestre)
        {
            return trimestre switch
            {
                "1" => "Febrero - Abril",
                "2" => "Mayo - Julio",
               "3" => "Agosto - Octubre",
                "4" => "Noviembre - Enero",
                _ => throw new ArgumentException("Trimestre no válido")
            };
        }
    }
}

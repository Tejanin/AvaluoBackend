using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.DTOs.InformeDTOs
{
    public class InformeDTO
    {
        public string Ruta { get; set; }
        public string Nombre { get; set; }
        public int IdTipo { get; set; }
        public int IdCarrera { get; set; }
        public int Año { get; set; }
        public char Trimestre { get; set; }
        public string Periodo { get; set; }
    }
}
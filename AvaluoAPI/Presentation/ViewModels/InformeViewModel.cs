﻿using Avaluo.Infrastructure.Data.Models;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class InformeViewModel
    {
        public int Id { get; set; }
        public string Ruta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Carrera { get; set; }
        public int Año { get; set; }
        public char Trimestre { get; set; }
        public string Periodo { get; set; }
    }
}

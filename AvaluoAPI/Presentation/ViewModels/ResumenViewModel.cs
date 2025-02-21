﻿using AvaluoAPI.Presentation.DTOs.RubricaDTOs;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class ResumenViewModel
    {
        
        public int IdPI { get; set; }
        public int CantExperto { get; set; }
        public int CantSatisfactorio { get; set; }
        public int CantPrincipiante { get; set; }
        public int CantDesarrollo { get; set; }
    }
}

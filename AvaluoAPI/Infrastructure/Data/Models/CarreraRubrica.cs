﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class CarreraRubrica
    {
        public int IdRubrica { get; set; }
        public int IdCarrera { get; set; }
        public virtual Rubrica Rubrica { get; set; }
        public virtual Carrera Carrera { get; set; }
    }
}

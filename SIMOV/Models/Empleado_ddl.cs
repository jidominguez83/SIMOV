using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class Empleado_ddl
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "RFC")]
        public string rfc { get; set; }

        [Display(Name = "Nombre")]
        public string nombre_completo { get; set; }
    }
}
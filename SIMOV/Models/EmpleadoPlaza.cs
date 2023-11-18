using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class EmpleadoPlaza
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "rfc")]
        public string rfc { get; set; }

        [Display(Name = "Plazas")]
        public List<Plaza>  plazas { get; set; }

    }
}
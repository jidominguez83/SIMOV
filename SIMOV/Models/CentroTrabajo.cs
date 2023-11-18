using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class CentroTrabajo
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "ct")]
        public string ct { get; set; }
    }
}
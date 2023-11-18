using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class bit_captura
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "IdCaptura")]
        public int IdCaptura { get; set; }

        [Display(Name = "Estatus Operacion")]
        public int EstatusOp { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Operacion")]
        public string Operacion { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }


    }
}
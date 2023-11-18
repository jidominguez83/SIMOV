using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class motivos_m
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Motivo")]
        public string motivo_mov { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class tipo_mov
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "cve_mov")]
        public int cve_mov { get; set; }

        [Display(Name = "Motivo Mov Cap")]
        public int mot_movcap { get; set; }

        [Display(Name = "Dias solicitud")]
        public int DiasSolicitud { get; set; }

        [Display(Name = "personalderecho")]
        public string Personalderecho { get; set; }

        [Display(Name = "Requisitos")]
        public string Requisitos { get; set; }

        [Display(Name = "Duracion")]
        public string Duracion { get; set; }

        [Display(Name = "Prorroga")]
        public string Prorroga { get; set; }

        [Display(Name = "Base Legal")]
        public string BaseLegal { get; set; }

        [Display(Name = "Doc solicitud")]
        public string DocSolicitud { get; set; }

        [Display(Name = "Estatus")]
        public string Estatus { get; set; }
    }
}
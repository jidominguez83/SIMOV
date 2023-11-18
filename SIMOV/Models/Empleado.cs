using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class Empleado
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nombre Completo")]
        public string Nombre_Completo { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Apellido Paterno")]
        public string Apellido_Paterno { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Apellido Materno")]
        public string Apellido_Materno { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "RFC")]
        [MaxLength(13)]
        public string RFC { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CURP")]
        [MaxLength(18)]
        public string CURP { get; set; }


    }
}
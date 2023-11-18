using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class Plaza
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Plaza")]
        public string _Plaza { get; set; }

        [Display(Name = "CodPago")]
        public int Codpago { get; set; }

        [Display(Name = "Unidad")]
        public int Unidad { get; set; }

        
        public string check { get; set; }


        [Display(Name = "Subunidad")]
        public int Subunidad { get; set; }

        [Display(Name = "Categoria Puesto")]
        public string CatPuesto { get; set; }

        [Display(Name = "Horas")]
        public double Horas { get; set; }

        [Display(Name = "Consecutivo Plaza")]
        public int ConsPlaza { get; set; }

        [Display(Name = "Centro de Trabajo")]
        public string CTrabajo { get; set; }

      
        public string antiguedad { get; set; }

    }
   
}
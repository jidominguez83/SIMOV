using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class tipo_doc
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "documento")]
        public string Documento { get; set; }
    }
}
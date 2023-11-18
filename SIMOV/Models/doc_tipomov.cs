using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class doc_tipomov
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "IdDocumento")]
        public int IdDoc { get; set; }

        [Display(Name = "id_tipo_doc")]
        public int id_tipo_doc { get; set; }

        [Display(Name = "Estatus")]
        public int Estatus { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class documentos_cap
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Id_Captura")]
        public int Id_Captura { get; set; }

        [Display(Name = "id_doc_tipodoc")]
        public int id_doc_tipodoc { get; set; }

        [Display(Name = "path")]
        public string path { get; set; }

        [Display(Name = "fecha")]
        public DateTime fecha { get; set; }

        [Display(Name = "Estatus")]
        public int Estatus { get; set; }
    }
}
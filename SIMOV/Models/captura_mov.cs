using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIMOV.Models
{
    public class captura_mov
    {
        
        [Display(Name = "Centro de Trabajo")]
        public List<CentroTrabajo> cts { get; set; }

        public String ct { get; set; }

        
        [Display(Name = "Movimiento")]
        public List<cve_movs> cve_movs { get; set; }

        public int id_cve_mov { get; set; }

        [Display(Name = "Movimiento")]
        public string cve_movs_ed { get; set; }

        public int id_antiguedad { get; set; }

        [Display(Name = "Motivo")]
        public List<motivos_m> mot_mov { get; set; }

        [Display(Name = "Motivo")]
        public string mot_mov_ed { get; set; }

        public int id_tipo_mov { get; set; }

        [Display(Name = "Docente/Administrativo")]
        public List<Empleado_ddl> empleados { get; set; }

        public int id_empleado { get; set; }

        [Display(Name = "Plazas")]
        public List<Emp_Plaza> plazas { get; set; }

        public IEnumerable<Emp_Plaza> Resultado { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime qna_ini { get; set; }

        //[Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime qna_fin { get; set; }

        [Display(Name = "Solicitud firmada")]
        public HttpPostedFileBase Solicitud { get; set; }

        [Display(Name = "Documento Oficial (INE)")]
        public HttpPostedFileBase Ine { get; set; }


        [ScaffoldColumn(false)]
        public int IdCaptura { get; set; }

        [Display(Name = "Area")]
        public string area { get; set; }

        [Display(Name = "rfc")]
        public string rfc { get; set; }

        [Display(Name = "Fecha_captura")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Fecha_captura { get; set; }

        [Display(Name = "Fecha_alerta")]
        public DateTime Fecha_alerta { get; set; }

        [Display(Name = "Fecha_envio")]
        public DateTime Fecha_envio { get; set; }

        [Display(Name = "Fecha_recibido")]
        public DateTime Fecha_recibido { get; set; }

        [Display(Name = "Fecha_cancelacion")]
        public DateTime Fecha_cancelacion { get; set; }

        [Display(Name = "Observacion")]
        public string Observacion { get; set; }

        [Display(Name = "Usuario captura")]
        public string Usuario { get; set; }

        [Display(Name = "Estatus")]
        public int Estatus { get; set; }

        [ScaffoldColumn(false)]
        public int dias { get; set; }
        public string reglamentacion { get; set; }
    }
    public class Emp_Plaza
    {
        public bool index { get; set; }
        public string plaza { get; set; }

    }
}
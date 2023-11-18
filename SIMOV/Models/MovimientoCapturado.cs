using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIMOV.Models
{
    public class MovimientoCapturado
    {
        public int id { get; set; }
        public string rfc { get; set; }
        public string nombre { get; set; }
        public string tipo_movimiento { get; set; }
        public string tipo_motivo { get; set; }
        public short? estatus { get; set; }
    }
}
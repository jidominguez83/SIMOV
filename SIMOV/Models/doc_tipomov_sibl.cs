//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIMOV.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class doc_tipomov_sibl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public doc_tipomov_sibl()
        {
            this.documentos_cap_sibl = new HashSet<documentos_cap_sibl>();
        }
    
        public int id_doc_tipdoc { get; set; }
        public int id_documento { get; set; }
        public int id_tipo { get; set; }
        public short estatus { get; set; }
    
        public virtual tipo_doc_sibl tipo_doc_sibl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<documentos_cap_sibl> documentos_cap_sibl { get; set; }
        public virtual tipo_mov_sibl tipo_mov_sibl { get; set; }
    }
}
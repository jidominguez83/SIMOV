﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AdmSIMOVBL_msEntities : DbContext
    {
        public AdmSIMOVBL_msEntities()
            : base("name=AdmSIMOVBL_msEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<app_usuario_objeto> app_usuario_objeto { get; set; }
        public virtual DbSet<mvc_permisos> mvc_permisos { get; set; }
        public virtual DbSet<mvc_permisos_opciones> mvc_permisos_opciones { get; set; }
        public virtual DbSet<mvc_usaurios> mvc_usaurios { get; set; }
        public virtual DbSet<doc_tipomov_sibl> doc_tipomov_sibl { get; set; }
        public virtual DbSet<tipo_doc_sibl> tipo_doc_sibl { get; set; }
        public virtual DbSet<configuracion_sibl> configuracion_sibl { get; set; }
        public virtual DbSet<cve_mov> cve_mov { get; set; }
        public virtual DbSet<motivos_mov> motivos_mov { get; set; }
        public virtual DbSet<emp_plaza_pmp> emp_plaza_pmp { get; set; }
        public virtual DbSet<hpc_pmp> hpc_pmp { get; set; }
        public virtual DbSet<ct_basico> ct_basico { get; set; }
        public virtual DbSet<empleado_u> empleado_u { get; set; }
        public virtual DbSet<bit_cap_mov_sibl> bit_cap_mov_sibl { get; set; }
        public virtual DbSet<documentos_cap_sibl> documentos_cap_sibl { get; set; }
        public virtual DbSet<plazas_cap_sibl> plazas_cap_sibl { get; set; }
        public virtual DbSet<captura_mov_sibl> captura_mov_sibl { get; set; }
        public virtual DbSet<Configuraciones> Configuraciones { get; set; }
        public virtual DbSet<categorias_areas> categorias_areas { get; set; }
        public virtual DbSet<emp_qna_ing_uid> emp_qna_ing_uid { get; set; }
        public virtual DbSet<areas> areas { get; set; }
        public virtual DbSet<tipo_mov_sibl> tipo_mov_sibl { get; set; }
    }
}
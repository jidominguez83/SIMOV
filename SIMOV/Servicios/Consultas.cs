using Newtonsoft.Json;
using SIMOV.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace SIMOV.Servicios
{
    public class Consultas
    {
        public static List<CentroTrabajo> GetCTPorUsuario(string Usuario)
        {
            List<CentroTrabajo> centroTrabajo = new List<CentroTrabajo>();
           // List<String> Validados1 = new List<String>();
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {
                var cts = db.app_usuario_objeto.SingleOrDefault(ac =>  ac.usuario == Usuario);
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(cts.Obj_Json);
             

                foreach (var item in values)
                {
                    CentroTrabajo ct = new CentroTrabajo(); 
                    ct.Id= Convert.ToInt32(item.Key);
                    ct.ct = item.Value;
                    centroTrabajo.Add(ct);
                   
                }

             
            }

            return centroTrabajo;
        }

        public static List<SelectListItem> GetCTPorUsuario2(string Usuario)
        {
            List<SelectListItem> centroTrabajo = new List<SelectListItem>();
            
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {
                var cts = db.app_usuario_objeto.SingleOrDefault(ac => ac.usuario == Usuario);
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(cts.Obj_Json);


                foreach (var item in values)
                {
                    SelectListItem ct = new SelectListItem();
                    ct.Text = item.Key;
                    ct.Value = item.Value;
                    centroTrabajo.Add(ct);

                }


            }

            return centroTrabajo;
        }

        public static List<cve_movs> GetMovimientos()
        {
            List<cve_movs> movs = new List<cve_movs>();
          
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {

                var datos = (from tip_m in db.tipo_mov_sibl
                             join cve_m in db.cve_mov on tip_m.cve_mov equals cve_m.cve_mov1
                             where tip_m.estatus == 1 

                             select new
                             {
                                 id = tip_m.cve_mov,
                                 descripcion = cve_m.descripcion

                             }).Distinct().ToList();


                foreach (var item in datos)
                {
                    cve_movs m = new cve_movs();
                    m.Id = Convert.ToInt32(item.id);
                    m.cve_mov = item.descripcion;
                    movs.Add(m);

                }
            }

            return movs;
        }

        public static List<motivos_m> GetMotivos_Movimiento(string cve_mov)
        {
            List<motivos_m> movs = new List<motivos_m>();
            if (cve_mov != "Selecciona el Tipo")
            {
                int _cve_mov = Convert.ToInt32(cve_mov);
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {

                    var datos = (from tip_m in db.tipo_mov_sibl
                                 join cve_m in db.cve_mov on tip_m.cve_mov equals cve_m.cve_mov1
                                 join mot in db.motivos_mov on tip_m.mot_movcap equals mot.mot_mov
                                 where tip_m.estatus == 1 && tip_m.cve_mov == _cve_mov

                                 select new
                                 {
                                     id = tip_m.mot_movcap,
                                     descripcion = mot.descripcion

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        motivos_m m = new motivos_m();
                        m.Id = Convert.ToInt32(item.id);
                        m.motivo_mov = "(" + item.id + ")-" + item.descripcion;
                        movs.Add(m);

                    }

                }
            }

            return movs;
        }
        public static List<motivos_m> GetMovimiento(string cve_mov,string mov_mot)
        {
            List<motivos_m> movs = new List<motivos_m>();
            if (cve_mov != "Selecciona el Tipo")
            {
                if (cve_mov == "" || cve_mov== "Selecciona el Tipo")
                {
                    cve_mov = "0";
                }
                if (mov_mot == "" || mov_mot == "Selecciona el Tipo")
                {
                    mov_mot = "0";
                }
                int _cve_mov = Convert.ToInt32(cve_mov);
                int _mov_mot = Convert.ToInt32(mov_mot);
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {

                    var datos = (from tip_m in db.tipo_mov_sibl
                                 join cve_m in db.cve_mov on tip_m.cve_mov equals cve_m.cve_mov1
                                 join mot in db.motivos_mov on tip_m.mot_movcap equals mot.mot_mov
                                 where tip_m.estatus == 1 && tip_m.cve_mov == _cve_mov && tip_m.mot_movcap == _mov_mot

                                 select new
                                 {
                                     id = tip_m.dias_soliditud,
                                     descripcion = tip_m.base_legal,

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        motivos_m m = new motivos_m();
                        m.Id = Convert.ToInt32(item.id);
                        m.motivo_mov =  item.descripcion;
                        movs.Add(m);

                    }

                }
            }

            return movs;
        }

        public static List<motivos_m> GetInformacionMovimiento(string cve_mov, string mov_mot)
        {
            List<motivos_m> movs = new List<motivos_m>();
            if (cve_mov != "Selecciona el Tipo")
            {
                int _cve_mov = Convert.ToInt32(cve_mov);
                int _mov_mot = Convert.ToInt32(mov_mot);
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {

                    var datos = (from tip_m in db.tipo_mov_sibl
                                 join cve_m in db.cve_mov on tip_m.cve_mov equals cve_m.cve_mov1
                                 join mot in db.motivos_mov on tip_m.mot_movcap equals mot.mot_mov
                                 where tip_m.estatus == 1 && tip_m.cve_mov == _cve_mov && tip_m.mot_movcap == _mov_mot

                                 select new
                                 {
                                     id = tip_m.id_tipo,
                                     descripcion = tip_m.requisitos,

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        motivos_m m = new motivos_m();
                        m.Id = Convert.ToInt32(item.id);
                        m.motivo_mov = item.descripcion;
                        movs.Add(m);

                    }

                }
            }

            return movs;
        }
        public static List<Empleado_ddl> GetEmpleados(string ct)
        {
            
            List<Empleado_ddl> empleado = new List<Empleado_ddl>();
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {

                var datos = (from ep in db.emp_plaza_pmp
                             join e_u in db.empleado_u on ep.rfc equals e_u.rfc
                             join hp in db.hpc_pmp on new { ep.cod_pago, ep.unidad, ep.subunidad, ep.cat_puesto, ep.horas, ep.cons_plaza }
                                    equals new { hp.cod_pago, hp.unidad, hp.subunidad, hp.cat_puesto, hp.horas, hp.cons_plaza }
                             join _ct in db.ct_basico on new { hp.ent_fed, hp.ct_clasif, hp.ct_id, hp.ct_secuencial, hp.ct_digito_ver }
                                    equals new { _ct.ent_fed, _ct.ct_clasif, _ct.ct_id, _ct.ct_secuencial, _ct.ct_digito_ver }
                             where  ((ep.estatus_plaza == 1 || ep.estatus_plaza == 6) && ep.qna_fin >= hp.qna_ini && ep.qna_fin <= hp.qna_fin)
                                   && _ct.clave_ct==ct
                             select new
                             {
                                 id= e_u.id,
                                 rfc= ep.rfc,
                                 nombre= e_u.nom_emp,
                                 centro_t= _ct.ct_nombre,
                                
                             }).Distinct().ToList();


                foreach (var item in datos)
                {
                    Empleado_ddl emp_s = new Empleado_ddl();
                    emp_s.Id = Convert.ToInt32(item.id);
                    emp_s.rfc = item.rfc;
                    emp_s.nombre_completo = "(" + item.rfc + ")-" + item.nombre;
                    empleado.Add(emp_s);

                }
               
            }

            return empleado;
        }

        public static List<tipo_doc> GetDocumentos_tipo(int id_tipo)
        {

            List<tipo_doc> tipo = new List<tipo_doc>();
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {

                var datos = (from doc_t in db.doc_tipomov_sibl
                             join t in db.tipo_doc_sibl on doc_t.id_documento equals t.id_documento
                             
                             where doc_t.id_tipo== id_tipo
                             select new
                             {
                                 id = t.id_documento,
                                 documento = t.documento.Trim(),
                                 

                             }).Distinct().ToList();


                foreach (var item in datos)
                {
                    tipo_doc d = new tipo_doc();
                    d.Id = Convert.ToInt32(item.id);
                    d.Documento = item.documento;
                    tipo.Add(d);

                }

            }

            return tipo;
        }
        public static List<Empleado_ddl> GetEmpleado(int id)
        {

            List<Empleado_ddl> empleado = new List<Empleado_ddl>();
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {

                var datos = (from e_u in db.empleado_u 
                               where e_u.id == id
                                  
                             select new
                             {
                                 id = e_u.id,
                                 rfc = e_u.rfc,
                                 nombre = e_u.nom_emp,
                                

                             }).Distinct().ToList();


                foreach (var item in datos)
                {
                    Empleado_ddl emp_s = new Empleado_ddl();
                    emp_s.Id = Convert.ToInt32(item.id);
                    emp_s.rfc = item.rfc;
                    emp_s.nombre_completo = "(" + item.rfc + ")-" + item.nombre;
                    empleado.Add(emp_s);

                }

            }

            return empleado;
        }
        public static List<Plaza> GetPlazas(string _id,string ddlCT)
        {
            List<Plaza> plazas = new List<Plaza>();
            if (_id != "Selecciona el Empleado")
            { 
                int id_empleado= Convert.ToInt32(_id);
       
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                
                    var datos = (from ep in db.emp_plaza_pmp
                                 join e_u in db.empleado_u on ep.rfc equals e_u.rfc
                                 join hp in db.hpc_pmp on new { ep.cod_pago, ep.unidad, ep.subunidad, ep.cat_puesto, ep.horas, ep.cons_plaza }
                                        equals new { hp.cod_pago, hp.unidad, hp.subunidad, hp.cat_puesto, hp.horas, hp.cons_plaza }
                                 join _ct in db.ct_basico on new { hp.ent_fed, hp.ct_clasif, hp.ct_id, hp.ct_secuencial, hp.ct_digito_ver }
                                        equals new { _ct.ent_fed, _ct.ct_clasif, _ct.ct_id, _ct.ct_secuencial, _ct.ct_digito_ver }
                                 join qna in db.emp_qna_ing_uid on e_u.id equals qna.id
                                 where ((ep.estatus_plaza == 1 || ep.estatus_plaza == 6) && ep.qna_fin >= hp.qna_ini && ep.qna_fin <= hp.qna_fin)
                                       && e_u.id == id_empleado && _ct.clave_ct == ddlCT
                                 select new
                                 {
                                     id = e_u.id,
                                     rfc = ep.rfc,
                                     ct= _ct.clave_ct,
                                     nombre = e_u.nom,
                                     centro_t = _ct.ct_nombre,
                                     cod_pago = ep.cod_pago,
                                     unidad = ep.unidad,
                                     subunidad = ep.subunidad,
                                     cat_puesto = ep.cat_puesto,
                                     horas = ep.horas,
                                     cons_plaza = ep.cons_plaza,
                                     ent_fed = hp.ent_fed,
                                     ct_clasif = hp.ct_clasif,
                                     ct_id = hp.ct_id,
                                     ct_secuencial = hp.ct_secuencial,
                                     ct_digito_ver = hp.ct_digito_ver,
                                     qna_ini_funcion = ep.qna_ini,
                                     antiguedad = qna.qna_ing

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        Plaza _plaza = new Plaza();
                        _plaza.Id = Convert.ToInt32(item.id);
                        _plaza._Plaza = ConviertePlaza(item.cod_pago.ToString(), item.unidad.ToString(), item.subunidad.ToString(), item.cat_puesto.ToString(), item.horas.ToString(), item.cons_plaza.ToString());
                        _plaza.Codpago = item.cod_pago;
                        _plaza.Unidad = item.unidad;
                        _plaza.Subunidad = item.subunidad;
                        _plaza.CatPuesto = item.cat_puesto;
                        _plaza.Horas = item.horas;
                        _plaza.ConsPlaza = item.cons_plaza;
                        _plaza.CTrabajo=  item.ct;
                        string cadena = item.antiguedad.ToString();
                        int dia = 0;
                        int mes = 0;
                        int year = Convert.ToInt32(cadena.Substring(0, 4));
                        int qna = Convert.ToInt32(cadena.Substring(4, 2));
                        if (qna == 1 || qna == 3 || qna == 5 || qna == 7 || qna == 9 || qna == 11 ||
                            qna == 13 || qna == 15 || qna == 17 || qna == 19 || qna == 21 || qna == 23)
                        {
                            dia = 1;

                        }
                        else
                        {
                            dia = 15;
                        }
                        if (qna == 1 || qna == 2)
                        {
                            mes = 1;
                        }
                        if (qna == 3 || qna == 4)
                        {
                            mes = 2;
                        }
                        if (qna == 5 || qna == 6)
                        {
                            mes = 3;
                        }
                        if (qna == 7 || qna == 8)
                        {
                            mes = 4;
                        }
                        if (qna == 9 || qna == 10)
                        {
                            mes = 5;
                        }
                        if (qna == 11 || qna == 12)
                        {
                            mes = 6;
                        }
                        if (qna == 13 || qna == 14)
                        {
                            mes = 7;
                        }
                        if (qna == 15 || qna == 16)
                        {
                            mes = 8;
                        }
                        if (qna == 17 || qna == 18)
                        {
                            mes = 9;
                        }
                        if (qna == 19 || qna == 20)
                        {
                            mes = 10;
                        }
                        if (qna == 21 || qna == 22)
                        {
                            mes = 11;
                        }
                        if (qna == 23 || qna == 24)
                        {
                            mes = 12;
                        }

                        DateTime fechaIngreso = new DateTime(year, mes, dia);
                        int anios = DateTime.Today.Year - fechaIngreso.Year;

                        //si el mes es menor restamos un año directamente
                        if (DateTime.Today.Month < fechaIngreso.Month)
                        {
                            --anios;
                        }
                        //sino preguntamos si estamos en el mismo mes, si es el mismo preguntamos si el dia de hoy es menor al de la fecha de nacimiento
                        else if (DateTime.Today.Month == fechaIngreso.Month && DateTime.Today.Day < fechaIngreso.Day)
                        {
                            --anios;
                        }



                        _plaza.antiguedad = anios.ToString();
                        plazas.Add(_plaza);
                    }
                }
            }
            return plazas;
        }

        public static List<Plaza> GetAllPlazas(string _id)
        {
            List<Plaza> plazas = new List<Plaza>();
            if (_id != "Selecciona el Empleado")
            {
                int id_empleado = Convert.ToInt32(_id);

                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {

                    var datos = (from ep in db.emp_plaza_pmp
                                 join e_u in db.empleado_u on ep.rfc equals e_u.rfc
                                 join hp in db.hpc_pmp on new { ep.cod_pago, ep.unidad, ep.subunidad, ep.cat_puesto, ep.horas, ep.cons_plaza }
                                        equals new { hp.cod_pago, hp.unidad, hp.subunidad, hp.cat_puesto, hp.horas, hp.cons_plaza }
                                 join _ct in db.ct_basico on new { hp.ent_fed, hp.ct_clasif, hp.ct_id, hp.ct_secuencial, hp.ct_digito_ver }
                                        equals new { _ct.ent_fed, _ct.ct_clasif, _ct.ct_id, _ct.ct_secuencial, _ct.ct_digito_ver }
                                join qna in db.emp_qna_ing_uid on e_u.id equals qna.id
                                 where ((ep.estatus_plaza == 1 || ep.estatus_plaza == 6) && ep.qna_fin >= hp.qna_ini && ep.qna_fin <= hp.qna_fin)
                                       && e_u.id == id_empleado 
                                 select new
                                 {
                                     id = e_u.id,
                                     rfc = ep.rfc,
                                     ct = _ct.clave_ct,
                                     nombre = e_u.nom,
                                     centro_t = _ct.ct_nombre,
                                     cod_pago = ep.cod_pago,
                                     unidad = ep.unidad,
                                     subunidad = ep.subunidad,
                                     cat_puesto = ep.cat_puesto,
                                     horas = ep.horas,
                                     cons_plaza = ep.cons_plaza,
                                     ent_fed = hp.ent_fed,
                                     ct_clasif = hp.ct_clasif,
                                     ct_id = hp.ct_id,
                                     ct_secuencial = hp.ct_secuencial,
                                     ct_digito_ver = hp.ct_digito_ver,
                                     qna_ini_funcion = ep.qna_ini,
                                     antiguedad = qna.qna_ing

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        Plaza _plaza = new Plaza();
                        _plaza.Id = Convert.ToInt32(item.id);
                        _plaza._Plaza = ConviertePlaza(item.cod_pago.ToString(), item.unidad.ToString(), item.subunidad.ToString(), item.cat_puesto.ToString(), item.horas.ToString(), item.cons_plaza.ToString());
                        _plaza.Codpago = item.cod_pago;
                        _plaza.Unidad = item.unidad;
                        _plaza.Subunidad = item.subunidad;
                        _plaza.CatPuesto = item.cat_puesto;
                        _plaza.Horas = item.horas;
                        _plaza.ConsPlaza = item.cons_plaza;
                        _plaza.CTrabajo = item.ct;
                       // _plaza.antiguedad = item.antiguedad.ToString();

                        string cadena = item.antiguedad.ToString();
                        int dia = 0;
                        int mes = 0;
                        int year = Convert.ToInt32(cadena.Substring(0, 4));
                        int qna = Convert.ToInt32(cadena.Substring(4, 2));
                        if (qna == 1 || qna == 3 || qna == 5 || qna == 7 || qna == 9 || qna == 11 ||
                            qna == 13 || qna == 15 || qna == 17 || qna == 19 || qna == 21 || qna == 23)
                        {
                            dia = 1;

                        }
                        else
                        {
                            dia = 15;
                        }
                        if (qna == 1 || qna == 2)
                        {
                            mes = 1;
                        }
                        if (qna == 3 || qna == 4)
                        {
                            mes = 2;
                        }
                        if (qna == 5 || qna == 6)
                        {
                            mes = 3;
                        }
                        if (qna == 7 || qna == 8)
                        {
                            mes = 4;
                        }
                        if (qna == 9 || qna == 10)
                        {
                            mes = 5;
                        }
                        if (qna == 11 || qna == 12)
                        {
                            mes = 6;
                        }
                        if (qna == 13 || qna == 14)
                        {
                            mes = 7;
                        }
                        if (qna == 15 || qna == 16)
                        {
                            mes = 8;
                        }
                        if (qna == 17 || qna == 18)
                        {
                            mes = 9;
                        }
                        if (qna == 19 || qna == 20)
                        {
                            mes = 10;
                        }
                        if (qna == 21 || qna == 22)
                        {
                            mes = 11;
                        }
                        if (qna == 23 || qna == 24)
                        {
                            mes = 12;
                        }

                        DateTime fechaIngreso = new DateTime(year, mes, dia);
                        int anios = DateTime.Today.Year - fechaIngreso.Year;

                        //si el mes es menor restamos un año directamente
                        if (DateTime.Today.Month < fechaIngreso.Month)
                        {
                            --anios;
                        }
                        //sino preguntamos si estamos en el mismo mes, si es el mismo preguntamos si el dia de hoy es menor al de la fecha de nacimiento
                        else if (DateTime.Today.Month == fechaIngreso.Month && DateTime.Today.Day < fechaIngreso.Day)
                        {
                            --anios;
                        }



                        _plaza.antiguedad =anios.ToString();
                        plazas.Add(_plaza);
                    }
                }
            }
            return plazas;
        }
        public static List<Plaza> GetPlazasSeleccionadas(string id_captura)
        {
            List<Plaza> plazas = new List<Plaza>();
            
                int id = Convert.ToInt32(id_captura);

                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {

                    var datos = (from p in db.plazas_cap_sibl
                                 where p.id_captura == id
                                 select new
                                 {
                                     id_captura = p.id_captura,
                                     id= p.id_plaza_cap,
                                     plaza= p.plaza
                                    

                                 }).Distinct().ToList();


                    foreach (var item in datos)
                    {
                        Plaza _plaza = new Plaza();
                        _plaza.Id = Convert.ToInt32(item.id);
                        _plaza._Plaza =item.plaza;
                      
                        
                        plazas.Add(_plaza);
                    }
                }
            
            return plazas;
        }

        public static List<MovimientoCapturado> GetMovimientosCapturados(List<string> cts)
        {
            List<MovimientoCapturado> movimientos = new List<MovimientoCapturado>();

            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {
                movimientos = (from cm in db.captura_mov_sibl
                               join emp in db.empleado_u on cm.id_empleado equals emp.id
                               join t_mot in db.tipo_mov_sibl on cm.id_tipo_mov equals t_mot.id_tipo
                               join mot in db.motivos_mov on t_mot.mot_movcap equals mot.mot_mov
                               join cve in db.cve_mov on t_mot.cve_mov equals cve.cve_mov1
                               where cts.Contains(cm.ct)
                               select new MovimientoCapturado
                               {
                                   id = cm.id_captura,
                                   rfc = emp.rfc,
                                   nombre = emp.nom.Trim() + " " + emp.paterno.Trim() + " " + emp.materno.Trim(),
                                   tipo_movimiento= cve.descripcion,
                                   tipo_motivo = mot.descripcion,
                                   estatus=cm.estatus
                               }).ToList();
            }

            return movimientos;
        }

        public static string ConviertePlaza(string cod_pago, string unidad, string subunidad, string cat_puesto, string horas, string cons_plaza)
        {
            String plaza = "";
            plaza = cod_pago.PadLeft(2, '0') + unidad.PadLeft(2, '0') + subunidad.PadLeft(2, '0') + cat_puesto.Trim() + horas.PadLeft(2, '0') + ".0" + cons_plaza.PadLeft(6, '0');

            return plaza;
        }
    }
}
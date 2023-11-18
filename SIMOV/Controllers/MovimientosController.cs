using Microsoft.Ajax.Utilities;
using Rotativa;
using SIMOV.Models;
using SIMOV.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIMOV.Controllers
{
    public class MovimientosController : Controller
    {
        // GET: Movimientos
     
        public ActionResult Captura()
        {
            string wMsj = string.Empty;
            captura_mov captura_movModel = new captura_mov();
            if (!Seguridad.LogIn(this.HttpContext))
            {
                return Redirect("http://cgia.seslp.gob.mx/nlogin/");
            }
            if (Seguridad.ValidoAccesoModulo(this.HttpContext, 1, ref wMsj))
            {
                //si tiene periso de entrar al módulo accede


                // obtengo el centro de trabajo o centros de trabajo a los que tiene acceso
                string wusuario = Session["user"].ToString();

                //if (model.ct ==null)
                //{
                //ModelState.Values.Clear();
                // }
                    List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
                    captura_movModel.cts = cts;
                    //ViewBag.cts = cts;


                    //obtener los tipos de movimientos activos para la captura
                    List<cve_movs> tipo_mov = Consultas.GetMovimientos();
                    captura_movModel.cve_movs = tipo_mov;
                    captura_movModel.id_cve_mov = 0;

                    List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento("0");
                    captura_movModel.mot_mov = mot_m;
                    List<Empleado_ddl> empleados = Consultas.GetEmpleados("0");
                    captura_movModel.empleados = empleados;
                //Emp_Plaza e = new Emp_Plaza();

                    ViewBag.antiguedad = 0;
                    ViewBag.plazasEmp = null;
                    ViewBag.plazasSel = null;
                    ViewBag.ct_seleccionado = 0;
                     ViewBag.Error = "";



            }
            return View(captura_movModel);
        }
       
        public JsonResult _GetMotivos(string id)
        {
            if (id == "" || id== "Selecciona el Tipo")
            {
                id = "0";
            }
            List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento(id);
            
            return Json(mot_m, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _GetEmpleados(string ct)
        {
            List<Empleado_ddl> empleados = Consultas.GetEmpleados(ct);
            return Json(empleados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _GetDatosMotivo(string cve_mot,string mot_mov)
        {
            List<motivos_m> datos = Consultas.GetMovimiento(cve_mot, mot_mov);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _GetPlazasEmpleado(string id, string ct,string cve_mot,string mot_mov)
        {
            int antiguedad = 0;
            List<Plaza> plaza = new List<Plaza>();
            if (cve_mot == "")
            { 
                cve_mot = "0";  
            }
            if (mot_mov == "")
            {
                mot_mov = "0";
            }
            if (id == "" || id == "Selecciona el Empleado")
            {
                id = "0";
            }
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())  // Conexión de pruebas remota
            {
                int _id = Convert.ToInt32(id);
                int _cve_mov= Convert.ToInt32(cve_mot);
                int _mot_mov=Convert.ToInt32(mot_mov);  
                var us = db.tipo_mov_sibl.SingleOrDefault(u => u.cve_mov == _cve_mov && u.mot_movcap==_mot_mov );
                if (us.all_plazas == 1)
                {
                    plaza = Consultas.GetAllPlazas(id);
                }
                else {
                    plaza = Consultas.GetPlazas(id, ct);
                }
               
                antiguedad = db.emp_qna_ing_uid.SingleOrDefault(ac => ac.id == _id).qna_ing.Value; 
            }
            int ant=ObtenerAñosAntiguedad(antiguedad);
            ViewBag.antiguedad = ant;
            return Json(plaza, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Guardar(captura_mov model)
        {
            captura_mov captura_movModel = new captura_mov();
            
            string wusuario = Session["user"].ToString();
            if (model.ct != null && model.id_tipo_mov != 0 && model.id_empleado != 0 && model.qna_ini != null)
            {
                

                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    captura_mov_sibl movimiento= new captura_mov_sibl();
                    bit_cap_mov_sibl bit_movimiento = new bit_cap_mov_sibl();
                    plazas_cap_sibl plaza_movimiento = new plazas_cap_sibl();

                    // busco el nivel educativo del movimiento segun el centro de trabajo
                    //por empleado y por ct, obtengo las plazas
                    // en que nivel educativo esta segun las plazas
                    List<Plaza> plazas_1 = new List<Plaza>();
                    plazas_1= Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
                    string area = "";
                    foreach(var pla in plazas_1)
                    {
                        area=db.categorias_areas.SingleOrDefault(ac => ac.unidad == pla.Unidad && ac.subunidad == pla.Subunidad &&
                                                                  ac.cat_puesto == pla.CatPuesto).area_ant.Value.ToString();
                    }
                    //3133
                    int id_tipo_mov = Consultas.GetInformacionMovimiento(model.id_cve_mov.ToString(), model.id_tipo_mov.ToString()).FirstOrDefault().Id;
                    //obtengo el rfc
                    string rfc= Consultas.GetEmpleado(model.id_empleado).FirstOrDefault().rfc;
                    movimiento.area = Convert.ToInt16(area);
                    movimiento.id_tipo_mov = id_tipo_mov;
                    movimiento.id_empleado = model.id_empleado;
                    movimiento.rfc = rfc;
                    movimiento.ct=model.ct;
                    movimiento.fecha_ini = model.qna_ini;
                    movimiento.fecha_fin = model.qna_fin;
                    movimiento.fecha_captura = DateTime.Today;
                    //movimiento.fecha_alerta = null;
                    //movimiento.fecha_envio = null;
                    //movimiento.fecha_recibido = null;
                    //movimiento.fecha_cancelacion = null;
                    //movimiento.observacion = null;
                    movimiento.usuario_cap = wusuario;
                    movimiento.estatus = 1;
                    db.captura_mov_sibl.Add(movimiento);
                    db.SaveChanges();

                    //guardar en bitcaptura
                    bit_movimiento.id_captura = movimiento.id_captura;
                    bit_movimiento.estatus_op = 1;
                    bit_movimiento.usuario = wusuario;
                    bit_movimiento.operacion = "Captura Responsable CT";
                    bit_movimiento.fecha=DateTime.Today;
                    db.bit_cap_mov_sibl.Add(bit_movimiento);
                    db.SaveChanges();
                    List<Emp_Plaza> plazas_ = model.plazas;
                    List<Emp_Plaza> plazaSel_ = new List<Emp_Plaza>();

                    foreach (var p in plazas_)
                    {
                        
                        if (p.plaza != null)
                        {
                            plaza_movimiento.id_captura=movimiento.id_captura;
                            plaza_movimiento.plaza=p.plaza;
                            plaza_movimiento.fecha_captura = DateTime.Today;
                            plaza_movimiento.estatus = 1;
                            db.plazas_cap_sibl.Add(plaza_movimiento);
                            db.SaveChanges();
                           
                        }

                    }
                    List<CentroTrabajo> cts2 = Consultas.GetCTPorUsuario(wusuario);
                    captura_movModel.cts = cts2;
                    


                    //obtener los tipos de movimientos activos para la captura
                    List<cve_movs> tipo_mov2 = Consultas.GetMovimientos();
                    captura_movModel.cve_movs = tipo_mov2;
                    captura_movModel.id_cve_mov = 0;

                    List<motivos_m> mot_m2 = Consultas.GetMotivos_Movimiento("0");
                    captura_movModel.mot_mov = mot_m2;
                    List<Empleado_ddl> empleados2 = Consultas.GetEmpleados("0");
                    captura_movModel.empleados = empleados2;

                    ViewBag.plazasEmp = null;
                    ViewBag.plazasSel = null;
                    ViewBag.ct_seleccionado = 0;
                    ViewBag.Success = "El movimiento se ha capturado con exito, favor de revisar el Administrador";
                    return RedirectToAction("Captura","Movimientos");
                }

                    List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
                    captura_movModel.cts = cts;
                    captura_movModel.ct = model.ct;
                    ViewBag.ct_seleccionado = model.ct;
                
                    //obtener los datos= del modelo
                    List<cve_movs> tipo_mov = Consultas.GetMovimientos();
                    captura_movModel.cve_movs = tipo_mov;
                    captura_movModel.id_cve_mov = model.id_cve_mov;
                
               
                    List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento(model.id_cve_mov.ToString());
                    captura_movModel.mot_mov = mot_m;
                    captura_movModel.id_tipo_mov = model.id_tipo_mov;
                
               
                    List<Empleado_ddl> empleados = Consultas.GetEmpleados(model.ct);
                    captura_movModel.empleados = empleados;
                    captura_movModel.id_empleado = model.id_empleado;
                    List<Plaza> plaza = Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
                    ViewBag.plazasE = plaza;
                    List<Emp_Plaza> plazas = model.plazas;
                    List<Emp_Plaza> plazaSel = new List<Emp_Plaza>();

                    foreach (var p in plazas)
                    {
                        Emp_Plaza plazaE = new Emp_Plaza();
                        if (p.plaza != null)
                        {
                            plazaE.plaza = p.plaza;
                            plazaSel.Add(plazaE);
                        }

                    }
                    // List<Emp_Plaza> plazaSel = Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
                    ViewBag.plazasEmp = true;
                    ViewBag.plazasSel = plazaSel;
                    Session["plazaSel"] = plazaSel;
                
               
                    captura_movModel.qna_ini = model.qna_ini;
                    captura_movModel.qna_fin = model.qna_fin;
                


            }
            else {
                List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
                captura_movModel.cts = cts;
                //ViewBag.cts = cts;


                //obtener los tipos de movimientos activos para la captura
                List<cve_movs> tipo_mov = Consultas.GetMovimientos();
                captura_movModel.cve_movs = tipo_mov;
                captura_movModel.id_cve_mov = 0;

                List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento("0");
                captura_movModel.mot_mov = mot_m;
                List<Empleado_ddl> empleados = Consultas.GetEmpleados("0");
                captura_movModel.empleados = empleados;
                
                ViewBag.plazasEmp = null;
                ViewBag.plazasSel = null;
                ViewBag.ct_seleccionado = 0;
            }

            return View("Captura", captura_movModel);
            

           
         //  return RedirectToAction("Captura", captura_movModel);
        }



        [HttpPost]
        public ActionResult GuardarEditar(captura_mov model)
        {
            captura_mov captura_movModel = new captura_mov();
            //captura_mov_sibl captura = new captura_mov_sibl();
            string wusuario = Session["user"].ToString();
            if (model.ct != null && model.id_tipo_mov != 0 && model.id_empleado != 0 && model.qna_ini != null)
            {


                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    captura_mov_sibl movimiento = new captura_mov_sibl();
                    bit_cap_mov_sibl bit_movimiento = new bit_cap_mov_sibl();
                    plazas_cap_sibl plaza_movimiento = new plazas_cap_sibl();

                    // busco el nivel educativo del movimiento segun el centro de trabajo
                    //por empleado y por ct, obtengo las plazas
                    // en que nivel educativo esta segun las plazas
                    List<Plaza> plazas_1 = new List<Plaza>();
                    plazas_1 = Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
                    string area = "";
                    foreach (var pla in plazas_1)
                    {
                        area = db.categorias_areas.SingleOrDefault(ac => ac.unidad == pla.Unidad && ac.subunidad == pla.Subunidad &&
                                                                  ac.cat_puesto == pla.CatPuesto).area_ant.Value.ToString();
                    }
                    //3133
                    int id_tipo_mov = Consultas.GetInformacionMovimiento(model.id_cve_mov.ToString(), model.id_tipo_mov.ToString()).FirstOrDefault().Id;
                    //obtengo el rfc
                    string rfc = Consultas.GetEmpleado(model.id_empleado).FirstOrDefault().rfc;
                    
                    movimiento = db.captura_mov_sibl.Find(model.IdCaptura);
                    movimiento.area = Convert.ToInt16(area);
                    movimiento.id_tipo_mov = id_tipo_mov;
                    movimiento.id_empleado = model.id_empleado;
                    movimiento.rfc = rfc;
                    movimiento.ct = model.ct;
                    movimiento.fecha_ini = model.qna_ini;
                    movimiento.fecha_fin = model.qna_fin;
                    movimiento.usuario_cap = wusuario;
                    movimiento.estatus = 1;

                    db.SaveChanges();

                    //guardar en bitcaptura
                    bit_movimiento.id_captura = movimiento.id_captura;
                    bit_movimiento.estatus_op = 5;
                    bit_movimiento.usuario = wusuario;
                    bit_movimiento.operacion = "Modificación de Movimiento Responsable CT";
                    bit_movimiento.fecha = DateTime.Today;
                    db.bit_cap_mov_sibl.Add(bit_movimiento);
                    db.SaveChanges();

                    /////////plazas eliminar y volver a insertar los registros
                    //var entidad = db.plazas_cap_sibl.Single(x => x.id_captura == movimiento.id_captura);
                    //db.plazas_cap_sibl.Remove(entidad);
                    //db.SaveChanges();
                    var plaz= db.plazas_cap_sibl.Where(d => d.id_captura == movimiento.id_captura);
                    db.plazas_cap_sibl.RemoveRange(plaz);
                    db.SaveChanges();

                    //guardar en bitcaptura
                    bit_movimiento.id_captura = movimiento.id_captura;
                    bit_movimiento.estatus_op = 12;
                    bit_movimiento.usuario = wusuario;
                    bit_movimiento.operacion = "Modificación de plazas Responsable CT";
                    bit_movimiento.fecha = DateTime.Today;
                    db.bit_cap_mov_sibl.Add(bit_movimiento);
                    db.SaveChanges();

                    List<Emp_Plaza> plazas_ = model.plazas;
                    List<Emp_Plaza> plazaSel_ = new List<Emp_Plaza>();

                    foreach (var p in plazas_)
                    {

                        if (p.plaza != null)
                        {
                            plaza_movimiento.id_captura = movimiento.id_captura;
                            plaza_movimiento.plaza = p.plaza;
                            plaza_movimiento.fecha_captura = DateTime.Today;
                            plaza_movimiento.estatus = 1;
                            db.plazas_cap_sibl.Add(plaza_movimiento);
                            db.SaveChanges();

                        }

                    }

                    //documentos
                    string RutaSitio = Server.MapPath("~/");
                    string PathSolicitud = Path.Combine(RutaSitio + "/PDF/" + movimiento.area + "_" + movimiento.id_captura + "_" + movimiento.rfc + "_Solicitud.pdf");
                    string PathINE = Path.Combine("/PDF/" + movimiento.area + "_" + movimiento.id_captura + "_" + movimiento.rfc + "_INE.pdf");

                    documentos_cap_sibl subirDoc = new documentos_cap_sibl();

                    if (model.Solicitud != null)
                    {
                        subirDoc = new documentos_cap_sibl();
                        model.Solicitud.SaveAs(PathSolicitud);
                        subirDoc.id_captura = movimiento.id_captura;
                        subirDoc.path = PathSolicitud;
                        subirDoc.fecha = DateTime.Now;
                        subirDoc.estatus = 1;
                        db.documentos_cap_sibl.Add(subirDoc);
                        db.SaveChanges();
                    }

                    //List<CentroTrabajo> cts2 = Consultas.GetCTPorUsuario(wusuario);
                    //captura_movModel.cts = cts2;



                    ////obtener los tipos de movimientos activos para la captura
                    //List<cve_movs> tipo_mov2 = Consultas.GetMovimientos();
                    //captura_movModel.cve_movs = tipo_mov2;
                    //captura_movModel.id_cve_mov = 0;

                    //List<motivos_m> mot_m2 = Consultas.GetMotivos_Movimiento("0");
                    //captura_movModel.mot_mov = mot_m2;
                    //List<Empleado_ddl> empleados2 = Consultas.GetEmpleados("0");
                    //captura_movModel.empleados = empleados2;

                    //ViewBag.plazasEmp = null;
                    //ViewBag.plazasSel = null;
                    //ViewBag.ct_seleccionado = 0;
                    ViewBag.Success = "El movimiento se ha modificado con exito, favor de revisar el Administrador";
                    return RedirectToAction("ListaMovimientos", "Movimientos");
                }

            //    List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
            //    captura_movModel.cts = cts;
            //    captura_movModel.ct = model.ct;
            //    ViewBag.ct_seleccionado = model.ct;

            //    //obtener los datos= del modelo
            //    List<cve_movs> tipo_mov = Consultas.GetMovimientos();
            //    captura_movModel.cve_movs = tipo_mov;
            //    captura_movModel.id_cve_mov = model.id_cve_mov;


            //    List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento(model.id_cve_mov.ToString());
            //    captura_movModel.mot_mov = mot_m;
            //    captura_movModel.id_tipo_mov = model.id_tipo_mov;


            //    List<Empleado_ddl> empleados = Consultas.GetEmpleados(model.ct);
            //    captura_movModel.empleados = empleados;
            //    captura_movModel.id_empleado = model.id_empleado;
            //    List<Plaza> plaza = Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
            //    ViewBag.plazasE = plaza;
            //    List<Emp_Plaza> plazas = model.plazas;
            //    List<Emp_Plaza> plazaSel = new List<Emp_Plaza>();

            //    foreach (var p in plazas)
            //    {
            //        Emp_Plaza plazaE = new Emp_Plaza();
            //        if (p.plaza != null)
            //        {
            //            plazaE.plaza = p.plaza;
            //            plazaSel.Add(plazaE);
            //        }

            //    }
            //    // List<Emp_Plaza> plazaSel = Consultas.GetPlazas(model.id_empleado.ToString(), model.ct);
            //    ViewBag.plazasEmp = true;
            //    ViewBag.plazasSel = plazaSel;
            //    Session["plazaSel"] = plazaSel;


            //    captura_movModel.qna_ini = model.qna_ini;
            //    captura_movModel.qna_fin = model.qna_fin;



            //}
            //else
            //{
            //    List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
            //    captura_movModel.cts = cts;
            //    //ViewBag.cts = cts;


            //    //obtener los tipos de movimientos activos para la captura
            //    List<cve_movs> tipo_mov = Consultas.GetMovimientos();
            //    captura_movModel.cve_movs = tipo_mov;
            //    captura_movModel.id_cve_mov = 0;

            //    List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento("0");
            //    captura_movModel.mot_mov = mot_m;
            //    List<Empleado_ddl> empleados = Consultas.GetEmpleados("0");
            //    captura_movModel.empleados = empleados;

            //    ViewBag.plazasEmp = null;
            //    ViewBag.plazasSel = null;
            //    ViewBag.ct_seleccionado = 0;
            }

            return View("Editar", captura_movModel);



            //  return RedirectToAction("Captura", captura_movModel);
        }
        public int ObtenerAñosAntiguedad(  int qna_ingreso )
        {
            string cadena = Convert.ToString(qna_ingreso);
            int dia=0;
            int mes = 0;
            int year = Convert.ToInt32(cadena.Substring(0,4 ));
            int qna = Convert.ToInt32(cadena.Substring(4, 2));
            if (qna == 1 || qna == 3 || qna == 5 || qna == 7 || qna == 9 || qna == 11 ||
                qna == 13 || qna == 15 || qna == 17 || qna == 19 || qna == 21 || qna == 23)
            {
                dia = 1;

            }
            else {
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

            

            return anios;
        }

        public ActionResult ListaMovimientos()
        {
            string wusuario = Session["user"].ToString();
            List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);

            List<string> states = new List<string>();
            foreach (var c in cts)
            {
                states.Add(c.ct);
            }

            List<MovimientoCapturado> movimientos = Consultas.GetMovimientosCapturados(states);
            ViewBag.Movimientos = movimientos;

            return View();
        }

        public ActionResult BloqueaMovimiento(ListaMovimientosCapturados model)
        {
            string wusuario = Session["user"].ToString();

            try
            {
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    foreach (var id in model.id)
                    {
                        // Modifica el estatus de la captura del movimiento a 2.
                        captura_mov_sibl oCaptura = db.captura_mov_sibl.Find(id);
                        oCaptura.estatus = 2;

                        db.Entry(oCaptura).State = System.Data.Entity.EntityState.Modified;

                        // Agrega movimiento en bit_cap_mov_sibl
                        bit_cap_mov_sibl nBit_cap = new bit_cap_mov_sibl();
                        nBit_cap.id_captura = id;
                        nBit_cap.estatus_op = 2;
                        nBit_cap.usuario = wusuario;
                        nBit_cap.operacion = "Bloquea movimiento";
                        nBit_cap.fecha = DateTime.Now;

                        db.bit_cap_mov_sibl.Add(nBit_cap);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al procesar la solicitud: {ex}";

                return RedirectToAction("ListaMovimientos");
            }


            ViewBag.Success = "El movimiento fue bloqueado correctamente.";

            return RedirectToAction("ListaMovimientos");
        }

        public ActionResult CancelarMovimiento(int capturaId)
        {
            string wusuario = Session["user"].ToString();

            try
            {
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    // Modifica el estatus de la captura del movimiento a 4.
                    captura_mov_sibl oCaptura = db.captura_mov_sibl.Find(capturaId);
                    oCaptura.estatus = 4;

                    db.Entry(oCaptura).State = System.Data.Entity.EntityState.Modified;

                    // Agrega movimiento en bit_cap_mov_sibl
                    bit_cap_mov_sibl nBit_cap = new bit_cap_mov_sibl();
                    nBit_cap.id_captura = capturaId;
                    nBit_cap.estatus_op = 4;
                    nBit_cap.usuario = wusuario;
                    nBit_cap.operacion = "Cancela movimiento";
                    nBit_cap.fecha = DateTime.Now;

                    db.bit_cap_mov_sibl.Add(nBit_cap);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al procesar la solicitud: {ex}";

                return RedirectToAction("ListaMovimientos");
            }

            ViewBag.Success = "El movimiento fue cancelado correctamente.";

            return RedirectToAction("ListaMovimientos");
        }

        public ActionResult EnviarYGenerarAcuse(int capturaId)
        {
            string wusuario = Session["user"].ToString();

            try
            {
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    // Modifica el estatus de la captura del movimiento a 3.
                    captura_mov_sibl oCaptura = db.captura_mov_sibl.Find(capturaId);
                    oCaptura.estatus = 3;

                    db.Entry(oCaptura).State = System.Data.Entity.EntityState.Modified;

                    // Agrega movimiento en bit_cap_mov_sibl
                    bit_cap_mov_sibl nBit_cap = new bit_cap_mov_sibl();
                    nBit_cap.id_captura = capturaId;
                    nBit_cap.estatus_op = 3;
                    nBit_cap.usuario = wusuario;
                    nBit_cap.operacion = "Envía movimiento";
                    nBit_cap.fecha = DateTime.Now;

                    db.bit_cap_mov_sibl.Add(nBit_cap);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al procesar la solicitud: {ex}";

                return RedirectToAction("ListaMovimientos");
            }

            return new ActionAsPdf("Acuse", new { captura_id = capturaId })
            { FileName = capturaId + "_Acuse.pdf" };
        }

        public ActionResult Acuse(int captura_id = 0)
        {
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
            {
                captura_mov_sibl Captura = db.captura_mov_sibl.Find(captura_id);

                ViewBag.Id = captura_id;
                ViewBag.FechaEmision = DateTime.Now.ToShortDateString();
                ViewBag.Emisor = Captura.ct;

                string depto = "";
                switch (Captura.area)
                {
                    case 3133:
                        depto = "EDUCACIÓN SECUNDARIA";
                        break;
                }
                ViewBag.Depto = depto;

                tipo_mov_sibl tipoMovimiento = db.tipo_mov_sibl.Find(Captura.id_tipo_mov);

                ViewBag.BaseLegal = tipoMovimiento.base_legal;
                ViewBag.NombreTipo = tipoMovimiento.nombre;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Lista()
        {
            string wusuario = Session["user"].ToString();
            List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);

            List<string> states = new List<string>();
            foreach (var c in cts)
            {
                states.Add(c.ct);
            }

            List<MovimientoCapturado> movimientos = Consultas.GetMovimientosCapturados(states);
            ViewBag.Movimientos = movimientos;

            return View();
        }

        public List<Plaza> _GetPlazasEmpleadoEditar(string id, string ct, string cve_mot, string mot_mov)
        {
            int antiguedad = 0;
            List<Plaza> plaza = new List<Plaza>();
            if (cve_mot == "")
            {
                cve_mot = "0";
            }
            if (mot_mov == "")
            {
                mot_mov = "0";
            }
            if (id == "" || id == "Selecciona el Empleado")
            {
                id = "0";
            }
            using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())  // Conexión de pruebas remota
            {
                int _id = Convert.ToInt32(id);
                int _cve_mov = Convert.ToInt32(cve_mot);
                int _mot_mov = Convert.ToInt32(mot_mov);
                var us = db.tipo_mov_sibl.SingleOrDefault(u => u.cve_mov == _cve_mov && u.mot_movcap == _mot_mov);
                if (us.all_plazas == 1)
                {
                    plaza = Consultas.GetAllPlazas(id);
                }
                else
                {
                    plaza = Consultas.GetPlazas(id, ct);
                }

                antiguedad = db.emp_qna_ing_uid.SingleOrDefault(ac => ac.id == _id).qna_ing.Value;
            }
            int ant = ObtenerAñosAntiguedad(antiguedad);
            ViewBag.antiguedad = ant;
            return plaza;
        }
     
        public ActionResult Editar(int id)
        {
            string wMsj = string.Empty;
            captura_mov captura_movModel = new captura_mov();
            captura_mov_sibl captura = new captura_mov_sibl();
            tipo_mov_sibl tipo_mov_sibl = new tipo_mov_sibl();
            //plazas_cap_sibl plazas_Seleccionadas = new plazas_cap_sibl();

            if (!Seguridad.LogIn(this.HttpContext))
            {
                return Redirect("http://cgia.seslp.gob.mx/nlogin/");
            }
            if (Seguridad.ValidoAccesoModulo(this.HttpContext, 1, ref wMsj))
            {
                using (AdmSIMOVBL_msEntities db = new AdmSIMOVBL_msEntities())
                {
                    string wusuario = Session["user"].ToString();
                    captura=db.captura_mov_sibl.SingleOrDefault(c => c.id_captura == id);
                    List<CentroTrabajo> cts = Consultas.GetCTPorUsuario(wusuario);
                    captura_movModel.cts = cts;
                    captura_movModel.ct= captura.ct;
                    ViewBag.ct_seleccionado = captura.ct;

                    tipo_mov_sibl = db.tipo_mov_sibl.SingleOrDefault(c => c.id_tipo == captura.id_tipo_mov);
                    List<cve_movs> tipo_mov = Consultas.GetMovimientos();
                    captura_movModel.cve_movs = tipo_mov;
                    captura_movModel.id_cve_mov = tipo_mov_sibl.cve_mov;


                    List<motivos_m> mot_m = Consultas.GetMotivos_Movimiento(captura_movModel.id_cve_mov.ToString());
                    captura_movModel.mot_mov = mot_m;
                    captura_movModel.id_tipo_mov = tipo_mov_sibl.mot_movcap;
                    List<Empleado_ddl> empleados = Consultas.GetEmpleados(captura.ct);
                    captura_movModel.empleados = empleados;
                    captura_movModel.id_empleado = captura.id_empleado;
                    List<Plaza> plaza = Consultas.GetPlazas(captura.id_empleado.ToString(), captura.ct);
                    captura_movModel.IdCaptura = captura.id_captura;

                    //obtener plazas seleccionadas de la tabla

                    List<Plaza> plazas_Seleccionadas = Consultas.GetPlazasSeleccionadas(captura.id_captura.ToString());
                   
                    List<Emp_Plaza> plazaSel = new List<Emp_Plaza>();

                    foreach (var p in plazas_Seleccionadas)
                    {
                        Emp_Plaza plazaE = new Emp_Plaza();
                       
                            plazaE.plaza = p._Plaza.Trim();
                            plazaSel.Add(plazaE);

                    }
                    ViewBag.INE = false;
                    ViewBag.Solicitud = false;
                    //obtener los tipos de documentos a subir
                    List<tipo_doc> doc = new List<tipo_doc>();
                    doc = Consultas.GetDocumentos_tipo(captura.id_tipo_mov);
                    foreach (var d in doc)
                    {
                        if (d.Documento == "INE")
                        {
                            ViewBag.INE = true;
                        }
                        if (d.Documento == "Solicitud")
                        {
                            ViewBag.Solicitud = true;
                        }
                    }

                    ViewBag.plazasE = plaza;
                    ViewBag.plazasEmp = true;
                    ViewBag.plazasSel = plazaSel;
                    Session["plazaSel"] = plazaSel;
                    ViewBag.editar = true;


                    captura_movModel.qna_ini = captura.fecha_ini.Value;
                    captura_movModel.qna_fin = captura.fecha_fin.Value;

                }

                   
            }
            return View(captura_movModel);
        }
       


    }
}

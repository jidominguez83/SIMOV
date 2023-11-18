using SIMOV.Models;
using SIMOV.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIMOV.Controllers
{
    public class HomeController : Controller
    {
         
        public ActionResult Index()
        {
            
            string wMsj = string.Empty;
            if (!Seguridad.LogIn(this.HttpContext))
            {
                return Redirect("http://cgia.seslp.gob.mx/nlogin/");
            }

            AdmSIMOVBL_msEntities a = new AdmSIMOVBL_msEntities();
            a.Configuration.LazyLoadingEnabled = false;

            string wuser = Session["user"].ToString();
            int wclave_app = int.Parse(Session["clave_app"].ToString());

            var us = a.mvc_usaurios.SingleOrDefault(u => u.usuario == wuser);

            ////obtengo el nombre completo del usuario que está entrando al sistema
            Session["userName"] = us.nombre.ToString().Trim();
            ViewBag.userName = Session["userName"].ToString();

            if (Seguridad.ValidoAccesoModulo(this.HttpContext, 1, ref wMsj))
            {
                Session["Menu_Movimientos"] = true;
                if (Seguridad.ValidoAccesoModulo(this.HttpContext, 2, ref wMsj))
                {
                    Session["Menu_Movimientos_Cap"] = true;
                }
                else {
                    Session["Menu_Movimientos_Cap"] = false;

                }
            }
            else {
                Session["Menu_Movimientos"] = false;
            }


            
            return View();
        }

       
        protected void cts_Change(object sender, EventArgs e)
        {
            int i = 0;
            //stuff that never gets hit
        }
    }
}
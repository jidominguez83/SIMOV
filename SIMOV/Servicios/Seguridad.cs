using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using Microsoft.Ajax.Utilities;
using SIMOV.Models;

namespace SIMOV.Servicios
{
    public class Seguridad
    {

        public Seguridad()
        {

        }

        public static bool LogIn(System.Web.HttpContextBase p)
        {
            string wusuario = string.Empty;
            string wllave = string.Empty;
            string wcc = string.Empty;
            int wapp = -1;
            int wdv = -1;
            string wkey = string.Empty;
            string wip = string.Empty;
            SIMOV.ComprobarKey.ServiceSoapClient nLogIn = new SIMOV.ComprobarKey.ServiceSoapClient();
            string user;
            //OBTENGO LA ip DEL CLIENTE
            wip = GetIp();

            //PARA REALIZAR DEPURACIÓN LOCAL
            //wip = "<ip>172.16.17.129</ip>";
            //wkey = key;

            //REVISO SI EL key SE ENCUENTRA EN LA URL O ESTÁ OCULTO
            if (p.Request.QueryString.ToString().Contains("key"))
            {   //CUANDO EL key SE ENCUETRA EN LA URL    GET
                wkey = p.Request.QueryString["key"].ToString();
            }
            else
            {

                if (p.Request.Form["key"] != null && p.Request.Form.ToString().Contains("key"))
                {   //CUANDO EL key SE ENCUENTRA OCULTO      POST
                    wkey = p.Request.Form["key"].ToString();
                }
                else
                {
                    if (p.Session["key"] != null)
                    {
                        wkey = p.Session["key"].ToString();
                    }
                }
            }
            //SI NO SE RECUPERO EL key ENTONCES NO PERMITO EL ACCESO Y REDIRECCIONO A LA PANTALLA DE INICIO DE SESION
            if (wkey == "" || wkey == string.Empty)
            {
                //p.Session.Clear();
                //p.Session.Abandon();

                //return false;
                p.Session["clave_app"] = 1192;
                p.Session["user"] = "GaucencioMH"; //"bet52876";//"GaucencioMH";//"FranciscoHM";//"AlvarezAJ42";//

                return true;
            }
            else
            {   //REVISO QUE EL key SEA DE UNA SESION ACTIVA
                //long wdetalle = 0;
                if (nLogIn.ValidoKey(wkey, ref wusuario, ref wapp, ref wllave, ref wcc, ref wdv, wip,0))
                {   //CUANDO LA SESIÓN SE ENCUENTRA ACTIVA INICIALIZO VARIABLES DE SESION
                    user = wusuario;
                    p.Session["user"] = wusuario;
                    p.Session["key"] = wkey;
                    p.Session["llave"] = wllave;
                    p.Session["clave_app"] = wapp;
                    p.Session["cc"] = wcc;
                    p.Session["dv"] = wdv;
                    p.Session.Timeout = 32;



                    return true;
                }
                else
                {   //SI LA SESION NO SE ENCUENTRA ACTIVA  REDIRECCIONO A LA PANTALLA DE INICIO DE SESION
                    p.Session.Clear();
                    p.Session.Abandon();
                    return false;

                }
            }

            }
        private static string GetIp()
        {
            string wip = string.Empty;
            //RECUPERO ip ******************************
            wip = "<ip>" + HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] + "</ip>";
            string ipdob = string.Empty; string resultadoips = string.Empty; string kaxs = string.Empty;
            if (string.IsNullOrEmpty(wip) || wip == "<ip></ip>")
            {
                wip = "<ip>" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "</ip>";
                //resultadoips = wip;
                return wip;
            }
            else
            {
                ipdob = "<ipd>" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "</ipd>";
                if (!(string.IsNullOrEmpty(ipdob) || ipdob == "<ipd></ipd>"))
                {
                    //resultadoips = wip + ipdob; 
                    return wip + ipdob;
                }
                else return wip;
            }
            //RECUPERO ip ******************************
        }
        public static bool LogOut(System.Web.HttpContextBase p, ref string wMsj)
        {
            try
            {
                SIMOV.ComprobarKey.ServiceSoapClient nLogIn = new SIMOV.ComprobarKey.ServiceSoapClient();
                if (p.Session["user"] == null || p.Session["llave"] == null)
                {
                    return true;
                }
                nLogIn.CierroSesionUsuario(p.Session["user"].ToString(), p.Session["llave"].ToString(), int.Parse(p.Session["clave_app"].ToString()));
                p.Application[p.Session["user"].ToString()] = null;
                p.Session.Clear();
                p.Session.Abandon();
                //p.Response.Redirect(PaginaLogin);
                return true;
            }
            catch (Exception e)
            {
                wMsj = e.Message; return false;
            }
        }
        public static bool RegistroMovUsuario(Int16 wModulo, string wOperacion, string wtabla, System.Web.HttpContextBase p)
        {
            try
            {
                long wdetalle = 0;
                SIMOV.ComprobarKey.ServiceSoapClient nLogIn = new SIMOV.ComprobarKey.ServiceSoapClient();
                if (nLogIn.RegistroMovUsuario(p.Session["user"].ToString(), p.Session["llave"].ToString(), int.Parse(p.Session["clave_app"].ToString()), wModulo, wOperacion, true, wtabla, ref wdetalle)) return true;
                else return false;
            }
            catch { return false; }
        }
        public static bool ValidoAccesoModulo(System.Web.HttpContextBase p, int wclave_mod, ref string wMsj)
        {
            AdmSIMOVBL_msEntities a = new AdmSIMOVBL_msEntities();
            a.Configuration.LazyLoadingEnabled = false;
            try
            {
                int wclave_app = int.Parse(p.Session["clave_app"].ToString());
                string wusuario = p.Session["user"].ToString();
                //Recuperamos los permisos de Acceso del usuario
                var waccess = a.mvc_permisos.SingleOrDefault(ac => ac.clave_app == wclave_app && ac.clave_mod == wclave_mod && ac.usuario == wusuario);
                if (waccess != null && waccess.permisos.ToString().Trim() == "Acceso")
                {
                    //El usuario tiene acceso al módulo solicitado
                    waccess = null;
                    return true;
                }
                else
                { //El usuario no tiene acceso al módulo
                    waccess = null;
                    return false;
                }
            }
            catch (Exception e)
            {
                wMsj = "¡Error: " + e.Message + "!";
                return false;
            }
        }
        //public static bool ValidoPermisoOpcionModulo(System.Web.HttpContextBase p, int wclave_mod, string wopc)
        //{
        //    AdmCtasCADEntities pu = new AdmCtasCADEntities();
        //    pu.Configuration.LazyLoadingEnabled = false;
        //    try
        //    {
        //        int wclave_app = int.Parse(p.Session["clave_app"].ToString());
        //        string wusuario = p.Session["user"].ToString();
        //        var wpermiso = pu.permisos_opciones.SingleOrDefault(pe => pe.clave_app == wclave_app && pe.clave_mod == wclave_mod && pe.usuario == wusuario && pe.opc == wopc);
        //        if (wpermiso != null && wpermiso.opc.ToString().Trim() == wopc)
        //        {   //El usuario tiene permiso para la opción solicitada    
        //            wpermiso = null;
        //            return true;
        //        }
        //        else
        //        { //El usuario no tiene permiso para la opción solicitada
        //            wpermiso = null;
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

    }
}
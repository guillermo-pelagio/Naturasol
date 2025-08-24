using CapaEntidades;
using CapaNegocios;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace WebArribosPlanta.Controllers
{
    public class ArribosController : Controller
    {
        EntradasBLL entradasBLL = new EntradasBLL();

        // GET: Arribos
        public ActionResult arribos()
        {
            return View();
        }

        //ACCION DE INICIO DE SESION
        [HttpPost]
        public int validar_informacion(SesionEntradaMaterial entradas)
        {

            SesionEntradaMaterial sesion = entradasBLL.validarOCArriboProveedor(entradas);

            if (sesion is null)
            {
                return 2;
            }
            else
            {
                if ((sesion.CardName.Length > 0))
                {
                    FormsAuthentication.SetAuthCookie(sesion.DocNum, false);
                    Session["cardcode"] = sesion.CardCode;
                    Session["cardname"] = sesion.CardName;
                    Session["docdate"] = entradas.DocDate;
                    Session["docnum"] = sesion.DocNum;

                    Session["doctime"] = entradas.DocTime;
                    Session["ocrelacionada"] = sesion.OCRelacionada;
                    Session["sociedad"] = entradas.Sociedad;

                    Session["ubicacion"] = entradas.Ubicacion;
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public ActionResult confirmar_arribo()
        {
            if ((Session["docnum"]) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("arribos", "arribos");
            }
        }

        //ACCION DE INICIO DE SESION
        [HttpPost]
        public int cancelar_informacion()
        {
            Session["cardcode"] = null;
            Session["cardname"] = null;
            Session["docdate"] = null;
            Session["docnum"] = null;

            Session["doctime"] = null;
            Session["sociedad"] = null;
            Session["ubicacion"] = null;

            return 0;
        }

        public int confirmar_informacion()
        {
            SesionEntradaMaterial sesion = new SesionEntradaMaterial();

            sesion.CardCode = Convert.ToString(Session["cardcode"]);
            sesion.CardName = Convert.ToString(Session["cardname"]);
            sesion.DocDate = Convert.ToString(Session["docdate"]);
            sesion.DocNum = Convert.ToString(Session["docnum"]);

            sesion.DocTime = Convert.ToString(Session["doctime"]);
            sesion.Sociedad = Convert.ToString(Session["sociedad"]);
            sesion.Ubicacion = Convert.ToString(Session["ubicacion"]);

            int resultado = entradasBLL.confirmarOCArriboProveedor(sesion);

            Session["cardcode"] = null;
            Session["cardname"] = null;
            Session["docdate"] = null;
            Session["docnum"] = null;

            Session["doctime"] = null;
            Session["sociedad"] = null;
            Session["ubicacion"] = null;

            return resultado;
        }
    }
}
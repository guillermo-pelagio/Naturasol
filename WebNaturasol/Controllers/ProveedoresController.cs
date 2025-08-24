using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacion.Content
{
    public class ProveedoresController : Controller
    {
        //VISTA DE INICIO DE SESION
        public ActionResult login()
        {
            if (Session["correo"] != null)
            {
                return RedirectToAction("inicio", "proveedores");
            }
            else
            {
                return View();
            }
        }
        //VISTA DASHBOARD
        [Authorize]
        public ActionResult inicio()
        {
            if ((Session["correo"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "proveedores");
            }
        }

        //VISTA DASHBOARD
        [Authorize]
        public ActionResult facturas()
        {
            if ((Session["correo"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "proveedores");
            }
        }

        //VISTA DASHBOARD
        [Authorize]
        public ActionResult estatus()
        {
            if ((Session["correo"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "proveedores");
            }
        }

        //ACCION DE INICIO DE SESION
        [HttpPost]
        public int inicio_sesion(Proveedores proveedor)
        {
            ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
            SesionProveedor sesion = proveedoresBLL.inicioSesion(proveedor);

            if ((sesion.estatus == 1))
            {
                if (sesion.correo != null)
                {
                    FormsAuthentication.SetAuthCookie(sesion.correo, false);

                    Session["correo"] = sesion.correo;
                    Session["nombreCompleto"] = sesion.nombreCompleto;
                    Session["contrasena"] = sesion.contrasena;
                    Session["numeroSocio"] = sesion.numeroSocio;
                    Session["nombreSocio"] = sesion.nombreSocio;
                    Session["RFC"] = sesion.RFC;
                    Session["estatus"] = sesion.estatus;
                    Session["monto"] = null;
                    Session["moneda"] = null;
                    Session["docNum"] = null;

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        //ACCION DE OBTENER LAS OF
        [HttpGet]
        public JsonResult obtener_entradas()
        {
            ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
            List<EntradaMaterial> entradas = new List<EntradaMaterial>();
            entradas = proveedoresBLL.obtenerEntradas("TSSL_NATURASOL", Convert.ToString(Session["numeroSocio"]));

            for (int f = 0; f < entradas.Count; f++)
            {
                if (entradas[f].DocCur == "MXP")
                {
                    entradas[f].leyendaTotal = entradas[f].DocTotal;
                }
                else
                {
                    entradas[f].leyendaTotal = entradas[f].DocTotalFC;
                }
            }

            return Json(new
            {
                data = entradas
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LAS OF
        [HttpGet]
        public JsonResult obtener_facturas()
        {
            ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
            List<FacturaProveedor> facturas = new List<FacturaProveedor>();
            facturas = proveedoresBLL.obtenerFacturas("TSSL_NATURASOL", Convert.ToString(Session["numeroSocio"]));

            for (int f = 0; f <= facturas.Count - 1; f++)
            {
                if (facturas[f].DocCur == "MXP")
                {
                    facturas[f].montoTotal = string.Format("{0:C}", Convert.ToDecimal(facturas[f].DocTotal));
                    facturas[f].montoPendiente = string.Format("{0:C}", Convert.ToDecimal(facturas[f].Pendiente));
                    facturas[f].estatusPago = float.Parse(facturas[f].Pendiente) == 0 ? "PAGADO" : float.Parse(facturas[f].DocTotal) - float.Parse(facturas[f].Pendiente) == 0 ? "PENDIENTE" : "PARCIAL";
                }
                else
                {
                    facturas[f].montoTotal = string.Format("{0:C}", Convert.ToDecimal(facturas[f].DocTotalFC));
                    facturas[f].montoPendiente = string.Format("{0:C}", Convert.ToDecimal(facturas[f].PendienteFC));
                    facturas[f].estatusPago = float.Parse(facturas[f].PendienteFC) == 0 ? "PAGADO" : float.Parse(facturas[f].DocTotalFC) - float.Parse(facturas[f].PendienteFC) == 0 ? "PENDIENTE" : "PARCIAL";
                }
            }

            return Json(new
            {
                data = facturas
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = Convert.ToString(Session["numeroSocio"]) + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Second + testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = Convert.ToString(Session["numeroSocio"]) + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Second + file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  

                        string path = "~/content/filessupplier";
                        fname = Path.Combine(Server.MapPath(path), fname);
                        file.SaveAs(fname);

                        EntradaMaterial entradaMaterial = new EntradaMaterial();
                        entradaMaterial.montoPortal = Convert.ToString(Session["monto"]);
                        entradaMaterial.monedaPortal = Convert.ToString(Session["moneda"]);
                        entradaMaterial.DocNum = Convert.ToString(Session["docNum"]);
                        entradaMaterial.archivoPortal = fname;

                        ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
                        int idRegistro = proveedoresBLL.guardarRegistroProveedor(entradaMaterial);
                    }
                    // Returns message that successfully uploaded  
                    return Json("Archivo guardado correctamente!");
                }
                catch (Exception ex)
                {
                    return Json("Ocurrio el siguiente error: " + ex.Message);
                }
            }
            else
            {
                return Json("Sin archivos xml");
            }
        }

        //ACCION DE GUARDADO
        [HttpPost]
        public JsonResult guardar_registro(EntradaMaterial entradaMaterial)
        {
            Session["monto"] = entradaMaterial.montoPortal;
            Session["moneda"] = entradaMaterial.monedaPortal;
            Session["docNum"] = entradaMaterial.DocNum;

            return Json(new
            {
                data = 1
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE VALIDAR SESION ACTIVA
        [HttpPost]
        public int validar_sesion()
        {
            if (Session["correo"] != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //ACCION DE CERRAR SESION
        public ActionResult cerrar_sesion()
        {
            FormsAuthentication.SignOut();
            Session["correo"] = null;
            Session["nombreCompleto"] = null;
            Session["contrasena"] = null;
            Session["numeroSocio"] = null;
            Session["nombreSocio"] = null;
            Session["RFC"] = null;
            Session["estatus"] = null;
            Session["monto"] = null;
            Session["moneda"] = null;
            Session["docNum"] = null;
            return RedirectToAction("login", "proveedores");
        }
    }
}

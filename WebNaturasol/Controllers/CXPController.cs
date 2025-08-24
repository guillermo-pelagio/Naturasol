using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CXPController : Controller
    {
        VentasBLL ventasBLL = new VentasBLL();
        ReportesBLL reportesBLL = new ReportesBLL();
        
        public ActionResult Index()
        {
            return View();
        }

        //VISTA DASHBOARD
        [Authorize]
        public ActionResult facturas_proveedores()
        {
            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        //ACCION DE OBTENER LAS OF
        [HttpGet]
        public JsonResult obtener_entradas_global()
        {
            ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
            List<EntradaMaterial> entradas = new List<EntradaMaterial>();
            entradas = proveedoresBLL.obtener_entradas_global("TSSL_NATURASOL");

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

        //[Authorize]
        public ActionResult reportes_cxp()
        {
            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        [HttpGet]
        public JsonResult obtener_reportes()
        {
            List<Reportes> reportes = new List<Reportes>();
            List<Reportes> reportesPlaneacion = new List<Reportes>();
            reportes = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reportes.Count; i++)
            {
                if (reportes[i].areaReporte == "X")
                {
                    reportesPlaneacion.Add(reportes[i]);
                }
            }
            return Json(new
            {
                data = reportesPlaneacion
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

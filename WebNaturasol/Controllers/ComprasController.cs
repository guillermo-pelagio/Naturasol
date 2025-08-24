using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ComprasController : Controller
    {
        ReportesBLL reportesBLL = new ReportesBLL();

        // GET: Inventario
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult reportes_compras()
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
            List<Reportes> reporte = new List<Reportes>();
            List<Reportes> reportesCompras = new List<Reportes>();
            reporte = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reporte.Count; i++)
            {
                if (reporte[i].areaReporte == "C")
                {
                    reportesCompras.Add(reporte[i]);
                }
            }
            return Json(new
            {
                data = reportesCompras
            }, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        public ActionResult reportes_existencia_consumo_bi()
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
    }
}

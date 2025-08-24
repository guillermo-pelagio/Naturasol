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
    public class InternacionalController : Controller
    {
        ReportesBLL reportesBLL = new ReportesBLL();        

        //[Authorize]
        public ActionResult reportes_internacional()
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
                if (reportes[i].areaReporte == "I")
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

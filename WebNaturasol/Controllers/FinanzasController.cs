using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class FinanzasController : Controller
    {
        ReportesBLL reportesBLL = new ReportesBLL();

        //[Authorize]
        public ActionResult reportes_finanzas()
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
                if (reportes[i].areaReporte == "F")
                {
                    if ((Convert.ToString(Session["permisos"]).Contains("Presupuesto global")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "57")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                    if ((Convert.ToString(Session["permisos"]).Contains("Presupuesto semillas")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "58")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                    if ((Convert.ToString(Session["permisos"]).Contains("Presupuesto untables")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "59")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                    if ((Convert.ToString(Session["permisos"]).Contains("Presupuesto tultepark")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "60")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                    if ((Convert.ToString(Session["permisos"]).Contains("Presupuesto transportes")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "61")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                    if ((Convert.ToString(Session["permisos"]).Contains("Gastos almacén")) || ((Convert.ToInt32(Session["puesto"]) == 100) && (Convert.ToInt32(Session["departamento"]) == 1)))
                    {
                        if (reportes[i].idReporte == "1121")
                        {
                            reportesPlaneacion.Add(reportes[i]);
                        }
                    }
                }
            }
            return Json(new
            {
                data = reportesPlaneacion
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

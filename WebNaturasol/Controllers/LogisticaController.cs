using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class LogisticaController : Controller
    {
        ReportesBLL reportesBLL = new ReportesBLL();
        LogisticaBLL logisticaBLL = new LogisticaBLL();

        //[Authorize]
        public ActionResult reportes_logistica()
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
                if (reportes[i].areaReporte == "L")
                {
                    reportesPlaneacion.Add(reportes[i]);
                }
            }
            return Json(new
            {
                data = reportesPlaneacion
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult calendario_entregas()
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
        public JsonResult obtener_calendario()
        {
            List<CalendarioEntrega> kardex = new List<CalendarioEntrega>();

            kardex = logisticaBLL.obtenerCalendario(Convert.ToString(Session["oc"]), Convert.ToString(Session["fecha"]), Convert.ToString(Session["fecha2"]), Convert.ToString(Session["cliente"]), Convert.ToString(Session["codigoSap"]), Convert.ToString(Session["descripcionSap"]));

            var jsonResult = Json(new
            {
                data = kardex
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_calendario(CalendarioEntrega calendario)
        {
            Session["oc"] = calendario.oc;
            Session["fecha"] = calendario.fecha;
            Session["fecha2"] = calendario.fecha2;
            Session["cliente"] = calendario.cliente;
            Session["codigoSap"] = calendario.codigoSap;
            Session["descripcionSap"] = calendario.descripcionSap;

            return "1";
        }
    }
}

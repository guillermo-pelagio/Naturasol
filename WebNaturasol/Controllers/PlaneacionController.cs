using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers
{
    public class PlaneacionController : Controller
    {
        ReportesBLL reportesBLL = new ReportesBLL();
        CalendarioBLL calendarioBLL = new CalendarioBLL();

        //[Authorize]
        public ActionResult reportes_planeacion()
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

        //[Authorize]
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
        //public List<Calendario> GetEvents()
        public JsonResult GetEvents()
        {
            var events = new List<object>
            {
                new { id=1, title = "Event 1", start = "2025-01-06", end = "2025-01-08" },
                new { id=2, title = "Event 2", start = "2025-01-10" },
                new { id=3, title = "Event 3", start = "2025-01-15", allDay = true }
            };

            List<Calendario> calendario = new List<Calendario>();
            calendario = calendarioBLL.obtenerEventos();

            //return calendario;

            JsonResult a = Json(events);

            JsonResult b = Json(calendario);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            var jsonString2 = Newtonsoft.Json.JsonConvert.SerializeObject(events);

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            
            string sd= System.Text.Json.JsonSerializer.Serialize(events, options);


            JavaScriptSerializer j = new JavaScriptSerializer();
            object ax = j.Deserialize(sd, typeof(object));

            JsonResult bx = Json(new
            {
                data = calendario
            }, JsonRequestBehavior.AllowGet);

            return bx;

            /*
            List<Calendario> calendario = new List<Calendario>();

            List<Appointment> appointments = _context.Appointments
        .Include(app => app.Employee)
        .Include(app => app.Customer)
        .ToList();
            List<HomeIndexGetViewModel> appointmentsIndexViewModel = _mapper.Map<List<Appointment>, List<HomeIndexGetViewModel>>(appointments);
            return new JsonResult(appointmentsIndexViewModel);
            */
        }

        [HttpGet]
        public JsonResult obtener_reportes()
        {
            List<Reportes> reportes = new List<Reportes>();
            List<Reportes> reportesPlaneacion = new List<Reportes>();
            reportes = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reportes.Count; i++)
            {
                if (reportes[i].areaReporte == "R")
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

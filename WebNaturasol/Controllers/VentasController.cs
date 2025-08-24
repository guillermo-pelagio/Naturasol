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
    public class VentasController : Controller
    {
        VentasBLL ventasBLL = new VentasBLL();
        ReportesBLL reportesBLL = new ReportesBLL();

        // GET: Cotizaciones
        public ActionResult Index()
        {
            return View();
        }

        //ACCION DE OBTENER LOS ESTATUS DE LAS COTIZACIONES
        [HttpGet]
        public JsonResult estatus_cotizaciones()
        {
            List<Cotizacion> cotizaciones = new List<Cotizacion>();

            cotizaciones = ventasBLL.obtenerEstatusCotizaciones();


            return Json(new
            {
                data = cotizaciones
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult obtener_motivo_rechazo()
        {
            List<Cotizacion> cotizaciones = new List<Cotizacion>();

            cotizaciones = ventasBLL.obtenerMotivoRechazoCotizaciones();


            return Json(new
            {
                data = cotizaciones
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult estatus_cotizaciones_kam()
        {
            List<Cotizacion> cotizaciones = new List<Cotizacion>();

            cotizaciones = ventasBLL.obtenerEstatusCotizacionesKAM();


            return Json(new
            {
                data = cotizaciones
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_numero_ventas()
        {
            List<string> numero = ventasBLL.obtener_numero_ventas();

            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        //////////////////////////////////////////////////////////////////////////COTIZACIONES
        public ActionResult cotizaciones()
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
        public JsonResult obtener_cotizacion()
        {
            List<Cotizacion> cotizaciones = new List<Cotizacion>();
            cotizaciones = ventasBLL.obtenerCotizacionesWeb();

            List<Cotizacion> cotizaciones2 = new List<Cotizacion>();

            for (int g = 0; g < cotizaciones.Count - 1; g++)
            {
                if (Convert.ToDateTime(cotizaciones[g].fechaCreacion) < DateTime.ParseExact("2025-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    if (((Convert.ToInt32(Session["puesto"]) == 10) || (Convert.ToInt32(Session["puesto"]) == 15)))
                    {
                        if (cotizaciones[g].KAM == Convert.ToString(Session["empleado"]))
                        {
                            cotizaciones2.Add(cotizaciones[g]);
                        }
                    }
                    else
                    {
                        cotizaciones2.Add(cotizaciones[g]);
                    }
                }
            }

            return Json(new
            {
                data = cotizaciones2
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_cotizacion_2025()
        {
            List<Cotizacion> cotizaciones = new List<Cotizacion>();
            cotizaciones = ventasBLL.obtenerCotizacionesWeb();

            List<Cotizacion> cotizaciones2 = new List<Cotizacion>();

            for (int g = 0; g < cotizaciones.Count - 1; g++)
            {
                if (Convert.ToDateTime(cotizaciones[g].fechaCreacion) >= DateTime.ParseExact("2025-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    if (((Convert.ToInt32(Session["puesto"]) == 10) || (Convert.ToInt32(Session["puesto"]) == 15)))
                    {
                        if (cotizaciones[g].KAM == Convert.ToString(Session["empleado"]))
                        {
                            cotizaciones2.Add(cotizaciones[g]);
                        }
                    }
                    else
                    {
                        cotizaciones2.Add(cotizaciones[g]);
                    }
                }
            }

            return Json(new
            {
                data = cotizaciones2
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardar_cotizacion(Cotizacion cotizacionWEB)
        {
            int idCotizacion = ventasBLL.guardarCotizacion(cotizacionWEB, Convert.ToInt32(Session["puesto"]));

            return Json(new
            {
                data = idCotizacion
            }, JsonRequestBehavior.AllowGet);
        }

        //////////////////////////////////////////////////////////////////////////REPORTES
        //[Authorize]
        public ActionResult reportes_ventas()
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
            List<Reportes> reportesVentas = new List<Reportes>();
            reporte = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reporte.Count; i++)
            {
                if (reporte[i].areaReporte == "V")
                {
                    reportesVentas.Add(reporte[i]);
                }
            }
            return Json(new
            {
                data = reportesVentas
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult obtener_excel(string idReporte)
        {
            /*
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Users\Desarrollo2\Downloads\Cotizaciones.xlsx");
            string fileName = "Cotizaciones.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            */
            DataTable table = new DataTable();
            ReportesBLL reportesBLL = new ReportesBLL();

            List<Reportes> reporte = new List<Reportes>();
            reporte = reportesBLL.obtenerQuery(idReporte);
            table = reportesBLL.obtenerResultadosQuery(reporte[0].queryReporte);


            using (XLWorkbook workbook = new XLWorkbook())
            {
                table.TableName = reporte[0].nombreReporte;
                workbook.Worksheets.Add(table);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reporte[0].nombreReporte + " " + DateTime.Now + ".xlsx");
                }
            }
        }


        //[Authorize]
        public ActionResult inventario_comercial_bi()
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
        public ActionResult ventas_2025_bi()
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
        public ActionResult fillrate_bi()
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

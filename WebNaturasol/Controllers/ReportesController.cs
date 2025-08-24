using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ReportesController : Controller
    {

        //[Authorize]
        public ActionResult reportes()
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
            ReportesBLL reportesBLL = new ReportesBLL();

            reporte = reportesBLL.obtenerReportes("TSSL_NATURASOL");

            return Json(new
            {
                data = reporte
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult obtener_excel(string idReporte)
        {
            DataTable table = new DataTable();
            ReportesBLL reportesBLL = new ReportesBLL();

            List<Reportes> reporte = new List<Reportes>();
            reporte = reportesBLL.obtenerQuery(idReporte);

            if (reporte[0].queryReporte.Contains("xls"))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\inetpub\Arribos\Reportes\" + reporte[0].queryReporte + "");
                string fileName = reporte[0].queryReporte;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
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
        }


        //[Authorize]
        public ActionResult reporte_bi()
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
        [HttpPost]
        public string obtener_reporte(string idReporte)
        {
            string select = "";

            List<Reportes> reporte = new List<Reportes>();
            ReportesBLL reportesBLL = new ReportesBLL();
            reporte = reportesBLL.obtenerQuery(idReporte);

            select = select + "<iframe title='Reporte BI' width='100%' height='100%' src='" + reporte[0].queryReporte + "' frameborder='0' allowFullScreen='true'></iframe>";

            return select;
        }
    }
}

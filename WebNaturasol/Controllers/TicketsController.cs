using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using CapaEntidades;
using CapaNegocios;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class TicketsController : Controller
    {
        public static HttpResponse response;

        //VISTA DE TICKETS
        //[Authorize]
        public ActionResult tickets()
        {
            /*
            if ((Session["usuarioId"]) != null)
            {
            */
            return View();
            /*
            }
            else
            {
                return RedirectToAction("inicio", "inicio");
            }
            */
        }


        public ActionResult consultar_ticket()
        {
            if ((Session["usuarioId"]) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("inicio", "inicio");
            }
        }

        //ACCION DE OBTENER LOS TICKETS
        [HttpGet]
        public JsonResult obtener_tickets()
        {
            List<Tickets> Tickets = new List<Tickets>();
            //Tickets = new TicketsBLL().obtenerTickets((int)Session["departamento"] == 1 ? true : false, (int)Session["usuarioId"]);
            Tickets = new TicketsBLL().obtenerTickets(true, 1);

            return Json(new
            {
                data = Tickets
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDADO DE TICKET
        [HttpPost]
        public string guardar_tickets(Tickets Ticket)
        {
            TicketsBLL ticket = new TicketsBLL();
            string folio = "";
            ticket.actualizarFolioTicket();
            folio = "TK" + ticket.obtenerFolioTicket(1).ToString("D6");

            Ticket.folio = folio;
            Ticket.estatus = 1;
            Ticket.horaSolicitud = DateTime.Now;

            //Ticket.idUsuarioSolicita = (int)Session["usuarioId"];
            ticket.guardarTicket(Ticket);

            return folio;
        }

        //ACCION DE GUARDADO DE TICKET
        [HttpPost]
        public void generar_excel(Tickets Ticket)
        {
            /*
            TicketsBLL ticket = new TicketsBLL();
            ExcelBLL excelBLL = new ExcelBLL();
            string folio = "";
            DataSet dsConsultaExcel = new DataSet();
            DataTable dtReportesFinal = new DataTable();
            dsConsultaExcel = null;

            dsConsultaExcel = new TicketsBLL().obtenerTicketsExcel(true, 1);
            dtReportesFinal = dsConsultaExcel.Tables[0];

            dtReportesFinal.Columns.RemoveAt(dtReportesFinal.Columns.Count - 1);
            response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"kjfnvfkcnfjk\"");
            response.ContentType = "application/vnd.ms-excel";
            response.ContentEncoding = Encoding.GetEncoding("utf-8");
            response.Charset = "utf-8";
            response.Write(ExcelBLL.exportarReportesCorreo(dtReportesFinal));
            response.End();
            */
        }

        //ACCION DE ACTUALIZADO DE TICKET
        [HttpPost]
        public JsonResult actualizar_tickets(Tickets Ticket)
        {
            Ticket.horaFinalizacion = DateTime.Now;
            //Ticket.idUsuarioFinalizo = (int)Session["usuarioId"];
            int idTicket = new TicketsBLL().actualizarTicket(Ticket);

            return Json(new
            {
                data = idTicket
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class MantenimientoController : Controller
    {
        MantenimientoBLL mantenimientoBLL = new MantenimientoBLL();
        ReportesBLL reportesBLL = new ReportesBLL();

        //[Authorize]
        public ActionResult inventario()
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

        public ActionResult catalogos()
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

        public ActionResult mantenimientos()
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
        public JsonResult obtener_inventario()
        {
            List<Inventario> inventario = new List<Inventario>();

            inventario = mantenimientoBLL.obtenerInventarioMantenimiento("TSSL_NATURASOL", Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen"]));

            var jsonResult = Json(new
            {
                data = inventario
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_inventario(Inventario inventario)
        {
            Session["almacen"] = inventario.Almacén;
            Session["lote"] = inventario.Lote;
            Session["articulo"] = inventario.Artículo;
            Session["descripcion"] = inventario.Descripción;

            return "1";
        }
    }
}

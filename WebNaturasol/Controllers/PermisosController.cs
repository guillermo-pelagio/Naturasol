using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class PermisosController : Controller
    {
        public ActionResult permisos()
        {
            if ((Convert.ToInt32(Session["departamento"]) == 1) && (Convert.ToInt32(Session["puesto"]) == 100))
            {
                return View();
            }
            else
            {
                return RedirectToAction("error403", "error");
            }
        }


        //ACCION DE OBTENER USUARIOS
        [HttpGet]
        public JsonResult obtener_accesos()
        {
            List<Permisos> usuarios = new List<Permisos>();
            usuarios = new ModulosBLL().obtenerPermisos();

            return Json(new
            {
                data = usuarios
            }, JsonRequestBehavior.AllowGet);
        }


        //ACCION DE GUARDAR USUARIOS
        [HttpPost]
        public JsonResult guardar_acceso(Permisos permiso)
        {
            int idUsuario = new ModulosBLL().guardarAcceso(permiso);

            return Json(new
            {
                data = idUsuario
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDAR USUARIOS
        [HttpPost]
        public JsonResult eliminar_acceso(Permisos permiso)
        {
            int idUsuario = new ModulosBLL().eliminarAcceso(permiso);

            return Json(new
            {
                data = idUsuario
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE ACTUALIZAR USUARIOS
        [HttpPost]
        public JsonResult actualizar_acceso(Permisos permiso)
        {
            int idUsuario = new ModulosBLL().actualizarAcceso(permiso);

            return Json(new
            {
                data = idUsuario
            }, JsonRequestBehavior.AllowGet);
        }

    }
}

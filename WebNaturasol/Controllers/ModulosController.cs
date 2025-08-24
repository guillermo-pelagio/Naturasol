using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ModulosController : Controller
    {
        //ACCION DE LISTAR TODOS LOS MODULOS
        [HttpPost]
        public string lista_modulos()
        {
            List<Modulos> modulos = new List<Modulos>();
            modulos = new ModulosBLL().obtenerModulos();

            string select = "<option value='0'>Selecciona el módulo</option>";

            for (int i = 0; i < modulos.Count; i++)
            {
                select = select + "<option value=" + modulos[i].idModulo + ">" + modulos[i].nombreModulo + "</option>";
            }

            return select;
        }

        //ACCION DE LISTAR TODOS LOS MODULOS EN LISTA
        [HttpGet]
        public JsonResult obtener_modulos()
        {
            List<Modulos> modulos = new List<Modulos>();
            modulos = new ModulosBLL().obtenerModulos();

            return Json(new
            {
                data = modulos
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDADO DE MODULO
        [HttpPost]
        public JsonResult guardar_modulo(Modulos modulo)
        {
            modulo.fechaActualizacion = DateTime.Now;
            modulo.fechaCreacion = DateTime.Now;
            modulo.usuarioCreacion = Convert.ToInt32(Session["usuarioId"].ToString());

            if (modulo.nombreModulo == null)
            {
                modulo.nombreModulo = "";
            }
            if (modulo.descripcion == null)
            {
                modulo.descripcion = "";
            }

            int idModulo = new ModulosBLL().guardarModulo(modulo);

            return Json(new
            {
                data = idModulo
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE ACTUALIZADO DE CORREO
        [HttpPost]
        public JsonResult actualizar_modulo(Modulos modulo)
        {
            modulo.fechaActualizacion = DateTime.Now;

            if (modulo.nombreModulo == null)
            {
                modulo.nombreModulo = "";
            }
            if (modulo.descripcion == null)
            {
                modulo.descripcion = "";
            }

            int idModulo = new ModulosBLL().actualizarModulo(modulo);

            return Json(new
            {
                data = idModulo
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

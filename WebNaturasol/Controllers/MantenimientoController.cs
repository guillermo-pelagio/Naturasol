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

        [HttpPost]
        public JsonResult guardar_linea(LineaMtto lineasMTTO)
        {
            lineasMTTO.idUsuario = Convert.ToInt32(Session["usuarioId"]);
            int idLinea = mantenimientoBLL.guardarLinea(lineasMTTO);

            return Json(new
            {
                data = idLinea
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_lineas()
        {
            List<LineaMtto> lineas = new List<LineaMtto>();

            lineas = mantenimientoBLL.obtenerLineas();

            for (int i = 0; i < lineas.Count; i++)
            {
                lineas[i].acciones = "<button type='button' id='btnEditar" + lineas[i].idLinea + "' onclick='validarConsumo(" + lineas[i].idLinea + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnEliminarLinea" + lineas[i].idLinea + "' onclick='validarConsumo(" + lineas[i].idLinea + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
            }

            var jsonResult = Json(new
            {
                data = lineas
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult guardar_maquina(MaquinasMtto maquinaMTTO)
        {
            maquinaMTTO.idUsuario = Convert.ToInt32(Session["usuarioId"]);
            int idMaquina = mantenimientoBLL.guardarMaquina(maquinaMTTO);

            return Json(new
            {
                data = idMaquina
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string obtener_linas_maquinas()
        {
            List<LineaMtto> lineas = new List<LineaMtto>();
            lineas = mantenimientoBLL.obtenerLineas();

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < lineas.Count; i++)
            {
                select = select + "<option value=" + lineas[i].idLinea + ">" + lineas[i].nombreLinea + " </option>";
            }

            return select;
        }

        [HttpGet]
        public JsonResult obtener_maquinas()
        {
            List<MaquinasMtto> maquinas = new List<MaquinasMtto>();

            maquinas = mantenimientoBLL.obtenerMaquinas();

            for (int i = 0; i < maquinas.Count; i++)
            {
                maquinas[i].acciones = "<button type='button' id='btnEditar" + maquinas[i].idMaquina + "' onclick='validarConsumo(" + maquinas[i].idMaquina + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnEliminarLinea" + maquinas[i].idMaquina + "' onclick='validarConsumo(" + maquinas[i].idMaquina + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
            }

            var jsonResult = Json(new
            {
                data = maquinas
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult guardar_personal(PersonalMtto personalMTTOS)
        {
            personalMTTOS.idUsuario = Convert.ToInt32(Session["usuarioId"]);
            int idPersonal = mantenimientoBLL.guardarPersonal(personalMTTOS);

            return Json(new
            {
                data = idPersonal
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_personal()
        {
            List<PersonalMtto> personal = new List<PersonalMtto>();

            personal = mantenimientoBLL.obtenerPersonal();

            for (int i = 0; i < personal.Count; i++)
            {
                personal[i].acciones = "<button type='button' id='btnEditar" + personal[i].idPersonal + "' onclick='validarConsumo(" + personal[i].idPersonal + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnEliminarLinea" + personal[i].idPersonal + "' onclick='validarConsumo(" + personal[i].idPersonal + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
            }

            var jsonResult = Json(new
            {
                data = personal
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_equipos_mtto()
        {
            List<MaquinasMtto> maquinas = new List<MaquinasMtto>();
            maquinas = mantenimientoBLL.obtenerMaquinas();

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < maquinas.Count; i++)
            {
                select = select + "<option value=" + maquinas[i].idMaquina + ">" + maquinas[i].nombreMaquina + " </option>";
            }

            return select;
        }

        [HttpGet]
        public JsonResult obtener_mantenimiento()
        {
            List<Mantenimiento> mantenimiento = new List<Mantenimiento>();

            mantenimiento = mantenimientoBLL.obtenerMantenimiento();

            for (int i = 0; i < mantenimiento.Count; i++)
            {
                mantenimiento[i].acciones = "<button type='button' id='btnEditar" + mantenimiento[i].idMantenimiento + "' onclick='validarConsumo(" + mantenimiento[i].idMantenimiento + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnEliminarLinea" + mantenimiento[i].idMantenimiento + "' onclick='validarConsumo(" + mantenimiento[i].idMantenimiento + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
            }

            var jsonResult = Json(new
            {
                data = mantenimiento
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult guardar_mtto(Mantenimiento dataMTTO)
        {
            dataMTTO.idUsuario = Convert.ToInt32(Session["usuarioId"]);
            int idMtto = mantenimientoBLL.guardarMantenimiento(dataMTTO);

            return Json(new
            {
                data = idMtto
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class RRHHController : Controller
    {
        EmpleadosBLL empleadosBLL = new EmpleadosBLL();

        // GET: RRHH
        public ActionResult ausencias()
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

        public ActionResult asistencia()
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

        //ACCION DE GUARDADO
        [HttpPost]
        public int guardar_ausencia(Empleado empleado)
        {
            int idRegistro = -1;
            if (Convert.ToString(Session["numeroEmpleado"]) != "" || (empleado.numeroEmpleado != "" && empleado.numeroEmpleado != null))
            {
                if (Convert.ToString(Session["numeroEmpleado"]) != "")
                {
                    empleado.numeroEmpleado = Convert.ToString(Session["numeroEmpleado"]);
                }
                else if ((empleado.numeroEmpleado != "") && empleado.numeroEmpleado != null)
                {
                    empleado.numeroEmpleado = empleado.numeroEmpleado;
                }

                idRegistro = empleadosBLL.guardarAusencia(empleado);
            }
            else
            {
                idRegistro = -2;
            }

            return idRegistro;
        }

        //ACCION DE GUARDADO
        [HttpPost]
        public JsonResult guardar_asistencia(Empleado empleado)
        {
            int idRegistro = empleadosBLL.guardarAsistencia(empleado);

            string s = "";

            return Json(new
            {
                data = idRegistro
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult obtener_asistencia()
        {
            List<Empleado> empleados = new List<Empleado>();
            empleados = empleadosBLL.obtenerAsistencia();

            for (int i = 0; i < empleados.Count; i++)
            {
                if(Convert.ToDateTime(empleados[i].fechaRegistro).Hour<12)
                {
                    empleados[i].tipo = "D";
                }
                else
                {
                    empleados[i].tipo = "C";
                }
                
                empleados[i].ubicacion = "SATELITE";
            }

            return Json(new
            {
                data = empleados
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_ausencias()
        {
            List<Ausencia> empleados = new List<Ausencia>();
            empleados = empleadosBLL.obtenerAusencia();

            for (int i = 0; i < empleados.Count; i++)
            {
                if (empleados[i].estatus == "")
                {
                    empleados[i].acciones = "<button type='button' id='btnDetalle" + empleados[i].idAusencia + "' onclick='validarConsumo(" + empleados[i].idAusencia + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-consumo" + empleados[i].idAusencia + "' onclick='validarConsumo(" + empleados[i].idAusencia + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else
                {
                    empleados[i].acciones = "";
                }
            }

            return Json(new
            {
                data = empleados
            }, JsonRequestBehavior.AllowGet);
        }

    }
}

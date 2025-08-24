using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class UsuariosController : Controller
    {
        //VISTA DE LISTA DE USUARIOS
        [Authorize]
        public ActionResult usuarios()
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
        public JsonResult obtener_usuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            usuarios = new UsuariosBLL().obtenerUsuarios();

            return Json(new
            {
                data = usuarios
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDAR USUARIOS
        [HttpPost]
        public int guardar_usuarios(Usuarios usuario)
        {
            if (Session["usuarioId"] != null)
            {
                usuario.usuarioCreacion = Convert.ToInt32(Session["usuarioId"].ToString());
            }
            else
            {
                usuario.usuarioCreacion = 0;
            }

            int idUsuario = new UsuariosBLL().guardarUsuario(usuario);

            return idUsuario;
        }

        //ACCION DE GUARDAR USUARIOS
        [HttpGet]
        public JsonResult obtener_usuario_detalle()
        {
            Usuarios usuarioResultado = new UsuariosBLL().obtenerUsuarioDetalle(Convert.ToInt32(Session["usuarioId"]));

            return Json(new
            {
                data = usuarioResultado
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE RECUPERAR CONTRASEÑA
        [HttpPost]
        public int recuperar_contrasena(Usuarios usuario)
        {
            return new UsuariosBLL().recuperarContrasena(usuario);
        }

        //ACCION DE ACTUALIZAR USUARIOS
        [HttpPost]
        public JsonResult actualizar_usuarios(Usuarios usuario)
        {
            int estatus = usuario.estatus;
            usuario.estatus = 1;
            int idUsuario = new UsuariosBLL().actualizarUsuario(usuario);

            return Json(new
            {
                data = idUsuario
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE LISTAR TODOS LOS USUARIOS
        [HttpPost]
        public string lista_usuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            usuarios = new UsuariosBLL().obtenerUsuarios();

            string select = "<option value='0'>Selecciona el usuario</option>";

            for (int i = 0; i < usuarios.Count; i++)
            {
                select = select + "<option value=" + usuarios[i].idUsuario + ">" + usuarios[i].nombreCompleto + "</option>";
            }

            return select;
        }

        //ACCION DE LISTAR TODOS LOS USUARIOS
        [HttpPost]
        public string lista_usuarios_sistemas()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            usuarios = new UsuariosBLL().obtenerUsuariosSistemas();

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < usuarios.Count; i++)
            {
                select = select + "<option value=" + usuarios[i].idUsuario + ">" + usuarios[i].nombreCompleto + "</option>";
            }

            return select;
        }

        //ACCION DE VALIDAR SESION ACTIVA
        [HttpPost]
        public int validar_sesion()
        {
            if (Session["usuarioId"] != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

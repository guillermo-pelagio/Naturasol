using CapaEntidades;
using CapaNegocios;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace WebArribosPlanta.Controllers
{
    public class InicioController : Controller
    {
        UsuariosBLL usuariosBLL = new UsuariosBLL();
        VentasBLL ventasBLL = new VentasBLL();

        //VISTA DASHBOARD
        [Authorize]
        public ActionResult inicio()
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

        //VISTA DE INICIO DE SESION
        public ActionResult pedido_comedor()
        {

            return View();

        }

        //VISTA DE INICIO DE SESION
        public ActionResult login()
        {
            if (Session["usuarioId"] != null)
            {
                return RedirectToAction("consultar_ticket", "tickets");
            }
            else
            {
                return View();
            }
        }

        //VISTA DE REGISTRO DE USUARIO
        public ActionResult registro()
        {
            return View();
        }

        //VISTA DE REGISTRO DE USUARIO
        public ActionResult recuperarcontrasena()
        {
            return View();
        }

        //VISTA DE CAMBIO DE CONTRASEÑA DE USUARIO
        public ActionResult mi_cuenta()
        {
            if ((Session["usuarioId"]) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("error403", "error");
            }
        }

        public ActionResult error()
        {
            return View();
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return true;
        }

        //ACCION DE INICIO DE SESION
        [HttpPost]
        public int inicio_sesion(Usuarios usuario)
        {
            UsuariosBLL usuariosBLL = new UsuariosBLL();
            SesionUsuario sesion = usuariosBLL.inicioSesion(usuario);

            if (IsReCaptchValid())
            {
                if ((sesion.estatus == 1))
                {
                    if (sesion.correo != null)
                    {
                        if ((sesion.accesoPublico == "1") || (usuario.accesoPublico == "1"))
                        {
                            usuario.idUsuario = sesion.idUsuario;
                            int idUsuario = new UsuariosBLL().actualizarUsuarioInicio(usuario);

                            SesionUsuarioGraficos sesionGraficos = usuariosBLL.obtenerInformacionGraficos(usuario);
                            SesionUsuario sesionPermisos = usuariosBLL.obtenerPermisos(usuario);
                            SesionUsuario sesionPermisosArea = usuariosBLL.permisosArea(usuario);

                            FormsAuthentication.SetAuthCookie(sesion.correo, false);
                            Session["permisos"] = sesionPermisos.permisos;
                            Session["permisosArea"] = sesionPermisosArea.permisosArea;

                            Session["usuarioId"] = sesion.idUsuario;
                            Session["departamento"] = sesion.departamento;
                            Session["estatus"] = sesion.estatus;
                            Session["empleado"] = sesion.empleado;

                            Session["nombreCompleto"] = sesion.nombreCompleto;
                            Session["apellidoPaterno"] = sesion.apellidoPaterno;
                            Session["nombre"] = sesion.nombre;
                            Session["apellidoMaterno"] = sesion.apellidoMaterno;

                            Session["correo"] = sesion.correo;
                            Session["nameUsuario"] = sesion.nameUsuario;
                            Session["contrasena"] = sesion.contrasena;
                            Session["ubicacion"] = sesion.ubicacion;
                            Session["contrasena"] = sesion.contrasena;
                            Session["puesto"] = sesion.puesto;
                            Session["numeroEmpleado"] = sesion.numeroEmpleado;
                            Session["sociedad"] = 0;
                            Session["idResponsiva"] = null;
                            Session["fecha1"] = null;
                            Session["fecha2"] = null;
                            Session["almacen"] = null;
                            Session["lote"] = null;
                            Session["articulo"] = null;
                            Session["descripcion"] = null;
                            Session["OFConsultar"] = null;
                            Session["OFTipoContabilizacion"] = null;

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////                        

                            Session["cotizacionTotal"] = sesionGraficos.cotizacionTotal;
                            Session["cotizacionAprobado"] = sesionGraficos.cotizacionAprobado;
                            Session["cotizacionRechazado"] = sesionGraficos.cotizacionRechazado;
                            Session["cotizacionRevision"] = sesionGraficos.cotizacionRevision;
                            Session["cotizacionPresentar"] = sesionGraficos.cotizacionPresentar;
                            Session["cotizacionSustituido"] = sesionGraficos.cotizacionSustituido;
                            Session["cotizacionMuestras"] = sesionGraficos.cotizacionMuestras;
                            Session["cotizacionSinMovimiento"] = sesionGraficos.cotizacionSinMovimiento;

                            Session["cotizacionPAprobado"] = sesionGraficos.cotizacionPAprobado;
                            Session["cotizacionPRechazado"] = sesionGraficos.cotizacionPRechazado;
                            Session["cotizacionPRevision"] = sesionGraficos.cotizacionPRevision;
                            Session["cotizacionPPresentar"] = sesionGraficos.cotizacionPPresentar;
                            Session["cotizacionPSustituido"] = sesionGraficos.cotizacionPSustituido;
                            Session["cotizacionPMuestras"] = sesionGraficos.cotizacionPMuestras;
                            Session["cotizacionPSinMovimiento"] = sesionGraficos.cotizacionPSinMovimiento;


                            return 1;
                        }
                        else
                        {
                            return 10;
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                if ((sesion.estatus == 3))
                {
                    if (sesion.correo != null)
                    {
                        FormsAuthentication.SetAuthCookie(sesion.correo, false);
                        Session["usuarioId"] = sesion.idUsuario;
                        Session["responsivaIdUsuario"] = null;
                        Session["departamento"] = sesion.departamento;
                        Session["estatus"] = sesion.estatus;
                        Session["nombreCompleto"] = sesion.nombreCompleto;
                        Session["empleado"] = null;

                        Session["apellidoPaterno"] = sesion.apellidoPaterno;
                        Session["nombre"] = sesion.nombre;
                        Session["apellidoMaterno"] = sesion.apellidoMaterno;

                        Session["correo"] = sesion.correo;
                        Session["contrasena"] = sesion.contrasena;
                        Session["ubicacion"] = sesion.ubicacion;
                        Session["contrasena"] = sesion.contrasena;
                        Session["puesto"] = sesion.puesto;
                        Session["numeroEmpleado"] = sesion.numeroEmpleado;
                        Session["sociedad"] = 0;
                        Session["idResponsiva"] = null;
                        Session["almacen"] = null;
                        Session["lote"] = null;
                        Session["articulo"] = null;
                        Session["descripcion"] = null;
                        Session["OFConsultar"] = null;
                        Session["OFTipoContabilizacion"] = null;
                        Session["fecha1"] = null;
                        Session["fecha2"] = null;

                        Session["cotizacionAprobado"] = null;
                        Session["cotizacionRechazado"] = null;
                        Session["cotizacionRevision"] = null;
                        Session["cotizacionPresentar"] = null;
                        Session["cotizacionSustituido"] = null;
                        Session["cotizacionMuestras"] = null;
                        Session["cotizacionSinMovimiento"] = null;

                        UsuariosBLL x = new UsuariosBLL();
                        return 4;
                    }
                    else
                    {
                        return 2;
                    }
                }
                if (sesion.estatus == 2)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 99;
            }

        }

        //ACCION DE CERRAR SESION
        public ActionResult cerrar_sesion()
        {
            FormsAuthentication.SignOut();
            Session["usuarioId"] = null;
            Session["estatus"] = null;
            Session["responsivaIdUsuario"] = null;
            Session["departamento"] = null;
            Session["nombreCompleto"] = null;
            Session["empleado"] = null;

            Session["apellidoPaterno"] = null;
            Session["nombre"] = null;
            Session["apellidoMaterno"] = null;

            Session["numeroEmpleado"] = null;

            Session["fecha1"] = null;
            Session["fecha2"] = null;
            Session["correo"] = null;
            Session["contrasena"] = null;
            Session["ubicacion"] = null;
            Session["contrasena"] = null;
            Session["extension"] = null;
            Session["puesto"] = null;
            Session["sociedad"] = null;
            Session["almacen"] = null;
            Session["lote"] = null;
            Session["articulo"] = null;
            Session["descripcion"] = null;
            Session["OFConsultar"] = null;
            Session["OFTipoContabilizacion"] = null;
            return RedirectToAction("login", "Inicio");
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
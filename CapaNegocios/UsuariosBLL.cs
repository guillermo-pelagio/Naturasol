using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class UsuariosBLL
    {
        private static Random random = new Random();

        UsuariosDAL usuariosDAL = new UsuariosDAL();

        //METODO DE OBTENER USUARIOS
        public List<Usuarios> obtenerUsuarios()
        {
            return usuariosDAL.obtenerUsuarios();
        }

        //METODO DE OBTENER USUARIOS DE SISTEMAS
        public List<Usuarios> obtenerUsuariosSistemas()
        {
            return usuariosDAL.obtenerUsuariosSistemas();
        }

        //METODO PARA OBTENER DETALLE DEL USUARIO POR ID
        public Usuarios obtenerUsuarioDetalle(int idUsuario)
        {
            return usuariosDAL.obtenerUsuarioDetalle(idUsuario);
        }

        /*
        //METODO PARA OBTENER DETALLE DEL USUARIO POR ID -RESPONSIVA-
        public List<Responsiva> obtenerUsuarioDetalleResponsiva(int idUsuario)
        {
            return usuariosDAL.obtenerUsuarioDetalleResponsiva(idUsuario);
        }
        */

        //METODO PARA OBTENER DETALLE DEL USUARIO POR CORREO
        public Usuarios obtenerUsuarioDetallexCorreo(string correo)
        {
            return usuariosDAL.obtenerUsuarioDetallexCorreo(correo);
        }

        //METODO PARA RECUPERAR UNA CONTRASEÑA
        public int recuperarContrasena(Usuarios usuario)
        {
            CorreosBLL correoBLL = new CorreosBLL();

            Usuarios usuarioEncontrado = usuariosDAL.recuperarContrasena(usuario.correo);

            if (usuarioEncontrado.idUsuario != 0)
            {
                string nueva_contrasena = contrasenaAleatoria(10);
                correoBLL.enviarCorreoRecuperacion("Recupera tu contraseña", leerPlantilla(usuarioEncontrado.nombre + " " + usuarioEncontrado.apellidoPaterno + " " + usuarioEncontrado.apellidoMaterno, " tu nueva contraseña para el portal interno es la siguiente: ", nueva_contrasena, "Cambiala en el próximo inicio de sesión"));
                usuarioEncontrado.contrasena = nueva_contrasena;
                usuarioEncontrado.estatus = 3;
                actualizarUsuario(usuarioEncontrado);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //METODO PARA GENERAR UNA CONTRASEÑA ALEATORIA
        public static string contrasenaAleatoria(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //PLANTILLA DE CORREO
        public static string leerPlantilla(string parrafo1, string parrafo2, string parrafo3, string parrafo4)
        {
            string body = "<!doctype html> <html> <head> <meta name='viewport' content='width=device-width, initial-scale=1.0' /> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /> <title>Simple Transactional Email</title> <style> img { border: none; -ms-interpolation-mode: bicubic; max-width: 100%; } body { background-color: #f6f6f6; font-family: sans-serif; -webkit-font-smoothing: antialiased; font-size: 14px; line-height: 1.4; margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; } table { border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%; } table td { font-family: sans-serif; font-size: 14px; vertical-align: top; } .body { background-color: #f6f6f6; width: 100%; } .container { display: block; margin: 0 auto !important; /* makes it centered */ max-width: 580px; padding: 10px; width: 580px; } .content { box-sizing: border-box; display: block; margin: 0 auto; max-width: 580px; padding: 10px; } .main { background: #ffffff; border-radius: 3px; width: 100%; } .wrapper { box-sizing: border-box; padding: 20px; } .content-block { padding-bottom: 10px; padding-top: 10px; } .footer { clear: both; margin-top: 10px; text-align: center; width: 100%; } .footer td, .footer p, .footer span, .footer a { color: #999999; font-size: 12px; text-align: center; } h1, h2, h3, h4 { color: #000000; font-family: sans-serif; font-weight: 400; line-height: 1.4; margin: 0; margin-bottom: 30px; } h1 { font-size: 35px; font-weight: 300; text-align: center; text-transform: capitalize; } p, ul, ol { font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; margin-bottom: 15px; } p li, ul li, ol li { list-style-position: inside; margin-left: 5px; } a { color: #3498db; text-decoration: underline; } .btn { box-sizing: border-box; width: 100%; } .btn > tbody > tr > td { padding-bottom: 15px; } .btn table { width: auto; } .btn table td { background-color: #ffffff; border-radius: 5px; text-align: center; } .btn a { background-color: #ffffff; border: solid 1px #3498db; border-radius: 5px; box-sizing: border-box; color: #3498db; cursor: pointer; display: inline-block; font-size: 14px; font-weight: bold; margin: 0; padding: 12px 25px; text-decoration: none; text-transform: capitalize; } .btn-primary table td { background-color: #3498db; } .btn-primary a { background-color: #3498db; border-color: #3498db; color: #ffffff; } .last { margin-bottom: 0; } .first { margin-top: 0; } .align-center { text-align: center; } .align-right { text-align: right; } .align-left { text-align: left; } .clear { clear: both; } .mt0 { margin-top: 0; } .mb0 { margin-bottom: 0; } .preheader { color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0; } .powered-by a { text-decoration: none; } hr { border: 0; border-bottom: 1px solid #f6f6f6; margin: 20px 0; } @media only screen and (max-width: 620px) { table.body h1 { font-size: 28px !important; margin-bottom: 10px !important; } table.body p, table.body ul, table.body ol, table.body td, table.body span, table.body a { font-size: 16px !important; } table.body .wrapper, table.body .article { padding: 10px !important; } table.body .content { padding: 0 !important; } table.body .container { padding: 0 !important; width: 100% !important; } table.body .main { border-left-width: 0 !important; border-radius: 0 !important; border-right-width: 0 !important; } table.body .btn table { width: 100% !important; } table.body .btn a { width: 100% !important; } table.body .img-responsive { height: auto !important; max-width: 100% !important; width: auto !important; } } @media all { .ExternalClass { width: 100%; } .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; } .apple-link a { color: inherit !important; font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; text-decoration: none !important; } #MessageViewBody a { color: inherit; text-decoration: none; font-size: inherit; font-family: inherit; font-weight: inherit; line-height: inherit; } .btn-primary table td:hover { background-color: #34495e !important; } .btn-primary a:hover { background-color: #34495e !important; border-color: #34495e !important; } } </style> </head> <body> <!--<span class='preheader'>preheader</span>--> <table role='presentation' border='0' cellpadding='0' cellspacing='0' class='body'> <tr> <td>&nbsp;</td> <td class='container'> <div class='content'> <table role='presentation' class='main'> <tr> <td class='wrapper'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tr> <td> <p>Buen día</p> <p><b>parrafo1</b> parrafo2 <b>parrafo3</b></p> <p>parrafo4</p> <!--<table role='presentation' border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'> <tbody> <tr> <td align='left'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tbody> <tr> <td> <a>Call To Action</a> </td> <td> <a>Call To Action</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table>--> <!--<p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p> <p>Good luck! Hope it works.</p>--> </td> </tr> </table> </td> </tr> </table> <div class='footer'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tr> <td class='content-block'> <span class='apple-link'>Cto. Circunvalacion Pte. 9A, Cd. Satélite, 53100 Naucalpan de Juárez, Méx.</span> <br> Por favor, no responda a este mensaje, ha sido enviado de forma automática . </td> </tr> <tr> <td class='content-block powered-by'> Visita nuestras páginas <br> <br> <a href='http://naturasol.com.mx' target='_blank'>naturasol.com.mx</a><br> <a href='http://mielmex.com' target='_blank'>mielmex.com</a>. </td> </tr> </table> </div> </div> </td> <td>&nbsp;</td> </tr> </table> </body> </html>";
            body = body.Replace("parrafo1", parrafo1);
            body = body.Replace("parrafo2", parrafo2);
            body = body.Replace("parrafo3", parrafo3);
            body = body.Replace("parrafo4", parrafo4);
            return body;
        }

        //METODO DE GUARDADO DEL USUARIO
        public int guardarUsuario(Usuarios usuario)
        {
            int resultado = 0;
            usuario.fechaCreacion = DateTime.Now;
            usuario.fechaActualizacion = DateTime.Now;

            if (usuario.telefono == null)
            {
                usuario.telefono = "";
            }
            if (usuario.extension == null)
            {
                usuario.extension = "";
            }
            if (usuario.puesto == null)
            {
                usuario.puesto = "";
            }

            if (obtenerUsuarioDetallexCorreo(usuario.correo).idUsuario == 0)
            {
                resultado = usuariosDAL.guardarUsuario(usuario);
                if (usuario.estatus == 2 && resultado == 0)
                {
                    CorreosBLL correoBLL = new CorreosBLL();
                    correoBLL.enviarCorreoRecuperacion("Registro de usuario", leerPlantilla(usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno, " se ha registrado en el sistema interno de tickets.", "", "El área de sistemas revisará la información y activará el usuario en caso de que todo este correcto."));
                }
                resultado = 1;
            }
            else
            {
                resultado = 0;
            }

            return resultado;
        }

        //METODO DE ACTUALIZADO DEL USUARIO
        public int actualizarUsuario(Usuarios usuario)
        {
            usuario.fechaActualizacion = DateTime.Now;

            if (usuario.telefono == null)
            {
                usuario.telefono = "";
            }
            if (usuario.extension == null)
            {
                usuario.extension = "";
            }
            if (usuario.puesto == null)
            {
                usuario.puesto = "";
            }

            return usuariosDAL.actualizarUsuario(usuario);
        }

        //METODO DE ACTUALIZADO DEL USUARIO
        public int actualizarUsuarioInicio(Usuarios usuario)
        {
            usuario.ultimoInicioSesion = DateTime.Now;
            usuario.maquina = Dns.GetHostName();

            return usuariosDAL.actualizarUsuarioInicio(usuario);
        }


        //METODO DE VALIDACION DE INICIO DE SESION
        public SesionUsuario inicioSesion(Usuarios usuario)
        {
            return usuariosDAL.inicioSesion(usuario);
        }

        public SesionUsuario obtenerPermisos(Usuarios usuario)
        {
            return usuariosDAL.obtenerPermisos(usuario);
        }

        public SesionUsuario permisosArea(Usuarios usuario)
        {
            return usuariosDAL.permisosArea(usuario);
        }

        //METODO DE OBTENCION DE DATOS PARA GRAFICAR
        public SesionUsuarioGraficos obtenerInformacionGraficos(Usuarios usuario)
        {
            return usuariosDAL.obtenerInformacionGraficos(usuario);
        }

        public bool conexionSAP()
        {
            return false;
        }
    }
}
using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace CapaNegocios
{
    public class CorreosBLL
    {
        CorreosDAL correosDAL = new CorreosDAL();

        //METODO PARA EL ENVIO DE CORREO
        public void enviarCorreo(string subject, string body, string to, string CC)
        {
            string from = "sistemas@naturasol.com.mx";
            string displayName = "Sistemas";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                to = to.Replace(';', ',');
                to = to.Replace('"', ' ');
                mail.To.Add(to);
                if (CC != null)
                {
                    CC = CC.Replace(';', ',');
                    CC = CC.Replace('"', ' ');
                    mail.CC.Add(CC);
                }
                mail.Bcc.Add("guillermo.pelagio@naturasol.com.mx");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("mail.naturasol.com.mx", 587);
                client.Credentials = new NetworkCredential(from, "JSistemas019$");
                client.EnableSsl = false;

                client.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        //CORREO DE NOTIFICACIONES DE PAGOS
        public void enviarCorreoNotificador(String subject, String body, string sociedad, string to)
        {
            if (to != null)
            {
                string from;
                string displayName;

                if (sociedad.Contains("Naturasol"))
                {
                    from = "pagos@naturasol.com.mx";
                    displayName = "Pagos Naturasol";
                }
                else
                {
                    //from = "pagos@mielmex.com";
                    from = "pagos@naturasol.com.mx";
                    displayName = "Pagos Mielmex";
                }

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(from, displayName);
                    to = to.Replace(';', ',');
                    mail.To.Add(to);
                    Console.WriteLine(to);
                    //mail.To.Add("guillermo.pelagio@naturasol.com.mx");
                    mail.Bcc.Add("guillermo.pelagio@naturasol.com.mx");
                    mail.Bcc.Add("cuentasxpagar@mielmex.com");
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    SmtpClient client;

                    if (sociedad.Contains("Naturasol"))
                    {
                        client = new SmtpClient("mail.naturasol.com.mx", 587);
                        client.Credentials = new NetworkCredential(from, "Pagos2023$");
                    }
                    else
                    {
                        //client = new SmtpClient("mail.mielmex.com", 26);
                        //client.Credentials = new NetworkCredential(from, "Pagos2023$");
                        client = new SmtpClient("mail.naturasol.com.mx", 587);
                        client.Credentials = new NetworkCredential(from, "Pagos2023$");
                    }

                    client.EnableSsl = false;

                    client.Send(mail);
                }
                catch (Exception ex)
                {

                }
            }
        }

        //METODO PARA EL ENVIO DE CORREO DE LOS TICKETS
        public void enviarCorreoTicket(String subject, String body, String correoUsuario)
        {
            string from = "tickets.sistemas@naturasol.com.mx";
            string displayName = "Tickets sistemas";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add("tickets.sistemas@naturasol.com.mx");
                mail.CC.Add(correoUsuario);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("mail.naturasol.com.mx", 587);
                client.Credentials = new NetworkCredential(from, "Tickets23$");
                client.EnableSsl = false;

                client.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        //METODO PARA EL ENVIO DE CORREO DE LOS TICKETS
        public void enviarCorreoRecuperacion(String subject, String body)
        {
            string from = "sistemas@naturasol.com.mx";
            string displayName = "Sistemas";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add("sistemas@naturasol.com.mx");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("mail.naturasol.com.mx", 587);
                client.Credentials = new NetworkCredential(from, "JSistemas019$");
                client.EnableSsl = false;

                client.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        //METODO PARA EL ENVIO DE CORREO DE LAS SOLICITUDES DE OC
        public void enviarNotificacionLiberacionCompra(String subject, String body, List<OrdenCompra> detalleOC, string destinatarios)
        {
            string from = "sistemas@naturasol.com.mx";
            string displayName = "Sistemas";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                Console.WriteLine(destinatarios);
                mail.To.Add(destinatarios);
                mail.CC.Add("guillermo.pelagio@naturasol.com.mx");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                for (int k = 0; k < detalleOC.Count; k++)
                {
                    string cadena = detalleOC[k].trgtPath.ToString();
                    Attachment data = new Attachment(detalleOC[k].trgtPath.ToString(), MediaTypeNames.Application.Octet);
                    mail.BodyEncoding = UTF8Encoding.UTF8;
                    mail.Attachments.Add(data);
                }

                SmtpClient client = new SmtpClient("mail.naturasol.com.mx", 587);
                client.Credentials = new NetworkCredential(from, "JSistemas019$");
                client.EnableSsl = false;

                client.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        //METODO PARA EL ENVIO DE CORREO DE LAS SOLICITUDES DE OC
        public void enviarNotificacionAdjunto(String subject, String body, String detalleOC, string destinatarios)
        {
            string from = "sistemas@naturasol.com.mx";
            string displayName = "Sistemas";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                Console.WriteLine(destinatarios);
                mail.To.Add(destinatarios);
                mail.CC.Add("guillermo.pelagio@naturasol.com.mx");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                
                    string cadena = detalleOC;
                    Attachment data = new Attachment(detalleOC, MediaTypeNames.Application.Octet);
                    mail.BodyEncoding = UTF8Encoding.UTF8;
                    mail.Attachments.Add(data);
                

                SmtpClient client = new SmtpClient("mail.naturasol.com.mx", 587);
                client.Credentials = new NetworkCredential(from, "JSistemas019$");
                client.EnableSsl = false;

                client.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        /*
        //METODO DE OBTENER CORREOS
        public List<Correos> obtenerCorreos()
        {
            return correosDAL.obtenerCorreos();
        }

        //METODO DE GUARDADO DEL USUARIO
        public int guardarCorreo(Correos correo)
        {
            if (obtenerCorreosCorreo(correo) < 1)
            {
                return correosDAL.guardarCorreo(correo);
            }
            else
            {
                return -1;
            }
        }
        
        //METODO DE ACTUALIZADO DEL USUARIO
        public int actualizarCorreo(Correos correo)
        {
            if (obtenerCorreosCorreo(correo) < 2)
            {
                return correosDAL.actualizarCorreo(correo);
            }
            else
            {
                return -1;
            }
        }
        
        //METODO DE VERIFICAR SI EXISTE LA CUENTA DE CORREO
        private int obtenerCorreosCorreo(Correos correo)
        {
            return correosDAL.obtenerCorreosCorreo(correo.correoElectronico);
        }
        */
    }
}

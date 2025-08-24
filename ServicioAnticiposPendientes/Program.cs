using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicioAnticiposPendientes
{
    class Program
    {
        static void Main(string[] args)
        {
            AnticiposBLL anticiposBLL = new AnticiposBLL();
            CorreosBLL correoBLL = new CorreosBLL();

            List<Anticipo> listaAnticiposAbiertas = new List<Anticipo>();
            listaAnticiposAbiertas = anticiposBLL.obtenerAnticiposAbiertas();
            if (listaAnticiposAbiertas.Count > 0)
            {
                string body = "";
                bool enviaCorreo = false;
                body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Anticipos por comprobar<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Los siguientes anticipos estan pendientes de comprobar. <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p><table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CODIGO CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>COMPRADOR<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA ANTICIPO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONTO TOTAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONEDA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONTO PENDIENTE<o:p></o:p><o:p></o:p></span></p> </td></tr>";

                string email = "";
                IEnumerable<string> emails = null;

                enviaCorreo = true;
                for (int j = 0; j < listaAnticiposAbiertas.Count; j++)
                {
                    body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFolio<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCodigoCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorNombreCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorComprador<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCreacion<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMonto<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMoneda<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPendiente<o:p></o:p></span></p> </td> </tr>";
                    body = body.Replace("valorSociedad", listaAnticiposAbiertas[j].Sociedad);
                    body = body.Replace("valorFolio", listaAnticiposAbiertas[j].NumeroAnticipo);
                    body = body.Replace("valorCodigoCliente", listaAnticiposAbiertas[j].CardCode);
                    body = body.Replace("valorNombreCliente", listaAnticiposAbiertas[j].CardName);
                    body = body.Replace("valorComprador", listaAnticiposAbiertas[j].SlpName);
                    body = body.Replace("valorCreacion", Convert.ToDateTime(listaAnticiposAbiertas[j].DocDate).ToShortDateString());
                    body = body.Replace("valorMonto", string.Format("{0:C}", Convert.ToDecimal(listaAnticiposAbiertas[j].TotalAnticipo)));
                    body = body.Replace("valorMoneda", listaAnticiposAbiertas[j].Moneda);
                    body = body.Replace("valorPendiente", string.Format("{0:C}", Convert.ToDecimal(listaAnticiposAbiertas[j].Pendiente)));
                    email = email + "," + listaAnticiposAbiertas[j].Email;
                    emails = email.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                }

                if (enviaCorreo)
                {
                    body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                    body = body.Replace("valor7", DateTime.Now.ToString());

                    email = "";
                    foreach (var entry in emails)
                    {
                        email = entry + "," + email;
                    }

                    email = email + "paola.posadas@naturasol.com.mx, brenda.murguia@naturasol.com.mx";

                    correoBLL.enviarCorreo("Anticipos pendientes de comprobar", body, email, "paola.posadas@naturasol.com.mx, cuentasxpagar@mielmex.com, gabriela.castillo@naturasol.com.mx, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                    //correoBLL.enviarCorreo("Anticipos pendientes de comprobar", body, "guillermo.pelagio@naturasol.com.mx", "guillermo.pelagio@naturasol.com.mx");
                }
            }
        }
    }
}

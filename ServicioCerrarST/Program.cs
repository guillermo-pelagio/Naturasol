using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioCerrarST
{
    class Program
    {
        static void Main(string[] args)
        {
            SolicitudTrasladoBLL solicitudTrasladoBLL = new SolicitudTrasladoBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<SolicitudTraslado> listaSTAbiertas = new List<SolicitudTraslado>();
                listaSTAbiertas = solicitudTrasladoBLL.obtenerSTAbiertas(basesDatos[i]);

                if (listaSTAbiertas.Count > 0)
                {

                    string body = "";
                    bool enviaCorreo = false;
                    body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>ST Abiertas<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Las siguientes ST estan abiertas. <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <p class=MsoNormal style='margin-bottom:12.0pt'> Las de fecha de vigencia vencida se cerraran hoy a las 23:00 hrs. <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA CREACION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA VIGENCIA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN ORIGEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN DESTINO<o:p></o:p><o:p></o:p></span></p> </td></tr>";
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);

                    for (int j = 0; j < listaSTAbiertas.Count; j++)
                    {
                        enviaCorreo = true;
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFolio<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCreacion<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorVigencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorOrigen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorDestino<o:p></o:p></span></p> </td> </tr>";
                        body = body.Replace("valorSociedad", basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL");
                        body = body.Replace("valorFolio", listaSTAbiertas[j].DocNum);
                        body = body.Replace("valorVigencia", Convert.ToDateTime(listaSTAbiertas[j].DocDueDate).ToShortDateString());
                        body = body.Replace("valorCreacion", Convert.ToDateTime(listaSTAbiertas[j].DocDate).ToShortDateString());
                        body = body.Replace("valorOrigen", listaSTAbiertas[j].Filler);
                        body = body.Replace("valorDestino", listaSTAbiertas[j].ToWhsCode);

                        if (DateTime.Now.Hour >= 23)
                        {
                            enviaCorreo = false;

                            if (DateTime.Compare(DateTime.Today.AddDays(0), Convert.ToDateTime(listaSTAbiertas[j].DocDate)) < 0)
                            {
                                Console.WriteLine("no cierro sdc" + listaSTAbiertas[j].DocNum + Convert.ToDateTime(listaSTAbiertas[j].DocDueDate).ToShortDateString());
                            }
                            else
                            {
                                Console.WriteLine("cierro sdc" + listaSTAbiertas[j].DocNum + Convert.ToDateTime(listaSTAbiertas[j].DocDueDate).ToShortDateString());
                                solicitudTrasladoBLL.cerrarSTAbiertas(listaSTAbiertas[j].DocEntry);
                            }
                        }
                    }

                    if (enviaCorreo)
                    {
                        body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                        body = body.Replace("valor7", DateTime.Now.ToString());

                        string sociedad = basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL";
                        correoBLL.enviarCorreo("ST Abiertas", body, "alejandra.bobadilla@mielmex.com, alejandra.vilchis@naturasol.com.mx, alejandro.hernandez@naturasol.com.mx, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx", null);
                    }

                    DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}

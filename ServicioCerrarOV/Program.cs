using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioCerrarOV
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesVentaBLL ordenesVentaBLL = new OrdenesVentaBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenVenta> listaVendedorOVAbiertas = new List<OrdenVenta>();
                listaVendedorOVAbiertas = ordenesVentaBLL.obtenerElaboradorOVAbiertas(basesDatos[i]);
                if (listaVendedorOVAbiertas.Count > 0)
                {
                    for (int k = 0; k < listaVendedorOVAbiertas.Count; k++)
                    {
                        List<OrdenVenta> listaOVAbiertas = new List<OrdenVenta>();
                        listaOVAbiertas = ordenesVentaBLL.obtenerOVAbiertas(basesDatos[i], listaVendedorOVAbiertas[k].SlpName);
                        if (listaOVAbiertas.Count > 0)
                        {
                            string body = "";
                            bool enviaCorreo = true;
                            body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>OV vencidas o a punto de vencer<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Las siguientes O.V. tienen fecha de vigencia vencida. <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <p class=MsoNormal style='margin-bottom:12.0pt'> <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>REFERENCIA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CODIGO CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>KAM<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA CREACION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA VIGENCIA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONTO TOTAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONEDA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD PENDIENTE<o:p></o:p><o:p></o:p></span></p> </td></tr>";
                            DIAPIBLL.conectarDIAPI(basesDatos[i]);

                            for (int j = 0; j < listaOVAbiertas.Count; j++)
                            {
                                if (listaVendedorOVAbiertas[k].E_Mail.Equals(listaOVAbiertas[j].E_Mail))
                                {

                                    body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFolio<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorReferencia<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCodigoCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorNombreCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorComprador<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCreacion<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorVigencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMonto<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMoneda<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPendiente<o:p></o:p></span></p> </td> </tr>";
                                    body = body.Replace("valorSociedad", basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL");
                                    body = body.Replace("valorFolio", listaOVAbiertas[j].DocNum);
                                    body = body.Replace("valorReferencia", listaOVAbiertas[j].NumAtCard);
                                    body = body.Replace("valorCodigoCliente", listaOVAbiertas[j].CardCode);
                                    body = body.Replace("valorNombreCliente", listaOVAbiertas[j].CardName);
                                    body = body.Replace("valorComprador", listaOVAbiertas[j].SlpName);
                                    body = body.Replace("valorVigencia", Convert.ToDateTime(listaOVAbiertas[j].TaxDate).ToShortDateString());
                                    body = body.Replace("valorCreacion", Convert.ToDateTime(listaOVAbiertas[j].DocDate).ToShortDateString());
                                    body = body.Replace("valorMonto", string.Format("{0:C}", Convert.ToDecimal(listaOVAbiertas[j].DocTotal)));
                                    body = body.Replace("valorMoneda", listaOVAbiertas[j].DocCur);
                                    body = body.Replace("valorPendiente", listaOVAbiertas[j].CantidadPendiente);

                                    if (DateTime.Now.Hour >= 9)
                                    {
                                        enviaCorreo = false;
                                        if (listaOVAbiertas[j].SlpName.Contains("ULISES VIGIL") || listaOVAbiertas[j].CardCode == "1122000081" || listaOVAbiertas[j].CardCode == "1122000020" || listaOVAbiertas[j].CardCode == "1122000147" || listaOVAbiertas[j].CardCode == "1122000069")
                                        {
                                            if (DateTime.Compare(DateTime.Today.AddDays(-10), Convert.ToDateTime(listaOVAbiertas[j].TaxDate)) < 0)
                                            {
                                                Console.WriteLine("no cierro of" + listaOVAbiertas[j].DocNum + Convert.ToDateTime(listaOVAbiertas[j].TaxDate).ToShortDateString());
                                            }
                                            else
                                            {
                                                Console.WriteLine("cierro of" + listaOVAbiertas[j].DocNum + Convert.ToDateTime(listaOVAbiertas[j].TaxDate).ToShortDateString());
                                                ordenesVentaBLL.cerrarOVAbiertas(listaOVAbiertas[j].DocEntry);
                                            }
                                        }
                                        else
                                        {
                                            if (listaOVAbiertas[j].MotNoEntrega != "")
                                            {
                                                if (DateTime.Compare(DateTime.Today.AddDays(-10), Convert.ToDateTime(listaOVAbiertas[j].TaxDate)) < 0)
                                                {
                                                    Console.WriteLine("no cierro of" + listaOVAbiertas[j].DocNum + Convert.ToDateTime(listaOVAbiertas[j].TaxDate).ToShortDateString());
                                                }
                                                else
                                                {
                                                    Console.WriteLine("cierro of" + listaOVAbiertas[j].DocNum + Convert.ToDateTime(listaOVAbiertas[j].TaxDate).ToShortDateString());
                                                    ordenesVentaBLL.cerrarOVAbiertas(listaOVAbiertas[j].DocEntry);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (enviaCorreo)
                            {
                                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                                body = body.Replace("valor7", DateTime.Now.ToString());

                                string sociedad = basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL";

                                if (listaVendedorOVAbiertas[k].E_Mail == null)
                                {
                                    //correoBLL.enviarCorreo("OV Vencidas / Proximas a vencer " + sociedad, body, "guillermo.pelagio@naturasol.com.mx", null);
                                }
                                else
                                {
                                    //correoBLL.enviarCorreo("OV Vencidas / Proximas a vencer " + sociedad, body, "guillermo.pelagio@naturasol.com.mx", null);
                                    //correoBLL.enviarCorreo("OV Vencidas / Proximas a vencer " + sociedad, body, listaVendedorOVAbiertas[k].E_Mail, "jenifer.castillo@naturasol.com.mx, rodrigo.lozano@naturasol.com.mx, admin.pedidos@naturasol.com.mx, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                                }
                            }

                            DIAPIBLL.desconectarDIAPI();
                        }
                    }
                }
            }
        }
    }
}
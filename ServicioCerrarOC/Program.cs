using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioCerrarOC
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
                listaCompradorOCAbiertas = ordenesCompraBLL.obtenerCompradorOCAbiertas(basesDatos[i]);
                if (listaCompradorOCAbiertas.Count > 0)
                {
                    for (int k = 0; k < listaCompradorOCAbiertas.Count; k++)
                    {
                        List<OrdenCompra> listaOCAbiertas = new List<OrdenCompra>();
                        listaOCAbiertas = ordenesCompraBLL.obtenerOCAbiertas(basesDatos[i], listaCompradorOCAbiertas[k].SlpName);

                        string body = "";
                        bool enviaCorreo = false;
                        body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>OC vencidas o a punto de vencer<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Las siguiente O.C. tienen fecha de vigencia vencida. <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <p class=MsoNormal style='margin-bottom:12.0pt'> <span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CODIGO CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>COMPRADOR<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA CREACION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA VIGENCIA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONTO TOTAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONEDA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>% PENDIENTE<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ANTICIPOS<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TIPO ORDEN<o:p></o:p><o:p></o:p></span></p> </td></tr>";
                        DIAPIBLL.conectarDIAPI(basesDatos[i]);

                        if (listaOCAbiertas.Count > 0)
                        {
                            enviaCorreo = true;
                            for (int j = 0; j < listaOCAbiertas.Count; j++)
                            {
                                body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFolio<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCodigoCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorNombreCliente<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorComprador<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCreacion<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorVigencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMonto<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMoneda<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPendiente<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorAnticipo<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorTipo<o:p></o:p></span></p> </td> </tr>";
                                body = body.Replace("valorSociedad", basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL");
                                body = body.Replace("valorFolio", listaOCAbiertas[j].DocNum);
                                body = body.Replace("valorCodigoCliente", listaOCAbiertas[j].CardCode);
                                body = body.Replace("valorNombreCliente", listaOCAbiertas[j].CardName);
                                body = body.Replace("valorComprador", listaOCAbiertas[j].SlpName);
                                body = body.Replace("valorVigencia", Convert.ToDateTime(listaOCAbiertas[j].TaxDate).ToShortDateString());
                                body = body.Replace("valorCreacion", Convert.ToDateTime(listaOCAbiertas[j].DocDate).ToShortDateString());
                                body = body.Replace("valorMonto", string.Format("{0:C}", Convert.ToDecimal(listaOCAbiertas[j].DocTotal)));
                                body = body.Replace("valorMoneda", listaOCAbiertas[j].DocCur);
                                body = body.Replace("valorPendiente", listaOCAbiertas[j].Pendiente);
                                body = body.Replace("valorAnticipo", listaOCAbiertas[j].Anticipo);
                                body = body.Replace("valorTipo", listaOCAbiertas[j].DocType);

                                if (DateTime.Now.Hour >= 19)
                                {
                                    enviaCorreo = false;
                                    /*
                                    ordenesCompraBLL.cerrarOCAbiertas(listaOCAbiertas[j].DocEntry);
                                    
                                    if (DateTime.Compare(DateTime.Today.AddDays(0), Convert.ToDateTime(listaOCAbiertas[j].TaxDate)) < 0)
                                    {
                                        Console.WriteLine("no cierro oc" + listaOCAbiertas[j].DocNum + Convert.ToDateTime(listaOCAbiertas[j].TaxDate).ToShortDateString());
                                    }
                                    else if (listaOCAbiertas[j].PendienteAnticipo is null)
                                    {
                                        Console.WriteLine("cierro oc" + listaOCAbiertas[j].DocNum + Convert.ToDateTime(listaOCAbiertas[j].TaxDate).ToShortDateString());
                                        ordenesCompraBLL.cerrarOCAbiertas(listaOCAbiertas[j].DocEntry);
                                    }
                                    else
                                    {
                                        if (listaOCAbiertas[j].PendienteAnticipo.Equals (""))
                                        {
                                            Console.WriteLine("cierro oc" + listaOCAbiertas[j].DocNum + Convert.ToDateTime(listaOCAbiertas[j].TaxDate).ToShortDateString());
                                            ordenesCompraBLL.cerrarOCAbiertas(listaOCAbiertas[j].DocEntry);
                                        }

                                        else if (Convert.ToDouble(listaOCAbiertas[j].PendienteAnticipo) == 0)
                                        {
                                            Console.WriteLine("cierro oc" + listaOCAbiertas[j].DocNum + Convert.ToDateTime(listaOCAbiertas[j].TaxDate).ToShortDateString());
                                            ordenesCompraBLL.cerrarOCAbiertas(listaOCAbiertas[j].DocEntry);
                                        }
                                    }
                                    */
                                }

                            }
                        }

                        if (enviaCorreo)
                        {
                            body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                            body = body.Replace("valor7", DateTime.Now.ToString());

                            string sociedad = basesDatos[i].Contains("Naturasol") ? "NATURASOL" : basesDatos[i].Contains("Mielmex") ? "MIELMEX" : "NOVAL";
                            correoBLL.enviarCorreo("OC Vencidas" + sociedad + " / " + listaCompradorOCAbiertas[k].SlpName, body, listaCompradorOCAbiertas[k].Email, "paola.posadas@naturasol.com.mx, brenda.murguia@naturasol.com.mx, jenifer.castillo@naturasol.com.mx, alejandro.duarte@naturasol.com.mx, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                            //correoBLL.enviarCorreo("OC Vencidas / Proximas a vencer " + sociedad + " / " + listaCompradorOCAbiertas[k].SlpName, body, "guillermo.pelagio@naturasol.com.mx", "guillermo.pelagio@naturasol.com.mx");                            
                        }

                        DIAPIBLL.desconectarDIAPI();
                    }
                }
            }
        }
    }
}
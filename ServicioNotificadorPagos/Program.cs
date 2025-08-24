using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioNotificadorPagos
{
    public class Program
    {
        static void Main(string[] args)
        {
            notificarPago();
        }

        private static void notificarPago()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            PagosEfectuadosBLL pagosEfectuadosBLL = new PagosEfectuadosBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };
            //string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval", "SBOMielmex", "SBOEvi", "SBONaturasol", "SBONoval" };

            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<PagosEfectuados> listaPagosEfectuadosNotificar = new List<PagosEfectuados>();
                listaPagosEfectuadosNotificar = pagosEfectuadosBLL.obtenerPagosNotificarBasico(basesDatos[i]);

                if (listaPagosEfectuadosNotificar.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaPagosEfectuadosNotificar.Count; j++)
                    {
                        //if ((listaPagosEfectuadosNotificar[j].DocNum == "58203") || (listaPagosEfectuadosNotificar[j].DocNum == "60155"))
                        {
                            if (listaPagosEfectuadosNotificar[j].PayNoDoc == "Y" && listaPagosEfectuadosNotificar[j].NoDocSum == 0)
                            {
                                string body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Envío automático confirmación de Pago a Proveedor<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'><span style='font-family: Palatino;color:black'>Estimado titular o representante de <strong><span style='font-family:Palatino;mso-bidi-font-family:Calibri'>valor1</span></strong>,<br> <br> <strong><i><span style='font-family:Palatino;mso-bidi-font-family:Calibri'>valor2</span></i></strong> le notifica la operación de pago de uno o más comprobantes fiscales listado(s) a continuación. Referencia interna de pago valor3, con fecha de aplicación <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'>valor4</span></strong>. Agradecemos anticipadamente la generación del Complemento de pago correspondiente para efectos fiscales.</span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO DE FACTURA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO FISCAL<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPORTE PAGADO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONEDA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TIPO DE CAMBIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPORTE PAGADO EN MXN<o:p></o:p></span></p> </td> </tr>";
                                string serie = basesDatos[i].Contains("Naturasol") ? "NAT" : "MM";
                                body = body.Replace("valor1", listaPagosEfectuadosNotificar[j].CardName);
                                body = body.Replace("valor2", listaPagosEfectuadosNotificar[j].sociedad);
                                body = body.Replace("valor3", serie + listaPagosEfectuadosNotificar[j].DocNum);
                                body = body.Replace("valor4", listaPagosEfectuadosNotificar[j].DocDate.ToShortDateString());

                                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Tipo de operación: Pago a cuenta <br><br> Monto total de la operación: valor5 valor6 <br> <br> Está recibiendo este mensaje debido a que ha proporcionado su dirección de correo electrónico a <strong><i><span style='font-family:Palatino;mso-bidi-font-family: Calibri'>valor0</span></i></strong>, para hacerle llegar información relacionada a las operaciones con su Empresa. En caso de cambio de correo electrónico favor de enviar un aviso a la siguiente cuenta de correo: <a href='mailto:cuentasxpagar@mielmex.com'>cuentasxpagar@mielmex.com</a>.<br> <br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                                body = body.Replace("valor0", listaPagosEfectuadosNotificar[j].sociedad);
                                body = body.Replace("valor5", Convert.ToDecimal(listaPagosEfectuadosNotificar[j].DocTotal).ToString("N2"));
                                body = body.Replace("valor6", "MXN");
                                body = body.Replace("valor7", DateTime.Now.ToString());

                                //Console.WriteLine(body);
                                correoBLL.enviarCorreoNotificador("Envío automático de pago a proveedor " + serie + "-" + listaPagosEfectuadosNotificar[j].DocNum, body, basesDatos[i], listaPagosEfectuadosNotificar[j].E_Mail);
                                PagosEfectuadosBLL.updatePago(Convert.ToInt32(listaPagosEfectuadosNotificar[j].DocEntry), "Notificado");
                            }
                            else
                            {
                                List<PagosEfectuados> listaPagosEfectuadosNotificarAvanzado = new List<PagosEfectuados>();
                                listaPagosEfectuadosNotificarAvanzado = pagosEfectuadosBLL.obtenerPagosNotificarAvanzado(basesDatos[i], listaPagosEfectuadosNotificar[j].DocNum);
                                if (listaPagosEfectuadosNotificarAvanzado[0].InvType == "18" || listaPagosEfectuadosNotificarAvanzado[0].InvType == "204")
                                {

                                    List<PagosEfectuados> listaPagosEfectuadosNotificarDetalle = new List<PagosEfectuados>();
                                    listaPagosEfectuadosNotificarDetalle = pagosEfectuadosBLL.obtenerPagosNotificarDetalle(basesDatos[i], listaPagosEfectuadosNotificarAvanzado[0].DocNum, listaPagosEfectuadosNotificarAvanzado[0].InvType);
                                    string body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Envío automático confirmación de Pago a Proveedor<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'><span style='font-family: Palatino;color:black'>Estimado titular o representante de <strong><span style='font-family:Palatino;mso-bidi-font-family:Calibri'>valor1</span></strong>,<br> <br> <strong><i><span style='font-family:Palatino;mso-bidi-font-family:Calibri'>valor2</span></i></strong> le notifica la operación de pago de uno o más comprobantes fiscales listado(s) a continuación. Referencia interna de pago valor3, con fecha de aplicación <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'>valor4</span></strong>. Agradecemos anticipadamente la generación del Complemento de pago correspondiente para efectos fiscales.</span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO DE FACTURA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FOLIO FISCAL<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPORTE PAGADO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>MONEDA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TIPO DE CAMBIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPORTE PAGADO EN MXN<o:p></o:p></span></p> </td> </tr>";
                                    string serie = basesDatos[i].Contains("Naturasol") ? "NAT" : "MM";
                                    body = body.Replace("valor1", listaPagosEfectuadosNotificarAvanzado[0].CardName);
                                    body = body.Replace("valor2", listaPagosEfectuadosNotificarAvanzado[0].sociedad);
                                    body = body.Replace("valor3", serie + listaPagosEfectuadosNotificarAvanzado[0].DocNum);
                                    body = body.Replace("valor4", listaPagosEfectuadosNotificarAvanzado[0].DocDate.ToShortDateString());
                                    for (int k = 0; k < listaPagosEfectuadosNotificarDetalle.Count; k++)
                                    {
                                        body = body + leerPlantillaNotificador(listaPagosEfectuadosNotificarDetalle[k]);

                                        if (k == listaPagosEfectuadosNotificarDetalle.Count - 1)
                                        {
                                            body = body + "<tr style='mso-yfti-irow:8;mso-yfti-lastrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'></td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TOTAL<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFC<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'></td> <td style='padding:.75pt .75pt .75pt .75pt'></td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMXN<o:p></o:p></span></p> </td> </tr>";
                                            //body = body.Replace("valorFC", Convert.ToDecimal(listaPagosEfectuadosNotificarDetalle[k].DocRate) > 1 ? listaPagosEfectuadosNotificarDetalle[k].DocTotalFC : listaPagosEfectuadosNotificarDetalle[k].DocTotal);
                                            body = body.Replace("valorFC", "");
                                            body = body.Replace("valorMXN", Convert.ToDecimal(listaPagosEfectuadosNotificarDetalle[k].DocTotal).ToString("N2"));
                                            body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Monto total de la operación: valor5 valor6 <br> <br> Está recibiendo este mensaje debido a que ha proporcionado su dirección de correo electrónico a <strong><i><span style='font-family:Palatino;mso-bidi-font-family: Calibri'>valor00</span></i></strong>, para hacerle llegar información relacionada a las operaciones con su Empresa. En caso de cambio de correo electrónico favor de enviar un aviso a la siguiente cuenta de correo: <a href='mailto:cuentasxpagar@mielmex.com'>cuentasxpagar@mielmex.com</a>.<br> <br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                                            body = body.Replace("valor00", listaPagosEfectuadosNotificar[j].sociedad);
                                            body = body.Replace("valor5", Convert.ToDecimal(listaPagosEfectuadosNotificarDetalle[k].DocTotal).ToString("N2"));
                                            body = body.Replace("valor6", "MXN");
                                            body = body.Replace("valor7", DateTime.Now.ToString());
                                        }
                                    }

                                    //Console.WriteLine(body);
                                    correoBLL.enviarCorreoNotificador("Envío automático de pago a proveedor " + serie + "-" + listaPagosEfectuadosNotificarAvanzado[0].DocNum, body, basesDatos[i], listaPagosEfectuadosNotificarAvanzado[0].E_Mail);
                                    PagosEfectuadosBLL.updatePago(Convert.ToInt32(listaPagosEfectuadosNotificar[j].DocEntry), "Notificado");

                                }
                                else
                                {
                                    PagosEfectuadosBLL.updatePago(Convert.ToInt32(listaPagosEfectuadosNotificar[j].DocEntry), "No Notificado");
                                }
                            }
                        }
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
        }

        public static string leerPlantillaNotificador(PagosEfectuados listaPagosEfectuados)
        {
            string body = "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFactura<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFiscal<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorImporte<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMoneda<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorTC<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPagadoMXN<o:p></o:p></span></p> </td> </tr>";
            body = body.Replace("valorFactura", listaPagosEfectuados.NumAtCard);
            body = body.Replace("valorFiscal", listaPagosEfectuados.EDocNum);
            body = body.Replace("valorMoneda", listaPagosEfectuados.DocCur);
            body = body.Replace("valorImporte", Convert.ToDecimal(listaPagosEfectuados.DocRate) > 1 ? Convert.ToDecimal(listaPagosEfectuados.AppliedFC).ToString("N2") : Convert.ToDecimal(listaPagosEfectuados.SumApplied).ToString("N2"));
            body = body.Replace("valorTC", Convert.ToDecimal(listaPagosEfectuados.DocRate) > 1 ? listaPagosEfectuados.DocRate : "1.0");
            body = body.Replace("valorPagadoMXN", Convert.ToDecimal(listaPagosEfectuados.SumApplied).ToString("N2"));
            return body;
        }
    }
}
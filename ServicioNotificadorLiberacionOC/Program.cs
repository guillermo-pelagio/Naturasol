using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioNotificadorLiberacionOC
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string[] basesDatos = { "TSSL_NATURASOL", "TSSL_MIELMEX" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenCompra> listaCompradorOCLiberar = new List<OrdenCompra>();
                listaCompradorOCLiberar = ordenesCompraBLL.listaBorradoresLiberacion(basesDatos[i]);
                if (listaCompradorOCLiberar.Count > 0)
                {
                    bool enviaCorreo = true;
                    enviaCorreo = true;
                    decimal semanasactuales = 0;
                    decimal semanasdespues = 0;

                    for (int k = 0; k < listaCompradorOCLiberar.Count; k++)
                    {
                        bool semanas12 = false;
                        bool fechaAntes = false;
                        bool planeacion = false;

                        semanasactuales = 0;
                        semanasdespues = 0;
                        List<OrdenCompra> listaCompradorOCLiberarDetalle = new List<OrdenCompra>();
                        List<OrdenCompra> listaCompradorOCLiberarDetalleAnexos = new List<OrdenCompra>();
                        listaCompradorOCLiberarDetalle = ordenesCompraBLL.listaBorradoresLiberacionDetalle(basesDatos[i], listaCompradorOCLiberar[k].DocEntry);

                        listaCompradorOCLiberarDetalleAnexos = ordenesCompraBLL.listaBorradoresLiberacionDetalleAnexos(basesDatos[i], listaCompradorOCLiberar[k].DocEntry);

                        ordenesCompraBLL.guardarSolicitudAutorizacionOC(listaCompradorOCLiberar[k]);
                        ordenesCompraBLL.actualizarOCAutorizada(listaCompradorOCLiberar[k].Sociedad, listaCompradorOCLiberar[k].WddCode);

                        string body = "";

                        /*
                        if (listaCompradorOCLiberarDetalle[0].DocType == "I")
                        {
                            List<OrdenCompra> listaForecastOC = new List<OrdenCompra>();
                            listaForecastOC = ordenesCompraBLL.semanasExistenciaLiberacionOC(basesDatos[i], listaCompradorOCLiberarDetalle[0].DocEntry);

                            if (listaForecastOC.Count > 0)
                            {
                                for (int g = 0; g < listaForecastOC.Count; g++)
                                {
                                    if (listaForecastOC[g].Inventario1 != "")
                                    {
                                        if ((Convert.ToDecimal(listaForecastOC[g].Inventario1) > 12 && (Convert.ToDecimal(listaForecastOC[g].OnHand) > 0)))
                                        {
                                            if (semanasactuales < Convert.ToDecimal(listaForecastOC[g].Inventario1))
                                            {
                                                semanasactuales = Convert.ToDecimal(listaForecastOC[g].Inventario1);
                                            }
                                        }
                                    }
                                    if ((listaForecastOC[g].Inventario1 == "") && (Convert.ToDecimal(listaForecastOC[g].OnHand) > 0))
                                    {
                                        semanasactuales = 1000;
                                    }
                                    if (listaForecastOC[g].Inventario2 != "")
                                    {
                                        if (Convert.ToDecimal(listaForecastOC[g].Inventario2) > 18)
                                        {
                                            if (semanasdespues < Convert.ToDecimal(listaForecastOC[g].Inventario2))
                                            {
                                                semanasdespues = Convert.ToDecimal(listaForecastOC[g].Inventario2);
                                            }
                                        }
                                    }
                                    if ((listaForecastOC[g].Inventario2 == "") && ((Convert.ToDecimal(listaForecastOC[g].OnHand) + Convert.ToDecimal(listaForecastOC[g].Quantity)) > 0))
                                    {
                                        semanasdespues = 1000;
                                    }
                                }
                            }
                        }
                        */

                        body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''><o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Monto total OC: <strong> valorMontoTotal </strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></strong></p><p class=MsoNormal style='margin-bottom:12.0pt'> Proveedor: <strong> valorProveedor </strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></strong></p>";


                        body = body + "<table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PRECIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPUESTO<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CUENTA<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PLANTA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CENTRO DE COSTO<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>EXISTENCIA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CONSUMO PROMEDIO SEMANAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA LLEGADA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PENDIENTE EN OC ABIERTAS<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>INVENTARIO A FECHA LLEGADA (CALCULADO)<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'> SEMANAS INVENTARIO DESPUES DE LLEGADA<o:p></o:p><o:p></o:p></span></p> </td>  </tr>";

                        if (listaCompradorOCLiberarDetalle.Count > 0)
                        {
                            for (int j = 0; j < listaCompradorOCLiberarDetalle.Count; j++)
                            {
                                string inventarioCalculado1 = "";
                                string inventarioCalculadoFinal = "";
                                string semanasInventario = "";
                                string ocAbiertas = "";
                                string consumodiario = "";
                                string OC = "";
                                double diferenciaDias = 0;

                                List<OrdenCompra> listaBorradoresCalculo = new List<OrdenCompra>();
                                List<OrdenCompra> listaBorradoresCalculoOC = new List<OrdenCompra>();

                                if (listaCompradorOCLiberarDetalle[j].DocType == "I")
                                {
                                    listaBorradoresCalculo = ordenesCompraBLL.listaBorradoresCalculo(basesDatos[i], listaCompradorOCLiberarDetalle[j].ItemCode, listaCompradorOCLiberarDetalle[j].ShipDate, listaCompradorOCLiberarDetalle[j].DocEntry);
                                    inventarioCalculado1 = listaBorradoresCalculo.Count > 0 ? listaBorradoresCalculo[0].Inventario1 : "0";
                                    consumodiario = Convert.ToString(Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo == "" ? "0" : listaCompradorOCLiberarDetalle[j].Consumo) / 84);
                                    diferenciaDias = (Convert.ToDateTime(listaCompradorOCLiberarDetalle[j].ShipDate) - DateTime.Now).TotalDays > 0 ? (Convert.ToDateTime(listaCompradorOCLiberarDetalle[j].ShipDate) - DateTime.Now).TotalDays : 0;
                                    inventarioCalculadoFinal = Convert.ToString(Math.Round(Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].OnHand) + Convert.ToDecimal(inventarioCalculado1) - (Convert.ToDecimal(consumodiario) * Convert.ToDecimal(diferenciaDias)), 4));
                                    listaBorradoresCalculoOC = ordenesCompraBLL.listaBorradoresCalculoOC(basesDatos[i], listaCompradorOCLiberarDetalle[j].ItemCode, listaCompradorOCLiberarDetalle[j].ShipDate);
                                    OC = listaBorradoresCalculoOC.Count > 0 ? listaBorradoresCalculoOC[0].Inventario2 : "0";
                                }

                                body = body + "<tr ";
                                if (Convert.ToDateTime(listaCompradorOCLiberarDetalle[j].ShipDate) < DateTime.Now)
                                {
                                    body = body + "style='margin-bottom:12.0pt; color:red; font-weight: bold;' ";
                                    fechaAntes = true;
                                }
                                try
                                {
                                    if ((listaCompradorOCLiberarDetalle[j].Consumo == "" ? 0 : Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)) > 12)
                                    {
                                        semanas12 = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    semanas12 = false;
                                }

                                try
                                {
                                    if ((listaCompradorOCLiberarDetalle[j].Consumo == "" ? 0 : Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)) > 4)
                                    {
                                        planeacion = true;
                                    }
                                }
                                catch (Exception)
                                {
                                    planeacion = false;
                                }

                                body = body + " style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorArticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>varloDescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCantidad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPrecio<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorImpuesto<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCuenta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPlanta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCentro<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorExistencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorConsumo<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorFecha<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorOC<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorInventario<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSemanas<o:p></o:p></span></p> </td> </tr>";
                                body = body.Replace("valorArticulo", listaCompradorOCLiberarDetalle[j].ItemCode);
                                body = body.Replace("varloDescripcion", listaCompradorOCLiberarDetalle[j].Dscription);
                                body = body.Replace("valorCantidad", listaCompradorOCLiberarDetalle[j].DocType == "I" ? listaCompradorOCLiberarDetalle[j].Quantity : listaCompradorOCLiberarDetalle[j].U_CantidadServicio);
                                body = body.Replace("valorPrecio", listaCompradorOCLiberarDetalle[j].DocCur == "MXP" ? string.Format("{0:C}", Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].LineTotal)) + " " + listaCompradorOCLiberarDetalle[j].Currency : string.Format("{0:C}", Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].TotalFrgn)) + " " + listaCompradorOCLiberarDetalle[j].Currency);
                                body = body.Replace("valorImpuesto", listaCompradorOCLiberarDetalle[j].TaxCode);
                                body = body.Replace("valorCuenta", listaCompradorOCLiberarDetalle[j].AcctCode + " - " + listaCompradorOCLiberarDetalle[j].AcctName);
                                body = body.Replace("valorPlanta", listaCompradorOCLiberarDetalle[j].OcrCode);
                                body = body.Replace("valorCentro", listaCompradorOCLiberarDetalle[j].OcrCode3);
                                body = body.Replace("valorOC", OC);
                                body = body.Replace("valorExistencia", listaCompradorOCLiberarDetalle[j].OnHand);
                                body = body.Replace("valorConsumo", listaCompradorOCLiberarDetalle[j].Consumo == "" ? "0" : Convert.ToString(Math.Round((Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)));
                                body = body.Replace("valorInventario", inventarioCalculadoFinal);
                                try
                                {
                                    body = body.Replace("valorSemanas", listaCompradorOCLiberarDetalle[j].Consumo == "" ? "-" : Convert.ToString(Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)));
                                }
                                catch (Exception)
                                {
                                    body = body.Replace("valorSemanas", listaCompradorOCLiberarDetalle[j].Consumo == "" ? "-" : "-");
                                }
                                body = body.Replace("valorFecha", listaCompradorOCLiberarDetalle[j].ShipDate);
                            }
                        }

                        if (enviaCorreo)
                        {
                            body = body + "</table> ";


                            if (fechaAntes == true)
                            {
                                body = body + "<p class=MsoNormal style='margin-bottom:12.0pt; color:red'> valorSemanas <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></strong></p>";
                                body = body.Replace("valorSemanas", "Hay artículos en esta OC que tienen fecha de llegada anterior a hoy");
                            }
                            if (semanas12 == true)
                            {
                                body = body + "<p class=MsoNormal style='margin-bottom:12.0pt; color: red'> valorSemanas2 <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></strong></p>";
                                body = body.Replace("valorSemanas2", "Al fincar esta OC habrá artículos que superaran las 12 semanas de inventario (existencia despues esta OC/consumo ultimas 12 semanas)");
                            }

                            body = body + "<p class=MsoNormal><span style='font-family:Palatino;color:black'><br> El consumo promedio semanal esta calculado con base a las ultimas 12 semanas. </span><span style='font-family:Palatino'><o:p></o:p></span></p><p class=MsoNormal><span style='font-family:Palatino;color:black'> Lo pendiente en las OC abiertas es el material en OC fincadas a la fecha de llegada </span><span style='font-family:Palatino'><o:p></o:p></span></p><p class=MsoNormal><span style='font-family:Palatino;color:black'> El inventario a la fecha de llegada es la suma del inventario actual más lo pendiente de entrega en este borrador a la fecha de llegada más lo pendiente en OC abiertas a la fecha de llegada menos el consumo estimado a esa fecha. </span><span style='font-family:Palatino'><o:p></o:p></span></p> <p class=MsoNormal><span style='font-family:Palatino;color:black'> Las semanas de inventario a fecha de llegada es el inventario calculado entre el consumo promedio semanal. </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";

                            body = body + "<p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";

                            body = body.Replace("valor7", DateTime.Now.ToString());
                            body = body.Replace("valorProveedor ", listaCompradorOCLiberarDetalle[0].CardCode + " " + listaCompradorOCLiberarDetalle[0].CardName);
                            body = body.Replace("valorMontoTotal ", listaCompradorOCLiberarDetalle[0].DocCur == "MXP" ? string.Format("{0:C}", Convert.ToDecimal(listaCompradorOCLiberarDetalle[0].DocTotal)) + " " + listaCompradorOCLiberarDetalle[0].DocCur : string.Format("{0:C}", Convert.ToDecimal(listaCompradorOCLiberarDetalle[0].DocTotalFC)) + " " + listaCompradorOCLiberarDetalle[0].DocCur);


                            string destinatarios = "";

                            //if ((listaCompradorOCLiberar[k].WtmCode.Equals("71") || listaCompradorOCLiberar[k].WtmCode.Equals("72")) && (listaCompradorOCLiberar[k].U_LiberacionOC.Equals("2") || listaCompradorOCLiberar[k].U_LiberacionOC.Equals("0")))
                            if ((listaCompradorOCLiberar[k].WtmCode.Equals("71") || listaCompradorOCLiberar[k].WtmCode.Equals("72")) && (listaCompradorOCLiberar[k].U_LiberacionOC.Equals("1") || listaCompradorOCLiberar[k].U_LiberacionOC.Equals("3")))
                            {
                                destinatarios = "miel.contabilidad@mielmex.com, guillermo.pelagio@naturasol.com.mx, paola.posadas@naturasol.com.mx, " + listaCompradorOCLiberarDetalle[0].Email;
                                //destinatarios = "guillermo.pelagio@naturasol.com.mx";
                            }
                            //if ((listaCompradorOCLiberar[k].WtmCode.Equals("43") || listaCompradorOCLiberar[k].WtmCode.Equals("44")) && (listaCompradorOCLiberar[k].U_LiberacionOC.Equals("1") || listaCompradorOCLiberar[k].U_LiberacionOC.Equals("0")))
                            if ((listaCompradorOCLiberar[k].WtmCode.Equals("43") || listaCompradorOCLiberar[k].WtmCode.Equals("44")) && (listaCompradorOCLiberar[k].U_LiberacionOC.Equals("1") || listaCompradorOCLiberar[k].U_LiberacionOC.Equals("2")))
                            {
                                destinatarios = "mario.esquivel@mielmex.com, guillermo.pelagio@naturasol.com.mx, paola.posadas@naturasol.com.mx, " + listaCompradorOCLiberarDetalle[0].Email;
                                //destinatarios = "guillermo.pelagio@naturasol.com.mx";

                                if ((planeacion == true))
                                {
                                    destinatarios = destinatarios + ",carlos.castaneda@naturasol.com.mx, eduardo.ugalde@naturasol.com.mx, fabian.aguirre@naturasol.com.mx";
                                }
                            }

                            string sociedad = basesDatos[i].Contains("NATURASOL") ? "NATURASOL" : basesDatos[i].Contains("MIELMEX") ? "MIELMEX" : "NOVAL";
                            correoBLL.enviarNotificacionLiberacionCompra("Liberacion OC " + listaCompradorOCLiberar[k].DocNum + " " + sociedad + " - " + listaCompradorOCLiberar[k].WddCode + " - " + listaCompradorOCLiberarDetalle[0].SlpName, body, listaCompradorOCLiberarDetalleAnexos, destinatarios);
                        }
                    }
                }
            }
        }
    }
}

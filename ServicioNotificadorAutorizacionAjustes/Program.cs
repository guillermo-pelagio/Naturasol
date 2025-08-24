using CapaDatos;
using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioNotificadorAutorizacionAjustes
{
    class Program
    {
        static void Main(string[] args)
        {
            SalidasMercanciaBLL salidasMercanciaBLL = new SalidasMercanciaBLL();
            string[] basesDatos = { "TSSL_NATURASOL", "TSSL_MIELMEX" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<SalidasMercancia> listaMuestraLiberar = new List<SalidasMercancia>();
                listaMuestraLiberar = salidasMercanciaBLL.listaBorradoresLiberacion(basesDatos[i]);
                if (listaMuestraLiberar.Count > 0)
                {
                    bool enviaCorreo = true;
                    enviaCorreo = true;

                    for (int k = 0; k < listaMuestraLiberar.Count; k++)
                    {
                        List<SalidasMercancia> listaMuestraLiberarDetalle = new List<SalidasMercancia>();
                        List<SalidasMercancia> listaMuestraLiberarDetalleAnexos = new List<SalidasMercancia>();
                        listaMuestraLiberarDetalle = salidasMercanciaBLL.listaBorradoresLiberacionDetalle(basesDatos[i], listaMuestraLiberar[k].DocEntry);

                        listaMuestraLiberarDetalleAnexos = salidasMercanciaBLL.listaBorradoresLiberacionDetalleAnexos(basesDatos[i], listaMuestraLiberar[k].DocEntry);

                        salidasMercanciaBLL.guardarSolicitudAutorizacionMuestras(listaMuestraLiberar[k]);
                        salidasMercanciaBLL.actualizarMuestraAutorizada(listaMuestraLiberar[k].Sociedad, listaMuestraLiberar[k].WddCode);

                        string body = "";

                        body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''><o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Monto total OC: <strong> valorMontoTotal </strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> </strong><p class=MsoNormal style='margin-bottom:12.0pt'> Proveedor: <strong> valorProveedor </strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p></strong><table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PRECIO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>IMPUESTO<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CUENTA<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PLANTA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CENTRO DE COSTO<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>EXISTENCIA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CONSUMO (12 SEM)<o:p></o:p><o:p></o:p></span></p> </td>   </tr>";

                        if (listaMuestraLiberarDetalle.Count > 0)
                        {

                            for (int j = 0; j < listaMuestraLiberarDetalle.Count; j++)
                            {
                                body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorArticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>varloDescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCantidad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPrecio<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorImpuesto<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCuenta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorPlanta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCentro<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorExistencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorConsumo<o:p></o:p></span></p> </td> </tr>";
                                body = body.Replace("valorArticulo", listaMuestraLiberarDetalle[j].ItemCode);
                                body = body.Replace("varloDescripcion", listaMuestraLiberarDetalle[j].Dscription);
                                body = body.Replace("valorCantidad", listaMuestraLiberarDetalle[j].DocType == "I" ? listaMuestraLiberarDetalle[j].Quantity : listaMuestraLiberarDetalle[j].U_CantidadServicio);
                                body = body.Replace("valorPrecio", listaMuestraLiberarDetalle[j].DocCur == "MXP" ? string.Format("{0:C}", Convert.ToDecimal(listaMuestraLiberarDetalle[j].LineTotal)) + " " + listaMuestraLiberarDetalle[j].Currency : string.Format("{0:C}", Convert.ToDecimal(listaMuestraLiberarDetalle[j].TotalFrgn)) + " " + listaMuestraLiberarDetalle[j].Currency);
                                body = body.Replace("valorImpuesto", listaMuestraLiberarDetalle[j].TaxCode);
                                body = body.Replace("valorCuenta", listaMuestraLiberarDetalle[j].AcctCode + " - " + listaMuestraLiberarDetalle[j].AcctName);
                                body = body.Replace("valorPlanta", listaMuestraLiberarDetalle[j].OcrCode);
                                body = body.Replace("valorCentro", listaMuestraLiberarDetalle[j].OcrCode3);
                                body = body.Replace("valorExistencia", listaMuestraLiberarDetalle[j].OnHand);
                                body = body.Replace("valorConsumo", listaMuestraLiberarDetalle[j].Consumo);
                            }
                        }


                        if (enviaCorreo)
                        {
                            body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                            body = body.Replace("valor7", DateTime.Now.ToString());
                            body = body.Replace("valorProveedor ", listaMuestraLiberarDetalle[0].CardCode + " " + listaMuestraLiberarDetalle[0].CardName);
                            body = body.Replace("valorMontoTotal ", listaMuestraLiberarDetalle[0].DocCur == "MXP" ? string.Format("{0:C}", Convert.ToDecimal(listaMuestraLiberarDetalle[0].DocTotal)) + " " + listaMuestraLiberarDetalle[0].DocCur : string.Format("{0:C}", Convert.ToDecimal(listaMuestraLiberarDetalle[0].DocTotalFC)) + " " + listaMuestraLiberarDetalle[0].DocCur);

                            string destinatarios = "";

                            if ((listaMuestraLiberar[k].WtmCode.Equals("71") || listaMuestraLiberar[k].WtmCode.Equals("72")) && (listaMuestraLiberar[k].U_LiberacionOC.Equals("1") || listaMuestraLiberar[k].U_LiberacionOC.Equals("3")))
                            {
                                //destinatarios = "miel.contabilidad@mielmex.com, guillermo.pelagio@naturasol.com.mx, " + listaMuestraLiberarDetalle[0].Email;
                                destinatarios = "guillermo.pelagio@naturasol.com.mx";
                            }
                            if ((listaMuestraLiberar[k].WtmCode.Equals("43") || listaMuestraLiberar[k].WtmCode.Equals("44")) && (listaMuestraLiberar[k].U_LiberacionOC.Equals("1") || listaMuestraLiberar[k].U_LiberacionOC.Equals("2")))
                            {
                                //destinatarios = "mario.esquivel@mielmex.com, guillermo.pelagio@naturasol.com.mx, " + listaMuestraLiberarDetalle[0].Email;
                                destinatarios = "guillermo.pelagio@naturasol.com.mx";
                            }

                            string sociedad = basesDatos[i].Contains("NATURASOL") ? "NATURASOL" : basesDatos[i].Contains("MIELMEX") ? "MIELMEX" : "NOVAL";
                            //correoBLL.enviarNotificacionLiberacionCompra("Liberacion Muestras " + listaMuestraLiberar[k].DocNum + " " + sociedad + " - " + listaMuestraLiberar[k].WddCode + " - " + listaMuestraLiberarDetalle[0].SlpName, body, listaMuestraLiberarDetalleAnexos, destinatarios);
                        }
                    }
                }
            }
        }
    }
}

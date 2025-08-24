using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioLiberacionMuestras
{
    class Program
    {
        static void Main(string[] args)
        {
            InventarioBLL inventarioBLL = new InventarioBLL();
            CorreosBLL correoBLL = new CorreosBLL();

            List<BorradorDocumento> listaDocumentos = new List<BorradorDocumento>();
            listaDocumentos = inventarioBLL.obtenerMuestrasAutorizar();

            if (listaDocumentos.Count > 0)
            {
                for (int k = 0; k < listaDocumentos.Count; k++)
                {
                    bool estatusMuestra = true;
                    List<BorradorDocumento> listaDocumentosDetalle = new List<BorradorDocumento>();
                    listaDocumentosDetalle = inventarioBLL.obtenerMuestrasAutorizarDetalle(listaDocumentos[k].wddCode);

                    string body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Autorización de muestras<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> La siguiente solicitud de muestras valor1 <br> <br> <strong> valor2</strong> <br> <br> Comentarios: <strong> valor3</strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CUENTA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PLANTA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CECOS<o:p></o:p><o:p></o:p></span></p> </td></tr>";

                    for (int m = 0; m < listaDocumentosDetalle.Count; m++)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorarticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valoralmacen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcantidad<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcuenta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorplanta<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcecos<o:p></o:p></span></p> </td> </tr>";

                        body = body.Replace("valorarticulo", listaDocumentosDetalle[m].articulo);
                        body = body.Replace("valordescripcion", listaDocumentosDetalle[m].descripcion);
                        body = body.Replace("valoralmacen", listaDocumentosDetalle[m].almacen);
                        body = body.Replace("valorcantidad", listaDocumentosDetalle[m].cantidad);
                        body = body.Replace("valorcuenta", listaDocumentosDetalle[m].cuentaContable);
                        body = body.Replace("valorplanta", listaDocumentosDetalle[m].planta);
                        body = body.Replace("valorcecos", listaDocumentosDetalle[m].centroCosto);

                        if (listaDocumentosDetalle[m].familia == "PT")
                        {
                            estatusMuestra = false;
                            /*
                            if (listaDocumentos[k].cuentaContable == "52401071" && listaDocumentos[k].centroCosto == "CALIDAD_" && listaDocumentos[k].maximoMuestra != null)
                            {
                                if ((Convert.ToDecimal(listaDocumentos[k].maximoMuestra) >= Convert.ToDecimal(listaDocumentos[k].cantidad)) && estatusMuestra == true)
                                {
                                    estatusMuestra = true;
                                }
                                else
                                {
                                    estatusMuestra = false;
                                }
                            }
                            else
                            {
                                estatusMuestra = false;
                            }                          
                            */
                        }
                        else
                        {
                            if (listaDocumentosDetalle[m].cuentaContable == "52401071" && listaDocumentosDetalle[m].centroCosto == "CALIDAD_" && listaDocumentosDetalle[m].maximoMuestra != null)
                            {
                                if ((Convert.ToDecimal(listaDocumentosDetalle[m].maximoMuestra) >= Convert.ToDecimal(listaDocumentosDetalle[m].cantidad)) && estatusMuestra == true)
                                {
                                    estatusMuestra = true;
                                }
                                else
                                {
                                    estatusMuestra = false;
                                }
                            }
                            else
                            {
                                estatusMuestra = false;
                            }
                        }
                    }

                    //string body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Autorización de muestras<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Las siguiente solicitud de muestras valor1 <br> <br>  <strong> valor2</strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CUENTA CONTABLE<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PLANTA<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CENTRO DE COSTO<o:p></o:p><o:p></o:p></span></p> </td>  </tr>";                    

                    if (estatusMuestra == true)
                    {
                        DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                        inventarioBLL.actualizarSalidaMercancia(Convert.ToInt32(listaDocumentos[k].wddCode), "ardApproved", "");
                        DIAPIBLL.desconectarDIAPI();

                        body = body.Replace("valor1", listaDocumentos[k].numeroDocumento + " HA SIDO AUTORIZADA AUTOMATICAMENTE");
                        body = body.Replace("valor2", "");
                        body = body.Replace("valor3", listaDocumentos[k].comentarios + "");

                        body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                        body = body.Replace("valor7", DateTime.Now.ToString());
                    }
                    else
                    {
                        DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                        inventarioBLL.actualizarSalidaMercancia(Convert.ToInt32(listaDocumentos[k].wddCode), "ardNotApproved", DateTime.Now.ToShortDateString());
                        DIAPIBLL.desconectarDIAPI();


                        body = body.Replace("valor1", listaDocumentos[k].numeroDocumento + " NO HA SIDO AUTORIZADA AUTOMATICAMENTE");
                        body = body.Replace("valor2", "Revise la cuenta contable, centro de costos, cantidad solicitada y solicite la autorizacion a Finanzas.");
                        body = body.Replace("valor3", listaDocumentos[k].comentarios + "");
                        /*
                        body = body.Replace("valorarticulo", listaDocumentos[k - 1].articulo);
                        body = body.Replace("valordescripcion", listaDocumentos[k - 1].descripcion);
                        body = body.Replace("valoralmacen", listaDocumentos[k - 1].almacen);
                        body = body.Replace("valorcantidad", listaDocumentos[k - 1].cantidad);
                        body = body.Replace("valorcuenta", listaDocumentos[k - 1].cuentaContable);
                        body = body.Replace("valorplanta", listaDocumentos[k - 1].planta);
                        body = body.Replace("valorcecos", listaDocumentos[k - 1].centroCosto);
                        */
                        body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                        body = body.Replace("valor7", DateTime.Now.ToString());

                    }

                    correoBLL.enviarCorreo("Solicitud de muestras", body, "guillermo.pelagio@naturasol.com.mx", null);

                }
            }
        }
    }
}

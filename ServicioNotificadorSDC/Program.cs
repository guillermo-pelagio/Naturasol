using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioNotificadorSDC
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            SolicitudCompraBLL solicitudCompraBLL = new SolicitudCompraBLL();
            string[] basesDatos = { "TSSL_NATURASOL", "TSSL_MIELMEX" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                bool enviaCorreo = true;
                enviaCorreo = true;

                List<SolicitudCompra> listaCompradorOCLiberarDetalle = new List<SolicitudCompra>();
                listaCompradorOCLiberarDetalle = solicitudCompraBLL.obtenerSDCDirectos(basesDatos[i]);

                string body = "";

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

                        if ((listaCompradorOCLiberarDetalle[j].Consumo == "" ? 0 : Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)) > 12)
                        {
                            semanas12 = true;
                        }

                        if ((listaCompradorOCLiberarDetalle[j].Consumo == "" ? 0 : Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)) > 4)
                        {
                            planeacion = true;
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
                        body = body.Replace("valorSemanas", listaCompradorOCLiberarDetalle[j].Consumo == "" ? "-" : Convert.ToString(Math.Round(Convert.ToDecimal(inventarioCalculadoFinal) / (Convert.ToDecimal(listaCompradorOCLiberarDetalle[j].Consumo) / 12), 4)));
                        body = body.Replace("valorFecha", listaCompradorOCLiberarDetalle[j].ShipDate);
                    }
                }

                if (enviaCorreo)
                {
                    body = body + "</table> ";

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
            */
        }
    }
}

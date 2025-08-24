using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioAjustes
{
    class Program
    {
        static void Main(string[] args)
        {
            InventarioBLL inventarioBLL = new InventarioBLL();
            CorreosBLL correoBLL = new CorreosBLL();
            bool enviaCorreo = false;

            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };

            for (int i = 0; i < basesDatos.Length; i++)
            {
                enviaCorreo = false;
                string[] ajustes = { "Entradas", "Salidas" };
                for (int j = 0; j < ajustes.Length; j++)
                {
                    enviaCorreo = false;
                    string[] tipoAjustes = { "Ajuste de inventario", "Merma", "Muestras", "Cambios de lote", "Cambio de código PT-MP", "Movimiento interno" };

                    for (int k = 0; k < tipoAjustes.Length; k++)
                    {
                        enviaCorreo = false;
                        List<Inventario> listaAjustes = new List<Inventario>();
                        string body = "";
                        decimal montoTotal = 0;
                        body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>valorAjuste por valorTipoAjuste<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Estas son las <strong>valorAjuste</strong> por <strong>valorTipoAjuste</strong> del último mes: <br><strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CANTIDAD<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>U.M.<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>VALOR MOVIMIENTO (MXP)<o:p></o:p><o:p></o:p></span></p> </td>  </tr>";
                        listaAjustes = inventarioBLL.obtenerAjustesMes(basesDatos[i].ToString(), j, k);

                        if (listaAjustes.Count > 0)
                        {
                            for (int m = 0; m < listaAjustes.Count; m++)
                            {
                                enviaCorreo = true;

                                body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorsociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorArticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorDescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCantidad<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorUM<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorMonto<o:p></o:p></span></p> </td> </tr>";

                                body = body.Replace("valorsociedad", basesDatos[i].Replace("TSSL_", ""));
                                body = body.Replace("valorArticulo", listaAjustes[m].Artículo);
                                body = body.Replace("valorDescripcion", listaAjustes[m].Descripción);
                                body = body.Replace("valorCantidad", string.Format("{0:N}", Convert.ToDecimal(listaAjustes[m].Stock)));
                                body = body.Replace("valorUM", listaAjustes[m].UM);
                                body = body.Replace("valorMonto", string.Format("{0:C}", Convert.ToDecimal(listaAjustes[m].valorInventario)));

                                body = body.Replace("valorAjuste", ajustes[j]);
                                body = body.Replace("valorTipoAjuste", tipoAjustes[k]);
                            }
                        }

                        if (enviaCorreo)
                        {
                            if (k == 0 || k == 2)
                            {
                                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                                body = body.Replace("valor7", DateTime.Now.ToString());
                                string cc = null;

                                if (k == 0)
                                {
                                    cc = "alejandra.bobadilla@mielmex.com, daniel.duran@naturasol.com.mx, alejandra.vilchis@naturasol.com.mx";
                                }
                                if (k == 2)
                                {
                                    cc = "alejandra.bobadilla@mielmex.com, daniel.duran@naturasol.com.mx, alejandra.vilchis@naturasol.com.mx, jenifer.castillo@naturasol.com.mx, angel.santoyo@naturasol.com.mx";
                                }

                                //correoBLL.enviarCorreo(basesDatos[i].Replace("TSSL_", "") + " - " + ajustes[j] + " por " + tipoAjustes[k] + " último mes", body, "guillermo.pelagio.hdz@gmail.com", null);
                                correoBLL.enviarCorreo(basesDatos[i].Replace("TSSL_", "") + " - " + ajustes[j] + " por " + tipoAjustes[k] + " último mes", body, "mario.esquivel@mielmex.com", cc);
                            }
                        }
                    }
                }
            }
        }
    }
}

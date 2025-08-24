using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioCaducosPTKAM
{
    class Program
    {
        static void Main(string[] args)
        {
            InventarioBLL inventariosBLL = new InventarioBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };
            CorreosBLL correoBLL = new CorreosBLL();

            List<Inventario> listaClientes = new List<Inventario>();
            listaClientes = inventariosBLL.obtenerClientesKAM();

            if (listaClientes.Count > 0)
            {

                for (int j = 0; j < listaClientes.Count; j++)
                {
                    string body = "";
                    body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Notificación de productos a caducar<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Los siguientes articulos que se le venden al cliente <strong><i><span style='font-family:Palatino;mso-bidi-font-family:Calibri'>valor2</span></i></strong> estan por caducar en las siguientes fechas. <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PLANTA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FAMILIA<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE ALMACEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ESTATUS<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>LOTE<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CADUCIDAD<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>UBICACION<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>STOCK<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>UM<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TIPO CADUCIDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DIAS PARA CADUCAR<o:p></o:p></span></p> </td> </tr>";
                    body = body.Replace("valor2", listaClientes[j].cardname);
                    bool enviaCorreo = false;

                    List<Inventario> listaArticulos = new List<Inventario>();
                    listaArticulos = inventariosBLL.obtenerArticulosCaducar(listaClientes[j].U_CLIENTE);

                    if (listaArticulos.Count > 0)
                    {
                        for (int k = 0; k < listaArticulos.Count; k++)
                        {
                            List<Inventario> listaExistenciasCaducas = new List<Inventario>();

                            for (int f = 0; f < 2; f++)
                            {
                                string dias = "0";

                                if (f == 0)
                                {
                                    dias = listaArticulos[k].U_DIAS;
                                }
                                else if (f == 1)
                                {
                                    dias = listaArticulos[k].U_DIAS + 30;
                                }

                                listaExistenciasCaducas = inventariosBLL.obtenerExistenciasCaducas(listaArticulos[k].U_ARTICULO, dias);
                                if (listaExistenciasCaducas.Count > 0)
                                {
                                    for (int m = 0; m < listaExistenciasCaducas.Count; m++)
                                    {
                                        if (listaExistenciasCaducas[m].Almacén.EndsWith("12"))
                                        {
                                            enviaCorreo = true;

                                            body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorarticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorsociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorplanta<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorfamilia<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valoralmacen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valornombrealmacen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorestatus<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorlote<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcaducidad<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorubicacion<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorstock<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorum<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valortipocaducidad<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordias<o:p></o:p></span></p> </td> </tr>";
                                            body = body.Replace("valorarticulo", listaArticulos[k].U_ARTICULO);
                                            body = body.Replace("valordescripcion", listaExistenciasCaducas[m].Descripción);
                                            body = body.Replace("valorsociedad", listaExistenciasCaducas[m].Sociedad);
                                            body = body.Replace("valorplanta", listaExistenciasCaducas[m].Planta);
                                            body = body.Replace("valorfamilia", listaExistenciasCaducas[m].Familia);
                                            body = body.Replace("valoralmacen", listaExistenciasCaducas[m].Almacén);
                                            body = body.Replace("valornombrealmacen", listaExistenciasCaducas[m].NombreA);
                                            body = body.Replace("valorestatus", listaExistenciasCaducas[m].Status);
                                            body = body.Replace("valorlote", listaExistenciasCaducas[m].Lote);
                                            body = body.Replace("valorcaducidad", Convert.ToDateTime(listaExistenciasCaducas[m].Caducidad).ToShortDateString());
                                            body = body.Replace("valorubicacion", listaExistenciasCaducas[m].Ubicación);
                                            body = body.Replace("valorstock", listaExistenciasCaducas[m].Stock);
                                            body = body.Replace("valorum", listaExistenciasCaducas[m].UM);
                                            body = body.Replace("valortipocaducidad", listaExistenciasCaducas[m].TipoC);
                                            body = body.Replace("valordias", dias);
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

                        correoBLL.enviarCorreo("Productos proximos a caducar " + listaClientes[j].U_CLIENTE + "-" + listaClientes[j].cardname, body, '"' + listaClientes[j].U_CORREO1 + "\"," + listaClientes[j].U_CORREO2 + "\"," + listaClientes[j].U_CORREO3 + ", alfredo.trejo@mielmex.com, fabian.aguirre@naturasol.com.mx, oswaldo.mendez@naturasol.com.mx, ricardo.vega@naturasol.com.mx, guadalupe.gonzalez@naturasol.com.mx, ngarcia@mielmex.com, angel.santoyo@naturasol.com.mx, jenifer.castillo@naturasol.com.mx, andres.sanchez@naturasol.com.mx, alejandra.vilchis@naturasol.com.mx, marco.garcia@naturasol.com.mx, alejandra.bobadilla@mielmex.com, daniel.duran@naturasol.com.mx, gustavo.valenzuela@naturasol.com.mx, hugo.hernandez@naturasol.com.mx", "paola.posadas@naturasol.com.mx, samuel.pena@naturasol.com.mx, mario.esquivel@mielmex.com, jorge.rincon@naturasol.com.mx, ivaladez@naturasol.com.mx, jorge.vazquez@naturasol.com.mx, alejandro.cantu@naturasol.com.mx, carlos.gonzalez@naturasol.com.mx, josue.velasco@naturasol.com.mx, mc.martinez@mielmex.com, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                        //correoBLL.enviarCorreo("Productos proximos a caducar " + listaClientes[j].U_CLIENTE + "-" + listaClientes[j].cardname, body, "guillermo.pelagio@naturasol.com.mx", null);
                    }
                }
            }
        }
    }
}

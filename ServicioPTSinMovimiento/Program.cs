using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioPTSinMovimiento
{
    class Program
    {
        static void Main(string[] args)
        {
            InventarioBLL inventariosBLL = new InventarioBLL();
            CorreosBLL correoBLL = new CorreosBLL();
            
            bool enviaCorreo = false;

            List<Inventario> listaPTSinMovimiento = new List<Inventario>();

            string body = "";
            decimal montoTotal = 0;
            body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>PT sin venta - últimas 12 semanas -<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Los siguientes articulos no han tenido movimiento de venta en la sociedad indicada en las ultimas 12 semanas: <br> <br> Monto total: <strong> valorMontoTotal MXP</strong> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ALMACEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE ALMACEN<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>LOTE<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CADUCIDAD<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>STOCK<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>UM<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>VALOR DE INVENTARIO<o:p></o:p></span></p> </td> </tr>";
            listaPTSinMovimiento = inventariosBLL.obtenerPTSinMovimiento();
            if (listaPTSinMovimiento.Count > 0)
            {
                for (int m = 0; m < listaPTSinMovimiento.Count; m++)
                {
                    enviaCorreo = true;

                    body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorarticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordescripcion<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorsociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valoralmacen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valornombrealmacen<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorLote<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorCaducidad<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorstock<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorUM<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorInventario<o:p></o:p></span></p> </td> </tr>";
                    body = body.Replace("valorarticulo", listaPTSinMovimiento[m].Artículo);
                    body = body.Replace("valordescripcion", listaPTSinMovimiento[m].Descripción);
                    body = body.Replace("valorsociedad", listaPTSinMovimiento[m].Sociedad);
                    body = body.Replace("valoralmacen", listaPTSinMovimiento[m].Almacén);
                    body = body.Replace("valornombrealmacen", listaPTSinMovimiento[m].NombreA);
                    body = body.Replace("valorUM", listaPTSinMovimiento[m].UM);
                    body = body.Replace("valorLote", listaPTSinMovimiento[m].Lote);
                    body = body.Replace("valorCaducidad", Convert.ToDateTime(listaPTSinMovimiento[m].Caducidad).ToShortDateString());
                    body = body.Replace("valorstock", listaPTSinMovimiento[m].Stock);
                    body = body.Replace("valorInventario", string.Format("{0:C}", Convert.ToDecimal(listaPTSinMovimiento[m].valorInventario)));

                    montoTotal = montoTotal + Convert.ToDecimal(listaPTSinMovimiento[m].valorInventario);
                }
            }

            body = body.Replace("valorMontoTotal", string.Format("{0:C}", montoTotal));

            if (enviaCorreo)
            {
                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                body = body.Replace("valor7", DateTime.Now.ToString());

                correoBLL.enviarCorreo("PT sin venta - ultimas 12 semanas", body, "alfredo.trejo@mielmex.com, fabian.aguirre@naturasol.com.mx, oswaldo.mendez@naturasol.com.mx, ricardo.vega@naturasol.com.mx, guadalupe.gonzalez@naturasol.com.mx, ngarcia@mielmex.com, angel.santoyo@naturasol.com.mx, jenifer.castillo@naturasol.com.mx, andres.sanchez@naturasol.com.mx, alejandra.vilchis@naturasol.com.mx, marco.garcia@naturasol.com.mx, alejandra.bobadilla@mielmex.com, daniel.duran@naturasol.com.mx, gustavo.valenzuela@naturasol.com.mx, hugo.hernandez@naturasol.com.mx", "paola.posadas@naturasol.com.mx, samuel.pena@naturasol.com.mx, mario.esquivel@mielmex.com, jorge.rincon@naturasol.com.mx, ivaladez@naturasol.com.mx, jorge.vazquez@naturasol.com.mx, alejandro.cantu@naturasol.com.mx, carlos.gonzalez@naturasol.com.mx, josue.velasco@naturasol.com.mx, mc.martinez@mielmex.com, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                //correoBLL.enviarCorreo("PT sin venta - ultimas 12 semanas", body, "guillermo.pelagio@naturasol.com.mx", null);
            }
        }
    }
}

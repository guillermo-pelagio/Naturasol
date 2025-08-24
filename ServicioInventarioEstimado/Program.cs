using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioInventarioEstimado
{
    class Program
    {
        static void Main(string[] args)
        {
            InventarioBLL inventariosBLL = new InventarioBLL();
            CorreosBLL correoBLL = new CorreosBLL();

            bool enviaCorreo = false;

            List<Inventario> listaSemanasStock = new List<Inventario>();

            string body = "";
            decimal montoTotal = 0;
            body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Semanas de stock <o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Basado en el consumo de las últimas 12 semanas en la sociedad respectiva se tienen estas semanas de stock para los siguientes artículos: <br> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>ARTICULO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DESCRIPCION<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>EXISTENCIA ACTUAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>UNIDAD DE MEDIDA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CONSUMO ULTIMAS 12 SEMANAS<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CONSUMO PROMEDIO SEMANAL<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SEMANAS DE EXISTENCIA ESTIMADAS<o:p></o:p></span></p> </td> </tr>";
            listaSemanasStock = inventariosBLL.obtenerSemanasStock();
            if (listaSemanasStock.Count > 0)
            {
                for (int m = 0; m < listaSemanasStock.Count; m++)
                {
                    enviaCorreo = true;

                    body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorarticulo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordescripcion<o:p></o:p></span></p> </td>  <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorActual<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorUM<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valor12<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorpromedio<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorEstimado<o:p></o:p></span></p> </td> </tr>";
                    body = body.Replace("valorarticulo", listaSemanasStock[m].Artículo);
                    body = body.Replace("valordescripcion", listaSemanasStock[m].Descripción);
                    //body = body.Replace("valorsociedad", listaSemanasStock[m].Sociedad);
                    body = body.Replace("valorActual", listaSemanasStock[m].Stock);
                    body = body.Replace("valor12", listaSemanasStock[m].Consumo);
                    body = body.Replace("valorUM", listaSemanasStock[m].UM);
                    body = body.Replace("valorpromedio", listaSemanasStock[m].ConsumoSemanal);
                    body = body.Replace("valorEstimado", listaSemanasStock[m].SemanasInventario);

                }

            }

            if (enviaCorreo)
            {
                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                body = body.Replace("valor7", DateTime.Now.ToString());

                correoBLL.enviarCorreo("Articulos con menos de 4 semanas de stock - basado en consumo ultimas 12 semanas", body, "fabian.aguirre@naturasol.com.mx, aplaneacion.semillas@naturasol.com.mx, aplaneacion.barras@naturasol.com.mx, planeacion.miel@mielmex.com, eduardo.ugalde@naturasol.com.mx, carlos.castaneda@naturasol.com.mx", "jenifer.castillo@naturasol.com.mx, angel.santoyo@naturasol.com.mx, noe.orozco@naturasol.com.mx, jose.chacon@naturasol.com.mx");
                //correoBLL.enviarCorreo("Articulos con menos de 4 semanas de stock - basado en consumo ultimas 12 semanas", body, "guillermo.pelagio@naturasol.com.mx ", null);
            }

        }
    }
}

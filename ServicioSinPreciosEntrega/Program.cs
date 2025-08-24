using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioSinPreciosEntrega
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenCompraBLL = new OrdenesCompraBLL();
            CorreosBLL correoBLL = new CorreosBLL();            
            bool enviaCorreo = false;

            List<OrdenCompra> listaEntradasSinPE = new List<OrdenCompra>();

            string body = "";
            decimal montoTotal = 0;
            body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Entradas sin costos de importación<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> Las siguientes entradas no han tenido algún movimiento de costo de importación: <br> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DOCUMENTO<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>PROVEEDOR<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>FECHA ENTRADA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>REFERENCIA<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>TOTAL<o:p></o:p><o:p></o:p></span></p> </td> </tr>";
            listaEntradasSinPE = ordenCompraBLL.obtenerEntradasSinPE();
            if (listaEntradasSinPE.Count > 0)
            {
                for (int m = 0; m < listaEntradasSinPE.Count; m++)
                {
                    enviaCorreo = true;

                    body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorsociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valordocumento<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorproveedor<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorfecha<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorreferencia<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valortotal<o:p></o:p></span></p> </td> </tr>";
                    body = body.Replace("valorsociedad", "NATURASOL");
                    body = body.Replace("valordocumento", listaEntradasSinPE[m].DocNum);
                    body = body.Replace("valorproveedor", listaEntradasSinPE[m].CardCode + " - " + listaEntradasSinPE[m].CardName);
                    body = body.Replace("valorfecha", Convert.ToDateTime(listaEntradasSinPE[m].DocDate).ToShortDateString());
                    body = body.Replace("valorreferencia", listaEntradasSinPE[m].NumAtCard);
                    body = body.Replace("valortotal", Convert.ToDouble(listaEntradasSinPE[m].DocTotalFC) == 0 ? string.Format("{0:C}", Convert.ToDecimal(listaEntradasSinPE[m].DocTotal)) : string.Format("{0:C}", Convert.ToDecimal(listaEntradasSinPE[m].DocTotal)) + " " + listaEntradasSinPE[m].Currency);
                }
            }

            if (enviaCorreo)
            {
                body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                body = body.Replace("valor7", DateTime.Now.ToString());

                correoBLL.enviarCorreo("Entradas sin precios de entrega", body, "yoselin.jasso@naturasol.com.mx, enrique.martinez@naturasol.com.mx, zulema.oviedo@naturasol.com.mx, ximena.abaroa@naturasol.com.mx", "angel.santoyo@naturasol.com.mx, jenifer.castillo@naturasol.com.mx");
            }
        }
    }
}
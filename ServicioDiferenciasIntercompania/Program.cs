using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServicioDiferenciasIntercompania
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string[] basesDatos = { "TSSL_Naturasol" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<Intercompania> saldosIntercompanias = new List<Intercompania>();
                saldosIntercompanias = ordenesCompraBLL.obtenerSaldosIntercompanias(basesDatos[i]);
                if (saldosIntercompanias.Count > 0)
                {
                    string body = "";
                    bool enviaCorreo = false;
                    body = "<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: 0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <h1 align=center style='text-align:center'><span style='font-family:Palatino; mso-fareast-font-family:'Times New Roman''>Diferencias intercompanias<o:p></o:p></span></h1> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='85%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> <tr style='mso-yfti-irow:1;mso-yfti-lastrow:yes'> <td style='background:#F8F8F8;padding:0cm 0cm 0cm 0cm'> <blockquote style='margin-top:5.0pt;margin-bottom:5.0pt'> <p class=MsoNormal style='margin-bottom:12.0pt'> <strong><span style='font-family:Palatino;mso-bidi-font-family: Calibri'></span><span style='font-family:Palatino'><o:p></o:p></span></p> <table class=MsoNormalTable border=1 cellspacing=3 cellpadding=0 style='mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'> <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CODIGO CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SALDO MXP<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SOCIEDAD<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>CODIGO CLIENTE<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>NOMBRE CLIENTE<o:p></o:p><o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>SALDO<o:p></o:p><o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>DIFERENCIA MXP<o:p></o:p><o:p></o:p></span></p> </td></tr>";

                    if (Convert.ToDecimal(saldosIntercompanias[0].Balance) + Convert.ToDecimal(saldosIntercompanias[29].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("valorSociedad2", saldosIntercompanias[29].Sociedad);
                        body = body.Replace("valorcodigo2", saldosIntercompanias[29].CardCode);
                        body = body.Replace("valornombre2", saldosIntercompanias[29].CardName);
                        body = body.Replace("balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[29].Balance)));
                        body = body.Replace("diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[0].Balance) + Convert.ToDecimal(saldosIntercompanias[29].Balance)));
                        body = body.Replace("valorSociedad", saldosIntercompanias[0].Sociedad);
                        body = body.Replace("valorcodigo", saldosIntercompanias[0].CardCode);
                        body = body.Replace("valornombre", saldosIntercompanias[0].CardName);
                        body = body.Replace("balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[0].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[2].Balance) + Convert.ToDecimal(saldosIntercompanias[61].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>2diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("23valorSociedad2", saldosIntercompanias[61].Sociedad);
                        body = body.Replace("2valorcodigo2", saldosIntercompanias[61].CardCode);
                        body = body.Replace("2valornombre2", saldosIntercompanias[61].CardName);
                        body = body.Replace("2balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[21].Balance)));
                        body = body.Replace("2diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[2].Balance) + Convert.ToDecimal(saldosIntercompanias[61].Balance)));
                        body = body.Replace("2valorSociedad", saldosIntercompanias[2].Sociedad);
                        body = body.Replace("2valorcodigo", saldosIntercompanias[2].CardCode);
                        body = body.Replace("2valornombre", saldosIntercompanias[2].CardName);
                        body = body.Replace("2balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[2].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[3].Balance) + Convert.ToDecimal(saldosIntercompanias[45].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>3diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("3valorSociedad2", saldosIntercompanias[45].Sociedad);
                        body = body.Replace("3valorcodigo2", saldosIntercompanias[45].CardCode);
                        body = body.Replace("3valornombre2", saldosIntercompanias[45].CardName);
                        body = body.Replace("3balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[45].Balance)));
                        body = body.Replace("3diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[3].Balance) + Convert.ToDecimal(saldosIntercompanias[45].Balance)));
                        body = body.Replace("3valorSociedad", saldosIntercompanias[3].Sociedad);
                        body = body.Replace("3valorcodigo", saldosIntercompanias[3].CardCode);
                        body = body.Replace("3valornombre", saldosIntercompanias[3].CardName);
                        body = body.Replace("3balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[3].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[5].Balance) + Convert.ToDecimal(saldosIntercompanias[57].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>4diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("4valorSociedad2", saldosIntercompanias[57].Sociedad);
                        body = body.Replace("4valorcodigo2", saldosIntercompanias[57].CardCode);
                        body = body.Replace("4valornombre2", saldosIntercompanias[57].CardName);
                        body = body.Replace("4balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[57].Balance)));
                        body = body.Replace("4diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[5].Balance) + Convert.ToDecimal(saldosIntercompanias[57].Balance)));
                        body = body.Replace("4valorSociedad", saldosIntercompanias[5].Sociedad);
                        body = body.Replace("4valorcodigo", saldosIntercompanias[5].CardCode);
                        body = body.Replace("4valornombre", saldosIntercompanias[5].CardName);
                        body = body.Replace("4balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[5].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[6].Balance) + Convert.ToDecimal(saldosIntercompanias[25].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>5diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("5valorSociedad2", saldosIntercompanias[25].Sociedad);
                        body = body.Replace("5valorcodigo2", saldosIntercompanias[25].CardCode);
                        body = body.Replace("5valornombre2", saldosIntercompanias[25].CardName);
                        body = body.Replace("5balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[25].Balance)));
                        body = body.Replace("5diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[6].Balance) + Convert.ToDecimal(saldosIntercompanias[25].Balance)));
                        body = body.Replace("5valorSociedad", saldosIntercompanias[6].Sociedad);
                        body = body.Replace("5valorcodigo", saldosIntercompanias[6].CardCode);
                        body = body.Replace("5valornombre", saldosIntercompanias[6].CardName);
                        body = body.Replace("5balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[6].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[7].Balance) + Convert.ToDecimal(saldosIntercompanias[41].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>6diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("6valorSociedad2", saldosIntercompanias[41].Sociedad);
                        body = body.Replace("6valorcodigo2", saldosIntercompanias[41].CardCode);
                        body = body.Replace("6valornombre2", saldosIntercompanias[41].CardName);
                        body = body.Replace("6balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[41].Balance)));
                        body = body.Replace("6diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[7].Balance) + Convert.ToDecimal(saldosIntercompanias[41].Balance)));
                        body = body.Replace("6valorSociedad", saldosIntercompanias[7].Sociedad);
                        body = body.Replace("6valorcodigo", saldosIntercompanias[7].CardCode);
                        body = body.Replace("6valornombre", saldosIntercompanias[7].CardName);
                        body = body.Replace("6balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[7].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[8].Balance) + Convert.ToDecimal(saldosIntercompanias[20].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>7diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("7valorSociedad2", saldosIntercompanias[20].Sociedad);
                        body = body.Replace("7valorcodigo2", saldosIntercompanias[20].CardCode);
                        body = body.Replace("7valornombre2", saldosIntercompanias[20].CardName);
                        body = body.Replace("7balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[20].Balance)));
                        body = body.Replace("7diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[8].Balance) + Convert.ToDecimal(saldosIntercompanias[20].Balance)));
                        body = body.Replace("7valorSociedad", saldosIntercompanias[8].Sociedad);
                        body = body.Replace("7valorcodigo", saldosIntercompanias[8].CardCode);
                        body = body.Replace("7valornombre", saldosIntercompanias[8].CardName);
                        body = body.Replace("7balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[8].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[10].Balance) + Convert.ToDecimal(saldosIntercompanias[52].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>8diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("8valorSociedad2", saldosIntercompanias[52].Sociedad);
                        body = body.Replace("8valorcodigo2", saldosIntercompanias[52].CardCode);
                        body = body.Replace("8valornombre2", saldosIntercompanias[52].CardName);
                        body = body.Replace("8balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[52].Balance)));
                        body = body.Replace("8diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[10].Balance) + Convert.ToDecimal(saldosIntercompanias[52].Balance)));
                        body = body.Replace("8valorSociedad", saldosIntercompanias[10].Sociedad);
                        body = body.Replace("8valorcodigo", saldosIntercompanias[10].CardCode);
                        body = body.Replace("8valornombre", saldosIntercompanias[10].CardName);
                        body = body.Replace("8balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[10].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[11].Balance) + Convert.ToDecimal(saldosIntercompanias[36].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>9diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("9valorSociedad2", saldosIntercompanias[36].Sociedad);
                        body = body.Replace("9valorcodigo2", saldosIntercompanias[36].CardCode);
                        body = body.Replace("9valornombre2", saldosIntercompanias[36].CardName);
                        body = body.Replace("9balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[36].Balance)));
                        body = body.Replace("9diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[11].Balance) + Convert.ToDecimal(saldosIntercompanias[36].Balance)));
                        body = body.Replace("9valorSociedad", saldosIntercompanias[11].Sociedad);
                        body = body.Replace("9valorcodigo", saldosIntercompanias[11].CardCode);
                        body = body.Replace("9valornombre", saldosIntercompanias[11].CardName);
                        body = body.Replace("9balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[11].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[12].Balance) + Convert.ToDecimal(saldosIntercompanias[17].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>10diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("10valorSociedad2", saldosIntercompanias[17].Sociedad);
                        body = body.Replace("10valorcodigo2", saldosIntercompanias[17].CardCode);
                        body = body.Replace("10valornombre2", saldosIntercompanias[17].CardName);
                        body = body.Replace("10balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[17].Balance)));
                        body = body.Replace("10diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[12].Balance) + Convert.ToDecimal(saldosIntercompanias[17].Balance)));
                        body = body.Replace("10valorSociedad", saldosIntercompanias[12].Sociedad);
                        body = body.Replace("10valorcodigo", saldosIntercompanias[12].CardCode);
                        body = body.Replace("10valornombre", saldosIntercompanias[12].CardName);
                        body = body.Replace("10balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[12].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[14].Balance) + Convert.ToDecimal(saldosIntercompanias[49].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>11diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("11valorSociedad2", saldosIntercompanias[49].Sociedad);
                        body = body.Replace("11valorcodigo2", saldosIntercompanias[49].CardCode);
                        body = body.Replace("11valornombre2", saldosIntercompanias[49].CardName);
                        body = body.Replace("11balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[49].Balance)));
                        body = body.Replace("11diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[14].Balance) + Convert.ToDecimal(saldosIntercompanias[49].Balance)));
                        body = body.Replace("11valorSociedad", saldosIntercompanias[14].Sociedad);
                        body = body.Replace("11valorcodigo", saldosIntercompanias[14].CardCode);
                        body = body.Replace("11valornombre", saldosIntercompanias[14].CardName);
                        body = body.Replace("11balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[14].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[15].Balance) + Convert.ToDecimal(saldosIntercompanias[33].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>12diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("12valorSociedad", saldosIntercompanias[33].Sociedad);
                        body = body.Replace("12valorcodigo", saldosIntercompanias[33].CardCode);
                        body = body.Replace("12valornombre", saldosIntercompanias[33].CardName);
                        body = body.Replace("12balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[33].Balance)));
                        body = body.Replace("12diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[15].Balance) + Convert.ToDecimal(saldosIntercompanias[33].Balance)));
                        body = body.Replace("12valorSociedad", saldosIntercompanias[15].Sociedad);
                        body = body.Replace("12valorcodigo", saldosIntercompanias[15].CardCode);
                        body = body.Replace("12valornombre", saldosIntercompanias[15].CardName);
                        body = body.Replace("12balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[15].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[18].Balance) + Convert.ToDecimal(saldosIntercompanias[60].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>13diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("13valorSociedad2", saldosIntercompanias[60].Sociedad);
                        body = body.Replace("13valorcodigo2", saldosIntercompanias[60].CardCode);
                        body = body.Replace("13valornombre2", saldosIntercompanias[60].CardName);
                        body = body.Replace("13balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[60].Balance)));
                        body = body.Replace("13diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[18].Balance) + Convert.ToDecimal(saldosIntercompanias[60].Balance)));
                        body = body.Replace("13valorSociedad", saldosIntercompanias[18].Sociedad);
                        body = body.Replace("13valorcodigo", saldosIntercompanias[18].CardCode);
                        body = body.Replace("13valornombre", saldosIntercompanias[18].CardName);
                        body = body.Replace("13balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[18].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[19].Balance) + Convert.ToDecimal(saldosIntercompanias[44].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>14diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("14valorSociedad2", saldosIntercompanias[44].Sociedad);
                        body = body.Replace("14valorcodigo2", saldosIntercompanias[44].CardCode);
                        body = body.Replace("14valornombre2", saldosIntercompanias[44].CardName);
                        body = body.Replace("14balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[44].Balance)));
                        body = body.Replace("14diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[19].Balance) + Convert.ToDecimal(saldosIntercompanias[44].Balance)));
                        body = body.Replace("14valorSociedad", saldosIntercompanias[19].Sociedad);
                        body = body.Replace("14valorcodigo", saldosIntercompanias[19].CardCode);
                        body = body.Replace("14valornombre", saldosIntercompanias[19].CardName);
                        body = body.Replace("14balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[19].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[21].Balance) + Convert.ToDecimal(saldosIntercompanias[56].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>15diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("15valorSociedad2", saldosIntercompanias[56].Sociedad);
                        body = body.Replace("15valorcodigo2", saldosIntercompanias[56].CardCode);
                        body = body.Replace("15valornombre2", saldosIntercompanias[56].CardName);
                        body = body.Replace("15balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[21].Balance)));
                        body = body.Replace("15diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[21].Balance) + Convert.ToDecimal(saldosIntercompanias[56].Balance)));
                        body = body.Replace("15valorSociedad", saldosIntercompanias[21].Sociedad);
                        body = body.Replace("15valorcodigo", saldosIntercompanias[21].CardCode);
                        body = body.Replace("15valornombre", saldosIntercompanias[21].CardName);
                        body = body.Replace("15balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[21].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[23].Balance) + Convert.ToDecimal(saldosIntercompanias[40].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>16diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("16valorSociedad2", saldosIntercompanias[40].Sociedad);
                        body = body.Replace("16valorcodigo2", saldosIntercompanias[40].CardCode);
                        body = body.Replace("16valornombre2", saldosIntercompanias[40].CardName);
                        body = body.Replace("16balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[40].Balance)));
                        body = body.Replace("16diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[23].Balance) + Convert.ToDecimal(saldosIntercompanias[40].Balance)));
                        body = body.Replace("16valorSociedad", saldosIntercompanias[23].Sociedad);
                        body = body.Replace("16valorcodigo", saldosIntercompanias[23].CardCode);
                        body = body.Replace("16valornombre", saldosIntercompanias[23].CardName);
                        body = body.Replace("16balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[23].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[26].Balance) + Convert.ToDecimal(saldosIntercompanias[54].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>17diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("17valorSociedad2", saldosIntercompanias[54].Sociedad);
                        body = body.Replace("17valorcodigo2", saldosIntercompanias[54].CardCode);
                        body = body.Replace("17valornombre2", saldosIntercompanias[0].CardName);
                        body = body.Replace("17balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[54].Balance)));
                        body = body.Replace("17diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[26].Balance) + Convert.ToDecimal(saldosIntercompanias[54].Balance)));
                        body = body.Replace("17valorSociedad", saldosIntercompanias[26].Sociedad);
                        body = body.Replace("17valorcodigo", saldosIntercompanias[26].CardCode);
                        body = body.Replace("17valornombre", saldosIntercompanias[26].CardName);
                        body = body.Replace("17balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[26].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[27].Balance) + Convert.ToDecimal(saldosIntercompanias[38].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>18diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("18valorSociedad2", saldosIntercompanias[38].Sociedad);
                        body = body.Replace("18valorcodigo2", saldosIntercompanias[38].CardCode);
                        body = body.Replace("18valornombre2", saldosIntercompanias[38].CardName);
                        body = body.Replace("18balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[38].Balance)));
                        body = body.Replace("18diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[27].Balance) + Convert.ToDecimal(saldosIntercompanias[38].Balance)));
                        body = body.Replace("18valorSociedad", saldosIntercompanias[27].Sociedad);
                        body = body.Replace("18valorcodigo", saldosIntercompanias[27].CardCode);
                        body = body.Replace("18valornombre", saldosIntercompanias[27].CardName);
                        body = body.Replace("18balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[27].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[30].Balance) + Convert.ToDecimal(saldosIntercompanias[48].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>19diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("19valorSociedad2", saldosIntercompanias[48].Sociedad);
                        body = body.Replace("19valorcodigo2", saldosIntercompanias[48].CardCode);
                        body = body.Replace("19valornombre2", saldosIntercompanias[48].CardName);
                        body = body.Replace("19balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[48].Balance)));
                        body = body.Replace("19diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[30].Balance) + Convert.ToDecimal(saldosIntercompanias[48].Balance)));
                        body = body.Replace("19valorSociedad", saldosIntercompanias[30].Sociedad);
                        body = body.Replace("19valorcodigo", saldosIntercompanias[30].CardCode);
                        body = body.Replace("19valornombre", saldosIntercompanias[30].CardName);
                        body = body.Replace("19balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[30].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[31].Balance) + Convert.ToDecimal(saldosIntercompanias[32].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>20diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("20valorSociedad2", saldosIntercompanias[32].Sociedad);
                        body = body.Replace("20valorcodigo2", saldosIntercompanias[32].CardCode);
                        body = body.Replace("20valornombre2", saldosIntercompanias[32].CardName);
                        body = body.Replace("20balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[32].Balance)));
                        body = body.Replace("20diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[31].Balance) + Convert.ToDecimal(saldosIntercompanias[32].Balance)));
                        body = body.Replace("20valorSociedad", saldosIntercompanias[31].Sociedad);
                        body = body.Replace("20valorcodigo", saldosIntercompanias[31].CardCode);
                        body = body.Replace("20valornombre", saldosIntercompanias[31].CardName);
                        body = body.Replace("20balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[31].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[34].Balance) + Convert.ToDecimal(saldosIntercompanias[63].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>21diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("21valorSociedad2", saldosIntercompanias[63].Sociedad);
                        body = body.Replace("21valorcodigo2", saldosIntercompanias[63].CardCode);
                        body = body.Replace("21valornombre2", saldosIntercompanias[63].CardName);
                        body = body.Replace("21balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[63].Balance)));
                        body = body.Replace("21diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[34].Balance) + Convert.ToDecimal(saldosIntercompanias[63].Balance)));
                        body = body.Replace("21valorSociedad", saldosIntercompanias[34].Sociedad);
                        body = body.Replace("21valorcodigo", saldosIntercompanias[34].CardCode);
                        body = body.Replace("21valornombre", saldosIntercompanias[34].CardName);
                        body = body.Replace("21balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[34].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[37].Balance) + Convert.ToDecimal(saldosIntercompanias[59].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>22diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("22valorSociedad2", saldosIntercompanias[59].Sociedad);
                        body = body.Replace("22valorcodigo2", saldosIntercompanias[59].CardCode);
                        body = body.Replace("22valornombre2", saldosIntercompanias[59].CardName);
                        body = body.Replace("22balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[59].Balance)));
                        body = body.Replace("22diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[37].Balance) + Convert.ToDecimal(saldosIntercompanias[59].Balance)));
                        body = body.Replace("22valorSociedad", saldosIntercompanias[37].Sociedad);
                        body = body.Replace("22valorcodigo", saldosIntercompanias[37].CardCode);
                        body = body.Replace("22valornombre", saldosIntercompanias[37].CardName);
                        body = body.Replace("22balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[37].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[42].Balance) + Convert.ToDecimal(saldosIntercompanias[55].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>23diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("23valorSociedad2", saldosIntercompanias[55].Sociedad);
                        body = body.Replace("23valorcodigo2", saldosIntercompanias[55].CardCode);
                        body = body.Replace("23valornombre2", saldosIntercompanias[55].CardName);
                        body = body.Replace("23balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[55].Balance)));
                        body = body.Replace("23diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[42].Balance) + Convert.ToDecimal(saldosIntercompanias[55].Balance)));
                        body = body.Replace("23valorSociedad", saldosIntercompanias[42].Sociedad);
                        body = body.Replace("23valorcodigo", saldosIntercompanias[42].CardCode);
                        body = body.Replace("23valornombre", saldosIntercompanias[42].CardName);
                        body = body.Replace("23balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[42].Balance)));
                    }
                    if (Convert.ToDecimal(saldosIntercompanias[46].Balance) + Convert.ToDecimal(saldosIntercompanias[51].Balance) != 0)
                    {
                        body = body + "<tr style='mso-yfti-irow:6'> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valorSociedad<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valorcodigo<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valornombre<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24balance<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valorSociedad2<o:p></o:p></span></p> </td> <td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valorcodigo2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24valornombre2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24balance2<o:p></o:p></span></p> </td><td style='padding:.75pt .75pt .75pt .75pt'> <p class=MsoNormal><span style='font-size:9.0pt;font-family:Palatino'>24diferencia<o:p></o:p></span></p> </td> </tr>";
                        enviaCorreo = true;

                        body = body.Replace("24valorSociedad2", saldosIntercompanias[51].Sociedad);
                        body = body.Replace("24valorcodigo2", saldosIntercompanias[51].CardCode);
                        body = body.Replace("24valornombre2", saldosIntercompanias[51].CardName);
                        body = body.Replace("24balance2", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[51].Balance)));
                        body = body.Replace("24diferencia", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[46].Balance) + Convert.ToDecimal(saldosIntercompanias[51].Balance)));
                        body = body.Replace("24valorSociedad", saldosIntercompanias[46].Sociedad);
                        body = body.Replace("24valorcodigo", saldosIntercompanias[46].CardCode);
                        body = body.Replace("24valornombre", saldosIntercompanias[46].CardName);
                        body = body.Replace("24balance", string.Format("{0:C}", Convert.ToDecimal(saldosIntercompanias[46].Balance)));
                    }

                    if (enviaCorreo)
                    {
                        body = body + "</table> <p class=MsoNormal><span style='font-family:Palatino;color:black'><br> Este correo electrónico ha sido generado automáticamente,por lo que no es necesario responder al presente.<br> <br> Fecha original de envío del mensaje: <i>valor7</i> </span><span style='font-family:Palatino'><o:p></o:p></span></p> </blockquote> <div class=MsoNormal align=center style='text-align:center'><span style='font-family:Palatino;mso-fareast-font-family:'Times New Roman'; color:black'> <hr size=2 width='90%' noshade style='color:#A0A0A0' align=center> </span></div> </td> </tr> </table> <p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'><o:p>&nbsp;</o:p></span></p> </div> </body> </html>";
                        body = body.Replace("valor7", DateTime.Now.ToString());
                        correoBLL.enviarCorreo("Diferencias intercompania", body, "contabilidad@mielmex.com, contabilidad.miel@mielmex.com, miel.contabilidad@mielmex.com, cuentasxpagar@mielmex.com, rodrigo.lozano@naturasol.com.mx, alma.padilla@naturasol.com.mx, lmejia@mielmex.com, analista.tesoreria@naturasol.com.mx, jrenteria@mielmex.com, lortiz@mielmex.com, aux.cobranza@naturasol.com.mx", "mario.esquivel@mielmex.com, angel.santoyo@naturasol.com.mx, jenifer.castillo@naturasol.com.mx");
                    }
                }
            }
        }
    }
}

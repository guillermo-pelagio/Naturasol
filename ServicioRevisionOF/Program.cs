using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioRevisionOF
{
    /********************************************************************************************RETOMAMOS, SOLO YO DE MIENTRAS*/
    class Program
    {
        static void Main(string[] args)
        {
            OrdenFabricacionBLL ordenFabricacionBLL = new OrdenFabricacionBLL();
            CorreosBLL correoBLL = new CorreosBLL();

            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenFabricacion> listaOFAbiertas = new List<OrdenFabricacion>();
                listaOFAbiertas = ordenFabricacionBLL.buscarOFAbiertas(basesDatos[i]);

                if (listaOFAbiertas.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaOFAbiertas.Count; j++)
                    {
                        string diferencias = "";
                        string diferenciasCantidades = "";
                        List<OrdenFabricacionDetalle> listaOFDetalle = new List<OrdenFabricacionDetalle>();
                        List<ListaMateriales> listaMateriales = new List<ListaMateriales>();
                        listaOFDetalle = ordenFabricacionBLL.detalleOF(basesDatos[i], listaOFAbiertas[j].DocEntry);
                        listaMateriales = ordenFabricacionBLL.listaMaterialesOF(basesDatos[i], listaOFAbiertas[j].ItemCode);

                        //ANALISIS DE LOS ARTICULOS DIFERENTES DE LA OF CONTRA LA BOM
                        for (int k = 0; k < listaOFDetalle.Count; k++)
                        {
                            bool diferente = true;
                            bool diferenteCantidad = true;
                            int equivalentes = 0;

                            if (listaMateriales.Count > 0)
                            {
                                for (int m = 0; m < listaMateriales.Count; m++)
                                {
                                    if (!listaOFDetalle[k].ItemCode.Equals(""))
                                    {
                                        if (!listaOFDetalle[k].U_BOM.Equals("1"))
                                        {
                                            //SI UN ARTICULO DE LA OF NO ESTA EN LA LISTA
                                            if (listaOFDetalle[k].ItemCode.Equals(listaMateriales[m].Code))
                                            {
                                                diferente = false;

                                                //SI LA CANTIDAD BASE DEL ARTICULO DE LA OF NO ES IGUAL AL DE LA LISTA
                                                if (Convert.ToDecimal(listaOFDetalle[k].BaseQty) == Convert.ToDecimal(listaMateriales[m].Quantity))
                                                {
                                                    diferenteCantidad = false;
                                                }
                                            }
                                            //SI UN ARTICULO DE LA OF NO ESTA ES IGUAL AL DEL TEXTO
                                            if (listaOFDetalle[k].ItemCode.Equals(listaMateriales[m].LineText))
                                            {
                                                diferente = false;

                                                for (int n = 1; n < listaMateriales.Count; n++)
                                                {
                                                    //BUSCAS EL INMEDIATO ANTERIOR QUE SI SEA ARTICULO
                                                    if ((listaMateriales[m - n].Quantity != "NULL") && (listaMateriales[m - n].Quantity != ""))
                                                    {
                                                        //SI LA CANTIDAD BASE DEL ARTICULO DE LA OF NO ES IGUAL AL ORIGINAL DE LA LISTA
                                                        if (Convert.ToDecimal(listaOFDetalle[k].BaseQty) == Convert.ToDecimal(listaMateriales[m - n].Quantity))
                                                        {
                                                            diferenteCantidad = false;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            diferente = false;
                                            diferenteCantidad = false;
                                        }

                                    }


                                }
                            }
                            else
                            {
                                //OF MANUAL
                                if (!listaOFDetalle[k].ItemCode.Equals(""))
                                {

                                    if (listaOFDetalle[k].U_BOM.Equals("1"))
                                    {
                                        diferente = false;
                                        diferenteCantidad = false;
                                    }

                                }
                            }

                            string U_BOM = listaOFDetalle[k].U_BOM;
                            if (diferente && (!listaOFDetalle[k].ItemCode.Equals("")) && (!listaOFDetalle[k].U_BOM.Equals("3")))
                            {
                                U_BOM = "3";
                                diferencias = diferencias + "<br>" + "CODIGO: <b>" + listaOFDetalle[k].ItemCode + "</b> (" + listaOFDetalle[k].ItemName + "), ";
                            }
                            else if (diferenteCantidad && (!listaOFDetalle[k].ItemCode.Equals("")) && (!listaOFDetalle[k].U_BOM.Equals("4")))
                            {
                                U_BOM = "4";
                                diferenciasCantidades = diferenciasCantidades + "<br>" + "CODIGO: <b>" + listaOFDetalle[k].ItemCode + "</b> (" + listaOFDetalle[k].ItemName + "), ";
                            }

                            if ((!U_BOM.Equals("0")) && (!U_BOM.Equals("1")))
                            {
                                ordenFabricacionBLL.actualizarOF(listaOFAbiertas[j].DocEntry, listaOFAbiertas[j].PlannedQty, listaOFAbiertas[j].U_Planificado_Original, listaOFDetalle[k].visOrder, U_BOM);
                            }
                        }

                        List<OrdenFabricacionDetalle> buscarOFPartidasEliminadas = new List<OrdenFabricacionDetalle>();
                        buscarOFPartidasEliminadas = ordenFabricacionBLL.buscarOFPartidasEliminadas(basesDatos[i], listaOFAbiertas[j].DocNum);

                        int estatusOF = 1;
                        string errorCodigo = "";
                        string errorCantidades = "";
                        string diferenciasEliminadas = "";
                        string errorEliminados = "";

                        if (diferencias != "")
                        {
                            estatusOF = 2;
                            errorCodigo = "La OF " + listaOFAbiertas[j].DocNum + " tiene los siguientes códigos diferentes a la BOM: " + diferencias;
                            Console.WriteLine("La OF " + listaOFAbiertas[j].DocNum + " tiene los siguientes códigos diferentes a la BOM: " + diferencias);
                        }
                        if (diferenciasCantidades != "")
                        {
                            estatusOF = 2;
                            errorCantidades = "La OF " + listaOFAbiertas[j].DocNum + " tiene las siguientes cantidades base diferentes a la BOM: " + diferenciasCantidades;
                            Console.WriteLine("La OF " + listaOFAbiertas[j].DocNum + " tiene las siguientes cantidades base diferentes a la BOM: " + diferenciasCantidades);
                        }
                        if (buscarOFPartidasEliminadas.Count > 0)
                        {
                            estatusOF = 2;
                            for (int pe = 0; pe < buscarOFPartidasEliminadas.Count; pe++)
                            {
                                diferenciasEliminadas = diferenciasEliminadas + "<br>" + "CODIGO: <b>" + buscarOFPartidasEliminadas[pe].ItemCode + "</b> (" + buscarOFPartidasEliminadas[pe].ItemName + "), ";
                            }

                            errorEliminados = "La OF " + listaOFAbiertas[j].DocNum + " tiene los siguientes articulos eliminados: " + diferenciasEliminadas;
                            Console.WriteLine("La OF " + listaOFAbiertas[j].DocNum + " tiene los siguientes articulos eliminados: " + diferenciasEliminadas);
                        }

                        ordenFabricacionBLL.actualizarOF(listaOFAbiertas[j].DocEntry, listaOFAbiertas[j].PlannedQty, listaOFAbiertas[j].U_Planificado_Original, null, null);

                        if (!errorCodigo.Equals("") || !errorCantidades.Equals("") || !errorEliminados.Equals(""))
                        {
                            string correosFijos = "guillermo.pelagio@naturasol.com.mx";
                            /*
                            correosFijos = "jenifer.castillo@naturasol.com.mx, planeacion.central@naturasol.com.mx, andrea.aranderi@naturasol.com.mx, ruth.armenta@naturasol.com.mx, noe.gutierrez@naturasol.com.mx, jonathan.jimenez@naturasol.com.mx, said.lerin@naturasol.com.mx,";
                            if (basesDatos[i].Contains("Naturasol"))
                            {
                                if (listaOFAbiertas[j].DocNum.StartsWith("2"))
                                {
                                    correosFijos = correosFijos + "aplaneacion.barras@naturasol.com.mx,aproduccion.cereales@naturasol.com.mx,analistaproduccion.barras@naturasol.com.mx,proceso.barras@naturasol.com.mx";
                                }
                                if (listaOFAbiertas[j].DocNum.StartsWith("3"))
                                {
                                    correosFijos = correosFijos + "aplaneacion.semillas@naturasol.com.mx,noemi.ramirez@naturasol.com.mx,aproduccion.semillas@naturasol.com.mx,prodsem.etiquetas@naturasol.com.mx";
                                }
                                if (listaOFAbiertas[j].DocNum.StartsWith("4"))
                                {
                                    correosFijos = correosFijos + "aplaneacion.barras@naturasol.com.mx, aproduccion.cereales@naturasol.com.mx,analistaproduccion.barras@naturasol.com.mx,juan.gomez@naturasol.com.mx";
                                }
                                if (listaOFAbiertas[j].DocNum.StartsWith("5"))
                                {
                                    correosFijos = correosFijos + "aplaneacion.semillas@naturasol.com.mx,anaoper.papas@naturasol.com.mx,lidia.cabrera@naturasol.com.mx";
                                }
                                if (listaOFAbiertas[j].DocNum.StartsWith("6"))
                                {
                                    correosFijos = correosFijos + "aplaneacion.semillas@naturasol.com.mx,aproduccion.cereales@naturasol.com.mx,analistaproduccion.barras@naturasol.com.mx,csazonadores@naturasol.com.mx";
                                }
                            }
                            */

                            correoBLL.enviarCorreo("OF/BOM diferentes", leerPlantilla(errorCodigo, errorCantidades, errorEliminados, ""), correosFijos, null);
                        }

                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
            Console.WriteLine("Analisis terminado");
        }

        public static string leerPlantilla(String parrafo1, String parrafo2, String parrafo3, String parrafo4)
        {
            string body = "<!doctype html> <html> <head> <meta name='viewport' content='width=device-width, initial-scale=1.0' /> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /> <title>Simple Transactional Email</title> <style> img { border: none; -ms-interpolation-mode: bicubic; max-width: 100%; } body { background-color: #f6f6f6; font-family: sans-serif; -webkit-font-smoothing: antialiased; font-size: 14px; line-height: 1.4; margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; } table { border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%; } table td { font-family: sans-serif; font-size: 14px; vertical-align: top; } .body { background-color: #f6f6f6; width: 100%; } .container { display: block; margin: 0 auto !important; /* makes it centered */ max-width: 580px; padding: 10px; width: 580px; } .content { box-sizing: border-box; display: block; margin: 0 auto; max-width: 580px; padding: 10px; } .main { background: #ffffff; border-radius: 3px; width: 100%; } .wrapper { box-sizing: border-box; padding: 20px; } .content-block { padding-bottom: 10px; padding-top: 10px; } .footer { clear: both; margin-top: 10px; text-align: center; width: 100%; } .footer td, .footer p, .footer span, .footer a { color: #999999; font-size: 12px; text-align: center; } h1, h2, h3, h4 { color: #000000; font-family: sans-serif; font-weight: 400; line-height: 1.4; margin: 0; margin-bottom: 30px; } h1 { font-size: 35px; font-weight: 300; text-align: center; text-transform: capitalize; } p, ul, ol { font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; margin-bottom: 15px; } p li, ul li, ol li { list-style-position: inside; margin-left: 5px; } a { color: #3498db; text-decoration: underline; } .btn { box-sizing: border-box; width: 100%; } .btn > tbody > tr > td { padding-bottom: 15px; } .btn table { width: auto; } .btn table td { background-color: #ffffff; border-radius: 5px; text-align: center; } .btn a { background-color: #ffffff; border: solid 1px #3498db; border-radius: 5px; box-sizing: border-box; color: #3498db; cursor: pointer; display: inline-block; font-size: 14px; font-weight: bold; margin: 0; padding: 12px 25px; text-decoration: none; text-transform: capitalize; } .btn-primary table td { background-color: #3498db; } .btn-primary a { background-color: #3498db; border-color: #3498db; color: #ffffff; } .last { margin-bottom: 0; } .first { margin-top: 0; } .align-center { text-align: center; } .align-right { text-align: right; } .align-left { text-align: left; } .clear { clear: both; } .mt0 { margin-top: 0; } .mb0 { margin-bottom: 0; } .preheader { color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0; } .powered-by a { text-decoration: none; } hr { border: 0; border-bottom: 1px solid #f6f6f6; margin: 20px 0; }@media only screen and (max-width: 620px) { table.body h1 { font-size: 28px !important; margin-bottom: 10px !important; } table.body p, table.body ul, table.body ol, table.body td, table.body span, table.body a { font-size: 16px !important; } table.body .wrapper, table.body .article { padding: 10px !important; } table.body .content { padding: 0 !important; } table.body .container { padding: 0 !important; width: 100% !important; } table.body .main { border-left-width: 0 !important; border-radius: 0 !important; border-right-width: 0 !important; } table.body .btn table { width: 100% !important; } table.body .btn a { width: 100% !important; } table.body .img-responsive { height: auto !important; max-width: 100% !important; width: auto !important; } }@media all { .ExternalClass { width: 100%; } .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; } .apple-link a { color: inherit !important; font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; text-decoration: none !important; } #MessageViewBody a { color: inherit; text-decoration: none; font-size: inherit; font-family: inherit; font-weight: inherit; line-height: inherit; } .btn-primary table td:hover { background-color: #34495e !important; } .btn-primary a:hover { background-color: #34495e !important; border-color: #34495e !important; } } </style> </head> <body> <!--<span class='preheader'>preheader</span>--> <table role='presentation' border='0' cellpadding='0' cellspacing='0' class='body'> <tr> <td>&nbsp;</td> <td class='container'> <div class='content'> <table role='presentation' class='main'> <tr> <td class='wrapper'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tr> <td> <p>Buen día</p> <p>parrafo1</p> <p>parrafo2</p> <p>parrafo3</p> <p>parrafo4</p> <!--<table role='presentation' border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'> <tbody> <tr> <td align='left'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tbody> <tr> <td> <a>Call To Action</a> </td> <td> <a>Call To Action</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table>--> <!--<p>This is a really simple email template. Its sole purpose is to get the recipient to click the button with no distractions.</p> <p>Good luck! Hope it works.</p>--> </td> </tr> </table> </td> </tr> </table> <div class='footer'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tr> <td class='content-block'> <span class='apple-link'>Cto. Circunvalacion Pte. 9A, Cd. Satélite, 53100 Naucalpan de Juárez, Méx.</span> <br> Por favor, no responda a este mensaje, ha sido enviado de forma automática . </td> </tr> <tr> <td class='content-block powered-by'> Visita nuestras páginas <br> <br> <a href='http://naturasol.com.mx' target='_blank'>naturasol.com.mx</a><br> <a href='http://mielmex.com' target='_blank'>mielmex.com</a>. </td> </tr> </table> </div> </div> </td> <td>&nbsp;</td> </tr> </table> </body> </html>";
            body = body.Replace("parrafo1", parrafo1);
            body = body.Replace("parrafo2", parrafo2);
            body = body.Replace("parrafo3", parrafo3);
            body = body.Replace("parrafo4", parrafo4);
            return body;

        }
    }
}

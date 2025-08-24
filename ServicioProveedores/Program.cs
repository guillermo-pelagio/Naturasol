using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioProveedores
{
    class Program
    {
        public static void Main(string[] args)
        {
            ProveedoresBLL proveedoresBLL = new ProveedoresBLL();
            CorreosBLL correoBLL = new CorreosBLL();
            bool enviaCorreo = false;

            List<EntradaMaterial> listaEntradas = new List<EntradaMaterial>();
            string body = "";
            decimal montoTotal = 0;

            //while (true)
            {
                bool bandera = true;
                //if (DateTime.Now.Minute % 1 == 0 && bandera == true && DateTime.Now.Second > 15 && DateTime.Now.Second < 35)
                {
                    listaEntradas = proveedoresBLL.obtenerPendientesRevisar();
                    if (listaEntradas.Count > 0)
                    {
                        try
                        {
                            for (int m = 0; m < listaEntradas.Count; m++)
                            {
                                string path = listaEntradas[m].archivoPortal;

                                // This text is added only once to the file.
                                if (File.Exists(path))
                                {
                                    // Open the file to read from.
                                    string texto = File.ReadAllText(path);

                                    string sub = "";
                                    string total = "";
                                    string moneda = "";
                                    string rfcemisor = "";
                                    string rfcreceptor = "";
                                    string uuid = "";
                                    sub = texto.Substring(texto.IndexOf("SubTotal=\"") + "SubTotal=\"".Length).Substring(0, texto.Substring(texto.IndexOf("SubTotal=\"") + "SubTotal=\"".Length).IndexOf("\"") + "\"".Length - 1);
                                    moneda = texto.Substring(texto.IndexOf("Moneda=\"") + "Moneda=\"".Length).Substring(0, texto.Substring(texto.IndexOf("Moneda=\"") + "Moneda=\"".Length).IndexOf("\"") + "\"".Length - 1);
                                    total = texto.Substring(texto.IndexOf(" Total=\"") + " Total=\"".Length).Substring(0, texto.Substring(texto.IndexOf(" Total=\"") + " Total=\"".Length).IndexOf("\"") + "\"".Length - 1);
                                    rfcemisor = texto.Substring(texto.IndexOf("<cfdi:Emisor Rfc=\"") + "<cfdi:Emisor Rfc=\"".Length).Substring(0, texto.Substring(texto.IndexOf("<cfdi:Emisor Rfc=\"") + "<cfdi:Emisor Rfc=\"".Length).IndexOf("\"") + "\"".Length - 1);
                                    rfcreceptor = texto.Substring(texto.IndexOf("<cfdi:Receptor Rfc=\"") + "<cfdi:Receptor Rfc=\"".Length).Substring(0, texto.Substring(texto.IndexOf("<cfdi:Receptor Rfc=\"") + "<cfdi:Receptor Rfc=\"".Length).IndexOf("\"") + "\"".Length - 1);
                                    uuid = texto.Substring(texto.IndexOf("UUID=\"") + "UUID=\"".Length).Substring(0, texto.Substring(texto.IndexOf("UUID=\"") + "UUID=\"".Length).IndexOf("\"") + "\"".Length - 1);

                                    moneda = moneda == "MXN" ? "MXP" : moneda;
                                    string mensaje = "";
                                    if (listaEntradas[m].DocCur == moneda)
                                    {
                                        if (float.Parse(listaEntradas[m].DocTotal) == float.Parse(total) || float.Parse(listaEntradas[m].DocTotalFC) == float.Parse(total))
                                        {
                                            double subtotalSAPL = Math.Round(Convert.ToDouble(listaEntradas[m].DocTotal), 2) - Math.Round(Convert.ToDouble(listaEntradas[m].VatSum), 2);
                                            Console.WriteLine(subtotalSAPL);

                                            double subtotalSAPF = Math.Round(Convert.ToDouble(listaEntradas[m].DocTotalFC), 2) - Math.Round(Convert.ToDouble(listaEntradas[m].VatSumFC), 2);
                                            Console.WriteLine(subtotalSAPF);

                                            if ((float.Parse(Convert.ToString(subtotalSAPL)) - float.Parse(sub) == 0))
                                            {
                                                Console.WriteLine("okis");
                                            }
                                            else
                                            {
                                                Console.WriteLine("no okis");
                                            }

                                            double redondeadoL = Math.Round(subtotalSAPL, 2);
                                            double subs = Math.Round(Convert.ToDouble(sub), 2);
                                            double redondeadoF = Math.Round(subtotalSAPF, 2);


                                            if ((Math.Round(subtotalSAPL, 2) == Math.Round(Convert.ToDouble(sub), 2)) || (redondeadoF == Math.Round(Convert.ToDouble(sub), 2)))
                                            {
                                                //if (listaEntradas[m].LicTradNum.Equals(rfcemisor))
                                                {
                                                    //if (rfcreceptor.Equals("NAT0104253X3"))
                                                    {
                                                        Console.WriteLine("okis");
                                                        FacturaProveedorBLL facturaProveedorBLL = new FacturaProveedorBLL();
                                                        listaEntradas[m].UUID = uuid;
                                                        DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                                                        mensaje = facturaProveedorBLL.crearFacturaProveedor(listaEntradas[m]);
                                                        DIAPIBLL.desconectarDIAPI();
                                                        mensaje = "0";
                                                    }/*
                                                    else
                                                    {
                                                        mensaje = "6";
                                                    }*/
                                                }/*
                                                else
                                                {
                                                    mensaje = "5";
                                                }*/
                                            }
                                            else
                                            {
                                                mensaje = "4";
                                            }
                                        }
                                        else
                                        {
                                            mensaje = "3";
                                        }
                                    }
                                    else
                                    {
                                        mensaje = "2";
                                    }

                                    proveedoresBLL.actualizarEstatus(mensaje, listaEntradas[m].idPortal);
                                }

                                else
                                {
                                    //ERROR ARCHIVO NO ENCONTRADO
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //ERROR EN LECTURA DE ARCHIVO
                        }
                    }
                }                
            }
        }
    }
}

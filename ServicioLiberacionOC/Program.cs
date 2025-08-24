using CapaEntidades;
using CapaNegocios;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Exception = System.Exception;

namespace ServicioLiberacionOC
{
    class Program
    {
        static void Main(string[] args)
        {

            //while (true)
            {
                bool bandera = true;
                //if (DateTime.Now.Minute % 1 == 0 && bandera == true && DateTime.Now.Second > 15 && DateTime.Now.Second < 35)
                {
                    OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();

                    try
                    {
                        // Get the local computer host name.
                        String hostName = Dns.GetHostName();

                        if (hostName.Contains("POWERBISVR"))
                        {
                            List<BorradorDocumento> listaDocumentos = new List<BorradorDocumento>();
                            listaDocumentos = ordenesCompraBLL.obtenerOrdenesCompraActualizar();

                            if (listaDocumentos.Count > 0)
                            {
                                for (int k = 0; k < listaDocumentos.Count; k++)
                                {
                                    if (listaDocumentos[k].estatusAutorizacion.Equals("ardApproved") && listaDocumentos[k].estatus == 1)
                                    {
                                        DIAPIBLL.conectarDIAPI(listaDocumentos[k].sociedad);
                                        ordenesCompraBLL.updatePedidoCompra(Convert.ToInt32(listaDocumentos[k].wddCode), "ardApproved", "");
                                        DIAPIBLL.desconectarDIAPI();
                                        ordenesCompraBLL.actualizarSolicitudAutorizacionOC(Convert.ToString(listaDocumentos[k].wddCode), "ardApproved", "-1");
                                        Console.WriteLine("AUTORIZADA: " + listaDocumentos[k].sociedad + " - " + Convert.ToInt32(listaDocumentos[k].wddCode));
                                    }
                                    if (listaDocumentos[k].estatusAutorizacion.Equals("ardNotApproved") && listaDocumentos[k].estatus == 1)
                                    {
                                        DIAPIBLL.conectarDIAPI(listaDocumentos[k].sociedad);
                                        ordenesCompraBLL.updatePedidoCompra(Convert.ToInt32(listaDocumentos[k].wddCode), "ardNotApproved", "");
                                        DIAPIBLL.desconectarDIAPI();
                                        ordenesCompraBLL.actualizarSolicitudAutorizacionOC(Convert.ToString(listaDocumentos[k].wddCode), "ardNotApproved", "-1");
                                        Console.WriteLine("AUTORIZADA: " + listaDocumentos[k].sociedad + " - " + Convert.ToInt32(listaDocumentos[k].wddCode));
                                    }
                                    if (listaDocumentos[k].estatusAutorizacion.Equals("ardApproved") && listaDocumentos[k].estatus == 2)
                                    {
                                        DIAPIBLL.conectarDIAPIContabilidad(listaDocumentos[k].sociedad);
                                        ordenesCompraBLL.updatePedidoCompra(Convert.ToInt32(listaDocumentos[k].wddCode), "ardApproved", "");
                                        DIAPIBLL.conectarDIAPIContabilidad();
                                        ordenesCompraBLL.actualizarSolicitudAutorizacionOC(Convert.ToString(listaDocumentos[k].wddCode), "ardApproved", "-1");
                                        Console.WriteLine("AUTORIZADA: " + listaDocumentos[k].sociedad + " - " + Convert.ToInt32(listaDocumentos[k].wddCode));
                                    }
                                    if (listaDocumentos[k].estatusAutorizacion.Equals("ardNotApproved") && listaDocumentos[k].estatus == 2)
                                    {
                                        DIAPIBLL.conectarDIAPIContabilidad(listaDocumentos[k].sociedad);
                                        ordenesCompraBLL.updatePedidoCompra(Convert.ToInt32(listaDocumentos[k].wddCode), "ardNotApproved", "");
                                        DIAPIBLL.conectarDIAPIContabilidad();
                                        ordenesCompraBLL.actualizarSolicitudAutorizacionOC(Convert.ToString(listaDocumentos[k].wddCode), "ardNotApproved", "-1");
                                        Console.WriteLine("AUTORIZADA: " + listaDocumentos[k].sociedad + " - " + Convert.ToInt32(listaDocumentos[k].wddCode));
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                Application outlookApp = new Application();

                                // Log in to Outlook
                                NameSpace outlookNamespace = outlookApp.GetNamespace("MAPI");
                                outlookNamespace.Logon();

                                // Get the Inbox folder
                                MAPIFolder inboxFolder = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                                MAPIFolder destFolder = inboxFolder.Folders["Liberaciones"];
                                Items oItems = inboxFolder.Items.Restrict("[UnRead] = true");

                                foreach (MailItem email in oItems)
                                {
                                    try
                                    {
                                        if (email.Subject.Contains("Liberacion OC"))
                                        {
                                            if (email.SenderEmailAddress.Contains("mario.esquivel") && email.Body.Contains("Adelante"))
                                            {
                                                Console.WriteLine("Subject: " + email.Subject);
                                                Console.WriteLine("Sender: " + email.SenderName);

                                                int pFrom = email.Subject.IndexOf("- ") + "- ".Length;
                                                int pTo = email.Subject.LastIndexOf("- ");

                                                String result = email.Subject.Substring(pFrom, pTo - pFrom);

                                                ordenesCompraBLL.actualizarSolicitudAutorizacionOC(result, "ardApproved", "1");

                                                Console.WriteLine("AUTORIZADA FINANZAS: " + result);
                                                email.UnRead = false;
                                                email.Move(destFolder);
                                            }
                                            if (email.SenderEmailAddress.Contains("mario.esquivel") && email.Body.Contains("Rechazado"))
                                            {
                                                Console.WriteLine("Subject: " + email.Subject);
                                                Console.WriteLine("Sender: " + email.SenderName);


                                                int pFrom = email.Subject.IndexOf("- ") + "- ".Length;
                                                int pTo = email.Subject.LastIndexOf("- ");

                                                String result = email.Subject.Substring(pFrom, pTo - pFrom);

                                                Console.WriteLine("RECHAZADA FINANZAS: " + result);

                                                ordenesCompraBLL.actualizarSolicitudAutorizacionOC(result, "ardNotApproved", "1");
                                                email.UnRead = false;
                                                email.Move(destFolder);
                                            }

                                            if (email.SenderEmailAddress.Contains("guillermo.pelagio") && email.Body.Contains("Listo"))
                                            {
                                                Console.WriteLine("Subject: " + email.Subject);
                                                Console.WriteLine("Sender: " + email.SenderName);

                                                int pFrom = email.Subject.IndexOf("- ") + "- ".Length;
                                                int pTo = email.Subject.LastIndexOf("- ");

                                                String result = email.Subject.Substring(pFrom, pTo - pFrom);

                                                ordenesCompraBLL.actualizarSolicitudAutorizacionOC(result, "ardApproved", "1");

                                                Console.WriteLine("AUTORIZADA FINANZAS: " + result);
                                                email.UnRead = false;
                                                email.Move(destFolder);
                                            }
                                        }

                                        if (email.Subject.Contains("Liberacion OC"))
                                        {
                                            if (email.SenderEmailAddress.Contains("miel.contabilidad") && email.Body.Contains("Autorizado"))
                                            {
                                                Console.WriteLine("Subject: " + email.Subject);
                                                Console.WriteLine("Sender: " + email.SenderName);

                                                int pFrom = email.Subject.IndexOf("- ") + "- ".Length;
                                                int pTo = email.Subject.LastIndexOf("- ");

                                                String result = email.Subject.Substring(pFrom, pTo - pFrom);

                                                Console.WriteLine("AUTORIZADA CONTABILIDAD: " + result);

                                                ordenesCompraBLL.actualizarSolicitudAutorizacionOC(result, "ardApproved", "2");
                                                email.UnRead = false;
                                                email.Move(destFolder);
                                            }
                                            if (email.SenderEmailAddress.Contains("miel.contabilidad") && email.Body.Contains("Rechazado"))
                                            {
                                                Console.WriteLine("Subject: " + email.Subject);
                                                Console.WriteLine("Sender: " + email.SenderName);

                                                int pFrom = email.Subject.IndexOf("- ") + "- ".Length;
                                                int pTo = email.Subject.LastIndexOf("- ");

                                                String result = email.Subject.Substring(pFrom, pTo - pFrom);

                                                Console.WriteLine("RECHAZADA CONTABILIDAD: " + result);

                                                ordenesCompraBLL.actualizarSolicitudAutorizacionOC(result, "ardNotApproved", "2");
                                                email.UnRead = false;
                                                email.Move(destFolder);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error processing email: " + ex.Message);
                                    }
                                }

                                // Log out and release resources
                                outlookNamespace.Logoff();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookNamespace);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookApp);
                            }
                            catch (Exception ex)
                            {
                                // Handle general exceptions related to Outlook connection or login
                                Console.WriteLine("Error connecting to Outlook: " + ex.Message);
                            }
                            finally
                            {
                                // Ensure resources are properly released even in case of exceptions
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                        }
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("SocketException caught!!!");
                        Console.WriteLine("Source : " + e.Source);
                        Console.WriteLine("Message : " + e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception caught!!!");
                        Console.WriteLine("Source : " + e.Source);
                        Console.WriteLine("Message : " + e.Message);
                    }
                }
            }
        }
    }
}
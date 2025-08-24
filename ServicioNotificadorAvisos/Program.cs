using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioNotificadorAvisos
{
    class Program
    {
        /**************************************************************************INACTIVO - YA NO DEBERIA SER NECESARIO 10-01-24 **********************************************/
        static void Main(string[] args)
        {
            notificarAvisoCXP();
        }

        private static void notificarAvisoCXP()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            PagosEfectuadosBLL pagosEfectuadosBLL = new PagosEfectuadosBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex" };

            CorreosBLL correoBLL = new CorreosBLL();
            /*
            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<PagosEfectuados> listaSociosNegocios = new List<PagosEfectuados>();
                listaSociosNegocios = pagosEfectuadosBLL.obtenerInfoCXP(basesDatos[i]);

                if (listaSociosNegocios.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaSociosNegocios.Count; j++)
                    {*/
            string body = "<html lang='en'> <head> <meta charset='UTF-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Responsive Email Template</title> </head> <body> <img src=http://intranet.naturasol.com.mx:8001/Content/images/intranet.png' alt='img' /> </body> </html>";
            //string serie = basesDatos[i].Contains("Naturasol") ? "NAT" : "MM";
            body = body.Replace("valor1", DateTime.Now.ToShortDateString());
            body = body.Replace("valor7", DateTime.Now.ToString());

            //Console.WriteLine(body);
            correoBLL.enviarNotificacionAdjunto("Reporte de inventario", body, "C:\\inetpub\\Arribos\\Reportes\\Inventario 30-05-2025.pdf", "guillermo.pelagio@naturasol.com.mx");
            //correoBLL.enviarCorreoNotificador("Saldo al 31 de Octubre ", body, basesDatos[i], "guillermo.pelagio@naturasol.com.mx");
            //}
            //DIAPIBLL.desconectarDIAPI();
            //}
            //}
        }
    }
}
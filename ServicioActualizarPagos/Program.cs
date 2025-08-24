using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioActualizarPagos
{
    class ServicioActualizarPagos
    {
        //CLASE PARA ACTUALIZAR UN PAGO DE SAP -TAREA DE CADA MINUTO-
        static void Main(string[] args)
        {
            PagosBancomerBLL pagosBancomerBLL = new PagosBancomerBLL();
            List<PagosBancomer> listaPagosBancomer = new List<PagosBancomer>();
            List<PagosBancomer> listaPagosBancomerMielmex = new List<PagosBancomer>();
            List<PagosBancomer> listaPagosBancomerNaturasol = new List<PagosBancomer>();

            //BUSCA LOS PAGOS PENDIENTES
            listaPagosBancomer = pagosBancomerBLL.buscarPagosPendientesSAP();

            Console.WriteLine(listaPagosBancomer.Count + " detectados");

            //RECORRE LOS PAGOS
            foreach (PagosBancomer pago in listaPagosBancomer)
            {
                if (pago.sociedad == "TSSL_Mielmex")
                {
                    PagosBancomer pagosBancomer = new PagosBancomer();
                    pagosBancomer.numeroDocumento = pago.numeroDocumento;
                    pagosBancomer.sociedad = pago.sociedad;
                    listaPagosBancomerMielmex.Add(pagosBancomer);
                }
                else if (pago.sociedad == "TSSL_Naturasol")
                {
                    PagosBancomer pagosBancomer = new PagosBancomer();
                    pagosBancomer.numeroDocumento = pago.numeroDocumento;
                    pagosBancomer.sociedad = pago.sociedad;
                    listaPagosBancomerNaturasol.Add(pagosBancomer);
                }
                Console.WriteLine(listaPagosBancomerMielmex.Count + " pagos mielmex");
                Console.WriteLine(listaPagosBancomerNaturasol.Count + " pagos naturasol");
            }

            if (listaPagosBancomerMielmex.Count > 0)
            {
                //SE CONECTA A SAP            
                DIAPIBLL.conectarDIAPI("TSSL_Mielmex").ToString();
                Console.WriteLine("Conectando a Mielmex");
                foreach (PagosBancomer pago in listaPagosBancomerMielmex)
                {
                    //ACTUALIZA EL PAGO
                    Console.WriteLine("Actualizando " + pago.numeroDocumento);
                    if (PagosBancomerBLL.updatePago(Convert.ToInt32(pago.numeroDocumento)) == 0)
                    {
                        PagosBancomerBLL.updatePagoDesarrollo(pago.numeroDocumento, pago.sociedad);
                    }
                }

                DIAPIBLL.desconectarDIAPI();
            }
            if (listaPagosBancomerNaturasol.Count > 0)
            {
                //SE CONECTA A SAP            
                DIAPIBLL.conectarDIAPI("TSSL_Naturasol").ToString();
                Console.WriteLine("Conectando a Naturasol");

                foreach (PagosBancomer pago in listaPagosBancomerNaturasol)
                {
                    //ACTUALIZA EL PAGO
                    Console.WriteLine("Actualizando " + pago.numeroDocumento);
                    if (PagosBancomerBLL.updatePago(Convert.ToInt32(pago.numeroDocumento)) == 0)
                    {
                        PagosBancomerBLL.updatePagoDesarrollo(pago.numeroDocumento, pago.sociedad);
                    }
                }

                DIAPIBLL.desconectarDIAPI();
            }
        }
    }
}
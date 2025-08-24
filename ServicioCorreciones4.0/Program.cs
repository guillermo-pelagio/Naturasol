using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioCorreciones4._0
{
    class Program
    {
        /**************************************************************************INACTIVO - YA NO DEBERIA SER NECESARIO 10-01-24 **********************************************/
        static void Main(string[] args)
        {
            FacturaDeudorBLL facturaDeudorBLL = new FacturaDeudorBLL();
            NotaCreditoBLL notaCreditoBLL = new NotaCreditoBLL();

            string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval", "SBOMielmex", "SBOEvi", "SBONaturasol", "SBONoval" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < 4; i++)
            {
                List<FacturaDeudorDetalle> listaFacturasDeudorDetalle = new List<FacturaDeudorDetalle>();
                listaFacturasDeudorDetalle = facturaDeudorBLL.obtenerFacturasCorreccionesV4(basesDatos[i]);

                if (listaFacturasDeudorDetalle.Count > 0)
                {
                    //DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaFacturasDeudorDetalle.Count; j++)
                    {
                        facturaDeudorBLL.actualizarFacturaDeudorCorrecciones(basesDatos[i], listaFacturasDeudorDetalle[j].DocEntry, listaFacturasDeudorDetalle[j].CardName);
                    }
                    //DIAPIBLL.desconectarDIAPI();
                }

                List<NotaCreditoDetalle> listaNotasCreditoDetalle = new List<NotaCreditoDetalle>();
                listaNotasCreditoDetalle = notaCreditoBLL.obtenerNotasCreditoCorreccionesV4(basesDatos[i]);

                if (listaNotasCreditoDetalle.Count > 0)
                {
                    //DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaNotasCreditoDetalle.Count; j++)
                    {
                        notaCreditoBLL.actualizarNotaCreditoDeudorCorrecciones(basesDatos[i], listaNotasCreditoDetalle[j].DocEntry, listaNotasCreditoDetalle[j].CardName);
                    }
                    //DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}

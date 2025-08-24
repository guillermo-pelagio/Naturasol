using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioOC
{
    class Program
    { 
        /**************************************************************************INACTIVO - YA NO DEBERIA SER NECESARIO 10-01-24 **********************************************/
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval" };

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenCompra> listaOCPendientes = new List<OrdenCompra>();
                listaOCPendientes = ordenesCompraBLL.obtenerDocumentosAutorizarPendiente(basesDatos[i]);

                if (listaOCPendientes.Count > 0)
                {
                    //DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaOCPendientes.Count; j++)
                    {
                        ordenesCompraBLL.actualizarOCAutorizada(basesDatos[i], listaOCPendientes[j].DocEntry);
                        //ordenesCompraBLL.actualizarOCPendientes(listaOCPendientes[j].DocEntry);
                    }
                    //DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}

using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioActualizarPreciosBOM
{
    class Program
    {
        /********************************************************************************************INACTIVO*********************************************************************/
        static void Main(string[] args)
        {
            ArticulosBLL articulosBLL = new ArticulosBLL();
            string[] basesDatos = { "TSSL_NATURASOL" };
            //string[] basesDatos = { "SBOMielmex", "SBOEvi", "SBONaturasol", "SBONoval" };

            for (int i = 0; i < basesDatos.Length; i++)
            {
                for (int x = 0; x < 8; x++)
                {
                    List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
                    listaArticulosDesactualizados = articulosBLL.obtenerArticulosPreciosDesactualizados(basesDatos[i]);

                    if (listaArticulosDesactualizados.Count > 0)
                    {
                        DIAPIBLL.conectarDIAPI(basesDatos[i]);
                        for (int j = 0; j < listaArticulosDesactualizados.Count; j++)
                        {
                            articulosBLL.actualizarPrecioListaPrecio(listaArticulosDesactualizados[j]);
                            //ordenesCompraBLL.actualizarOCPendientes(listaOCPendientes[j].DocEntry);
                        }
                        DIAPIBLL.desconectarDIAPI();
                    }
                }
            }
        }
    }
}

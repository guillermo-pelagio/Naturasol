using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioEquivalentesBOM
{
    class Program
    {
        /**************************************************************************DESHABILITADO -----------------------NO EJECUTAR - 10-01-24 **********************************************/
        static void Main(string[] args)
        {
            ArticulosBLL articulosBLL = new ArticulosBLL();
            string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval", "SBOMielmex", "SBOEvi", "SBONaturasol", "SBONoval" };
            //string[] basesDatos = { "SBOMielmex", "SBOEvi", "SBONaturasol", "SBONoval" };

            /*
            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<Articulos> listaArticulosEquivalentes = new List<Articulos>();
                listaArticulosEquivalentes = articulosBLL.obtenerArticulosEquivalenteTSSL_M(basesDatos[i]);

                if (listaArticulosEquivalentes.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaArticulosEquivalentes.Count; j++)
                    {
                        articulosBLL.actualizarCantidadBOM(listaArticulosEquivalentes[j]);                        
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
            */
        }
    }
}
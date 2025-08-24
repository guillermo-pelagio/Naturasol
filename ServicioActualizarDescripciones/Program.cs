using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioActualizarDescripciones
{
    class Program
    {
        static void Main(string[] args)
        {
            ArticulosBLL articulosBLL = new ArticulosBLL();
            string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval" };
            
            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
                listaArticulosDesactualizados = articulosBLL.obtenerArticulosDesactualizados(basesDatos[i]);

                if (listaArticulosDesactualizados.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaArticulosDesactualizados.Count; j++)
                    {
                        articulosBLL.actualizarDescripcionListaPrecio(listaArticulosDesactualizados[j]);
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
                listaArticulosDesactualizados = articulosBLL.obtenerArticulosDesactualizadosPadre(basesDatos[i]);

                if (listaArticulosDesactualizados.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaArticulosDesactualizados.Count; j++)
                    {
                        articulosBLL.actualizarDescripcionListaPrecioPadre(listaArticulosDesactualizados[j]);
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}

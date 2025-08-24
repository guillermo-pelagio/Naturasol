using CapaEntidades;
using CapaNegocios;
using System.Collections.Generic;

namespace ServicioActualizarArticulosAlmacen
{
    class Program
    {
        static void Main(string[] args)
        {
            ArticulosBLL articulosBLL = new ArticulosBLL();
            AlmacenesBLL almacenesBLL = new AlmacenesBLL();
            string[] basesDatos = { "TSSL_Mielmex", "TSSL_DISTRIBUIDORA", "TSSL_Naturasol", "TSSL_Noval" };

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<Articulos> listaArticulos = new List<Articulos>();
                List<Almacenes> listaAlmacenes = new List<Almacenes>();
                listaArticulos = articulosBLL.obtenerArticulos(basesDatos[i]);
                listaAlmacenes = almacenesBLL.obtenerAlmacenes(basesDatos[i]);

                if (listaArticulos.Count > 0)
                {
                    DIAPIBLL.conectarDIAPI(basesDatos[i]);
                    for (int j = 0; j < listaArticulos.Count; j++)
                    {
                        for (int k = 0; k < listaAlmacenes.Count; k++)
                        {
                            int articulos = articulosBLL.obtenerArticulosAlmacen(basesDatos[i], listaArticulos[j].ItemCode, listaAlmacenes[k].WhsCode).Count;
                            //if (listaArticulos[j].ItemCode == "2028-310-279")
                            {
                                if (articulos == 0)
                                {
                                    articulosBLL.actualizarAlmacenesArticulo(listaArticulos[j].ItemCode, listaAlmacenes[k].WhsCode);
                                }
                            }
                        }
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}

using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioCerrarBorrador
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
            CorreosBLL correoBLL = new CorreosBLL();

            for (int i = 0; i < basesDatos.Length; i++)
            {

                List<OrdenCompra> listaBorradoresAbiertas = new List<OrdenCompra>();
                listaBorradoresAbiertas = ordenesCompraBLL.obtenerBorradoresAbiertas(basesDatos[i]);

                if (listaBorradoresAbiertas.Count > 0)
                {
                    for (int j = 0; j < listaBorradoresAbiertas.Count; j++)
                    {
                        DIAPIBLL.conectarDIAPI(basesDatos[i]);
                        if (DateTime.Now.Hour >= 0)
                        {
                            ordenesCompraBLL.cerrarBorradorAbiertas(listaBorradoresAbiertas[j].DocEntry);
                        }
                        DIAPIBLL.desconectarDIAPI();
                    }
                }
            }
        }
    }
}
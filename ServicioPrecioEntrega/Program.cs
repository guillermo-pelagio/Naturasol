using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioPrecioEntrega
{
    class Program
    {
        static void Main(string[] args)
        {
            EntradasBLL entradasBLL = new EntradasBLL();
            CorreosBLL correoBLL = new CorreosBLL();
            bool enviaCorreo = false;

            List<EntradaMaterial> listaEntradas = new List<EntradaMaterial>();
            string body = "";
            decimal montoTotal = 0;
            
            listaEntradas = entradasBLL.obtenerEntradaInternacionalPE();
            if (listaEntradas.Count > 0)
            {
                for (int m = 0; m < listaEntradas.Count; m++)
                {
                    DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                    
                    entradasBLL.crearPrecioEntrega(listaEntradas[m].DocNum);                    
                    entradasBLL.actualizarEntrada(listaEntradas[m].DocEntry, "2");
                    
                    //entradasBLL.crearPrecioEntrega("100013459");

                    DIAPIBLL.desconectarDIAPI();
                }
            }            
        }
    }
}

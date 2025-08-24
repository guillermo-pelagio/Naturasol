using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioOFMasiva
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenFabricacionBLL ordenFabricacionBLL = new OrdenFabricacionBLL();

            DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
            ordenFabricacionBLL.crearOFMasivo();
            DIAPIBLL.desconectarDIAPI();
        }
    }
}

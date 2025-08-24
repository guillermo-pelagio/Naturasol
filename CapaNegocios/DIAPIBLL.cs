using CapaDatos;
using System;

namespace CapaNegocios
{
    public class DIAPIBLL
    {
        //METODO PARA CONECTARSE A SAP.
        public static bool conectarDIAPI(String Base)
        {
            Console.WriteLine("Conectandose a SAP...");
            return DIAPIDAL.conectarDIAPI(Base);
        }

        //METODO PARA DESCONECTARSE DE SAP.
        public static bool desconectarDIAPI()
        {
            return DIAPIDAL.desconectarDIAPI();
        }

        public static bool conectarDIAPIContabilidad(String Base)
        {
            Console.WriteLine("Conectandose a SAP...");
            return DIAPIDAL.conectarDIAPIContabilidad(Base);
        }

        //METODO PARA DESCONECTARSE DE SAP.
        public static bool conectarDIAPIContabilidad()
        {
            return DIAPIDAL.desconectarDIAPIContabilidad();
        }

    }
}

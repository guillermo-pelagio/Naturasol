using System;

namespace CapaDatos
{
    public class DIAPIDAL
    {
        //VARIABLES.
        public static SAPbobsCOM.Company company;
        public static SAPbobsCOM.Company companyPrueba;
        public static int retCode = 0;
        public static int errorCode = 0;
        public static string errorMessage = "";
        private static bool resultado = false;

        //CONEXION  DIAPI.
        public static bool conectarDIAPI(String Base)
        {
            company = Companias.conexionBD(Base);
            retCode = company.Connect();

            //SI NO LLEGA A CONECTARSE.
            if (retCode != 0)
            {
                company.GetLastError(out errorCode, out errorMessage);
                resultado = false;
                Console.WriteLine("Error al conectarse a SAP" + errorMessage);
            }

            //SI SE CONECTA CORRECTAMENTE.
            else
            {
                resultado = true;
                Console.WriteLine("Sin error al conectarse a SAP");
            }

            return resultado;
        }

        //METODO PARA DESCONECTARSE DE SAP.
        public static bool desconectarDIAPI()
        {
            if (company.Connected)
            {
                company.Disconnect();
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }

        public static bool conectarDIAPIContabilidad(String Base)
        {
            company = Companias.conexionBDContabilidad(Base);
            retCode = company.Connect();

            //SI NO LLEGA A CONECTARSE.
            if (retCode != 0)
            {
                company.GetLastError(out errorCode, out errorMessage);
                resultado = false;
                Console.WriteLine("Error al conectarse a SAP" + errorMessage);
            }

            //SI SE CONECTA CORRECTAMENTE.
            else
            {
                resultado = true;
                Console.WriteLine("Sin error al conectarse a SAP");
            }

            return resultado;
        }

        //METODO PARA DESCONECTARSE DE SAP.
        public static bool desconectarDIAPIContabilidad()
        {
            if (company.Connected)
            {
                company.Disconnect();
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }
    }
}

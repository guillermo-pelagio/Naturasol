using System.Configuration;

namespace CapaDatos
{
    //CLASE DE LA CONEXION A BASE DE DATOS, LOS DATOS ESTAN EN WEB.CONFIG
    public class ConexionDAL
    {
        public static string conexionWeb = ConfigurationManager.ConnectionStrings["conexionWeb"].ToString();
        public static string mielmex = ConfigurationManager.ConnectionStrings["mielmex"].ToString();
        public static string master = ConfigurationManager.ConnectionStrings["master"].ToString();
    }
}

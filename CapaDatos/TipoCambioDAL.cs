using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class TipoCambioDAL
    {
        //METODO DE BUSQUEDA DEL ULTIMO TIPO DE CAMBIO
        public string ultimoTipoCambio(String moneda)
        {
            string tipoCambio = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT TOP 1 [Rate] " +
                                        "FROM TestMielmex.[dbo].[ORTT] " +
                                        "WHERE Currency = @Currency " +
                                        "ORDER BY RateDate DESC";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@Currency", moneda);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            tipoCambio = datareader["Rate"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                tipoCambio = "N/A";
            }

            return tipoCambio;
        }
    }
}

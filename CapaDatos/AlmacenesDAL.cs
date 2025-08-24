using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class AlmacenesDAL
    {
        //S1-REQUERIDO-METODO PARA OBTENER LOS ALMACENES
        public List<Almacenes> listaAlmacenes(string sociedad)
        {
            List<Almacenes> listaAlmacenes = new List<Almacenes>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT WhsCode, WhsName FROM " + sociedad + ".dbo.OWHS T1 WHERE Inactive='N' " +
                        "ORDER BY T1.WhsCode DESC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaAlmacenes.Add(new Almacenes()
                            {
                                WhsName = datareader["WhsName"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaAlmacenes = new List<Almacenes>();
            }

            return listaAlmacenes;
        }
    }
}
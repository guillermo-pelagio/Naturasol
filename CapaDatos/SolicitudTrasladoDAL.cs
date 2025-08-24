using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class SolicitudTrasladoDAL
    {
        //S16-REQUERIDO
        public List<SolicitudTraslado> obtenerSTAbiertas(string sociedad)
        {
            List<SolicitudTraslado> listaDocumentos = new List<SolicitudTraslado>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum, DocDueDate, DocDate, DocTime, DocEntry, Filler, ToWhsCode FROM " + sociedad + ".dbo.OWTQ WHERE DocStatus='O' AND DocDate<DATEADD(DAY,-7,GETDATE()) ORDER BY DocEntry ASC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTraslado()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocDueDate = datareader["DocDueDate"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocTime = datareader["DocTime"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Filler = datareader["Filler"].ToString(),
                                ToWhsCode = datareader["ToWhsCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTraslado>();
            }

            return listaDocumentos;
        }
    }
}


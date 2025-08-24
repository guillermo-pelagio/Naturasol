using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class SolicitudCompraDAL
    {
        //S15-REQUERIDO
        public List<SolicitudCompra> obtenerSDCAbiertas(string sociedad, string email)
        {
            List<SolicitudCompra> listaDocumentos = new List<SolicitudCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    
                    
                    if (email == "")
                    {
                        consulta = "SELECT DocNum, DocDueDate, DocDate, DocTime, DocEntry, Email FROM " + sociedad + ".dbo.OPRQ WHERE DocStatus='O' and DocType='S' AND DATEDIFF(DAY, DocDate, getdate())>90 and EMAIL is null " +
                                "UNION ALL " +
                                "SELECT DocNum, DocDueDate, DocDate, DocTime, DocEntry, Email FROM " + sociedad + ".dbo.OPRQ WHERE DocStatus = 'O' and DocType = 'I' AND DATEDIFF(DAY, DocDate, getdate())> 90 and EMAIL is null ";
                    }
                    else
                    {
                        consulta = "SELECT DocNum, DocDueDate, DocDate, DocTime, DocEntry, Email FROM " + sociedad + ".dbo.OPRQ WHERE DocStatus='O' and DocType='S' AND DATEDIFF(DAY, DocDate, getdate())>90 and EMAIL = @email " +
                                "UNION ALL " +
                                "SELECT DocNum, DocDueDate, DocDate, DocTime, DocEntry, Email FROM " + sociedad + ".dbo.OPRQ WHERE DocStatus = 'O' and DocType = 'I' AND DATEDIFF(DAY, DocDate, getdate())> 90 and EMAIL = @email ";                        
                    }
                    

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudCompra()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocDueDate = datareader["DocDueDate"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocTime = datareader["DocTime"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                email = datareader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudCompra>();
            }

            return listaDocumentos;
        }

        //S15-REQUERIDO
        public List<SolicitudCompra> obtenerSolicitanteSDCAbiertas(string sociedad)
        {
            List<SolicitudCompra> listaDocumentos = new List<SolicitudCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct EMAIL " +
                                        "FROM " + sociedad + ".dbo.OPRQ " +
                                        "WHERE DocStatus='O' ORDER BY email ASC  ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudCompra()
                            {
                                email = datareader["Email"].ToString(),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudCompra>();
            }

            return listaDocumentos;
        }
        /*
        //S35-REQUERIDO
        public List<SolicitudCompra> obtenerSolicitanteSDCAbiertas(string sociedad)
        {
            List<SolicitudCompra> listaDocumentos = new List<SolicitudCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct EMAIL " +
                                        "FROM " + sociedad + ".dbo.OPRQ " +
                                        "WHERE DocStatus='O' ORDER BY email ASC  ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudCompra()
                            {
                                email = datareader["Email"].ToString(),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudCompra>();
            }

            return listaDocumentos;
        }
        */
    }
}

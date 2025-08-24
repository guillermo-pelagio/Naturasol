using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ReportesDAL
    {
        public List<Reportes> obtenerReportes(string sociedad)
        {
            List<Reportes> listaReportes = new List<Reportes>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * FROM DESARROLLOWEB.DBO.REPORTES";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaReportes.Add(new Reportes()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                idReporte = datareader["idReporte"].ToString(),
                                nombreReporte = datareader["nombreReporte"].ToString(),
                                areaReporte = datareader["areaReporte"].ToString(),
                                queryReporte = datareader["queryReporte"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaReportes = new List<Reportes>();
            }

            return listaReportes;
        }

        public List<Formatos> obtenerFormatos()
        {
            List<Formatos> listaFormatos = new List<Formatos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.FORMATOS";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaFormatos.Add(new Formatos()
                            {
                                idFormato = datareader["idFormato"].ToString(),
                                nombreFormato = datareader["nombreFormato"].ToString(),
                                linkFormato = datareader["linkFormato"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaFormatos = new List<Formatos>();
            }

            return listaFormatos;
        }

        public List<Reportes> obtenerQuery(string idReporte)
        {
            List<Reportes> listaReportes = new List<Reportes>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * FROM DESARROLLOWEB.DBO.REPORTES WHERE idReporte=@idReporte";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idReporte", idReporte);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaReportes.Add(new Reportes()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                idReporte = datareader["idReporte"].ToString(),
                                nombreReporte = datareader["nombreReporte"].ToString(),
                                areaReporte = datareader["areaReporte"].ToString(),
                                queryReporte = datareader["queryReporte"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaReportes = new List<Reportes>();
            }

            return listaReportes;
        }

        public DataTable obtenerResultadosQuery(string query)
        {
            DataSet dataset = new DataSet();
            SqlConnection SqlConnection = new SqlConnection(ConexionDAL.conexionWeb);

            try
            {
                string consulta = query;

                SqlConnection.Open();
                SqlCommand SqlCommand = new SqlCommand(consulta, SqlConnection);
                
                SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                SqlDataAdapter.SelectCommand = SqlCommand;
                SqlCommand.CommandTimeout = 0;

                dataset.Clear();
                SqlDataAdapter.Fill(dataset);
            }
            catch (Exception ex)
            {
                
            }

            return dataset.Tables[0];
        }
    }
}

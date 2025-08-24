using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ArticulosDAL
    {
        //S2-REQUERIDO-METODO PARA OBTENER LOS ARTICULOS CON ITEMS EN LA LISTA DE PRECIOS CON DESCRIPCIONES DIFERENTES
        public List<Articulos> obtenerArticulosDesactualizados(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.Father, T1.Code, T2.ItemName FROM " + sociedad + ".dbo.ITT1 T1 " +
                        "JOIN " + sociedad + ".dbo.OITM T2 ON T2.ItemCode = T1.code " +
                        "WHERE T1.ItemName<> T2.ItemName ORDER BY T1.Father DESC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                Father = datareader["Father"].ToString(),
                                ItemCode = datareader["Code"].ToString(),
                                ItemName = datareader["ItemName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }
        //S2-REQUERIDO-
        public List<Articulos> obtenerArticulosDesactualizadosPadre(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.Code, T2.ItemName FROM " + sociedad + ".dbo.OITT T1 " +
                        "JOIN " + sociedad + ".dbo.OITM T2 ON T2.ItemCode = T1.Code " +
                        "WHERE T1.Name<> T2.ItemName ORDER BY T1.Code DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                Father = datareader["Code"].ToString(),                                
                                ItemName = datareader["ItemName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }

        //METODO PARA OBTENER LOS ARTICULOS CON ITEMS EN LA LISTA DE PRECIOS CON DESCRIPCIONES DIFERENTES
        public List<Articulos> obtenerArticulosPreciosDesactualizados(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {

                    string consulta = "SELECT T5.Qauntity, T1.Father, T1.Code, T2.Price,CASE WHEN T2.Currency = 'USD' THEN 'USD' ELSE 'MXP' END AS 'Currency' FROM " + sociedad + ".dbo.ITT1 T1 " +
                        "JOIN " + sociedad + ".dbo.OITT T5 ON T5.Code = T1.Father " +
                        "JOIN " + sociedad + ".dbo.ITM1 T2 ON T2.ItemCode = T1.code " +
                        "JOIN " + sociedad + ".dbo.OPLN T3 ON T3.ListNum = T2.PriceList " +
                        "JOIN " + sociedad + ".dbo.OWHS T4 ON T4.WhsCode = T1.Warehouse " +
                        "WHERE (T1.Price <> T2.Price OR T1.Currency <> T2.Currency) AND T3.ListName = 'Lista de Materiales' AND T4.Inactive='N' ORDER BY T1.Father DESC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                Qauntity = datareader["Qauntity"].ToString(),
                                Father = datareader["Father"].ToString(),
                                ItemCode = datareader["Code"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }

        //METODO PARA OBTENER LOS ARTICULOS CON ITEMS EN LA LISTA DE PRECIOS CON DESCRIPCIONES DIFERENTES
        public List<Articulos> obtenerArticulosEquivalentesBOM(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * " +
                        "FROM " + sociedad + ".dbo.ITT1 T1 " +
                        "JOIN " + sociedad + ".dbo.OITT T0 ON T0.Code = T1.Father " +
                        "WHERE T0.UpdateDate >= '2023-06-09' " +
                        "AND T1.Quantity = 0.0001 AND T1.OcrCode IS NULL ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                Father = datareader["Father"].ToString(),
                                ItemCode = datareader["Code"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }

        //S1-REQUERIDO-METODO PARA OBTENER TODOS LOS ARTICULOS
        public List<Articulos> obtenerArticulos(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT ItemCode FROM " + sociedad + ".dbo.OITM T1 WHERE T1.CreateDate<=CAST( GETDATE()-1 AS Date ) " +
                        "ORDER BY T1.ItemCode DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                ItemCode = datareader["ItemCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }

        //S1-REQUERIDO ACTUALIZACION
        public List<Articulos> obtenerArticulosAlmacen(string sociedad, string ItemCode, string WhsCode)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT ItemCode FROM " + sociedad + ".dbo.OITW T1 WHERE ItemCode=@ItemCode AND WhsCode = @WhsCode " +
                        " ORDER BY T1.ItemCode DESC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@WhsCode", WhsCode);
                    comando.Parameters.AddWithValue("@ItemCode", ItemCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                ItemCode = datareader["ItemCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<Articulos>();
            }

            return listaArticulos;
        }
    }
}

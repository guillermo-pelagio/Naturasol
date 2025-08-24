using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class LogisticaDAL
    {
        public bool verificarRegistro(object v1, object v2, object v3, object v4, object v5, object v6, object v7, object v8, object v9, object v10, object v11, object v12)
        {
            List<string> listaVentas = new List<string>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * from [DesarrolloWeb].[dbo].[CALENDARIOENTREGAS] WHERE [CLIENTE]=@cliente AND [OC]=@oc AND [CODIGO SAP]=@codigo ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@cliente", v2);
                    comando.Parameters.AddWithValue("@oc", v1);
                    comando.Parameters.AddWithValue("@codigo", v5);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaVentas.Add(datareader["OC"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaVentas = null;
            }

            return listaVentas == null ? false : listaVentas.Count > 0 ? true : false;
        }

        public int insertarRegistro(object oc, object cliente, object planta, object upc, object codigo, object descripcion, object cantidad, object um, object fecha, object confirmacion, object observaciones, object color)
        {
            int idCotizacion = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO desarrolloweb.[dbo].[calendarioEntregas] " +
                                           "([OC] " +
                                           ",[CLIENTE] " +
                                           ",[PLANTA] " +
                                           ",[UPC] " +
                                           ",[CODIGO SAP] " +
                                           ",[DESCRIPCION SAP] " +
                                           ",[CANTIDAD SOLICITADA] " +
                                           ",[UM] " +
                                           ",[Fecha y Hora Cita Destino] " +
                                           ",[CONFIRMACION] " +
                                           ",[OBSERVACIONES], [COLOR]) " +
                                     "VALUES " +
                                           "(@oc, " +
                                           "@cliente, " +
                                           "@planta, " +
                                           "@upc, " +
                                           "@codigo, " +
                                           "@descripcion, " +
                                           "@cantidad, " +
                                           "@um, " +
                                           "@fecha, " +
                                           "@confirmacion, " +
                                           "@observaciones, @color); ";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@oc", oc == null ? "" : oc);
                    comando.Parameters.AddWithValue("@cliente", cliente == null ? "" : cliente);
                    comando.Parameters.AddWithValue("@planta", planta == null ? "" : planta);
                    comando.Parameters.AddWithValue("@upc", upc == null ? "" : upc);
                    comando.Parameters.AddWithValue("@codigo", codigo == null ? "" : codigo);
                    comando.Parameters.AddWithValue("@descripcion", descripcion == null ? "" : descripcion);
                    comando.Parameters.AddWithValue("@cantidad", cantidad == null ? "" : cantidad);
                    comando.Parameters.AddWithValue("@um", um == null ? "" : um);
                    comando.Parameters.AddWithValue("@fecha", fecha == null ? "" : fecha);
                    comando.Parameters.AddWithValue("@confirmacion", confirmacion == null ? "" : confirmacion);
                    comando.Parameters.AddWithValue("@observaciones", observaciones == null ? "" : observaciones);
                    comando.Parameters.AddWithValue("@color", color == null ? "" : color);
                    comando.CommandType = CommandType.Text;

                    idCotizacion = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idCotizacion = -1;
            }
            return idCotizacion;
        }

        public int actualizarRegistro(object oc, object cliente, object planta, object upc, object codigo, object descripcion, object cantidad, object um, object fecha, object confirmacion, object observaciones, object color)
        {
            int idCotizacion = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE desarrolloweb.[dbo].[calendarioEntregas] " +
                                           "SET [PLANTA] = @planta " +
                                           ",[CANTIDAD SOLICITADA] =@cantidad" +
                                           ",[UM] =@um" +
                                           ",[Fecha y Hora Cita Destino] =@fecha" +
                                           ",[CONFIRMACION] =@confirmacion" +
                                           ",[OBSERVACIONES]=@observaciones" +
                                           " ,[COLOR]=@color WHERE [OC]=@oc AND [CLIENTE]=@cliente AND [CODIGO SAP]=@codigo";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@oc", oc);
                    comando.Parameters.AddWithValue("@cliente", cliente);
                    comando.Parameters.AddWithValue("@codigo", codigo);

                    comando.Parameters.AddWithValue("@planta", planta == null ? "" : planta);
                    comando.Parameters.AddWithValue("@cantidad", cantidad == null ? "" : cantidad);
                    comando.Parameters.AddWithValue("@um", um == null ? "" : um);
                    comando.Parameters.AddWithValue("@fecha", fecha == null ? "" : fecha);
                    comando.Parameters.AddWithValue("@confirmacion", confirmacion == null ? "" : confirmacion);
                    comando.Parameters.AddWithValue("@observaciones", observaciones == null ? "" : observaciones);
                    comando.Parameters.AddWithValue("@color", color == null ? "" : color);

                    comando.Parameters.AddWithValue("@upc", upc);
                    comando.Parameters.AddWithValue("@descripcion", descripcion);
                    comando.CommandType = CommandType.Text;

                    idCotizacion = Convert.ToInt32(comando.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                idCotizacion = -1;
            }

            return idCotizacion;
        }

        public List<CalendarioEntrega> obtenerCalendario(string oc, string fecha1, string fecha2, string cliente, string codigo, string descripcion)
        {
            List<CalendarioEntrega> listaCalendario = new List<CalendarioEntrega>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (oc == "" && fecha1 == "" && fecha2 == "" && codigo == "" && cliente == "" && descripcion == "")
                    {
                        consulta = "select * from [DesarrolloWeb].[dbo].[CALENDARIOENTREGAS] ";
                    }
                    else
                    {
                        consulta = "select * FROM [DesarrolloWeb].[dbo].[CALENDARIOENTREGAS] where [OC] LIKE '%" + oc + "%' and [Descripcion sap] LIKE '%" + descripcion + "%' and [cliente] LIKE '%" + cliente + "%'  and [codigo sap] LIKE '%" + codigo + "%' ";

                        if (fecha1 != "")
                        {
                            consulta = consulta + " and [Fecha y Hora Cita Destino] >= '" + fecha1 + "' ";
                        }
                        if (fecha2 != "")
                        {
                            consulta = consulta + " and [Fecha y Hora Cita Destino] <= '" + fecha2 + "' ";
                        }
                    }                    

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCalendario.Add(new CalendarioEntrega()
                            {
                                cliente = datareader["CLIENTE"].ToString(),
                                planta = datareader["PLANTA"].ToString(),
                                upc = datareader["UPC"].ToString(),
                                codigoSap = datareader["CODIGO SAP"].ToString(),
                                observaciones = datareader["OBSERVACIONES"].ToString(),
                                cantidad = datareader["CANTIDAD SOLICITADA"].ToString(),
                                descripcionSap = datareader["DESCRIPCION SAP"].ToString(),
                                um = datareader["UM"].ToString(),
                                fecha = datareader["Fecha y Hora Cita Destino"].ToString(),
                                confirmacion = datareader["CONFIRMACION"].ToString(),
                                oc = datareader["OC"].ToString(),
                                color = datareader["COLOR"].ToString() == null ? "" : datareader["COLOR"].ToString()
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                listaCalendario = null;
            }

            return listaCalendario;
        }
    }
}

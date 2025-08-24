using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class VentasDAL
    {
        public List<Cotizacion> obtenerCotizacionesWeb()
        {
            List<Cotizacion> listaCotizaciones = new List<Cotizacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = "select * FROM DESARROLLOWEB.DBO.cotizaciones ORDER BY case estatus when 0 then 6 else estatus end desc";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            string[] k = { "-", "HECTOR IBARROLA", "GRECIA LEY", "KARINA MONDRAGON", "SAMUEL PEÑA", "DAVID ALBA", "ROBERTO CERVANTES", "JAVIER CARRASCO", "ELIU DOMINGUEZ", "MONTSERRAT BUENDIA", "LILIA BECERRIL","ALBERTO BECERRIL", "IVAN CHAGOYA", "CASIMIRO FLORES", "LEONARDO YAÑEZ", "ANDREA VILLANUEVA"};
                            string[] c = { "SIN MOVIMIENTO", "APROBADO", "RECHAZADO", "EN REVISIÓN", "POR PRESENTAR", "SUSTITUIDO", "EN ESPERA DE MUESTRAS" };
                            string[] m = { "-", "NO DAMOS EL PERFIL", "FALTA DE ESPACIO DE ANAQUEL", "PRECIO", "OTRO" };

                            listaCotizaciones.Add(new Cotizacion()
                            {
                                KAM = datareader["KAM"].ToString(),
                                Cliente = datareader["cliente"].ToString(),
                                estatus = datareader["estatus"].ToString(),
                                idCotizacion = datareader["idCotizacion"].ToString(),
                                fechaCreacion = Convert.ToDateTime(datareader["fechaCreacion"].ToString()).ToString("yyyy-MM-dd"),
                                descripcion = datareader["descripcion"].ToString(),
                                fechaPresentacion = datareader["fechaPresentacion"].ToString() == "" ? Convert.ToDateTime("1900-01-01").ToString("yyyy-MM-dd") : Convert.ToDateTime(datareader["fechaPresentacion"].ToString()).ToString("yyyy-MM-dd"),
                                motivoRechazo = datareader["motivoRechazo"].ToString(),
                                categoria = datareader["categoria"].ToString(),
                                comentariosOtroMotivo = datareader["otrosMotivos"].ToString(),
                                fechaRespuesta = datareader["fechaRespuesta"].ToString() == "" ? Convert.ToDateTime("1900-01-01").ToString("yyyy-MM-dd") : Convert.ToDateTime(datareader["fechaRespuesta"].ToString()).ToString("yyyy-MM-dd"),
                                fechaLanzamiento = datareader["fechaLanzamiento"].ToString() == "" ? Convert.ToDateTime("1900-01-01").ToString("yyyy-MM-dd") : Convert.ToDateTime(datareader["fechaLanzamiento"].ToString()).ToString("yyyy-MM-dd"),
                                comentarios = datareader["comentarios"].ToString(),
                                KAMDescripcion = datareader["KAM"].ToString() == "" ? k[0] : k[Convert.ToInt32(datareader["KAM"].ToString())],
                                estatusDescripcion = datareader["estatus"].ToString() == "" ? c[0] : c[Convert.ToInt32(datareader["estatus"].ToString())],
                                motivoRechazoDescripcion = datareader["motivoRechazo"].ToString() == "" ? m[0] : m[Convert.ToInt32(datareader["motivoRechazo"].ToString())]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaCotizaciones = new List<Cotizacion>();
            }

            return listaCotizaciones;
        }

        public List<string> obtener_numero_ventas()
        {
            List<string> listaVentas = new List<string>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select Venta from  [DesarrolloWeb].[dbo].[RESUMENVENTAS] union all (select * from [DesarrolloWeb].[dbo].[RESUMENACUERDOS]) ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaVentas.Add(
                                string.Format("{0:C}", Convert.ToDecimal(datareader["Venta"].ToString()))
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaVentas = null;
            }

            return listaVentas;
        }

        public List<Cotizacion> obtenerEstatusCotizaciones()
        {
            List<Cotizacion> listaCotizaciones = new List<Cotizacion>();
            string[] c = { "SIN MOVIMIENTO", "APROBADO", "RECHAZADO", "EN REVISIÓN", "POR PRESENTAR", "SUSTITUIDO", "EN ESPERA DE MUESTRAS" };
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = "select COUNT(estatus) as suma, estatus from DesarrolloWeb.dbo.cotizaciones group by estatus";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCotizaciones.Add(new Cotizacion()
                            {
                                sumaReporte = datareader["suma"].ToString(),
                                estatus = datareader["estatus"].ToString() == "" ? c[0] : c[Convert.ToInt32(datareader["estatus"].ToString())]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaCotizaciones = new List<Cotizacion>();
            }

            return listaCotizaciones;
        }

        public List<Cotizacion> obtenerMotivoRechazoCotizaciones()
        {
            List<Cotizacion> listaMotivosRechazo = new List<Cotizacion>();
            string[] c = { "-", "NO DAMOS EL PERFIL", "FALTA DE ESPACIO EN EL ANAQUEL", "PRECIO", "OTRO" };
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = "select COUNT(motivoRechazo) as suma, motivoRechazo from DesarrolloWeb.dbo.cotizaciones where estatus=2 group by motivoRechazo";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaMotivosRechazo.Add(new Cotizacion()
                            {
                                sumaReporte = datareader["suma"].ToString(),
                                estatus = datareader["motivoRechazo"].ToString() == "" ? c[0] : c[Convert.ToInt32(datareader["motivoRechazo"].ToString())]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaMotivosRechazo = new List<Cotizacion>();
            }

            return listaMotivosRechazo;
        }

        public List<Cotizacion> obtenerEstatusCotizacionesKAM()
        {
            List<Cotizacion> listaCotizaciones = new List<Cotizacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = "select COUNT(estatus) as suma, kam, estatus from DesarrolloWeb.dbo.cotizaciones group by estatus, kam";
                    string[] k = { "-", "HECTOR IBARROLA", "GRECIA LEY", "KARINA MONDRAGON", "SAMUEL PEÑA", "DAVID ALBA", "ROBERTO CERVANTES", "JAVIER CARRASCO", "ELIU DOMINGUEZ", "MONTSERRAT BUENDIA", "LILIA BECERRIL","ALBERTO BECERRIL", "IVAN CHAGOYA", "CASIMIRO FLORES", "LEONARDO YAÑEZ", "ANDREA VILLANUEVA" };


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCotizaciones.Add(new Cotizacion()
                            {
                                sumaReporte = datareader["suma"].ToString(),
                                KAMDescripcion = datareader["KAM"].ToString() == "" ? k[0] : k[Convert.ToInt32(datareader["KAM"].ToString())],
                                estatus = datareader["estatus"].ToString(),
                                KAM = datareader["KAM"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaCotizaciones = new List<Cotizacion>();
            }

            return listaCotizaciones;
        }

        public int guardarCotizacion(Cotizacion cotizacionWEB, int puesto)
        {
            int idCotizacion = 0;
            if (cotizacionWEB.idCotizacion != null)
            {
                idCotizacion = 0;
                try
                {
                    using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                    {
                        string consulta = "";

                        if (puesto == 100)
                        {
                            consulta = "UPDATE [dbo].[cotizaciones] SET " +
                       "cliente = @cliente, fechaCreacion=@fechaCreacion, categoria=@categoria, descripcion=@descripcion, [comentarios] = @comentarios, [otrosMotivos] = @otrosMotivos, [estatus] = @estatus, [motivoRechazo] = @motivoRechazo ";
                        }
                        else
                        {
                            consulta = "UPDATE [dbo].[cotizaciones] SET " +
                       "[comentarios] = @comentarios, [otrosMotivos] = @otrosMotivos, [estatus] = @estatus, [motivoRechazo] = @motivoRechazo ";
                        }

                        if (cotizacionWEB.fechaPresentacion != null)
                        {
                            consulta = consulta + ", [fechaPresentacion] = @fechaPresentacion";
                        }
                        if (cotizacionWEB.fechaPresentacion != null)
                        {
                            consulta = consulta + ", [fechaRespuesta] = @fechaRespuesta";
                        }
                        if (cotizacionWEB.fechaPresentacion != null)
                        {
                            consulta = consulta + ", [fechaLanzamiento] = @fechaLanzamiento";
                        }

                        consulta = consulta + " WHERE idCotizacion = @idCotizacion";

                        conexionDB.Open();
                        SqlCommand comando = new SqlCommand(consulta, conexionDB);

                        comando.Parameters.AddWithValue("@cliente", cotizacionWEB.Cliente);
                        comando.Parameters.AddWithValue("@categoria", cotizacionWEB.categoria);
                        comando.Parameters.AddWithValue("@fechaCreacion", cotizacionWEB.fechaCreacion == null ? Convert.ToDateTime("1900-01-01").ToShortDateString() : cotizacionWEB.fechaCreacion);
                        comando.Parameters.AddWithValue("@descripcion", cotizacionWEB.descripcion);

                        comando.Parameters.AddWithValue("@comentarios", cotizacionWEB.comentarios == null ? "" : cotizacionWEB.comentarios);
                        comando.Parameters.AddWithValue("@otrosMotivos", cotizacionWEB.comentariosOtroMotivo == null ? "" : cotizacionWEB.comentariosOtroMotivo);
                        comando.Parameters.AddWithValue("@estatus", cotizacionWEB.estatus);
                        comando.Parameters.AddWithValue("@motivoRechazo", cotizacionWEB.motivoRechazo == null ? "0" : cotizacionWEB.motivoRechazo);
                        comando.Parameters.AddWithValue("@fechaPresentacion", cotizacionWEB.fechaPresentacion == null ? Convert.ToDateTime("1900-01-01").ToShortDateString() : cotizacionWEB.fechaPresentacion);
                        comando.Parameters.AddWithValue("@fechaRespuesta", cotizacionWEB.fechaRespuesta == null ? Convert.ToDateTime("1900-01-01").ToShortDateString() : cotizacionWEB.fechaRespuesta);
                        comando.Parameters.AddWithValue("@fechaLanzamiento", cotizacionWEB.fechaLanzamiento == null ? Convert.ToDateTime("1900-01-01").ToShortDateString() : cotizacionWEB.fechaLanzamiento);
                        comando.Parameters.AddWithValue("@idCotizacion", cotizacionWEB.idCotizacion);
                        comando.CommandType = CommandType.Text;

                        idCotizacion = Convert.ToInt32(comando.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    idCotizacion = -1;
                }
            }
            else
            {
                idCotizacion = 0;
                try
                {
                    using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                    {
                        string consulta = "INSERT INTO [dbo].[cotizaciones] " +
                       "([KAM] " +
                       ",[cliente] " +
                       ",[fechaCreacion] " +
                       ",[categoria] " +
                       ",[estatus] " +
                       ",[descripcion]) " +
                        "VALUES" +
                       "(@KAM," +
                       "@cliente," +
                       "@fechaCreacion," +
                       "@categoria," +
                       "@estatus," +
                       "@descripcion);";

                        conexionDB.Open();
                        SqlCommand comando = new SqlCommand(consulta, conexionDB);
                        comando.Parameters.AddWithValue("@KAM", cotizacionWEB.KAM);
                        comando.Parameters.AddWithValue("@cliente", cotizacionWEB.Cliente);
                        comando.Parameters.AddWithValue("@categoria", cotizacionWEB.categoria);
                        comando.Parameters.AddWithValue("@estatus", 0);
                        comando.Parameters.AddWithValue("@fechaCreacion", cotizacionWEB.fechaCreacion);
                        comando.Parameters.AddWithValue("@descripcion", cotizacionWEB.descripcion);
                        comando.CommandType = CommandType.Text;

                        idCotizacion = Convert.ToInt32(comando.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    idCotizacion = -1;
                }
            }

            return idCotizacion;
        }
    }
}

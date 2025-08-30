using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class MantenimientoDAL
    {
        public List<Inventario> obtenerInventarioMantenimiento(string sociedad, string articulo, string descripcion, string almacen)
        {
            List<Inventario> listaSemanasStock = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (articulo == "" && almacen == "" && descripcion == "")
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[M1 - Inventario]";
                    }
                    else
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[M1 - Inventario] where [Articulo] LIKE '%" + articulo + "%' and [Descripción Articulo] LIKE '%" + descripcion + "%' and [Almacén] LIKE '%" + almacen + "%'  ORDER BY [Articulo], [Almacén] ASC ";
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaSemanasStock.Add(new Inventario()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["Articulo"].ToString(),
                                Descripción = datareader["Descripción Articulo"].ToString(),
                                Almacén = datareader["Almacén"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaSemanasStock = new List<Inventario>();
            }

            return listaSemanasStock;
        }

        public int guardarLinea(LineaMtto lineasMtto)
        {
            int idLinea = 0;
            /*if (lineasMtto.idCotizacion != null)
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

                        if (lineasMtto.fechaPresentacion != null)
                        {
                            consulta = consulta + ", [fechaPresentacion] = @fechaPresentacion";
                        }
                        if (lineasMtto.fechaPresentacion != null)
                        {
                            consulta = consulta + ", [fechaRespuesta] = @fechaRespuesta";
                        }
                        if (lineasMtto.fechaPresentacion != null)
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
            {*/
            idLinea = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO desarrolloweb.[dbo].[lineasMtto] " +
                   "([idUsuario] " +
                   "" +
                   ",[idNegocioLinea] " +
                   ",[nombreLinea] " +
                   ",[estatusLinea] " +
                   ",[descripciónLinea]) " +
                    "VALUES" +
                   "(@idUsuario," +
                   "@idNegocioLinea," +
                   "@nombreLinea," +
                   "@estatusLinea," +
                   "@descripcionLinea);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idUsuario", lineasMtto.idUsuario);
                    comando.Parameters.AddWithValue("@idNegocioLinea", lineasMtto.idNegocioLinea);
                    comando.Parameters.AddWithValue("@nombreLinea", lineasMtto.nombreLinea);
                    comando.Parameters.AddWithValue("@estatusLinea", 1);
                    comando.Parameters.AddWithValue("@descripcionLinea", lineasMtto.descripcionLinea);
                    comando.CommandType = CommandType.Text;

                    idLinea = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idLinea = -1;
            }
            /*}*/

            return idLinea;
        }

        public List<LineaMtto> obtenerLineas()
        {
            string consulta = "";
            List<LineaMtto> lineaMttos = new List<LineaMtto>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    consulta = "select [idUsuario],[idLinea],[idNegocioLinea],[descripciónLinea],[estatusLinea],[nombreLinea] FROM DESARROLLOWEB.DBO.[lineasMtto]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            lineaMttos.Add(new LineaMtto()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString()),
                                idLinea = Convert.ToInt32(datareader["idLinea"].ToString()),
                                idNegocioLinea = Convert.ToInt32(datareader["idNegocioLinea"].ToString()),
                                descripcionLinea = datareader["descripciónLinea"].ToString(),
                                estatusLinea = Convert.ToInt32(datareader["estatusLinea"].ToString()),
                                nombreLinea = datareader["nombreLinea"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lineaMttos = new List<LineaMtto>();
            }

            return lineaMttos;
        }

        public int guardarMaquina(MaquinasMtto maquinasMtto)
        {
            int idMaquina = 0;
            /*if (cotizacionWEB.idCotizacion != null)
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
            {*/
            idMaquina = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[equiposMtto] " +
                   "(" +
                   "[frecuenciaMttoMaquina] " +
                   ",[descripcionMaquina] " +
                   ",[idTiempoMttoMaquina] " +
                   ",[estatusMaquina] " +
                   ") " +
                    "VALUES" +
                   "(@KAM," +
                   "@cliente," +
                   "@fechaCreacion," +
                   "@categoria," +
                   "@estatus," +
                   "@descripcion);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@frecuenciaMttoMaquina", maquinasMtto.frecuenciaMttoMaquina);
                    comando.Parameters.AddWithValue("@descripcionMaquina", maquinasMtto.descripcionMaquina);
                    comando.Parameters.AddWithValue("@estatusMaquina", 0);
                    comando.Parameters.AddWithValue("@idTiempoMttoMaquina", maquinasMtto.idTiempoMttoMaquina);
                    comando.CommandType = CommandType.Text;

                    idMaquina = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idMaquina = -1;
            }
            //}

            return idMaquina;
        }

        public int guardarPersonal(PersonalMtto personalMtto)
        {
            int idPersonal = 0;
            /*if (cotizacionWEB.idCotizacion != null)
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
            {*/
            idPersonal = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[personalMtto] " +
                   "([nombrePersonal] " +
                   ",[estatusPersonal] " +
                   ") " +
                    "VALUES" +
                   "(@nombrePersonal," +
                   "@estatusPersonal);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nombrePersonal", personalMtto.nombrePersonal);
                    comando.Parameters.AddWithValue("@estatusPersonal", 1);
                    comando.CommandType = CommandType.Text;

                    idPersonal = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idPersonal = -1;
            }
            //}

            return idPersonal;
        }
    }
}

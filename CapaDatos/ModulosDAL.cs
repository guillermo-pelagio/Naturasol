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
    public class ModulosDAL
    {
        //METODO PARA OBTENER TODOS LOS MODULOS
        public List<Modulos> obtenerModulos()
        {
            List<Modulos> listaModulos = new List<Modulos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idModulo, " +
                        "nombreModulo, " +
                        "estatusModulo, " +
                        "descripcion " +
                        "FROM DesarrolloWeb.dbo.modulos";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaModulos.Add(new Modulos()
                            {
                                nombreModulo = datareader["nombreModulo"].ToString(),
                                estatusModulo = Convert.ToInt32(datareader["estatusModulo"].ToString()),
                                descripcion = datareader["descripcion"].ToString(),
                                idModulo = Convert.ToInt32(datareader["idModulo"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaModulos = new List<Modulos>();
            }

            return listaModulos;
        }

        //METODO PARA GUARDAR UN MODULO
        public int guardarModulo(Modulos modulo)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[modulos]" +
                   "([nombreModulo]," +
                   "[descripcion]," +
                   "[estatusModulo]," +
                   "[usuarioCreacion]," +
                   "[fechaCreacion]," +
                   "[fechaActualizacion])" +
                    "VALUES" +
                   "(@nombreModulo," +
                   "@descripcion," +
                   "@estatusModulo," +
                   "@usuarioCreacion," +
                   "@fechaCreacion," +
                   "@fechaActualizacion);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nombreModulo", modulo.nombreModulo);
                    comando.Parameters.AddWithValue("@descripcion", modulo.descripcion);
                    comando.Parameters.AddWithValue("@estatusModulo", modulo.estatusModulo);
                    comando.Parameters.AddWithValue("@usuarioCreacion", modulo.usuarioCreacion);
                    comando.Parameters.AddWithValue("@fechaCreacion", modulo.fechaCreacion);
                    comando.Parameters.AddWithValue("@fechaActualizacion", modulo.fechaActualizacion);
                    comando.CommandType = CommandType.Text;

                    idModulo = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idModulo = 0;
            }

            return idModulo;
        }

        //METODO PARA ACTUALIZAR UN MODULO
        public int actualizarModulo(Modulos modulo)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [dbo].[modulos]" +
                    "SET[nombreModulo] = @nombreModulo " +
                    ",[descripcion] = @descripcion " +
                    ",[estatusModulo] = @estatusModulo " +
                    ",[fechaActualizacion] = @fechaActualizacion " +
                    "WHERE idModulo = @idModulo";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nombreModulo", modulo.nombreModulo);
                    comando.Parameters.AddWithValue("@descripcion", modulo.descripcion);
                    comando.Parameters.AddWithValue("@estatusModulo", modulo.estatusModulo);
                    comando.Parameters.AddWithValue("@fechaActualizacion", modulo.fechaActualizacion);
                    comando.Parameters.AddWithValue("@idModulo", modulo.idModulo);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idModulo = modulo.idModulo;
                }
            }
            catch (Exception ex)
            {
                idModulo = 0;
            }

            return idModulo;
        }

        public List<Permisos> obtenerPermisos()
        {
            List<Permisos> listaPermisos = new List<Permisos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T2.nombreModulo, T1.nameUsuario, permiso, T0.idModulo, T0.idUsuario " +
                                      "FROM DesarrolloWeb.dbo.permisos T0 " +
                                      "JOIN DesarrolloWeb.dbo.usuarios T1 ON T0.idUsuario = T1.idUsuario " +
                                      "JOIN DesarrolloWeb.DBO.modulos T2 ON T2.idModulo = T0.idModulo";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPermisos.Add(new Permisos()
                            {
                                moduloPermiso = datareader["idModulo"].ToString(),
                                usuarioPermiso = datareader["idUsuario"].ToString(),
                                tipoPermiso = datareader["permiso"].ToString(),
                                nameUsuario = datareader["nameUsuario"].ToString(),
                                nombreModulo = datareader["nombreModulo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaPermisos = new List<Permisos>();
            }

            return listaPermisos;
        }

        public int guardarAcceso(Permisos permisos)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[permisos]" +
                   "([idusuario]," +
                   "[idmodulo]," +
                   "[permiso]" +
                   ")" +
                    "VALUES" +
                   "(@idusuario," +
                   "@idmodulo," +
                   "@permiso);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idusuario", permisos.usuarioPermiso);
                    comando.Parameters.AddWithValue("@idmodulo", permisos.moduloPermiso);
                    comando.Parameters.AddWithValue("@permiso", permisos.tipoPermiso);
                    comando.CommandType = CommandType.Text;

                    idModulo = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idModulo = 0;
            }

            return idModulo;
        }

        public int eliminarAcceso(Permisos permisos)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "DELETE FROM [dbo].[permisos] " +
                   "WHERE [idusuario]=@idusuario AND " +
                   "[idmodulo]=@idmodulo; ";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idusuario", permisos.usuarioPermiso);
                    comando.Parameters.AddWithValue("@idmodulo", permisos.moduloPermiso);
                    comando.CommandType = CommandType.Text;

                    idModulo = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idModulo = 0;
            }

            return idModulo;
        }

        //METODO PARA ACTUALIZAR UN MODULO
        public int actualizarAcceso(Permisos permisos)
        {
            int idPermiso = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [dbo].[permisos]" +
                    "SET[permiso] = @permiso " +
                    "WHERE idModulo = @idModulo AND idUsuario=@idUsuario";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@permiso", permisos.tipoPermiso);
                    comando.Parameters.AddWithValue("@idUsuario", permisos.usuarioPermiso);
                    comando.Parameters.AddWithValue("@idModulo", permisos.moduloPermiso);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idPermiso = permisos.idPermiso;
                }
            }
            catch (Exception ex)
            {
                idPermiso = 0;
            }

            return idPermiso;
        }
    }
}

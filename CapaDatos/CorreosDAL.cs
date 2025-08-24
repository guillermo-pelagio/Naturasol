namespace CapaDatos
{
    public class CorreosDAL
    {
        /*
        //METODO PARA OBTENER TODOS LOS CORREOS
        public List<Correos> obtenerCorreos()
        {
            List<Correos> listaCorreos = new List<Correos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT Correo.idCorreo, " +
                        "Correo.correoElectronico, " +
                        "Correo.contrasena, " +
                        "Correo.estatus, " +
                        "Correo.usuarioAsignado, " +
                        "Correo.fechaCreacion, " +
                        "Correo.comentarios, " +
                        "Correo.fechaActualizacion, " +
                        "Usuarios.nombre, " +
                        "Usuarios.apellidoPaterno, " +
                        "Usuarios.apellidoMaterno, " +
                        "Usuarios.departamento, " +
                        "Usuarios.ubicacion " +
                        ",(SELECT COUNT (*) FROM [DesarrolloWeb].[dbo].[correos] T2 WHERE T2.correoElectronico=Correo.correoElectronico GROUP BY correoElectronico) AS cantidadCorreo " +
                        "FROM DesarrolloWeb.dbo.correos AS Correo " +
                        "LEFT JOIN DesarrolloWeb.dbo.usuarios AS Usuarios ON Usuarios.idUsuario = Correo.usuarioAsignado;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCorreos.Add(new Correos()
                            {
                                contrasena = datareader["contrasena"].ToString(),
                                usuarioAsignado = Convert.ToInt32(datareader["usuarioAsignado"].ToString()),
                                correoElectronico = datareader["correoElectronico"].ToString(),
                                comentarios = datareader["comentarios"].ToString(),
                                estatus = Convert.ToInt32(datareader["estatus"].ToString()),
                                fechaActualizacion = Convert.ToDateTime(datareader["fechaActualizacion"].ToString()),
                                fechaCreacion = Convert.ToDateTime(datareader["fechaCreacion"].ToString()),
                                idCorreo = Convert.ToInt32(datareader["idCorreo"].ToString()),
                                cantidadCorreo = datareader["cantidadCorreo"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["cantidadCorreo"].ToString()),
                                departamento = datareader["departamento"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["departamento"].ToString()),
                                nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString(),
                                ubicacion = datareader["ubicacion"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["ubicacion"].ToString()),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaCorreos = new List<Correos>();
            }

            return listaCorreos;
        }

        //METODO PARA GUARDAR UN CORREO
        public int guardarCorreo(Correos correo)
        {
            int idUsuario = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[correos]" +
                   "([correoElectronico]," +
                   "[contrasena]," +
                   "[estatus]," +
                   "[usuarioAsignado]," +
                   "[usuarioCreacion]," +
                   "[fechaCreacion]," +
                   "[comentarios]," +
                   "[fechaActualizacion])" +
                    "VALUES" +
                   "(@correoElectronico," +
                   "@contrasena," +
                   "@estatus," +
                   "@usuarioAsignado," +
                   "@usuarioCreacion," +
                   "@fechaCreacion," +
                   "@comentarios," +
                   "@fechaActualizacion);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correoElectronico", correo.correoElectronico);
                    comando.Parameters.AddWithValue("@contrasena", correo.contrasena);
                    comando.Parameters.AddWithValue("@estatus", correo.estatus);
                    comando.Parameters.AddWithValue("@usuarioCreacion", correo.usuarioCreacion);
                    comando.Parameters.AddWithValue("@comentarios", correo.comentarios);
                    comando.Parameters.AddWithValue("@usuarioAsignado", correo.usuarioAsignado);
                    comando.Parameters.AddWithValue("@fechaCreacion", correo.fechaCreacion);
                    comando.Parameters.AddWithValue("@fechaActualizacion", correo.fechaActualizacion);
                    comando.CommandType = CommandType.Text;

                    idUsuario = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idUsuario = 0;
            }

            return idUsuario;
        }

        //METODO PARA ACTUALIZAR UN CORREO
        public int actualizarCorreo(Correos correo)
        {
            int idCorreo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [dbo].[correos]" +
                    "SET[correoElectronico] = @correoElectronico " +
                    ",[contrasena] = @contrasena " +
                    ",[estatus] = @estatus " +
                    ",[comentarios] = @comentarios " +
                    ",[usuarioAsignado] = @usuarioAsignado " +
                    ",[fechaActualizacion] = @fechaActualizacion " +
                    "WHERE idCorreo = @idCorreo";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correoElectronico", correo.correoElectronico);
                    comando.Parameters.AddWithValue("@contrasena", correo.contrasena);
                    comando.Parameters.AddWithValue("@estatus", correo.estatus);
                    comando.Parameters.AddWithValue("@comentarios", correo.comentarios);
                    comando.Parameters.AddWithValue("@usuarioAsignado", correo.usuarioAsignado);
                    comando.Parameters.AddWithValue("@fechaActualizacion", correo.fechaActualizacion);
                    comando.Parameters.AddWithValue("@idCorreo", correo.idCorreo);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idCorreo = correo.idCorreo;
                }
            }
            catch (Exception ex)
            {
                idCorreo = 0;
            }

            return idCorreo;
        }

        //METODO PARA OBTENER TODOS CORREOS REPETIDOS
        public int obtenerCorreosCorreo(string correoElectronico)
        {
            int existe = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT  " +
                        "Correo.correoElectronico " +
                        "FROM DesarrolloWeb.dbo.correos AS Correo " +
                        "WHERE correoElectronico=@correoElectronico;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correoElectronico", correoElectronico);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            existe = existe + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                existe = 0;
            }

            return existe;
        }
        */
    }
}

using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class UsuariosDAL
    {
        //METODO PARA OBTENER DETALLE DEL USUARIO POR ID
        public Usuarios obtenerUsuarioDetalle(int idUsuario)
        {
            Usuarios usuario = new Usuarios();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario, " +
                        "correo, " +
                        "nombre, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "departamento, " +
                        "ubicacion, " +
                        "contrasena, " +
                        "estatus, " +
                        "puesto " +
                        "FROM DesarrolloWeb.dbo.usuarios WHERE idUsuario = @idUsuario " +
                        "ORDER BY nombre ASC";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            usuario.nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString();
                            usuario.correo = datareader["correo"].ToString();
                            usuario.puesto = datareader["puesto"].ToString();
                            usuario.idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString());
                            usuario.apellidoPaterno = datareader["apellidoPaterno"].ToString();
                            usuario.apellidoMaterno = datareader["apellidoMaterno"].ToString();
                            usuario.nombre = datareader["nombre"].ToString();
                            usuario.departamento = Convert.ToInt32(datareader["departamento"].ToString());
                            usuario.ubicacion = datareader["ubicacion"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["ubicacion"].ToString());
                            usuario.contrasena = datareader["contrasena"].ToString();
                            usuario.estatus = Convert.ToInt32(datareader["estatus"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return usuario;
        }

        /*
        //METODO PARA OBTENER DETALLE DEL USUARIO POR ID - RESPONSIVA
        public List<Responsiva> obtenerUsuarioDetalleResponsiva(int idUsuario)
        {
            List<Responsiva> listaResponsiva = new List<Responsiva>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT CONCAT(T1.nombre,' ',T1.apellidoPaterno, ' ', T1.apellidoMaterno) AS nombreCompleto, " +
                                        "T2.descripcionDepartamento, " +
                                        "T3.descripcionUbicacion, " +
                                        "T1.puesto, " +
                                        "T1.correo " +
                                        "FROM DesarrolloWeb.dbo.usuarios T1 " +
                                        "LEFT JOIN DesarrolloWeb.dbo.departamentos T2 ON T2.idDepartamento = T1.departamento " +
                                        "LEFT JOIN DesarrolloWeb.dbo.ubicaciones T3 ON T1.ubicacion = T3.idUbicacion " +
                                        "WHERE idUsuario = @idUsuario ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaResponsiva.Add(new Responsiva()
                            {
                                nombreCompleto = datareader["nombreCompleto"].ToString(),
                                correo = datareader["correo"].ToString(),
                                puesto = datareader["puesto"].ToString(),
                                departamento = datareader["descripcionDepartamento"].ToString(),
                                ubicacion = datareader["descripcionUbicacion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listaResponsiva;
        }
        */

        //METODO PARA OBTENER DETALLE DEL USUARIO POR CORREO
        public Usuarios obtenerUsuarioDetallexCorreo(string correo)
        {
            Usuarios usuario = new Usuarios();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario, " +
                        "correo, " +
                        "nombre, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "departamento, " +
                        "ubicacion, " +
                        "contrasena, " +
                        "estatus, " +
                        "puesto " +
                        "FROM DesarrolloWeb.dbo.usuarios WHERE correo = @correo " +
                        "ORDER BY nombre ASC";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            usuario.nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString();
                            usuario.correo = datareader["correo"].ToString();
                            usuario.puesto = datareader["puesto"].ToString();
                            usuario.idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString());
                            usuario.apellidoPaterno = datareader["apellidoPaterno"].ToString();
                            usuario.apellidoMaterno = datareader["apellidoMaterno"].ToString();
                            usuario.nombre = datareader["nombre"].ToString();
                            usuario.departamento = Convert.ToInt32(datareader["departamento"].ToString());
                            usuario.ubicacion = datareader["ubicacion"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["ubicacion"].ToString());
                            usuario.contrasena = datareader["contrasena"].ToString();
                            usuario.estatus = Convert.ToInt32(datareader["estatus"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return usuario;
        }

        //METODO PARA OBTENER TODOS LOS USUARIOS
        public List<Usuarios> obtenerUsuarios()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario, " +
                        "correo, " +
                        "nameUsuario, " +
                        "nombre, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "departamento, " +
                        "ubicacion, " +
                        "contrasena, " +
                        "estatus, " +
                        "puesto " +
                        "FROM DesarrolloWeb.dbo.usuarios ORDER BY nombre ASC";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaUsuarios.Add(new Usuarios()
                            {
                                nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString(),
                                correo = datareader["correo"].ToString(),
                                nameUsuario = datareader["nameUsuario"].ToString(),
                                puesto = datareader["puesto"].ToString(),
                                idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString()),
                                apellidoPaterno = datareader["apellidoPaterno"].ToString(),
                                apellidoMaterno = datareader["apellidoMaterno"].ToString(),
                                nombre = datareader["nombre"].ToString(),
                                departamento = Convert.ToInt32(datareader["departamento"].ToString()),
                                ubicacion = datareader["ubicacion"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["ubicacion"].ToString()),
                                contrasena = datareader["contrasena"].ToString(),
                                estatus = Convert.ToInt32(datareader["estatus"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaUsuarios = new List<Usuarios>();
            }

            return listaUsuarios;
        }

        //METODO PARA OBTENER TODOS LOS USUARIOS
        public List<Usuarios> obtenerUsuariosSistemas()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario, " +
                        "correo, " +
                        "nombre, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "departamento, " +
                        "ubicacion, " +
                        "contrasena, " +
                        "estatus, " +
                        "puesto " +
                        "FROM DesarrolloWeb.dbo.usuarios WHERE departamento=1 AND estatus=1 ORDER BY nombre ASC";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaUsuarios.Add(new Usuarios()
                            {
                                nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString(),
                                correo = datareader["correo"].ToString(),
                                puesto = datareader["puesto"].ToString(),
                                idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString()),
                                apellidoPaterno = datareader["apellidoPaterno"].ToString(),
                                apellidoMaterno = datareader["apellidoMaterno"].ToString(),
                                nombre = datareader["nombre"].ToString(),
                                departamento = Convert.ToInt32(datareader["departamento"].ToString()),
                                ubicacion = datareader["ubicacion"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["ubicacion"].ToString()),
                                contrasena = datareader["contrasena"].ToString(),
                                estatus = Convert.ToInt32(datareader["estatus"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaUsuarios = new List<Usuarios>();
            }

            return listaUsuarios;
        }

        //METODO PARA GUARDAR UN USUARIO
        public int guardarUsuario(Usuarios usuario)
        {
            int idUsuario = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[usuarios]" +
                   "([correo]," +
                   "[nameUsuario]," +
                   "[nombre]," +
                   "[apellidoPaterno]," +
                   "[apellidoMaterno]," +
                   "[ubicacion]," +
                   "[departamento]," +
                   "[usuarioCreacion]," +
                   "[fechaCreacion]," +
                   "[fechaActualizacion]," +
                   "[estatus]," +
                   "[puesto]," +
                   "[contrasena])" +
                    "VALUES" +
                   "(@correo," +
                   "@nameUsuario," +
                   "@nombre," +
                   "@apellidoPaterno," +
                   "@apellidoMaterno," +
                   "@ubicacion," +
                   "@departamento," +
                   "@usuarioCreacion," +
                   "@fechaCreacion," +
                   "@fechaActualizacion," +
                   "@estatus, @puesto, @contrasena);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correo", usuario.correo);
                    comando.Parameters.AddWithValue("@nameUsuario", usuario.correo); 
                    comando.Parameters.AddWithValue("@nombre", usuario.nombre);
                    comando.Parameters.AddWithValue("@apellidoPaterno", usuario.apellidoPaterno);
                    comando.Parameters.AddWithValue("@apellidoMaterno", usuario.apellidoMaterno);
                    comando.Parameters.AddWithValue("@ubicacion", usuario.ubicacion);
                    comando.Parameters.AddWithValue("@fechaCreacion", usuario.fechaCreacion);
                    comando.Parameters.AddWithValue("@fechaActualizacion", usuario.fechaActualizacion);
                    comando.Parameters.AddWithValue("@departamento", usuario.departamento);
                    comando.Parameters.AddWithValue("@usuarioCreacion", usuario.usuarioCreacion);
                    comando.Parameters.AddWithValue("@estatus", usuario.estatus);
                    comando.Parameters.AddWithValue("@puesto", usuario.puesto);
                    comando.Parameters.AddWithValue("@contrasena", usuario.contrasena);

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

        //METODO PARA ACTUALIZAR UN USUARIO
        public int actualizarUsuario(Usuarios usuario)
        {
            int idUsuario = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE[dbo].[usuarios]" +
                    " SET[correo] = @correo " +
                    ",[nombre] = @nombre" +
                    ",[nameUsuario] = @nameUsuario" +
                    ",[apellidoPaterno] = @apellidoPaterno " +
                    ",[apellidoMaterno] = @apellidoMaterno " +
                    ",[ubicacion] = @ubicacion " +
                    ",[departamento] = @departamento " +
                    ",[estatus] = @estatus " +
                    ",[puesto] = @puesto " +
                    ",[fechaActualizacion] = @fechaActualizacion " +
                    ",[contrasena] = @contrasena " +
                    " WHERE idUsuario = @idUsuario";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correo", usuario.correo);
                    comando.Parameters.AddWithValue("@nombre", usuario.nombre);
                    comando.Parameters.AddWithValue("@nameUsuario", usuario.nameUsuario);
                    comando.Parameters.AddWithValue("@apellidoPaterno", usuario.apellidoPaterno);
                    comando.Parameters.AddWithValue("@apellidoMaterno", usuario.apellidoMaterno);
                    comando.Parameters.AddWithValue("@ubicacion", usuario.ubicacion);
                    comando.Parameters.AddWithValue("@departamento", usuario.departamento);
                    comando.Parameters.AddWithValue("@estatus", usuario.estatus);
                    comando.Parameters.AddWithValue("@puesto", usuario.puesto);
                    comando.Parameters.AddWithValue("@fechaActualizacion", usuario.fechaActualizacion);
                    comando.Parameters.AddWithValue("@contrasena", usuario.contrasena);
                    
                    comando.Parameters.AddWithValue("@idUsuario", usuario.idUsuario);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idUsuario = usuario.idUsuario;
                }
            }
            catch (Exception ex)
            {
                idUsuario = 0;
            }

            return idUsuario;
        }

        //METODO PARA ACTUALIZAR UN USUARIO
        public int actualizarUsuarioInicio(Usuarios usuario)
        {
            int idUsuario = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE[dbo].[usuarios]" +
                    " SET[ultimoInicioSesion] = @ultimoInicioSesion " +
                    " ,[maquina] = @maquina " +
                    " WHERE idUsuario = @idUsuario";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@ultimoInicioSesion", usuario.ultimoInicioSesion);
                    comando.Parameters.AddWithValue("@maquina", usuario.maquina);
                    comando.Parameters.AddWithValue("@idUsuario", usuario.idUsuario);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idUsuario = usuario.idUsuario;
                }
            }
            catch (Exception ex)
            {
                idUsuario = 0;
            }

            return idUsuario;
        }

        //METODO DE BUSQUEDA DE USUARIO EN EL INICIO DE SESION
        public SesionUsuario inicioSesion(Usuarios usuario)
        {
            SesionUsuario sesion = new SesionUsuario();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario," +
                        " correo, " +
                        "nombre, " +
                        "empleado, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "nameUsuario, " +                        
                        "departamento, " +
                        "ubicacion, " +
                        "contrasena, " +
                        "puesto, " +
                        "numeroEmpleado, " +                        
                        "estatus, " +
                        "accesoPublico " +
                        "FROM DesarrolloWeb.dbo.usuarios " +
                        "WHERE nameUsuario = @nameUsuario " +
                        "AND contrasena= @contrasena";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nameUsuario", usuario.nameUsuario);
                    comando.Parameters.AddWithValue("@contrasena", usuario.contrasena);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesion.idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString());
                            sesion.nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString();
                            sesion.correo = datareader["correo"].ToString();
                            sesion.nombre = datareader["nombre"].ToString();
                            sesion.nameUsuario = datareader["nameUsuario"].ToString();
                            sesion.apellidoPaterno = datareader["apellidoPaterno"].ToString();
                            sesion.apellidoMaterno = datareader["apellidoMaterno"].ToString();
                            sesion.departamento = Convert.ToInt32(datareader["departamento"].ToString());
                            sesion.ubicacion = Convert.ToInt32(datareader["ubicacion"].ToString());
                            sesion.contrasena = datareader["contrasena"].ToString();
                            sesion.empleado = datareader["empleado"].ToString();
                            sesion.puesto = datareader["puesto"].ToString();
                            sesion.accesoPublico = datareader["accesoPublico"].ToString();
                            sesion.numeroEmpleado = datareader["numeroEmpleado"].ToString();
                            sesion.estatus = Convert.ToInt32(datareader["estatus"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sesion;
        }

        public SesionUsuario obtenerPermisos(Usuarios usuario)
        {
            SesionUsuario sesion = new SesionUsuario();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT STUFF((SELECT ','+ T2.nombreModulo FROM DesarrolloWeb.dbo.permisos T0 JOIN DesarrolloWeb.dbo.usuarios T1 ON T0.idUsuario = T1.idUsuario JOIN DesarrolloWeb.DBO.modulos T2 ON T2.idModulo = T0.idModulo WHERE permiso=1 AND T1.nameUsuario=@nameUsuario FOR XML PATH('')),1,1, '') as 'permisos'";                        
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nameUsuario", usuario.nameUsuario);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesion.permisos = datareader["permisos"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sesion;
        }

        public SesionUsuario permisosArea(Usuarios usuario)
        {
            SesionUsuario sesion = new SesionUsuario();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT STUFF((SELECT ','+ CONVERT(VARCHAR,T2.descripcion) FROM DesarrolloWeb.dbo.permisos T0 JOIN DesarrolloWeb.dbo.usuarios T1 ON T0.idUsuario = T1.idUsuario JOIN DesarrolloWeb.DBO.modulos T2 ON T2.idModulo = T0.idModulo WHERE permiso=1 AND T1.nameUsuario=@nameUsuario FOR XML PATH('')),1,1, '') as 'permisos'";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nameUsuario", usuario.nameUsuario);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesion.permisosArea = datareader["permisos"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sesion;
        }

        //METODO DE BUSQUEDA DE USUARIO EN EL INICIO DE SESION
        public SesionUsuarioGraficos obtenerInformacionGraficos(Usuarios usuario)
        {
            SesionUsuarioGraficos sesion = new SesionUsuarioGraficos();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select estatus, COUNT(estatus) as 'Suma' from desarrolloweb.dbo.cotizaciones group by estatus";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    int total = 0;
                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesion.cotizacionTotal = sesion.cotizacionTotal + Convert.ToInt32(datareader["Suma"]);

                            if (datareader["estatus"].ToString() == "0")
                            {
                                sesion.cotizacionSinMovimiento = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "1")
                            {
                                sesion.cotizacionAprobado = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "2")
                            {
                                sesion.cotizacionRechazado = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "3")
                            {
                                sesion.cotizacionRevision = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "4")
                            {
                                sesion.cotizacionPresentar = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "5")
                            {
                                sesion.cotizacionSustituido = Convert.ToInt32(datareader["Suma"]);
                            }
                            if (datareader["estatus"].ToString() == "6")
                            {
                                sesion.cotizacionMuestras = Convert.ToInt32(datareader["Suma"]);

                            }
                        }

                        sesion.cotizacionPSinMovimiento = (Convert.ToDouble(sesion.cotizacionSinMovimiento) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPAprobado = (Convert.ToDouble(sesion.cotizacionAprobado) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPRechazado = (Convert.ToDouble(sesion.cotizacionRechazado) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPRevision = (Convert.ToDouble(sesion.cotizacionRevision) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPPresentar = (Convert.ToDouble(sesion.cotizacionPresentar) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPSustituido = (Convert.ToDouble(sesion.cotizacionSustituido) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                        sesion.cotizacionPMuestras = (Convert.ToDouble(sesion.cotizacionMuestras) / Convert.ToDouble(sesion.cotizacionTotal)) * 100;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sesion;
        }

        //METODO DE BUSQUEDA DE USUARIO EN EL INICIO DE SESION
        public Usuarios recuperarContrasena(string correo)
        {
            Usuarios usuario = new Usuarios();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUsuario," +
                        " correo, " +
                        "nombre, " +
                        "nameUsuario, " +
                        "apellidoPaterno, " +
                        "apellidoMaterno, " +
                        "departamento, " +
                        "ubicacion, " +
                        "puesto, " +
                        "contrasena, " +
                        "estatus " +
                        "FROM DesarrolloWeb.dbo.usuarios " +
                        "WHERE correo = @correo; ";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            usuario.idUsuario = Convert.ToInt32(datareader["idUsuario"].ToString());
                            usuario.correo = datareader["correo"].ToString();
                            usuario.nameUsuario = datareader["nameUsuario"].ToString();
                            usuario.nombre = datareader["nombre"].ToString();
                            usuario.apellidoPaterno = datareader["apellidoPaterno"].ToString();
                            usuario.apellidoMaterno = datareader["apellidoMaterno"].ToString();
                            usuario.departamento = Convert.ToInt32(datareader["departamento"].ToString());
                            usuario.ubicacion = Convert.ToInt32(datareader["ubicacion"].ToString());
                            usuario.contrasena = datareader["contrasena"].ToString();                            
                            usuario.estatus = Convert.ToInt32(datareader["estatus"].ToString());
                            usuario.puesto = datareader["puesto"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return usuario;
        }

        //METODO PARA OBTENER LA UBICACION DEL USUARIO
        public UbicacionUsuario obtenerUbicacionUsuario(int idUbicacion)
        {
            UbicacionUsuario ubicacionUsuario = new UbicacionUsuario();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idUbicacion, " +
                        "descripcionUbicacion " +
                        "FROM DesarrolloWeb.dbo.ubicaciones AS UbicacionUsuario " +
                        "WHERE idUbicacion = @idUbicacion;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idUbicacion", idUbicacion);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            ubicacionUsuario.descripcionUbicacion = datareader["descripcionUbicacion"].ToString();
                            ubicacionUsuario.idUbicacion = Convert.ToInt32(datareader["idUbicacion"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return ubicacionUsuario;
        }
    }
}
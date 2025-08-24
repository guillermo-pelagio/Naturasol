using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class EmpleadosDAL
    {
        //METODO PARA GUARDAR UN MODULO
        public int guardarAsistencia(Empleado empleado)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[asistencia]" +
                   "([numeroEmpleado]," +
                   "[fechaRegistro]" +
                   ")" +
                    "VALUES" +
                   "(@numeroEmpleado," +
                   "@fechaRegistro" +
                   ");";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@numeroEmpleado", empleado.numeroEmpleado);
                    comando.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
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

        public int guardarAusencia(Empleado empleado)
        {
            int idModulo = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[ausencia]" +
                   "([numeroEmpleado]," +
                   "[inicioAusencia]," +
                   "[finAusencia]," +
                   "[tipoAusencia]," +
                   "[comentariosAusencia]" +
                   ")" +
                    "VALUES" +
                   "(@numeroEmpleado," +
                   "@inicioAusencia," +
                   "@finAusencia," +
                   "@tipoAusencia," +
                   "@comentariosAusencia" +
                   ");";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@numeroEmpleado", empleado.numeroEmpleado);
                    comando.Parameters.AddWithValue("@inicioAusencia", empleado.inicioAusencia);
                    comando.Parameters.AddWithValue("@finAusencia", empleado.finAusencia);
                    comando.Parameters.AddWithValue("@tipoAusencia", empleado.tipoAusencia);
                    comando.Parameters.AddWithValue("@comentariosAusencia", empleado.comentarios == null ? "" : empleado.comentarios);
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


        public List<Empleado> obtenerAsistencia()
        {
            List<Empleado> listaDocumentos = new List<Empleado>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT code, concat(lastName, ' ', firstName,' ', middleName) as 'Empleado', T1.fecharegistro FROM [DesarrolloWeb].[dbo].[asistencia] T1 " +
                        "join TSSL_NATURASOL.dbo.OHEM T2 on T2.code = T1.numeroEmpleado order by t1.idRegistro desc";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Empleado()
                            {
                                numeroEmpleado = datareader["code"].ToString(),
                                nombreEmpleado = datareader["Empleado"].ToString(),
                                fechaRegistro = datareader["Fecharegistro"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Empleado>();
            }

            return listaDocumentos;
        }

        public List<Ausencia> obtenerAusencia()
        {
            List<Ausencia> listaDocumentos = new List<Ausencia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.estatusAusencia, T2.email, T3.empID, concat(T3.lastName, ' ', T3.firstName,' ', T3.middleName) as 'Autorizante', concat(T2.lastName, ' ', T2.firstName,' ', T2.middleName) as 'Empleado', numeroEmpleado, T1.inicioAusencia, T1.idAusencia, T1.finAusencia, T1.tipoAusencia " +
                        "FROM[DesarrolloWeb].[dbo].ausencia T1 " +
                        "join TSSL_NATURASOL.dbo.OHEM T2 on T2.code = T1.numeroEmpleado " +
                        "left join TSSL_NATURASOL.dbo.OHEM T3 on T3.empID = T2.manager order by t1.idAusencia desc";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Ausencia()
                            {
                                numeroEmpleado = datareader["numeroEmpleado"].ToString(),
                                nombreEmpleado = datareader["Empleado"].ToString(),
                                autorizante = datareader["Autorizante"].ToString(),
                                estatus = datareader["estatusAusencia"].ToString(),
                                inicioAusencia = Convert.ToDateTime(datareader["inicioAusencia"].ToString()).ToShortDateString(),
                                idAusencia = datareader["idAusencia"].ToString(),
                                finAusencia = Convert.ToDateTime(datareader["finAusencia"].ToString()).ToShortDateString(),
                                tipoAusencia = datareader["tipoAusencia"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Ausencia>();
            }

            return listaDocumentos;
        }
    }
}

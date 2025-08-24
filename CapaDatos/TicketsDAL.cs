using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class TicketsDAL
    {
        //METODO PARA LISTAR TODOS LOS TICKETS
        public List<Tickets> obtenerTickets(bool administrador, int usuario)
        {
            string filtro = "";

            if (administrador)
            {
                filtro = "";
            }
            else
            {
                filtro = " WHERE idUsuarioSolicita = @usuarioSolicitado";
            }
            List<Tickets> listaTickets = new List<Tickets>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "TK.idTicket, " +
                        "TK.folio, " +
                        "TK.titulo, " +
                        "TK.descripcion, " +
                        "TK.nombreUsuario, " +
                        "TK.estatus, " +
                        "TK.tipo, " +
                        "TK.correo, " +
                        "TK.ubicacion, " +
                        "TK.prioridad, " +
                        "TK.idUsuarioFinalizo, " +
                        "TK.departamentoTexto, " +
                        "TK.horaSolicitud, " +
                        "TK.comentarioFinalizacion, " +
                        "TK.horaFinalizacion, " +
                        "US.nombre, " +
                        "US.apellidoPaterno, " +
                        "US.apellidoMaterno " +
                        "FROM [DesarrolloWeb].[dbo].[tickets] AS TK " +
                        "LEFT JOIN DesarrolloWeb.dbo.usuarios AS US ON TK.idUsuarioFinalizo = US.idUsuario" +
                        //filtro +
                        " ORDER BY TK.folio DESC;  ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    //comando.Parameters.AddWithValue("@usuarioSolicitado", usuario);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaTickets.Add(new Tickets()
                            {
                                idTicket = Convert.ToInt32(datareader["idTicket"].ToString()),
                                folio = datareader["folio"].ToString(),
                                titulo = datareader["titulo"].ToString(),
                                descripcion = datareader["descripcion"].ToString(),
                                nombreUsuario = datareader["nombreUsuario"].ToString(),
                                ubicacion = Convert.ToInt32(datareader["ubicacion"].ToString()),
                                estatus = Convert.ToInt32(datareader["estatus"].ToString()),
                                tipo = Convert.ToInt32(datareader["tipo"].ToString()),
                                prioridad = Convert.ToInt32(datareader["prioridad"].ToString()),
                                idUsuarioFinalizo = datareader["idUsuarioFinalizo"] == DBNull.Value ? 0 : Convert.ToInt32(datareader["idUsuarioFinalizo"].ToString()),
                                departamentoTexto = datareader["departamentoTexto"].ToString(),
                                horaSolicitud = Convert.ToDateTime(datareader["horaSolicitud"].ToString()),
                                fechaSolicitud = Convert.ToString(Convert.ToDateTime(datareader["horaSolicitud"].ToString())),
                                horaFinalizacion = datareader["horaFinalizacion"] == DBNull.Value ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(datareader["horaFinalizacion"].ToString()),
                                comentarioFinalizacion = datareader["comentarioFinalizacion"].ToString(),
                                correo = datareader["correo"].ToString(),
                                nombreCompleto = datareader["nombre"].ToString() + " " + datareader["apellidoPaterno"].ToString() + " " + datareader["apellidoMaterno"].ToString(),
                                //departamento = Convert.ToInt32(datareader["departamento"].ToString()),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaTickets = new List<Tickets>();
            }

            return listaTickets;
        }

        //METODO PARA LISTAR TODOS LOS TICKETS
        public static DataSet obtenerTicketsExcel(bool administrador, int usuario)
        {
            DataSet dataset = new DataSet();
            SqlConnection SqlConnection = new SqlConnection(ConexionDAL.conexionWeb);

            string filtro = "";

            if (administrador)
            {
                filtro = "";
            }
            else
            {
                filtro = " WHERE idUsuarioSolicita = @usuarioSolicitado";
            }

            try
            {
                string consulta = "SELECT " +
                    "TK.idTicket, " +
                    "TK.folio, " +
                    "TK.titulo, " +
                    "TK.descripcion, " +
                    "TK.nombreUsuario, " +
                    "TK.estatus, " +
                    "TK.tipo, " +
                    "TK.correo, " +
                    "TK.ubicacion, " +
                    "TK.prioridad, " +
                    "TK.idUsuarioFinalizo, " +
                    "TK.departamentoTexto, " +
                    "TK.horaSolicitud, " +
                    "TK.comentarioFinalizacion, " +
                    "TK.horaFinalizacion, " +
                    "US.nombre, " +
                    "US.apellidoPaterno, " +
                    "US.apellidoMaterno " +
                    "FROM [DesarrolloWeb].[dbo].[tickets] AS TK " +
                    "LEFT JOIN DesarrolloWeb.dbo.usuarios AS US ON TK.idUsuarioFinalizo = US.idUsuario" +
                    //filtro +
                    " ORDER BY TK.folio DESC;  ";

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
            finally
            {
                SqlConnection.Close();
            }

            return dataset;
        }

        //METODO PARA LISTAR EL FOLIO DE UN TICKETS
        public int obtenerFolioTicket(int tipoMovimiento)
        {
            int folio = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "FL.numeroFolio " +
                        "FROM [DesarrolloWeb].[dbo].[foliador] AS FL " +
                        "WHERE FL.tipoMovimiento = @tipoMovimiento;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@tipoMovimiento", tipoMovimiento);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {

                            folio = Convert.ToInt32(datareader["numeroFolio"].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return folio;
        }

        //METODO PARA GUARDAR UN TICKET
        public int guardarTicket(Tickets Ticket)
        {
            int idTicket = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    /*
                    string consulta = "INSERT INTO [dbo].[tickets]" +
                   "([idUsuarioSolicita]" +
                   ",[folio]" +
                   ",[titulo]" +
                   ",[descripcion]" +
                   ",[estatus]" +
                   ",[horaSolicitud])" +
                    "VALUES" +
                   "(@idUsuarioSolicita," +
                   "@folio," +
                   "@titulo," +
                   "@descripcion," +
                   "@estatus," +
                   "@horaSolicitud);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idUsuarioSolicita", Ticket.idUsuarioSolicita);
                    comando.Parameters.AddWithValue("@folio", Ticket.folio);
                    comando.Parameters.AddWithValue("@titulo", Ticket.titulo);
                    comando.Parameters.AddWithValue("@descripcion", Ticket.descripcion);
                    comando.Parameters.AddWithValue("@estatus", Ticket.estatus);
                    comando.Parameters.AddWithValue("@horaSolicitud", Ticket.horaSolicitud);
                    */

                    string consulta = "INSERT INTO [dbo].[tickets]" +
                   "([nombreUsuario]" +
                   ",[folio]" +
                   ",[titulo]" +
                   ",[descripcion]" +
                   ",[correo]" +
                   ",[tipo]" +
                   ",[prioridad]" +
                   ",[estatus]" +
                   ",[departamentoTexto]" +
                   ",[ubicacion]" +
                   ",[horaSolicitud])" +
                    "VALUES" +
                   "(@nombreUsuario," +
                   "@folio," +
                   "@titulo," +
                   "@descripcion," +
                   "@correo," +
                   "@tipo," +
                   "@prioridad," +
                   "@estatus," +
                   "@departamentoTexto," +
                   "@ubicacion," +
                   "@horaSolicitud);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nombreUsuario", Ticket.nombreUsuario);
                    comando.Parameters.AddWithValue("@folio", Ticket.folio);
                    comando.Parameters.AddWithValue("@titulo", Ticket.titulo);
                    comando.Parameters.AddWithValue("@descripcion", Ticket.descripcion);
                    comando.Parameters.AddWithValue("@correo", Ticket.correo);
                    comando.Parameters.AddWithValue("@tipo", Ticket.tipo);
                    comando.Parameters.AddWithValue("@prioridad", Ticket.prioridad);
                    comando.Parameters.AddWithValue("@estatus", Ticket.estatus);
                    comando.Parameters.AddWithValue("@ubicacion", Ticket.ubicacion);
                    comando.Parameters.AddWithValue("@departamentoTexto", Ticket.departamentoTexto);
                    comando.Parameters.AddWithValue("@horaSolicitud", Ticket.horaSolicitud);

                    comando.CommandType = CommandType.Text;

                    idTicket = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idTicket = 0;
            }

            return idTicket;
        }

        //METODO PARA ACTUALIZAR UN TICKET
        public int actualizarTicket(Tickets Ticket)
        {
            int idTicket = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [dbo].[tickets]" +
                    "SET" +
                    "[estatus] = @estatus " +
                    ",[idUsuarioFinalizo] = @idUsuarioFinalizo " +
                    ",[horaFinalizacion] = @horaFinalizacion " +
                    ",[comentarioFinalizacion] = @comentarioFinalizacion " +
                    "WHERE idTicket = @idTicket";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", Ticket.estatus);
                    comando.Parameters.AddWithValue("@idUsuarioFinalizo", Ticket.idUsuarioFinalizo);
                    comando.Parameters.AddWithValue("@horaFinalizacion", Ticket.horaFinalizacion);
                    comando.Parameters.AddWithValue("@comentarioFinalizacion", Ticket.comentarioFinalizacion == null ? "" : Ticket.comentarioFinalizacion);
                    comando.Parameters.AddWithValue("@idTicket", Ticket.idTicket);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    idTicket = Ticket.idTicket;
                }
            }
            catch (Exception ex)
            {
                idTicket = 0;
            }

            return idTicket;
        }

        //METODO PARA ACTUALIZAR FOLIO DE UN TICKET
        public int actualizarFolioTicket()
        {
            int resultado = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE " +
                        "DesarrolloWeb.dbo.foliador " +
                        "SET numeroFolio = ((SELECT numeroFolio WHERE tipoMovimiento=1) +1) " +
                        "WHERE tipoMovimiento=1;";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                    resultado = 1;
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }

            return resultado;
        }
    }
}

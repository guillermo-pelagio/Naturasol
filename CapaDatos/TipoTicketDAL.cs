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
    public class TipoTicketDAL
    {
        //METODO PARA OBTENER TODOS LOS TIPOS DE TICKET
        public List<TipoTicket> obtenerTipoTicket()
        {
            List<TipoTicket> listaTipoTicket = new List<TipoTicket>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idTipoTicket, " +
                        "descripcionTipoTicket, " +
                        "usuarioCreacion, " +
                        "fechaCreacion, " +
                        "fechaActualizacion, " +
                        "FROM DesarrolloWeb.dbo.tipoTicket AS TipoTicket;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaTipoTicket.Add(new TipoTicket()
                            {
                                descripcionTipoTicket = datareader["descripcionTipoTicket"].ToString(),
                                idTipoTicket = Convert.ToInt32(datareader["idTipoTicket"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaTipoTicket = new List<TipoTicket>();
            }

            return listaTipoTicket;
        }

        //METODO PARA OBTENER TODOS LOS TIPOS DE TICKET POR ID
        public TipoTicket obtenerTipoTicketDetalle(int idTipoTicket)
        {
            TipoTicket tipoTicket = new TipoTicket();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT idTipoTicket, " +
                        "descripcionTipoTicket, " +
                        "usuarioCreacion, " +
                        "fechaCreacion, " +
                        "fechaActualizacion " +
                        "FROM DesarrolloWeb.dbo.tipoTicket AS TipoTicket " +
                        "WHERE idTipoTicket = @idTipoTicket;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idTipoTicket", idTipoTicket);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            tipoTicket.descripcionTipoTicket = datareader["descripcionTipoTicket"].ToString();
                            tipoTicket.idTipoTicket = Convert.ToInt32(datareader["idTipoTicket"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return tipoTicket;
        }
    }
}

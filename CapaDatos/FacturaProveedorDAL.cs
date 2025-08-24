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
    public class FacturaProveedorDAL
    {
        //METODO PARA OBTENER LA FACTURA DE PROVEEDOR INTERCOMPANIA CREADO A PARTIR DE LA ENTRADA Y FACTURA DE DEUDOR
        public List<FacturaProveedor> obtenerFacturaProveedorIntercompania(FacturaDeudor facturaDeudor, string sociedadVenta)
        {
            List<FacturaProveedor> listaDocumentos = new List<FacturaProveedor>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadVenta + ".dbo.OPCH " +
                        "WHERE U_INT_DOCRE = @U_INT_DOCRE AND U_INT_SOREL=@U_INT_SOREL";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@U_INT_DOCRE", facturaDeudor.DocNum);
                    comando.Parameters.AddWithValue("@U_INT_SOREL", facturaDeudor.Sociedad == "TestMielmex" ? "MM" : (facturaDeudor.Sociedad == "TestNaturasol" ? "NA" : (facturaDeudor.Sociedad == "TestEvi") ? "EV" : "NO"));
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new FacturaProveedor()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<FacturaProveedor>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER EL DOCENTRY DE LA UM EN LA SOCIEDAD DE LA ORDEN DE VENTA BASADO EN LA DESCRIPCION
        public int obtenerUM(string uM, string sociedadVenta)
        {
            int unidadMedida = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT UomEntry " +
                        "FROM " + sociedadVenta + ".dbo.OUOM " +
                        "WHERE UomCode = @UomCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@UomCode", uM);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            unidadMedida = Convert.ToInt32(datareader["UomEntry"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                unidadMedida = -1;
            }

            return unidadMedida;
        }
    }
}

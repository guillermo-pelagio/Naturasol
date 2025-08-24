using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class OrdenesVentaDAL
    {
        //METODO PARA OBTENER LAS OV QUE SON INTERCOMPANIA - PARA REALIZAR ORDEN DE VENTA
        public List<OrdenVenta> obtenerOrdenesVentaIntercompania(string sociedad)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "ORDR.DocEntry, " +
                        "ORDR.DocNum, " +
                        "ORDR.CardCode, " +
                        "ORDR.CardName, " +
                        "ORDR.DocDate, " +
                        "ORDR.DocType, " +
                        "ORDR.DocCur, " +
                        "ORDR.UserSign " +
                        "FROM " + sociedad + ".dbo.ORDR " +
                        "WHERE ORDR.CardCode IN (" +
                        "'1122000069','1120400132'," +
                        "'1120400133','1122000147'," +
                        "'1122000081','1120400130'," +
                        "'1120400131','1122000020'" +
                        ") " +
                        "AND ORDR.CANCELED='N' AND ORDR.DocDate >= '2024-07-11' AND ORDR.U_NoCaja is null";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVenta()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Sociedad = sociedad,
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                UserSign = datareader["UserSign"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                DocCur = datareader["DocCur"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenVenta>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER EL PEDIDO DE VENTA INTERCOMPANIA CREADO A PARTIR DE LA OC
        public List<OrdenVenta> obtenerPedidoVentaIntercompania(OrdenCompra ordenCompra, string sociedadVenta)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadVenta + ".dbo.ORDR " +
                        "WHERE U_INT_DOCRE = @U_INT_DOCRE AND U_INT_SOREL=@U_INT_SOREL";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@U_INT_DOCRE", ordenCompra.DocNum);
                    comando.Parameters.AddWithValue("@U_INT_SOREL", ordenCompra.Sociedad == "TestMielmex" ? "MM" : (ordenCompra.Sociedad == "TestNaturasol" ? "NA" : (ordenCompra.Sociedad == "TestEvi") ? "EV" : "NO"));
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVenta()
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
                listaDocumentos = new List<OrdenVenta>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER EL PEDIDO DE VENTA INTERCOMPANIA CREADO A PARTIR DE LA OC
        public List<OrdenVenta> obtenerPedidoVenta(string ordenCompra)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT docentry " +
                        "FROM TSSL_NATURASOL.dbo.ORDR " +
                        "WHERE numatcard = @ordencompra";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@ordencompra", ordenCompra);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVenta()
                            {
                                DocEntry = datareader["docentry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenVenta>();
            }

            return listaDocumentos;
        }

        //S14-REQUERIDO
        public List<OrdenVenta> obtenerElaboradorOVAbiertas(string sociedadVenta)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct OSLP.SlpName, oslp.Email  " +
                                        "FROM " + sociedadVenta + ".dbo.ORDR " +
                                             "JOIN " + sociedadVenta + ".dbo.OSLP ON OSLP.SlpCode = ORDR.SlpCode " +
                                             "JOIN " + sociedadVenta + ".dbo.RDR1 ON RDR1.DocEntry = ORDR.DocEntry " +
                                             "JOIN " + sociedadVenta + ".dbo.OUSR ON OUSR.USERID = ORDR.UserSign " +
                                        "WHERE ORDR.taxdate < GETDATE() + 3 AND ORDR.DocStatus = 'O' " +
                                        "group by OSLP.SlpName, oslp.Email " +
                                        "ORDER BY OSLP.SlpName ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVenta()
                            {
                                //E_Mail = datareader["Email"].ToString(),
                                SlpName = datareader["SlpName"].ToString(),
                                E_Mail = datareader["Email"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenVenta>();
            }

            return listaDocumentos;
        }

        //S14-REQUERIDO
        public List<OrdenVenta> obtenerOVAbiertas(string sociedadVenta, string email)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT OSLP.EMail, ORDR.DocEntry, ORDR.NumAtCard, ORDR.cardcode,RDR1.U_MOT_NO_ENTREGA, ORDR.cardname, ORDR.docdate, ORDR.docnum, slpname, ORDR.taxdate, CASE WHEN ORDR.doccur='MXP' THEN ORDR.doctotal ELSE ORDR.DocTotalFC end as doctotal, ORDR.doccur, " +
                                        "sum(RDR1.OpenQty) as 'Cantidad Pendiente' " +
                                        "FROM " + sociedadVenta + ".dbo.ORDR " +
                                             "JOIN " + sociedadVenta + ".dbo.OSLP ON OSLP.SlpCode = ORDR.SlpCode " +
                                             "JOIN " + sociedadVenta + ".dbo.RDR1 ON RDR1.DocEntry = ORDR.DocEntry " +
                                             "JOIN " + sociedadVenta + ".dbo.OUSR ON OUSR.USERID = ORDR.UserSign " +
                                        "WHERE ORDR.taxdate < GETDATE() - 30 AND ORDR.docdate < GETDATE() - 30 AND ORDR.DocStatus = 'O'  " +
                                        "AND OSLP.SlpName= @email " +
                                        "group by OSLP.EMail, ORDR.DocEntry, ORDR.NumAtCard,RDR1.U_MOT_NO_ENTREGA, ORDR.cardcode, ORDR.cardname, ORDR.docdate, ORDR.docnum, slpname, ORDR.taxdate,ORDR.doctotal, ORDR.DocTotalFC, ORDR.doccur, ORDR.PaidToDate, ORDR.PaidSys,  oRDR.doctype, ORDR.DocType " +
                                        "ORDER BY ORDR.docnum ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVenta()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                E_Mail = datareader["EMail"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                SlpName = datareader["slpname"].ToString(),
                                MotNoEntrega = datareader["U_MOT_NO_ENTREGA"].ToString(),
                                TaxDate = datareader["taxdate"].ToString(),
                                DocTotal = datareader["doctotal"].ToString(),
                                DocCur = datareader["doccur"].ToString(),
                                CantidadPendiente = datareader["Cantidad Pendiente"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenVenta>();
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

        //METODO PARA OBTENER LOS DETALLES DE LAS OC QUE SON INTERCOMPANIA
        public List<OrdenVentaDetalle> obtenerOrdenesVentaDetalleIntercompania(string sociedad, OrdenVenta orden)
        {
            List<OrdenVentaDetalle> listaDocumentos = new List<OrdenVentaDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "RDR1.ItemCode, " +
                        "RDR1.Quantity, " +
                        "RDR1.TaxCode, " +
                        "RDR1.LineNum, " +
                        "RDR1.Price, " +
                        "RDR1.Currency, " +
                        "RDR1.UomCode " +
                        "FROM " + sociedad + ".dbo.RDR1 " +
                        "WHERE DocEntry = @DocEntry";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", orden.DocEntry);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenVentaDetalle()
                            {
                                ItemCode = datareader["ItemCode"].ToString(),
                                UM = datareader["UomCode"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                TaxCode = datareader["TaxCode"].ToString(),
                                LineNum = datareader["LineNum"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenVentaDetalle>();
            }

            return listaDocumentos;
        }

        public int actualizarDocumentosVenta(string sociedad, string DocEntry, string linea, string item)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE " + sociedad + ".[dbo].[RDR1] " +
                    "SET [U_FechaRevision] = @DocEntry " +
                    "WHERE DocEntry = @DocEntry";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", DocEntry);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }

            return 1;
        }

        public int actualizarFechaCitaOV2(string ordenCompra, string fecha, string hora, string cita)
        {
            string ddd = Convert.ToString(fecha).Split('/')[2] + "-" + Convert.ToString(fecha).Split('/')[1] + "-" + Convert.ToString(fecha).Split('/')[0];
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    conexionDB.Open();
                    string consulta = "UPDATE TSSL_NATURASOL.[dbo].[ORDR] " +
                    "SET [U_HoraEntrega] = cast( @U_HoraEntrega as smallint), " +
                    " [U_ETABodega] =  @fecha ," +
                    " [U_Observaciones] = @U_Observaciones " +
                    "WHERE numatcard = @DocNum";

                    string horas = (Convert.ToString(hora).Split(':')[0] + Convert.ToString(hora).Split(':')[1]);

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", ordenCompra);
                    comando.Parameters.AddWithValue("@fecha", ddd);
                    comando.Parameters.AddWithValue("@U_HoraEntrega", horas);
                    comando.Parameters.AddWithValue("@U_Observaciones", cita);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                    conexionDB.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return 1;
        }
    }
}

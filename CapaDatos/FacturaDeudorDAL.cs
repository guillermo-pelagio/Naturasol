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
    public class FacturaDeudorDAL
    {
        //METODO PARA OBTENER LAS FACTURAS DE DEUDOR QUE SON INTERCOMPANIA - PARA REALIZAR FACTURA PROVEEDOR
        public List<FacturaDeudor> obtenerFacturaDeudorIntercompania(string sociedad)
        {
            List<FacturaDeudor> listaDocumentos = new List<FacturaDeudor>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "OINV.DocEntry, " +
                        "OINV.DocNum, " +
                        "OINV.CardCode, " +
                        "OINV.CardName, " +
                        "OINV.DocDate, " +
                        "OINV.DocType, " +
                        "OINV.UserSign, " +
                        "OINV.U_INT_DOCRE " +
                        "FROM " + sociedad + ".dbo.OINV " +
                        "WHERE OINV.U_INT_Generar = 'Y' " +
                        "AND OINV.DocStatus='O'";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new FacturaDeudor()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                U_INT_DOCRE = datareader["U_INT_DOCRE"].ToString(),
                                Sociedad = sociedad,
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                UserSign = datareader["UserSign"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<FacturaDeudor>();
            }

            return listaDocumentos;
        }

        //REVISADO
        public List<FacturaDeudor> obtenerReservaSinEntrega()
        {
            List<FacturaDeudor> listaDocumentos = new List<FacturaDeudor>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.RESERVASINENTREGA ORDER BY docdate DESC ";
                    
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new FacturaDeudor()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<FacturaDeudor>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE LAS FACTURAS DE DEUDOR QUE SON INTERCOMPANIA
        public List<FacturaDeudorDetalle> obtenerFacturaDeudorDetalleIntercompania(string sociedad, FacturaDeudor facturaDeudor)
        {
            List<FacturaDeudorDetalle> listaDocumentos = new List<FacturaDeudorDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "INV1.ItemCode, " +
                        "INV1.Quantity, " +
                        "INV1.TaxCode, " +
                        "INV1.LineNum, " +
                        "INV1.Price, " +
                        "INV1.Currency, " +
                        "INV1.UomCode " +
                        "FROM " + sociedad + ".dbo.INV1 " +
                        "WHERE DocEntry = @DocEntry";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", facturaDeudor.DocEntry);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new FacturaDeudorDetalle()
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
                listaDocumentos = new List<FacturaDeudorDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE LAS FACTURAS CON NOMBRE DE SOCIO INCORRECTO O VERSION INCORRECTA
        public List<FacturaDeudorDetalle> obtenerFacturasCorreccionesV4(string sociedad)
        {
            List<FacturaDeudorDetalle> listaDocumentos = new List<FacturaDeudorDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.U_FT3_IDVERSIONCFDI, T1.DocEntry, T2.CardName " +
                        "FROM " + sociedad + ".dbo.OINV T1 " +
                        "JOIN " + sociedad + ".dbo.OCRD T2 ON T2.CardCode = T1.CardCode " +
                        "WHERE T1.DocDate >= '2023-04-01' " +
                        "AND(T2.CardName <> T1.CardName OR T1.U_FT3_IDVERSIONCFDI <> 4) AND T1.DocStatus = 'O'; ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new FacturaDeudorDetalle()
                            {
                                CardName = datareader["CardName"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<FacturaDeudorDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA ACTUALIZAR UN ACCESORIO
        public int actualizarFacturaDeudor(string sociedad, string DocEntry, string Cardname)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE " + sociedad + ".[dbo].[OINV] " +
                    "SET[CardName] = @Cardname " +
                    ",[U_FT3_IDVERSIONCFDI] = 4 " +
                    "WHERE DocEntry = @DocEntry";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@Cardname", Cardname);
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
    }
}

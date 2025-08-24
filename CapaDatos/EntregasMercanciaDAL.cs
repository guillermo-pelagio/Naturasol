using CapaEnidades;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class EntregaMercanciaDAL
    {
        //METODO PARA OBTENER LAS ENTREGAS QUE SON INTERCOMPANIA, DE LAS QUE SE HARAN ENTRADAS
        public List<EntregaMercancia> obtenerEntregasIntercompania(string sociedad)
        {
            List<EntregaMercancia> listaDocumentos = new List<EntregaMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "ODLN.DocEntry, " +
                        "ODLN.DocNum, " +
                        "ODLN.CardCode, " +
                        "ODLN.CardName, " +
                        "ODLN.DocDate, " +
                        "ODLN.DocCur, " +
                        "ODLN.DocType, " +
                        "ODLN.UserSign, " +
                        "ODLN.Comments, " +
                        "ORDR.U_NoCaja, " +
                        "ODLN.U_REPLICAR, " +
                        "ORDR.DocEntry as 'OV', " +
                        "OINV.DocEntry as 'FC', " +
                        "DLN1.BaseType " +
                        "FROM " + sociedad + ".dbo.ODLN " +
                        "INNER JOIN  " + sociedad + ".dbo.DLN1 ON DLN1.DocEntry = ODLN.DocEntry " +
                        "LEFT JOIN  " + sociedad + ".dbo.ORDR ON ORDR.DocEntry = DLN1.BaseEntry AND ORDR.ObjType = DLN1.BaseType " +
                        "LEFT JOIN  " + sociedad + ".dbo.OINV ON OINV.DocEntry = DLN1.BaseEntry AND OINV.ObjType = DLN1.BaseType " +
                        "WHERE ODLN.DocDate >= '2024-07-11' " +
                        "AND ODLN.CardCode IN (" +
                        "'1122000069','1120400132'," +
                        "'1120400133','1122000147'," +
                        "'1122000081','1120400130'," +
                        "'1120400131','1122000020'" +
                        ") " +
                        "AND ODLN.CANCELED = 'N' AND ODLN.U_NoCaja is null AND ORDR.U_NoCaja IS NOT NULL AND ODLN.DocType='I'" +
                        "UNION ALL " +
                         "SELECT " +
                        "ODLN.DocEntry, " +
                        "ODLN.DocNum, " +
                        "ODLN.CardCode, " +
                        "ODLN.CardName, " +
                        "ODLN.DocDate, " +
                        "ODLN.DocCur, " +
                        "ODLN.DocType, " +
                        "ODLN.UserSign, " +
                        "ODLN.Comments, " +
                        "ORDR.U_NoCaja, " +
                        "ODLN.U_REPLICAR, " +
                        "ORDR.DocEntry as 'OV', " +
                        "OINV.DocEntry as 'FC', " +
                        "DLN1.BaseType " +
                        "FROM " + sociedad + ".dbo.ODLN " +
                        "INNER JOIN  " + sociedad + ".dbo.DLN1 ON DLN1.DocEntry = ODLN.DocEntry " +
                        "LEFT JOIN  " + sociedad + ".dbo.OINV ON OINV.DocEntry = DLN1.BaseEntry AND OINV.ObjType = DLN1.BaseType  " +
                        "LEFT JOIN  " + sociedad + ".dbo.INV1 ON OINV.DocEntry = INV1.DocEntry " +
                        "LEFT JOIN  " + sociedad + ".dbo.ORDR ON ORDR.DocEntry = INV1.BaseEntry AND ORDR.ObjType = INV1.BaseType  " +
                        "WHERE ODLN.DocDate >= '2024-07-11' " +
                        "AND ODLN.CardCode IN (" +
                        "'1122000069','1120400132'," +
                        "'1120400133','1122000147'," +
                        "'1122000081','1120400130'," +
                        "'1120400131','1122000020'" +
                        ") " +
                        "AND ODLN.CANCELED = 'N' AND ODLN.U_NoCaja is null AND ORDR.U_NoCaja IS NOT NULL AND ODLN.DocType='I'";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntregaMercancia()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                Comments = datareader["Comments"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Sociedad = sociedad,
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                U_NoCaja = datareader["U_NoCaja"].ToString(),
                                U_REPLICAR = datareader["U_REPLICAR"].ToString(),
                                OV = datareader["OV"].ToString(),
                                FC = datareader["FC"].ToString(),
                                BaseType = datareader["BaseType"].ToString(),
                                UserSign = datareader["UserSign"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntregaMercancia>();
            }

            return listaDocumentos;
        }

        //S22-REQUERIDO
        public List<EntregaMercancia> obtenerEntregaSinFactura()
        {
            List<EntregaMercancia> listaDocumentos = new List<EntregaMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM [W22-EntregaSinFactura] ORDER BY docdate ASC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntregaMercancia()
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
                listaDocumentos = new List<EntregaMercancia>();
            }

            return listaDocumentos;
        }


        //METODO PARA OBTENER LOS LOTES DE UNA ENTREGA
        public List<LoteEntregaMercancia> obtenerLotesEntregaDetalleIntercompania(string sociedad, EntregaMercanciaDetalle entregaMercanciaDetalle)
        {
            List<LoteEntregaMercancia> listaDocumentos = new List<LoteEntregaMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DISTINCT " +
                        "BatchNum, " +
                        "ExpDate, " +
                        "BaseLinNum, " +
                        "BaseEntry, " +
                        "IBT1.Quantity " +
                        "FROM " + sociedad + ".dbo.IBT1 " +
                        "JOIN " + sociedad + ".dbo.OBTN ON OBTN.DistNumber = IBT1.BatchNum " +
                        "WHERE BaseEntry = @BaseEntry " +
                        "AND BaseType = 15 " +
                        "AND OBTN.ItemCode=@itemcode " +
                        "AND BaseLinNum=@LineNum";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@BaseEntry", entregaMercanciaDetalle.DocEntry);
                    comando.Parameters.AddWithValue("@LineNum", entregaMercanciaDetalle.LineNum);
                    comando.Parameters.AddWithValue("@itemcode", entregaMercanciaDetalle.ItemCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new LoteEntregaMercancia()
                            {
                                BatchNum = datareader["BatchNum"].ToString(),
                                ExpDate = datareader["ExpDate"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                BaseEntry = datareader["BaseEntry"].ToString(),
                                BaseLinNum = datareader["BaseLinNum"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return listaDocumentos;
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER EL DETALLE DE UNA ENTREGA
        public List<EntregaMercanciaDetalle> obtenerEntregasDetalleIntercompania(string sociedad, EntregaMercancia orden)
        {
            List<EntregaMercanciaDetalle> listaDocumentos = new List<EntregaMercanciaDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "DLN1.DocEntry, " +
                        "DLN1.ItemCode, " +
                        "DLN1.Quantity, " +
                        "DLN1.TaxCode, " +
                        "DLN1.LineNum, " +
                        "DLN1.Price, " +
                        "DLN1.Currency " +
                        "FROM " + sociedad + ".dbo.DLN1 " +
                        "JOIN " + sociedad + ".dbo.ODLN on ODLN.DocEntry = DLN1.DocEntry " +
                        "WHERE ODLN.DocEntry=DLN1.DocEntry " +
                        "AND ODLN.DocEntry= @DocEntry " +
                        "AND DLN1.Quantity>0";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", orden.DocEntry);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntregaMercanciaDetalle()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
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
                listaDocumentos = new List<EntregaMercanciaDetalle>();
            }

            return listaDocumentos;
        }

        public List<EntregaMercancia> obtenerEntradaIntercompania(EntregaMercancia ordenCompra, string sociedadVenta)
        {
            List<EntregaMercancia> listaDocumentos = new List<EntregaMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadVenta + ".dbo.OPDN " +
                        "WHERE NumAtCard = @U_INT_DOCRE";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@U_INT_DOCRE", (ordenCompra.Sociedad.Contains("MIELMEX") ? "MM" : ordenCompra.Sociedad.Contains("NATURASOL") ? "NA" : ordenCompra.Sociedad.Contains("NOVAL") ? "NO" : "EV") + ordenCompra.DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntregaMercancia()
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
                listaDocumentos = new List<EntregaMercancia>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE LAS OC QUE SON INTERCOMPANIA
        public List<EntregaMercanciaDetalle> obtenerOrdenesVentaDetalleIntercompania(string sociedad, EntregaMercancia orden)
        {
            List<EntregaMercanciaDetalle> listaDocumentos = new List<EntregaMercanciaDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "DLN1.ItemCode, " +
                        "DLN1.Quantity, " +
                        "DLN1.TaxCode, " +
                        "DLN1.Docentry, " +
                        "DLN1.LineNum, " +
                        "DLN1.Price, " +
                        "DLN1.Currency, " +
                        "DLN1.UomCode " +
                        "FROM " + sociedad + ".dbo.DLN1 " +
                        "WHERE DocEntry = @DocEntry";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", orden.DocEntry);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntregaMercanciaDetalle()
                            {
                                ItemCode = datareader["ItemCode"].ToString(),
                                UM = datareader["UomCode"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                TaxCode = datareader["TaxCode"].ToString(),
                                LineNum = datareader["LineNum"].ToString(),
                                Price = datareader["Price"].ToString(),
                                DocEntry = datareader["Docentry"].ToString(),
                                Currency = datareader["Currency"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntregaMercanciaDetalle>();
            }

            return listaDocumentos;
        }
    }
}

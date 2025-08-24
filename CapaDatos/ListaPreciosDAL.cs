using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ListaPreciosDAL
    {
        //METODO PARA 
        public List<ListaPrecios> obtenerDocumentosVenta(string sociedad)
        {
            List<ListaPrecios> listaPrecios = new List<ListaPrecios>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.DocEntry, T0.DocNum, T1.VisOrder, T0.DocDate, T0.CardCode, T0.CardName, T1.ItemCode, T1.Dscription, T1.Quantity, T1.unitMsr, T1.Price, T1.Currency, T3.Price AS 'PrecioLE', T3.Currency AS 'MonedaLE', T4.UomName, T3.ListNum, T6.BaseQty, ISNULL(T3.Price, 0 )* T6.BaseQty AS 'PrecioUOM' " +
                        "FROM " + sociedad + ".dbo.ORDR T0 " +
                        "LEFT JOIN " + sociedad + ".dbo.RDR1 T1 ON T1.DocEntry = T0.DocEntry " +
                        "LEFT JOIN " + sociedad + ".dbo.OITM T2 ON T2.ItemCode = T1.ItemCode " +
                        "LEFT JOIN " + sociedad + ".dbo.OSPP T3 ON T3.CardCode = T0.CardCode AND T3.ItemCode = T1.ItemCode " +
                        "LEFT JOIN " + sociedad + ".dbo.OUOM T4 ON T4.UomEntry = T2.PriceUnit " +
                        "LEFT JOIN " + sociedad + ".dbo.OUGP T5 ON T1.ItemCode = T5.UgpCode " +
                        "LEFT JOIN " + sociedad + ".dbo.UGP1 T6 ON T6.UgpEntry = T5.UgpEntry " +
                        "LEFT JOIN " + sociedad + ".dbo.OUOM T7 ON T7.UomEntry = T6.UomEntry " +
                        "WHERE T1.OpenQty=T1.Quantity AND T2.U_Familia NOT IN ('MD') AND T0.DocStatus='O' AND T1.LineStatus='O' " +
                        "AND((T1.U_FechaRevision is null) OR (T1.U_FechaRevision<>T1.ItemCode)) AND T3.UpdateDate<T0.UpdateDate AND T7.UomName = T1.unitMsr " +
                        "ORDER BY T0.DocNum ASC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPrecios.Add(new ListaPrecios()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                VisOrder = datareader["VisOrder"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                Dscription = datareader["Dscription"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                unitMsr = datareader["unitMsr"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                                PrecioLE = datareader["PrecioLE"].ToString(),
                                MonedaLE = datareader["MonedaLE"].ToString(),
                                UomName = datareader["UomName"].ToString(),
                                ListNum = datareader["ListNum"].ToString(),
                                BaseQty = datareader["BaseQty"].ToString(),
                                PrecioUOM = datareader["PrecioUOM"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaPrecios = new List<ListaPrecios>();
            }

            return listaPrecios;
        }

        //METODO PARA 
        public List<ListaPrecios> obtenerDocumentosCompra(string sociedad)
        {
            List<ListaPrecios> listaPrecios = new List<ListaPrecios>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.DocEntry, T0.DocNum, T1.VisOrder, T0.DocDate, T0.CardCode, T0.CardName, T1.ItemCode, T1.Dscription, T1.Quantity, T1.unitMsr, T1.Price, T1.Currency, T3.Price AS 'PrecioLE', T3.Currency AS 'MonedaLE', T4.UomName, T3.ListNum, T6.BaseQty, ISNULL(T3.Price, 0 )* T6.BaseQty AS 'PrecioUOM' " +
                        "FROM " + sociedad + ".dbo.ODRF T0 " +
                        "LEFT JOIN " + sociedad + ".dbo.DRF1 T1 ON T1.DocEntry = T0.DocEntry " +
                        "LEFT JOIN " + sociedad + ".dbo.OITM T2 ON T2.ItemCode = T1.ItemCode " +
                        "LEFT JOIN " + sociedad + ".dbo.OSPP T3 ON T3.CardCode = T0.CardCode AND T3.ItemCode = T1.ItemCode " +
                        "LEFT JOIN " + sociedad + ".dbo.OUOM T4 ON T4.UomEntry = T2.PriceUnit " +
                        "LEFT JOIN " + sociedad + ".dbo.OUGP T5 ON T1.ItemCode = T5.UgpCode " +
                        "LEFT JOIN " + sociedad + ".dbo.UGP1 T6 ON T6.UgpEntry = T5.UgpEntry " +
                        "LEFT JOIN " + sociedad + ".dbo.OUOM T7 ON T7.UomEntry = T6.UomEntry " +
                        "WHERE T0.CreateDate>='2024/01/01' " +
                        //"AND T1.OpenQty=T1.Quantity " +
                        "AND T1.ObjType = 22 "+
                        "AND T1.ItemCode NOT LIKE ('2001-010%') " +
                        "AND T0.CardCode NOT IN ('2110100502', '2110100501') " +
                        "AND T0.DocStatus='O' AND T1.LineStatus='O' " +
                        "AND((T1.U_FechaRevision is null) OR (T1.U_FechaRevision<>T1.ItemCode)) AND T3.UpdateDate<T0.UpdateDate" +
                        "AND T4.UomName = T1.unitMsr " +
                        "ORDER BY T0.DocNum ASC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPrecios.Add(new ListaPrecios()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                VisOrder = datareader["VisOrder"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                Dscription = datareader["Dscription"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                unitMsr = datareader["unitMsr"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                                PrecioLE = datareader["PrecioLE"].ToString(),
                                MonedaLE = datareader["MonedaLE"].ToString(),
                                UomName = datareader["UomName"].ToString(),
                                ListNum = datareader["ListNum"].ToString(),
                                BaseQty = datareader["BaseQty"].ToString(),
                                PrecioUOM = datareader["PrecioUOM"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaPrecios = new List<ListaPrecios>();
            }

            return listaPrecios;
        }
    }
}

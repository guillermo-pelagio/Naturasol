using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class OrdenesCompraDAL
    {
        //METODO PARA OBTENER TODOS LAS OC DEL FOLIO
        public List<BorradorDocumento> obtenerDocumentos()
        {
            List<BorradorDocumento> listaDocumentos = new List<BorradorDocumento>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * " +
                        "FROM liberacionOCSAP " +
                        "WHERE " +
                        "estatus NOT IN (0,-1)";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new BorradorDocumento()
                            {
                                estatusAutorizacion = datareader["estatusAutorizacion"].ToString(),
                                sociedad = datareader["sociedad"].ToString(),
                                estatus = Convert.ToInt32(datareader["estatus"].ToString()),
                                wddCode = Convert.ToInt32(datareader["wddCode"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<BorradorDocumento>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE UNA OC
        public List<Intercompania> obtenerSaldosIntercompanias(string sociedad)
        {
            List<Intercompania> listaDocumentos = new List<Intercompania>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT * FROM SALDOSINTERCOMPANIA ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Intercompania()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                Balance = datareader["balance"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Intercompania>();
            }

            return listaDocumentos;
        }

        //REVISADO
        public List<OrdenCompra> obtenerEntradasSinPE()
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT DocNum, DocDate, opdn.CardCode, opdn.CardName, NumAtCard, DocTotal, DocTotalFC, DocCur, opdn.DocEntry " +
                                        "FROM DESARROLLOWEB.dbo.ENTREGASSINPE ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                DocCur = datareader["DocCur"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE UNA OC
        public DataSet obtenerDetalleOrdenesCompra(string sociedad, string docEntry)
        {
            DataSet dataset = new DataSet();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT " +
                        "DRF1.ItemCode AS Artículo, " +
                        "DscriptiON AS Descripción, " +
                        "OITM.SalUnitMsr AS 'U.M.', " +
                        "CASE WHEN ODRF.DocType = 'I' THEN Quantity ELSE U_CantidadServicio END AS 'Cantidad', " +
                        "Price AS Precio, " +
                        "VatPrcnt AS 'Impuesto %', CASE WHEN ODRF.DocCur ='MXP' THEN DRF1.LineTotal ELSE DRF1.TotalFrgn END AS 'Total', " +
                        "Currency AS Moneda, " +
                        "AcctCode AS 'Cuenta de mayor', " +
                        "OcrCode AS 'Planta', " +
                        "OcrCode2 AS 'Centro de costo' " +
                        "FROM " + sociedad + ".dbo.DRF1 " +
                        "JOIN " + sociedad + ".dbo.ODRF ON DRF1.DocEntry = ODRF.DocEntry " +
                        "LEFT JOIN " + sociedad + ".dbo.OITM ON OITM.ItemCode = DRF1.ItemCode " +
                        "WHERE " + sociedad + ".dbo.DRF1.DocEntry = @docEntry ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docEntry", docEntry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comANDo;
                    comANDo.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return dataset;
        }

        //METODO PARA OBTENER LAS OC QUE SON INTERCOMPANIA - PARA REALIZAR ORDEN DE VENTA
        public List<OrdenCompra> obtenerOrdenesCompraIntercompania(string sociedad)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "OPOR.DocEntry, " +
                        "OPOR.DocNum, " +
                        "OPOR.CardCode, " +
                        "OPOR.CardName, " +
                        "OPOR.DocDate, " +
                        "OPOR.DocType, " +
                        "OPOR.UserSign " +
                        "FROM " + sociedad + ".dbo.OPOR " +
                        "WHERE OPOR.U_INT_Generar = 'Y' " +
                        "AND OPOR.DocStatus='O'";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
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
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //S12-REQUERIDO
        public List<OrdenCompra> listaCompradorOCAbiertas(string sociedad)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct slpname, oslp.Email " +
                                        "FROM " + sociedad + ".dbo.OPOR " +
                                             "JOIN " + sociedad + ".dbo.OSLP ON OSLP.SlpCode = OPOR.SlpCode " +
                                             "JOIN " + sociedad + ".dbo.POR1 ON POR1.DocEntry = OPOR.DocEntry " +
                                             "LEFT JOIN " + sociedad + ".dbo.DPO1 ON DPO1.BaseRef = OPOR.DocNum AND DPO1.BaseType = 22 " +
                                             "LEFT JOIN " + sociedad + ".dbo.ODPO ON ODPO.DocEntry = DPO1.DocEntry " +
                                        "WHERE OPOR.docduedate < GETDATE() AND OPOR.DocStatus = 'O' " +
                                        "GROUP BY OPOR.DocEntry, OPOR.cardcode, OPOR.cardname, OPOR.docdate, OPOR.docnum, slpname, OPOR.taxdate,OPOR.doctotal, OPOR.DocTotalFC, OPOR.doccur, OPOR.PaidToDate, OPOR.PaidSys,  opor.doctype, ODPO.DocNum, OPOR.DocType, oslp.Email " +
                                        "ORDER BY SlpName ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                SlpName = datareader["slpname"].ToString(),
                                Email = datareader["Email"].ToString(),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> listaBorradoresLiberacion(string sociedad)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.U_LiberacionOC, T0.Docnum, T0.DocEntry, T5.WddCode, T5.WtmCode " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "left JOIN  " + sociedad + ".dbo.OWDD T5 ON T0.DocEntry = T5.DraftEntry " +
                        "WHERE T0.ObjType = 22  " +
                    //"and t0.DocEntry='104442' ";
                    //"AND T0.U_LiberacionOC = 0 and T0.DocDate>'2025-03-10' and T0.DocDate<'2025-03-19'";
                    "AND T0.U_LiberacionOC <> 0 ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                U_LiberacionOC = datareader["U_LiberacionOC"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["Docnum"].ToString(),
                                WddCode = datareader["WddCode"].ToString(),
                                WtmCode = datareader["WtmCode"].ToString(),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> listaBorradoresLiberacionDetalle(string sociedad, string docentry)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.DocEntry, DocNum, cast(T0.DocDate as date) as DocDate, DocType, T0.CardCode, CardName, DocCur, DocRate, T0.VatSum, T0.VatSumFC, DocTotal, DocTotalFC, SlpName, T1.Email, T2.ItemCode, Dscription, T2.AcctCode, T3.AcctName, U_CantidadServicio, Quantity, Price, t2.Currency, LineTotal, TotalFrgn, TaxCode, T2.OcrCode, T2.OcrCode3, T10.Existencia, t7.Consumo,case when T2.ShipDate is null then getdate() else cast(T2.ShipDate as date) end as ShipDate " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "JOIN " + sociedad + ".dbo.oslp T1 ON T1.SlpCode = T0.SlpCode " +
                        "JOIN " + sociedad + ".dbo.drf1 T2 ON T2.DocEntry = T0.DocEntry " +
                        "JOIN " + sociedad + ".dbo.oact T3 ON T2.AcctCode = T3.AcctCode " +
                        "LEFT JOIN  " + sociedad + ".dbo.oitm T4 ON T4.ItemCode = T2.ItemCode " +
                        "LEFT JOIN (SELECT ITEMCODE, SUM(OnHand) as 'Existencia' FROM " + sociedad + ".dbo.OITW where WhsCode NOT IN ('1204', '1208', '1210', '1214', '1216', '1218', '1304', '1308', '1310', '1314', '1316', '2304', '1318', '2308', '2310', '2314', '2316', '2318', '1704', '1708', '1710', '1714', '1716', '1718', '1804', '1808', '1810', '1814', '1816', '1818', '1504', '1508', '1510', '1514', '1516', '1518', '1404', '1408', '1410', '1414', '1416', '1418', '1213', '1513', '1813', '1413', '2313' ,'1411', '1211', '1311', '1511', '1811', '2311', '1106', '1206', '1306', '1406', '1506', '1606', '1706', '1806', '1906', '2006', '2106', '2206', '2306') GROUP BY ItemCode) T10 ON T10.ItemCode = T2.ItemCode " +
                        /*
                        "LEFT JOIN (SELECT ItemCode, SUM(OnHand) AS 'Onhand' FROM " + sociedad + ".dbo.OITW T4 " +
                        "WHERE OnHand>0 AND (WhsCode NOT LIKE '%04' AND WhsCode NOT LIKE '%07' AND WhsCode NOT LIKE '%17' " +
                        "AND WhsCode NOT LIKE '%06' AND WhsCode NOT LIKE '%08' AND WhsCode NOT LIKE '%10' AND WhsCode NOT LIKE '%14' " +
                        "AND WhsCode NOT LIKE '%13' AND WhsCode NOT LIKE '%19' AND WhsCode NOT LIKE '%20' AND WhsCode NOT LIKE '%16' " +
                        "AND WhsCode NOT LIKE '%11') GROUP BY ItemCode )T44 ON T44.ItemCode = T8.ItemCode  " +
                        */

                        "LEFT JOIN ( " +
                            "SELECT T56.ITEMCODE, SUM(T56.Consumo) as 'Consumo' FROM( " +
                                "SELECT IGE1.ItemCode, (Quantity) as 'Consumo' FROM TSSL_NATURASOL.dbo.OIGE " +
                                            "JOIN TSSL_NATURASOL.dbo.IGE1 ON IGE1.DocEntry = oIGE.DocEntry " +
                                            "WHERE(OIGE.DocDate) >= (CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND ige1.BaseType = 202 " +
                                                 "UNION ALL " +
                                                 "SELECT IGE1.ItemCode, (Quantity) as 'Consumo' FROM TSSL_MIELMEX.dbo.OIGE " +
                                            "JOIN TSSL_MIELMEX.dbo.IGE1 ON IGE1.DocEntry = oIGE.DocEntry " +
                                            "WHERE(OIGE.DocDate) >= (CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND ige1.BaseType = 202 " +
                                                 "UNION ALL " +
                                                 "SELECT IGN1.ItemCode, -(Quantity) as 'Consumo' FROM TSSL_NATURASOL.dbo.OIGN " +
                                            "JOIN TSSL_NATURASOL.dbo.IGN1 ON IGN1.DocEntry = OIGN.DocEntry " +
                                            "WHERE(OIGN.DocDate) >= (CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND IGN1.BaseType = 202 " +
                                                 "UNION ALL " +
                                                 "SELECT IGN1.ItemCode, -(Quantity) as 'Consumo' FROM TSSL_MIELMEX.dbo.OIGN " +
                                            "JOIN TSSL_MIELMEX.dbo.IGN1 ON IGN1.DocEntry = OIGN.DocEntry " +
                                            "WHERE(OIGN.DocDate) >= (CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND IGN1.BaseType = 202 " +
                                                 ")T56 GROUP BY T56.ItemCode) T7 ON T7.ItemCode = T2.ItemCode " +
                        "WHERE T0.ObjType = 22  AND T0.docentry = @docentry ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docentry", docentry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                OcrCode = datareader["OcrCode"].ToString(),
                                OcrCode3 = datareader["OcrCode3"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = datareader["DocDate"] == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(datareader["DocDate"].ToString()).ToShortDateString(),
                                DocType = datareader["DocType"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),
                                DocRate = datareader["DocRate"].ToString(),
                                VatSum = datareader["VatSum"].ToString(),
                                VatSumFC = datareader["VatSumFC"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                OnHand = datareader["Existencia"].ToString(),
                                Consumo = datareader["Consumo"].ToString(),

                                ItemCode = datareader["ItemCode"].ToString(),
                                Dscription = datareader["Dscription"].ToString(),
                                AcctCode = datareader["AcctCode"].ToString(),
                                AcctName = datareader["AcctName"].ToString(),
                                U_CantidadServicio = datareader["U_CantidadServicio"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                                LineTotal = datareader["LineTotal"].ToString(),
                                TotalFrgn = datareader["TotalFrgn"].ToString(),
                                TaxCode = datareader["TaxCode"].ToString(),

                                ShipDate = datareader["ShipDate"] == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(datareader["ShipDate"].ToString()).ToShortDateString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                SlpName = datareader["slpname"].ToString(),
                                Email = datareader["Email"].ToString(),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> listaBorradoresCalculo(string sociedad, string itemcode, string fecha, string docentry)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT SUM(T7.Quantity) as quantity from( " +
                                        "select SUM(openqty) AS Quantity " +
                                        "from   " + sociedad + ".dbo.POR1 where ItemCode = @itemcode and ShipDate <= convert(datetime, @fecha, 103)" +
                                        "group by ItemCode " +
                                        "union all " +
                                        "select SUM(Quantity) AS Quantity " +
                                        "from   " + sociedad + ".dbo.DRF1 where ItemCode = @itemcode and ShipDate <= convert(datetime, @fecha, 103) AND DocEntry = @docentry " +
                                        "group by ItemCode) T7 ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docentry", docentry);
                    comANDo.Parameters.AddWithValue("@itemcode", itemcode);
                    comANDo.Parameters.AddWithValue("@fecha", fecha);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                Inventario1 = datareader["quantity"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> listaBorradoresCalculoOC(string sociedad, string itemcode, string fecha)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT SUM(T7.Quantity) as quantity from( " +
                                        "select SUM(openqty) AS Quantity " +
                                        "from   " + sociedad + ".dbo.POR1 where ItemCode = @itemcode and ShipDate <= convert(datetime, @fecha, 103) " +
                                        "group by ItemCode " +
                                        ") T7 ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@itemcode", itemcode);
                    comANDo.Parameters.AddWithValue("@fecha", fecha);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                Inventario2 = datareader["quantity"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> semanasExistenciaLiberacionOC(string sociedad, string docEntry)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select T8.DocEntry, DocNum, T8.DocDate, DocType, T8.CardCode, CardName, T8.ItemCode, t8.cantidad, T4.OnHAND, t7.Consumo, (OnHand/Consumo)*12 as 'Inventario1', ((OnHand+cantidad)/Consumo)*12  as 'Inventario2'  from (SELECT T0.DocEntry, DocNum, T0.DocDate, DocType, T0.CardCode, CardName, T2.ItemCode, sum(Quantity) as 'cantidad' " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "JOIN " + sociedad + ".dbo.oslp T1 ON T1.SlpCode = T0.SlpCode " +
                        "JOIN " + sociedad + ".dbo.drf1 T2 ON T2.DocEntry = T0.DocEntry " +
                        "JOIN " + sociedad + ".dbo.oact T3 ON T2.AcctCode = T3.AcctCode " +
                        "WHERE T0.ObjType = 22 AND T0.DocEntry = @docentry " +
                        "group by T0.DocEntry, DocNum, T0.DocDate, DocType, T0.CardCode, CardName, T2.ItemCode, Quantity)T8 " +
                        "LEFT JOIN (SELECT ItemCode, SUM(OnHand) AS 'Onhand' FROM " + sociedad + ".dbo.OITW T4 WHERE OnHand>0 AND (WhsCode NOT LIKE '%04' AND WhsCode NOT LIKE '%07' AND WhsCode NOT LIKE '%17' AND WhsCode NOT LIKE '%06' AND WhsCode NOT LIKE '%08' AND WhsCode NOT LIKE '%10' AND WhsCode NOT LIKE '%14' AND WhsCode NOT LIKE '%13' AND WhsCode NOT LIKE '%19' AND WhsCode NOT LIKE '%20' AND WhsCode NOT LIKE '%16' AND WhsCode NOT LIKE '%11') GROUP BY ItemCode )T44 ON T44.ItemCode = T8.ItemCode  " +
                        "LEFT JOIN(SELECT IGE1.ItemCode, sum(Quantity) as 'Consumo' FROM " + sociedad + ".dbo.OIGE " +
                                            "JOIN " + sociedad + ".dbo.IGE1 ON IGE1.DocEntry = oIGE.DocEntry " +
                                            "WHERE (OIGE.DocDate) >= month(CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND ige1.BaseType = 202 " +
                                                 "GROUP BY ItemCode) T7 ON T7.ItemCode = T8.ItemCode ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docentry", docEntry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {

                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),

                                OnHand = datareader["OnHAND"].ToString(),
                                Quantity = datareader["cantidad"].ToString(),
                                Consumo = datareader["Consumo"].ToString(),

                                ItemCode = datareader["ItemCode"].ToString(),

                                Inventario1 = datareader["Inventario1"].ToString(),
                                Inventario2 = datareader["Inventario2"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> listaBorradoresLiberacionDetalleAnexos(string sociedad, string docentry)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct CONCAT(T4.trgtpath,'\\',T4.FileName,'.',T4.FileExt) as 'trgtpath' " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "JOIN " + sociedad + ".dbo.oslp T1 ON T1.SlpCode = T0.SlpCode " +
                        "JOIN " + sociedad + ".dbo.drf1 T2 ON T2.DocEntry = T0.DocEntry " +
                        "JOIN " + sociedad + ".dbo.oact T3 ON T2.AcctCode = T3.AcctCode " +
                        "JOIN " + sociedad + ".dbo.atc1 T4 ON T4.AbsEntry = T0.AtcEntry " +
                        "WHERE T0.ObjType = 22  AND T0.docentry = @docentry ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docentry", docentry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                trgtPath = datareader["trgtPath"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //S12-REQUERIDO
        public List<OrdenCompra> obtenerOCAbiertas(string sociedadVenta, string slpname)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT distinct T1.DocEntry, T1.cardcode, T1.cardname, T1.docdate, T1.docnum, T1.slpname, T1.docduedate, T1.doctotal, T1.doccur, T1.Pendiente, T1.Anticipo AS 'Anticipo',  T1.Doctype  " +
                                        "FROM(SELECT OPOR.DocEntry, OPOR.cardcode, OPOR.cardname, OPOR.docdate, OPOR.docnum, slpname, OPOR.docduedate, CASE WHEN OPOR.doccur = 'MXP' THEN OPOR.doctotal ELSE OPOR.DocTotalFC END AS doctotal, OPOR.doccur, CASE WHEN opor.DocType = 'I' THEN SUM(POR1.OpenQty) / SUM(POR1.Quantity) * 100 ELSE 100 - (CASE WHEN OPOR.doccur = 'MXP' THEN OPOR.PaidToDate / OPOR.DocTotal ELSE OPOR.PaidSys / OPOR.DocTotalFC END) * 100 END AS 'Pendiente', ODPO.DocNum AS 'Anticipo', ODPO.DocTotal AS 'TotalAnticipo', ODPO.VatSum, PCH9.DrawnSum, opch.canceled, CASE WHEN OPOR.DocType = 'I' THEN 'Articulo' ELSE 'Servicio' END AS Doctype " +
                                                          "FROM " + sociedadVenta + ".dbo.OPOR " +
                                                               "JOIN " + sociedadVenta + ".dbo.OSLP ON OSLP.SlpCode = OPOR.SlpCode " +
                                                  "JOIN " + sociedadVenta + ".dbo.POR1 ON POR1.DocEntry = OPOR.DocEntry " +
                                                  "LEFT JOIN " + sociedadVenta + ".dbo.DPO1 ON DPO1.BaseRef = OPOR.DocNum AND DPO1.BaseType = 22 AND DPO1.TargetType NOT IN('19') " +
                                                  "LEFT JOIN " + sociedadVenta + ".dbo.ODPO ON ODPO.DocEntry = DPO1.DocEntry " +
                                                  "LEFT JOIN " + sociedadVenta + ".dbo.PCH9 ON PCH9.BaseAbs = ODPO.DocEntry " +
                                                  "LEFT JOIN " + sociedadVenta + ".dbo.OPCH ON PCH9.DocEntry = OPCH.DocEntry " +
                                             "WHERE OPOR.docduedate < GETDATE() AND OPOR.DocStatus = 'O' " +
                                             "GROUP BY OPOR.DocEntry, OPOR.cardcode, OPOR.cardname, OPOR.docdate, OPOR.docnum, slpname, OPOR.docduedate, OPOR.doctotal, OPOR.DocTotalFC, OPOR.doccur, OPOR.PaidToDate, OPOR.PaidSys, opor.doctype, ODPO.DocNum, OPOR.DocType, ODPO.DocTotal, ODPO.VatSum, PCH9.DrawnSum, opch.canceled)T1 " +
                                        "WHERE(T1.CANCELED IS NULL OR T1.CANCELED = 'N') " +
                                        "AND T1.SlpName = @slpname " +
                                        "ORDER BY T1.docnum";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@slpname", slpname);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                SlpName = datareader["slpname"].ToString(),
                                TaxDate = datareader["docduedate"].ToString(),
                                DocTotal = datareader["doctotal"].ToString(),
                                DocCur = datareader["doccur"].ToString(),
                                Pendiente = datareader["Pendiente"].ToString(),
                                Anticipo = datareader["Anticipo"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //S11-REQUERIDO
        public List<OrdenCompra> obtenerBorradoresAbiertas(string sociedadVenta)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocEntry FROM " + sociedadVenta + ".dbo.ODRF WHERE DocStatus='O' and DATEDIFF(day,docdate,getdate())>31";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER EL PEDIDO DE COMPRA INTERCOMPANIA CREADO A PARTIR DE LA OV
        public List<OrdenVenta> obtenerPedidoCompraIntercompania(OrdenCompra ordenCompra, string sociedadVenta)
        {
            List<OrdenVenta> listaDocumentos = new List<OrdenVenta>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadVenta + ".dbo.ORDR " +
                        "WHERE U_INT_DOCRE = @U_INT_DOCRE AND U_INT_SOREL=@U_INT_SOREL";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@U_INT_DOCRE", ordenCompra.DocNum);
                    comANDo.Parameters.AddWithValue("@U_INT_SOREL", ordenCompra.Sociedad == "TestMielmex" ? "MM" : (ordenCompra.Sociedad == "TestNaturasol" ? "NA" : (ordenCompra.Sociedad == "TestEvi") ? "EV" : "NO"));
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
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

        public List<OrdenCompra> obtenerOCLiberar(string sociedadVenta, string slpname)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT OPOR.DocEntry, OPOR.cardcode, OPOR.cardname, OPOR.docdate, OPOR.docnum, slpname, OPOR.docduedate, CASE WHEN OPOR.doccur='MXP' THEN OPOR.doctotal ELSE OPOR.DocTotalFC end as doctotal, OPOR.doccur, " +
                        "case when opor.DocType = 'I' then sum(POR1.OpenQty)/ sum(POR1.Quantity) * 100 else 100 - (CASE WHEN OPOR.doccur = 'MXP' THEN OPOR.PaidToDate / OPOR.DocTotal else OPOR.PaidSys / OPOR.DocTotalFC END)*100 end as 'Pendiente', ODPO.DocNum as 'Anticipo', CASE WHEN OPOR.DocType = 'I' THEN 'Articulo' ELSE 'Servicio' END as Doctype " +
                                        "FROM OPOR " +
                                             "JOIN OSLP ON OSLP.SlpCode = OPOR.SlpCode " +
                                             "JOIN POR1 ON POR1.DocEntry = OPOR.DocEntry " +
                                             "LEFT JOIN  DPO1 ON DPO1.BaseRef = OPOR.DocNum AND DPO1.BaseType = 22 " +
                                             "LEFT JOIN  ODPO ON ODPO.DocEntry = DPO1.DocEntry " +
                                        "WHERE OPOR.U_Sol_Aut = 'Y1' " +
                                        "GROUP BY OPOR.DocEntry, OPOR.cardcode, OPOR.cardname, OPOR.docdate, OPOR.docnum, slpname, OPOR.docduedate, OPOR.doctotal, OPOR.DocTotalFC, OPOR.doccur, OPOR.PaidToDate, OPOR.PaidSys,  opor.doctype, ODPO.DocNum, OPOR.DocType " +
                                        "ORDER BY OPOR.docnum";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@slpname", slpname);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                SlpName = datareader["slpname"].ToString(),
                                TaxDate = datareader["docduedate"].ToString(),
                                DocTotal = datareader["doctotal"].ToString(),
                                DocCur = datareader["doccur"].ToString(),
                                Pendiente = datareader["Pendiente"].ToString(),
                                Anticipo = datareader["Anticipo"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LOS DETALLES DE LAS OC QUE SON INTERCOMPANIA
        public List<OrdenCompraDetalle> obtenerOrdenesCompraDetalleIntercompania(string sociedad, OrdenCompra orden)
        {
            List<OrdenCompraDetalle> listaDocumentos = new List<OrdenCompraDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "POR1.ItemCode, " +
                        "POR1.Quantity, " +
                        "POR1.TaxCode, " +
                        "POR1.LineNum, " +
                        "POR1.Price, " +
                        "POR1.Currency, " +
                        "POR1.UomCode " +
                        "FROM " + sociedad + ".dbo.POR1 " +
                        "WHERE DocEntry = @DocEntry";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@DocEntry", orden.DocEntry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompraDetalle()
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
                listaDocumentos = new List<OrdenCompraDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER LA OC INTERCOMPANIA DE LA QUE SE HIZO ORDEN DE VENTA
        public List<OrdenCompraDetalle> obtenerOrdenesCompraEntregaDetalleIntercompania(string sociedad, string DocNum)
        {
            List<OrdenCompraDetalle> listaDocumentos = new List<OrdenCompraDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "OPOR.CardCode, " +
                        "POR1.DocEntry, " +
                        "POR1.ItemCode, " +
                        "POR1.Quantity, " +
                        "POR1.TaxCode, " +
                        "POR1.LineNum, " +
                        "POR1.Price, " +
                        "POR1.Currency " +
                        "FROM " + sociedad + ".dbo.POR1 " +
                        "JOIN " + sociedad + ".dbo.OPOR ON OPOR.DocEntry=POR1.DocEntry " +
                        "WHERE OPOR.DocNum = @DocNum";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@DocNum", DocNum);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompraDetalle()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                TaxCode = datareader["TaxCode"].ToString(),
                                LineNum = datareader["LineNum"].ToString(),
                                Price = datareader["Price"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompraDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA OBTENER TODOS LAS OC
        public DataSet obtenerOrdenesCompra(string sociedad, int wddcode)
        {
            DataSet dataset = new DataSet();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT " +
                        "'" + sociedad + "' AS Sociedad, " +
                        "OUSR.U_NAME AS Usuario, " +
                        "OSLP.SlpName AS Comprador, " +
                        "ODRF.DocNum AS Documento, " +
                        "ODRF.DocDate AS 'Fecha creacion', " +
                        "ODRF.CardCode AS 'Codigo proveedor', " +
                        "ODRF.CardName AS Proveedor, " +
                        "CASE WHEN ODRF.DocCur ='MXP' THEN ODRF.DocTotal ELSE ODRF.DocTotalFC END AS 'Total', " +
                        "ODRF.DocCur AS Moneda, " +
                        "ODRF.Comments AS Comentarios, " +
                        "WDD1.WddCode, " +
                        "ODRF.DocEntry " +
                        "FROM " + sociedad + ".dbo.ODRF " +
                        "JOIN " + sociedad + ".dbo.OWDD ON ODRF.DocEntry = OWDD.DocEntry " +
                        "JOIN " + sociedad + ".dbo.WDD1 ON WDD1.WddCode = OWDD.WddCode " +
                        "JOIN " + sociedad + ".dbo.OUSR ON ODRF.UserSign2 = OUSR.USERID " +
                        "LEFT JOIN " + sociedad + ".dbo.OSLP ON OSLP.SlpCode = ODRF.SlpCode " +
                        "WHERE ODRF.ObjType = 22 " +
                        "AND WDD1.WddCode = @wddcode " +
                        "AND ODRF.DocStatus != 'C' " +
                        "AND DATEADD(MONTH, 12, ODRF.DocDueDate)>=GETDATE() " +
                        "AND WDD1.UserID = 96 " +
                        "AND WDD1.Status = 'W' " +
                        "ORDER BY ODRF.DocNum DESC;";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@wddcode", wddcode);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comANDo;
                    comANDo.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return dataset;
        }

        //METODO PARA OBTENER TODOS LAS OC SEGUN SOCIEDAD
        public DataSet obtenerOrdenesCompra(string sociedad, string comprador)
        {
            DataSet dataset = new DataSet();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT " +
                        "'" + sociedad + "' AS Sociedad, " +
                        "OUSR.U_NAME AS Usuario,  " +
                        "OSLP.SlpName AS Comprador, " +
                        "ODRF.DocNum AS Documento, " +
                        "ODRF.DocDate AS 'Fecha creacion', " +
                        "ODRF.CardCode AS 'Codigo proveedor', " +
                        "ODRF.CardName AS Proveedor, " +
                        "CASE WHEN ODRF.DocCur ='MXP' THEN ODRF.DocTotal ELSE ODRF.DocTotalFC END AS 'Total', " +
                        "ODRF.DocCur AS Moneda, " +
                        "ODRF.Comments AS Comentarios, " +
                        "WDD1.WddCode, " +
                        "ODRF.DocEntry " +
                        "FROM " + sociedad + ".dbo.ODRF " +
                        "JOIN " + sociedad + ".dbo.OWDD ON ODRF.DocEntry = OWDD.DocEntry " +
                        "JOIN " + sociedad + ".dbo.WDD1 ON WDD1.WddCode = OWDD.WddCode " +
                        "JOIN " + sociedad + ".dbo.OUSR ON ODRF.UserSign2 = OUSR.USERID " +
                        "LEFT JOIN " + sociedad + ".dbo.OSLP ON OSLP.SlpCode = ODRF.SlpCode " +
                        "WHERE ODRF.ObjType = 22 " +
                        "AND OSLP.SlpName = '" + comprador + "' " +
                        "AND ODRF.DocStatus != 'C' " +
                        "AND DATEADD(MONTH, 12, ODRF.DocDueDate)>=GETDATE() " +
                        "AND WDD1.UserID = 96 " +
                        "AND WDD1.Status = 'W' " +
                        "ORDER BY ODRF.DocNum DESC;";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comANDo;
                    comANDo.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return dataset;
        }

        //CONSULTA QUE OBTIENE LOS USUARIOS DE COMPRAS
        public DataSet obtenerUsuariosCompras(string sociedad)
        {
            DataSet dataset = new DataSet();
            using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                try
                {
                    string consulta = "SELECT DISTINCT OSLP.SlpName  " +
                        "FROM " + sociedad + ".dbo.ODRF " +
                        "JOIN " + sociedad + ".dbo.OWDD ON ODRF.DocEntry= OWDD.DocEntry " +
                        "JOIN " + sociedad + ".dbo.WDD1 ON WDD1.WddCode= OWDD.WddCode " +
                        "LEFT JOIN " + sociedad + ".dbo.OSLP ON OSLP.SlpCode = ODRF.SlpCode " +
                        "WHERE ODRF.ObjType=22 " +
                        "AND DATEADD(MONTH, 12, ODRF.DocDueDate)>=GETDATE() " +
                        "AND WDD1.UserID=96 " +
                        "ORDER BY OSLP.SlpName ASC;";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comANDo;
                    comANDo.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    conexionDB.Close();
                }

            return dataset;
        }

        //METODO PARA ACTUALIZAR FOLIO DE UNA SOLICITUD DE AUTORIZACION DE OC
        public int actualizarFolioAutorizacionOC()
        {
            int resultado = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE " +
                        "foliador set numeroFolio = ((SELECT numeroFolio WHERE tipoMovimiento=2) +1) " +
                        "WHERE tipoMovimiento=2;";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);

                    comANDo.CommandType = CommandType.Text;
                    comANDo.ExecuteNonQuery();
                    resultado = 1;
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }

            return resultado;
        }

        //METODO PARA LISTAR EL FOLIO
        public int obtenerFolioAutorizacionOC(int tipoMovimiento)
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

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@tipoMovimiento", tipoMovimiento);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
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

        //METODO PARA GUARDAR UNA AUTORIZACION/RECHAZO EN LA TABLA INTERMEDIA
        public int guardarSolicitudAutorizacionOC(OrdenCompra borradorOrdenCompra)
        {
            int id = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[liberacionOCSAP]" +
                        "([numeroDocumento]" +
                        ",[wddCode]" +
                        ",[sociedad]" +
                        ",[estatus]" +
                        ",[fechaRegistro]" +
                        ",[folioSolicitud]" +
                        ",[total]" +
                        ",[estatusAutorizacion]" +
                        ",[motivoRechazo]" +
                        ",[moneda]) VALUES" +
                        "(@numeroDocumento," +
                        "@wddCode," +
                        "@sociedad," +
                        "@estatus," +
                        "@fechaRegistro," +
                        "@folioSolicitud," +
                        "@total," +
                        "@estatusAutorizacion," +
                        "@motivoRechazo," +
                        "@moneda);";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@numeroDocumento", borradorOrdenCompra.DocNum);
                    comANDo.Parameters.AddWithValue("@wddCode", borradorOrdenCompra.WddCode);
                    comANDo.Parameters.AddWithValue("@estatus", "");
                    comANDo.Parameters.AddWithValue("@sociedad", borradorOrdenCompra.Sociedad);
                    comANDo.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
                    comANDo.Parameters.AddWithValue("@folioSolicitud", 0);
                    comANDo.Parameters.AddWithValue("@total", "");
                    comANDo.Parameters.AddWithValue("@estatusAutorizacion", "");
                    comANDo.Parameters.AddWithValue("@motivoRechazo", "");
                    comANDo.Parameters.AddWithValue("@moneda", "");

                    comANDo.CommandType = CommandType.Text;

                    id = Convert.ToInt32(comANDo.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                id = 0;
            }

            return id;
        }

        //METODO PARA ACTUALIZAR LA AUTORIZACION EN LA TABLA INTERMEDIA
        public int actualizarSolicitudAutorizacionOC(string wddcode, string estatus, string estatusArea)
        {
            int id = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [DesarrolloWeb].[dbo].[liberacionOCSAP] " +
                        "SET " +
                        "estatus = @estatus, " +
                        "estatusAutorizaciON = @estatusAutorizaciON " +
                        "WHERE wddCode= @wddCode ";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@wddCode", wddcode);
                    comANDo.Parameters.AddWithValue("@estatus", estatusArea);
                    comANDo.Parameters.AddWithValue("@estatusAutorizacion", estatus);
                    comANDo.CommandType = CommandType.Text;

                    id = Convert.ToInt32(comANDo.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                id = 0;
            }

            return id;
        }

        //METODO PARA OBTENER TODOS LAS OC DQUE LES FALTA EL CHECK DE AUTORIZADO
        public List<OrdenCompra> obtenerDocumentosAutorizarPendiente(string sociedad)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocEntry FROM " + sociedad + ".dbo.OPOR WHERE Confirmed <> 'Y' AND DocStatus='O' ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocEntry = datareader["DocEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }

        //METODO PARA ACTUALIZAR UN ACCESORIO
        public int actualizarOCAutorizada(string sociedad, string wddCode)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE " + sociedad + ".[dbo].[ODRF] " +
                    "SET [U_LiberacionOC] = 0 " +
                    "WHERE DocEntry = (SELECT ODRF.docentry FROM " + sociedad + ".[dbo].ODRF " +
                    "JOIN " + sociedad + ".[dbo].OWDD ON OWDD.DraftEntry = ODRF.docentry " +
                    "WHERE owdd.WddCode = @wddCode)";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@wddCode", wddCode);

                    comANDo.CommandType = CommandType.Text;
                    comANDo.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }

            return 1;
        }

        //METODO PARA OBTENER EL DOCENTRY DE LA UM EN LA SOCIEDAD DE LA ORDEN DE VENTA BASADO EN LA DESCRIPCION
        public string obtenerUM(string uM, string sociedadVenta)
        {
            string unidadMedida = "-1";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT uomentry " +
                        "FROM " + sociedadVenta + ".dbo.OUOM " +
                        "WHERE UomCode = @UomCode ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@UomCode", uM);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            unidadMedida = Convert.ToString(datareader["uomentry"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                unidadMedida = "-1";
            }

            return unidadMedida;
        }

        //METODO PARA OBTENER EL PEDIDO DE VENTA INTERCOMPANIA CREADO A PARTIR DE LA OC
        public List<OrdenCompra> obtenerOCIntercompania(OrdenVenta ordenCompra, string sociedadVenta)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadVenta + ".dbo.OPOR " +
                        "WHERE NumAtCard = @U_INT_DOCRE";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@U_INT_DOCRE", (ordenCompra.Sociedad.Contains("MIELMEX") ? "MM" : ordenCompra.Sociedad.Contains("NATURASOL") ? "NA" : ordenCompra.Sociedad.Contains("NOVAL") ? "NO" : "EV") + ordenCompra.DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
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
                listaDocumentos = new List<OrdenCompra>();
            }

            return listaDocumentos;
        }
    }
}
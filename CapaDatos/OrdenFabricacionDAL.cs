using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class OrdenFabricacionDAL
    {
        public List<OrdenFabricacion> buscarOFCreada(string sociedad, string itemcode, string whscode, string quantity)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select docentry from " + sociedad + ".dbo.owor where Warehouse=@whscode and itemcode=@itemcode and PlannedQty=@quantity order by docentry desc; ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@whscode", whscode);
                    comando.Parameters.AddWithValue("@itemcode", itemcode);
                    comando.Parameters.AddWithValue("@quantity", quantity);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocEntry = datareader["docentry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> buscarInventarioOF(string sociedad, string itemCode, string whsCode)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DISTINCT " +
                                            "T0.ItemCode, " +
                                            "T4.WhsCode, " +
                                            "T0.DistNumber, " +
                                            "T4.AbsEntry, " +
                                            "T3.OnHandQty " +
                                            "FROM " +
                                            sociedad + ".dbo.OBTN T0 INNER JOIN " +
                                            sociedad + ".dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber INNER JOIN " +
                                            sociedad + ".dbo.OITM T2 ON T0.ItemCode = T2.ItemCode INNER JOIN " +
                                            sociedad + ".dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs INNER JOIN " +
                                            sociedad + ".dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry INNER JOIN " +
                                            sociedad + ".dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod INNER JOIN " +
                                            sociedad + ".dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode INNER JOIN " +
                                            sociedad + ".dbo.OLCT T7 ON T6.Location = T7.Code INNER JOIN " +
                                            sociedad + ".dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
                                            "WHERE " +
                                            "T6.Inactive = 'N' AND " +
                                            "T1.Quantity > 0 AND " +
                                            "T3.OnHandQty > 0 " +
                                            "AND T8.ItemCode=@itemcode AND T8.WhsCode=@whscode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", itemCode);
                    comando.Parameters.AddWithValue("@whscode", whsCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                ReleasedQty = datareader["OnHandQty"].ToString(),
                                BinCode = datareader["AbsEntry"].ToString(),
                                DistNumber = datareader["DistNumber"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> buscarTSOF(string sociedad)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select OWTR.DocNum,  WTR1.LineNum as'L2', WTR1.ItemCode, WTR1.Quantity, WTR1.WhsCode, WOR1.DocEntry, WOR1.LineNum, " +
                                        "WOR1.PlannedQty, WOR1.IssuedQty, OWOR.Status " +
                                        "from " + sociedad + ".dbo.wtr1 " +
                                        "LEFT JOIN " + sociedad + ".dbo.OWTr ON OWtr.DocEntry = wtr1.DocEntry " +
                                        "left join " + sociedad + ".dbo.WTQ1 on WTR1.BaseEntry = WTQ1.DocEntry and WTR1.BaseLine = WTQ1.LineNum " +
                                        "LEFT JOIN " + sociedad + ".dbo.OWTQ ON OWTQ.DocEntry = WTQ1.DocEntry " +
                                            "LEFT JOIN " + sociedad + ".dbo.WTQ21 ON WTQ1.DocEntry = WTQ21.DocEntry " +
                                            "LEFT JOIN " + sociedad + ".dbo.OWOR ON OWOR.DocNum = WTQ21.RefDocNum " +
                                            "LEFT JOIN " + sociedad + ".dbo.WOR1 ON WOR1.DocEntry = OWOR.DocEntry AND WOR1.ItemCode = WTQ1.ItemCode " +
                                        "where wtr1.DocDate >= '2024-12-15' " +
                                        "and wtr1.WhsCode like '%06' and wtr1.whscode not in ('1406','2306') and WTR1.U_CantidadServicio NOT IN (1,2) AND WOR1.lineNum IS NOT NULL " +
                                        "ORDER BY WTR1.DocEntry DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                ItemCode = datareader["ItemCode"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                PlannedQty = datareader["Quantity"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Linea = datareader["LineNum"].ToString(),
                                Linea2 = datareader["L2"].ToString(),
                                IssuedQty = datareader["IssuedQty"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        //S17-REQUERIDO
        public List<OrdenFabricacion> buscarConsumosROVEAutorizados()
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM [W17 - ConsumosROVE] ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                U_LINEA = datareader["idConsumoROVE"].ToString(),
                                DistNumber = datareader["BatchNumber"].ToString(),
                                WhsCode = datareader["Warehouse"].ToString(),
                                IssuedQty = datareader["Quantity"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Linea = datareader["LineNum"].ToString(),
                                BinCode = datareader["AbsEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        //S17-REQUERIDO
        public List<OrdenFabricacion> buscarGeneracionesROVEAutorizados()
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM  [W18 - LiberacionesROVE]  ORDER BY idLiberacionROVE DESC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                U_LINEA = datareader["idLiberacionROVE"].ToString(),
                                DistNumber = datareader["BatchNumber"].ToString(),
                                WhsCode = datareader["Warehouse"].ToString(),
                                IssuedQty = datareader["Quantity"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        //S19-REQUERIDO
        public List<OrdenFabricacion> obtenerLiberacionesCosto()
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.[W19 - CostoGeneraciones]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocNum = datareader["BaseRef"].ToString(),
                                ItemCode = datareader["itemcode"].ToString(),
                                ItemName = datareader["Dscription"].ToString(),
                                ReleasedQty = datareader["Quantity"].ToString(),
                                CostoOF = datareader["Price"].ToString(),
                                Series = datareader["Series"].ToString(),
                                Linea = datareader["LineNum"].ToString(),
                                CostoLM = datareader["U_ListaPesos"].ToString(),
                                Desviacion = datareader["U_Apicultor"].ToString(),
                                FechaLiberacion = datareader["DocDate"].ToString(),
                                UM = datareader["unitMsr"].ToString(),
                                DocEntry = datareader["docentry"].ToString(),
                                Sociedad = datareader["Sociedad"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }


        public List<OrdenFabricacion> buscarUbicacionDestino(string sociedad, string whscode)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select top 1 absentry from " + sociedad + ".dbo.obin where WhsCode=@whscode order by AbsEntry asc; ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@whscode", whscode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                BinCode = datareader["AbsEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> buscarUbicacionOrigen(string whscode, string distnumber, string itemcode)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT TOP 1 T0.BinAbs FROM  TSSL_NATURASOL.DBO.obbq T0" +
                        " JOIN TSSL_NATURASOL.DBO.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs" +
                        " join TSSL_NATURASOL.DBO.OITM t6 on t6.ItemCode = T0.ItemCode" +
                        " WHERE T0.ItemCode = @itemcode and T0.WhsCode = @whscode and t1.DistNumber=@distnumber ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", itemcode);
                    comando.Parameters.AddWithValue("@WhsCode", whscode);
                    comando.Parameters.AddWithValue("@DistNumber", distnumber);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                BinCode = datareader["BinAbs"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> buscarMaterialMayoreo(string sociedad)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DISTINCT " +
                                            "T0.ItemCode, " +
                                            "T4.WhsCode, " +
                                            "T0.DistNumber, " +
                                            "T4.AbsEntry, " +
                                            "T2.ValidComm, " +
                                            "T3.OnHandQty " +
                                            "FROM " +
                                            sociedad + ".dbo.OBTN T0 INNER JOIN " +
                                            sociedad + ".dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber INNER JOIN " +
                                            sociedad + ".dbo.OITM T2 ON T0.ItemCode = T2.ItemCode INNER JOIN " +
                                            sociedad + ".dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs INNER JOIN " +
                                            sociedad + ".dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry INNER JOIN " +
                                            sociedad + ".dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod INNER JOIN " +
                                            sociedad + ".dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode INNER JOIN " +
                                            sociedad + ".dbo.OLCT T7 ON T6.Location = T7.Code INNER JOIN " +
                                            sociedad + ".dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
                                            "WHERE " +
                                            "T6.Inactive = 'N' AND " +
                                            "T1.Quantity > 0 AND " +
                                            "T3.OnHandQty > 0 " +
                                            "AND T8.ItemCode in ('2001-081-006', '2001-081-019', '2001-081-017', '1501-100-099', '1501-050-056', '2001-081-016', '2001-082-003', '1501-100-086', '1501-100-093', '1501-100-097', '1501-100-071', '2001-083-041', " +
                                            "'2001-100-001', '2001-085-008', '2001-150-002', '2001-150-001', '1501-100-015', '1501-100-053', '2001-086-003', '2001-085-004', '2001-085-002','2001-082-007','1501-050-057', '2001-085-005','2001-085-003','2001-082-006', '1501-100-047','2001-081-024','1501-070-009', '1501-100-045','2001-085-009') and T8.WhsCode IN('1320','1720','1820','1220') ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                PlannedQty = datareader["OnHandQty"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                BinCode = datareader["AbsEntry"].ToString(),
                                ValidComm = datareader["ValidComm"].ToString(),
                                DistNumber = datareader["DistNumber"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> buscarOFMasiva(string sociedad)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * from " + sociedad + ".dbo.[@9_OF_DIARIA] WHERE U_NumeroOF is null AND U_Articulo is not null ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                Code = datareader["Code"].ToString(),
                                ItemCode = datareader["U_Articulo"].ToString(),
                                PlannedQty = datareader["U_Cantidad"].ToString(),
                                DueDate = datareader["U_FechaInicio"].ToString(),
                                WhsCode = datareader["U_Almacen"].ToString(),
                                Series = datareader["U_Serie"].ToString(),
                                DocNum = datareader["U_NumeroOF"].ToString(),
                                Linea = datareader["U_Planta"].ToString(),
                                Linea2 = datareader["U_CentroCosto"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public void actualizarTablaOFMasiva(string code, string docNum)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE TSSL_NATURASOL.[dbo].[@9_OF_DIARIA] " +
                    "SET [U_NumeroOF] = (SELECT DocNum FROM TSSL_NATURASOL.[dbo].OWOR WHERE DocEntry=@numeroOF) " +
                    "WHERE code = @code";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@code", code);
                    comANDo.Parameters.AddWithValue("@numeroOF", docNum);

                    comANDo.CommandType = CommandType.Text;
                    comANDo.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void actualizarTransferencia(string sociedad, string docNum, string linea2)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE " + sociedad + ".[dbo].[WTR1] " +
                    "SET [U_CantidadServicio] = 2 " +
                    "WHERE DocEntry = (SELECT OWTR.docentry FROM " + sociedad + ".[dbo].OWTR " +
                    "WHERE OWTR.DocNum = @docNum) AND linenum=@linenum";

                    conexionDB.Open();
                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docNum", docNum);
                    comANDo.Parameters.AddWithValue("@linenum", linea2);

                    comANDo.CommandType = CommandType.Text;
                    comANDo.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<OrdenFabricacion> buscarOFAbiertas(string sociedad)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM " + sociedad + ".dbo.OWOR " +
                      "WHERE Status NOT IN('L','C') AND((U_Fecha_Revision is null) OR " +
                      "(FORMAT(DATEADD(MINUTE, 1, CAST(U_Fecha_Revision as datetime)), 'yyyy-MM-dd HH:mm:ss') < CONCAT(FORMAT(UpdateDate, 'yyyy-MM-dd'), ' ', case when LEN(UpdateTS) = 5 then CONCAT('0', LEFT(UpdateTS, 1)) ELSE LEFT(UpdateTS, 2) END, ':', LEFT(RIGHT(UpdateTS, 4), 2), ':', RIGHT(UpdateTS, 2)))) " +
                      "ORDER BY DocEntry ASC";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                U_Planificado_Original = datareader["U_Planificado_Original"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<ListaMateriales> listaMaterialesOF(string sociedad, string ItemCode)
        {
            List<ListaMateriales> listaDocumentos = new List<ListaMateriales>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.Qauntity, T1.Code, ROUND(T1.Quantity / T0.Qauntity, 4) AS Quantity, substring(T1.LineText, 1, 12) AS LineText FROM " + sociedad + ".dbo.ITT1 T1 " +
                        "JOIN " + sociedad + ".dbo.OITT T0 ON T0.Code = T1.Father " +
                        "WHERE T1.Father=@ItemCode order by VisOrder ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@ItemCode", ItemCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ListaMateriales()
                            {
                                Code = datareader["Code"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                LineText = datareader["LineText"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ListaMateriales>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> detalleOF(string sociedad, string DocEntry)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.ItemCode, T2.ItemName, T1.BaseQty, T1.U_BOM, T1.VisOrder FROM " + sociedad + ".dbo.WOR1 T1 " +
                        " JOIN " + sociedad + ".dbo.OITM T2 ON T2.ItemCode = T1.ItemCode WHERE T1.DocEntry=@DocEntry ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", DocEntry);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString(),
                                BaseQty = datareader["BaseQty"].ToString(),
                                U_BOM = datareader["U_BOM"].ToString(),
                                visOrder = datareader["VisOrder"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        //REVISADO
        public List<OrdenFabricacion> OFSinCierre()
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.OFSINCIERRE ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                createDate = datareader["PostDate"].ToString(),
                                DocNum = datareader["docnum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString(),
                                Sociedad = datareader["SOCIEDAD"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                DueDate = datareader["DueDate"].ToString(),
                                UOM = datareader["Uom"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        //S13-REQUERIDO
        public List<OrdenFabricacion> obtenerOFAbiertas(string sociedadVenta)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.DocEntry, E_Mail, DocNum, T2.ItemCode, T2.ItemName, PlannedQty, T0.createDate, DueDate " +
                        "FROM " + sociedadVenta + ".dbo.OWOR T0 " +
                        "JOIN " + sociedadVenta + ".dbo.OUSR T1 ON T0.UserSign=T1.USERID " +
                        "JOIN " + sociedadVenta + ".dbo.OITM T2 ON T2.ItemCode=T0.ItemCode WHERE duedate < GETDATE()-1 AND Status='P'";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                E_Mail = datareader["E_Mail"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                createDate = datareader["createDate"].ToString(),
                                DueDate = datareader["DueDate"].ToString(),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> buscarOFPartidasEliminadas(string sociedad, string docnum)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.[DocNum], T1.[ItemCode], T1.ItemName FROM " + sociedad + ".dbo.AWOR T0 " +
                        "INNER JOIN " + sociedad + ".dbo.AWO1 T1 ON T0.[DocEntry] = T1.[DocEntry] " +
                        "LEFT JOIN " + sociedad + ".dbo.WOR1 T2 ON T2.[DocEntry] = T1.Docentry and T2.[LineNum] = T1.[LineNum] " +
                        "LEFT JOIN " + sociedad + ".dbo.OWOR T3 ON T2.[DocEntry] = T3.[DocEntry] " +
                        "WHERE T2.[ItemCode] NOT IN (select T1.Itemcode FROM " + sociedad + ".dbo.AWO1 T1 WHERE T1.Docentry = T2.Docentry) " +
                        "AND T0.Docnum = @docnum AND T1.ItemCode IS NOT NULL " +
                        "GROUP BY T0.[DocNum], T1.[ItemCode], T1.ItemName";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", docnum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA GUARDAR UN CONSUMO
        public int guardarConsumoROVE(ConsumosROVE consumoROVE)
        {
            int idConsumoROVE = 0;
            try
            {
                if (consumoROVE.DocNum == "0")
                {
                    idConsumoROVE = -1;
                }
                else
                {
                    using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                    {
                        string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[consumosROVE] " +
                                           "([DocNum] " +
                                           ",[ItemCode] " +
                                           ",[BatchNumber] " +
                                           ",[Quantity] " +
                                           ",[Merma] " +
                                           ",[CausaMerma] " +
                                           ",[PNC] " +
                                           ",[usuarioCreacion] " +
                                           ",[supervisor] " +
                                           ",[operador] " +
                                           ",[linea] " +
                                           ",[Comentarios]) " +
                        "VALUES" +
                       "(@DocNum," +
                       "@ItemCode," +
                       "@BatchNumber," +
                       "@Quantity," +
                       "@Merma," +
                       "@CausaMerma," +
                       "@PNC," +
                       "@usuarioCreacion," +
                       "@supervisor," +
                       "@operador," +
                       "@linea," +
                       "@Comentarios);";

                        conexionDB.Open();
                        SqlCommand comando = new SqlCommand(consulta, conexionDB);
                        comando.Parameters.AddWithValue("@DocNum", consumoROVE.DocNum);
                        comando.Parameters.AddWithValue("@supervisor", consumoROVE.supervisor);
                        comando.Parameters.AddWithValue("@operador", consumoROVE.operador);
                        comando.Parameters.AddWithValue("@linea", consumoROVE.linea);
                        comando.Parameters.AddWithValue("@usuarioCreacion", consumoROVE.usuarioCreacion);

                        comando.Parameters.AddWithValue("@ItemCode", consumoROVE.ItemCode);
                        comando.Parameters.AddWithValue("@BatchNumber", consumoROVE.BatchNumber);
                        comando.Parameters.AddWithValue("@Quantity", consumoROVE.Quantity == null ? "" : consumoROVE.Quantity);
                        comando.Parameters.AddWithValue("@Merma", consumoROVE.Merma == null ? "" : consumoROVE.Merma);
                        comando.Parameters.AddWithValue("@CausaMerma", consumoROVE.CausaMerma == null ? "" : consumoROVE.CausaMerma);
                        comando.Parameters.AddWithValue("@PNC", consumoROVE.PNC == null ? "" : consumoROVE.PNC);
                        comando.Parameters.AddWithValue("@Comentarios", consumoROVE.Comentarios == null ? "" : consumoROVE.Comentarios);
                        comando.CommandType = CommandType.Text;

                        idConsumoROVE = Convert.ToInt32(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                idConsumoROVE = -1;
            }

            return idConsumoROVE;
        }

        //S17-REQUERIDO
        public int validarConsumoROVE(ConsumosROVE consumoROVE)
        {
            int idConsumoROVE = 0;
            try
            {

                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (consumoROVE.estatus == "4")
                    {
                        consulta = "UPDATE [DesarrolloWeb].[dbo].[consumosROVE] SET " +
                                       "[comentarios]= @comentarios, [estatus]= @estatus " +
                    "WHERE idConsumoROVE = @idConsumoROVE AND @estatus < 5;";
                    }
                    else
                    {
                        consulta = "UPDATE [DesarrolloWeb].[dbo].[consumosROVE] SET " +
                                       "[estatus]= @estatus ";
                        if (consumoROVE.estatus == "1")
                        {
                            consulta = consulta + ", [usuarioSupervisor]=@usuarioSupervisor";
                        }
                        else if (consumoROVE.estatus == "2")
                        {
                            consulta = consulta + ", [usuarioAnalista]=@usuarioAnalista";
                        }
                        else if (consumoROVE.estatus == "-1")
                        {
                            consulta = consulta + ", [usuarioRechazo]=@usuarioRechazo";
                        }
                        consulta = consulta + " WHERE idConsumoROVE = @idConsumoROVE AND @estatus < 5;";
                    }

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", consumoROVE.estatus);
                    comando.Parameters.AddWithValue("@comentarios", consumoROVE.Comentarios == null ? "" : consumoROVE.Comentarios);
                    comando.Parameters.AddWithValue("@idConsumoROVE", consumoROVE.idConsumoROVE);

                    if (consumoROVE.estatus == "1")
                    {
                        comando.Parameters.AddWithValue("@usuarioSupervisor", consumoROVE.usuarioSupervisor);
                    }
                    else if (consumoROVE.estatus == "2")
                    {
                        comando.Parameters.AddWithValue("@usuarioAnalista", consumoROVE.usuarioAnalista);
                    }
                    else if (consumoROVE.estatus == "-1")
                    {
                        comando.Parameters.AddWithValue("@usuarioRechazo", consumoROVE.usuarioRechazo);
                    }

                    comando.CommandType = CommandType.Text;

                    idConsumoROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idConsumoROVE = -1;
            }

            return idConsumoROVE;
        }

        //S17-REQUERIDO
        public int validarGeneracionROVE(LiberacionesROVE liberacionesROVE)
        {
            int idConsumoROVE = 0;
            try
            {

                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (liberacionesROVE.estatus == "4")
                    {
                        consulta = "UPDATE [DesarrolloWeb].[dbo].[liberacionesROVE] SET " +
                                       "[comentarios]= @comentarios, [estatus]= @estatus " +
                        "WHERE idLiberacionROVE = @idLiberacionROVE;";
                    }
                    else
                    {
                        consulta = "UPDATE [DesarrolloWeb].[dbo].[liberacionesROVE] SET " +
                                       "[estatus]= @estatus " +
                        "WHERE idLiberacionROVE = @idLiberacionROVE";
                    }


                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", liberacionesROVE.estatus);
                    comando.Parameters.AddWithValue("@comentarios", liberacionesROVE.Comentarios == null ? "" : liberacionesROVE.Comentarios);
                    comando.Parameters.AddWithValue("@idLiberacionROVE", liberacionesROVE.idLiberacionROVE);
                    comando.CommandType = CommandType.Text;

                    idConsumoROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idConsumoROVE = -1;
            }

            return idConsumoROVE;
        }


        public List<ConsumosROVE> confirmarExistenciaLote(ConsumosROVE consumoROVE)
        {
            List<ConsumosROVE> listaDocumentos = new List<ConsumosROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DistNumber, OnHandQty, T6.CntUnitMsr FROM TSSL_NATURASOL.dbo.obbq T0 " +
                                        "JOIN TSSL_NATURASOL.dbo.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs " +
                                        "join TSSL_NATURASOL.dbo.OITM t6 on t6.ItemCode = T0.ItemCode " +
                                        "join DesarrolloWeb.DBO.consumosROVE t7 ON T7.ItemCode = T0.ItemCode COLLATE SQL_Latin1_General_CP850_CI_AS AND T1.DistNumber = T7.BatchNumber COLLATE SQL_Latin1_General_CP850_CI_AS " +
                                        "JOIN TSSL_NATURASOL.dbo.owor t8 on t8.DocNum = t7.DocNum COLLATE SQL_Latin1_General_CP850_CI_AS " +
                                        "WHERE OnHandQty> 0 and T0.WhsCode = t8.Warehouse AND T0.OnHandQty >= T7.Quantity and idConsumoROVE=@idConsumoROVE ORDER BY T0.ItemCode; ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idConsumoROVE", consumoROVE.idConsumoROVE);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ConsumosROVE()
                            {
                                BatchNumber = datareader["DistNumber"].ToString(),
                                Quantity = datareader["OnHandQty"].ToString(),
                                UM = datareader["CntUnitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ConsumosROVE>();
            }

            return listaDocumentos;
        }

        public List<ConsumosROVE> confirmarExistenciaLote2(ConsumosROVE solicitudTrasladoDetalle)
        {
            List<ConsumosROVE> listaDocumentos = new List<ConsumosROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DistNumber, OnHandQty, T6.CntUnitMsr FROM TSSL_NATURASOL.dbo.obbq T0 " +
                                        "JOIN TSSL_NATURASOL.dbo.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs " +
                                        "join TSSL_NATURASOL.dbo.OITM t6 on t6.ItemCode = T0.ItemCode " +
                                        "JOIN TSSL_NATURASOL.dbo.owor t8 on t8.DocNum = @DocNum COLLATE SQL_Latin1_General_CP850_CI_AS " +
                                        "WHERE OnHandQty> 0 AND T0.ItemCode=LEFT(@itemcode,12) and T0.WhsCode=T8.Warehouse AND T1.DistNumber=@DistNumber AND T0.OnHandQty>=@quantity ORDER BY T0.ItemCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", solicitudTrasladoDetalle.ItemCode);
                    comando.Parameters.AddWithValue("@DocNum", solicitudTrasladoDetalle.DocNum);
                    comando.Parameters.AddWithValue("@quantity", solicitudTrasladoDetalle.Quantity);
                    comando.Parameters.AddWithValue("@DistNumber", solicitudTrasladoDetalle.BatchNumber);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ConsumosROVE()
                            {
                                BatchNumber = datareader["DistNumber"].ToString(),
                                Quantity = datareader["OnHandQty"].ToString(),
                                UM = datareader["CntUnitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ConsumosROVE>();
            }

            return listaDocumentos;
        }

        //REQUERIDO S17
        public List<OrdenFabricacion> buscarVidaUtil(string docEntry)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT case when ValidComm is null then 1 else ValidComm end as ValidComm FROM TSSL_NATURASOL.dbo.OITM T0 " +
                                        "JOIN TSSL_NATURASOL.dbo.OWOR T1 ON T1.itemcode = T0.itemcode " +
                                        "WHERE T1.docentry=@docentry ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docEntry", docEntry);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                ValidComm = datareader["ValidComm"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public int validarLiberacionROVE(LiberacionesROVE liberacionesROVE)
        {
            int idConsumoROVE = 0;
            try
            {

                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [DesarrolloWeb].[dbo].[liberacionesROVE] SET " +
                                       "[estatus]= @estatus ";

                    if (liberacionesROVE.estatus == "1")
                    {
                        consulta = consulta + ", [usuarioSupervisor]=@usuarioSupervisor";
                    }
                    else if (liberacionesROVE.estatus == "2")
                    {
                        consulta = consulta + ", [usuarioAnalista]=@usuarioAnalista";
                    }
                    else if (liberacionesROVE.estatus == "-1")
                    {
                        consulta = consulta + ", [usuarioRechazo]=@usuarioRechazo";
                    }

                    consulta = consulta + " WHERE idLiberacionROVE = @idLiberacionROVE ;";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", liberacionesROVE.estatus);
                    comando.Parameters.AddWithValue("@idLiberacionROVE", liberacionesROVE.idLiberacionROVE);

                    if (liberacionesROVE.estatus == "1")
                    {
                        comando.Parameters.AddWithValue("@usuarioSupervisor", liberacionesROVE.usuarioSupervisor);
                    }
                    else if (liberacionesROVE.estatus == "2")
                    {
                        comando.Parameters.AddWithValue("@usuarioAnalista", liberacionesROVE.usuarioAnalista);
                    }
                    else if (liberacionesROVE.estatus == "-1")
                    {
                        comando.Parameters.AddWithValue("@usuarioRechazo", liberacionesROVE.usuarioRechazo);
                    }

                    comando.CommandType = CommandType.Text;

                    idConsumoROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idConsumoROVE = -1;
            }

            return idConsumoROVE;
        }

        public int validarParoROVE(ParosROVE parosROVE)
        {
            int idConsumoROVE = 0;
            try
            {

                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [DesarrolloWeb].[dbo].[parosROVE] SET " +
                                       "[estatus]= @estatus ";

                    if (parosROVE.estatus == "1")
                    {
                        consulta = consulta + ", [usuarioSupervisor]=@usuarioSupervisor";
                    }
                    else if (parosROVE.estatus == "2")
                    {
                        consulta = consulta + ", [usuarioAnalista]=@usuarioAnalista";
                    }
                    else if (parosROVE.estatus == "-1")
                    {
                        consulta = consulta + ", [usuarioRechazo]=@usuarioRechazo";
                    }

                    consulta = consulta + " WHERE idParo = @idParo ;";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", parosROVE.estatus);
                    comando.Parameters.AddWithValue("@idParo", parosROVE.idParo);

                    if (parosROVE.estatus == "1")
                    {
                        comando.Parameters.AddWithValue("@usuarioSupervisor", parosROVE.usuarioSupervisor);
                    }
                    else if (parosROVE.estatus == "2")
                    {
                        comando.Parameters.AddWithValue("@usuarioAnalista", parosROVE.usuarioAnalista);
                    }
                    else if (parosROVE.estatus == "-1")
                    {
                        comando.Parameters.AddWithValue("@usuarioRechazo", parosROVE.usuarioRechazo);
                    }

                    comando.CommandType = CommandType.Text;

                    idConsumoROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idConsumoROVE = -1;
            }

            return idConsumoROVE;
        }

        //METODO PARA GUARDAR UNA LIBERACION
        public int guardarLiberacionesROVE(LiberacionesROVE liberacionesROVE)
        {
            int idLiberacionROVE = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[liberacionesROVE] " +
                   "([DocNum] " +
                   ",[BatchNumber] " +
                   ",[Quantity] " +
                   ",[Comentarios] " +
                   ",[VelocidadLinea] " +
                   ",[Merma] " +
                   ",[Peso] " +
                   ",[Eficiencia] " +
                   ",[MermaP] " +
                   ",[Cajas] " +
                   ",[Sobrepeso] " +
                   ",[GPMReal] " +
                   ",[Linea] " +
                   ",[Operador] " +
                   ",[Headcount2] " +
                   ",[usuarioCreacion] " +
                   ",[Supervisor] " +
                   ",[Headcount]) " +
                    "VALUES" +
                   "(@DocNum," +
                   "@BatchNumber," +
                   "@Quantity," +
                   "@Comentarios," +
                   "@VelocidadLinea," +
                   "@Merma," +
                   "@Peso," +
                   "@Eficiencia," +
                   "@MermaP," +
                   "@Cajas," +
                   "@Sobrepeso," +

                   "@GPMReal," +
                   "@Linea," +
                   "@Operador," +
                   "@Headcount2," +
                   "@usuarioCreacion," +
                   "@Supervisor," +
                   "@Headcount);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", liberacionesROVE.DocNum);
                    comando.Parameters.AddWithValue("@BatchNumber", liberacionesROVE.BatchNumber);
                    comando.Parameters.AddWithValue("@Quantity", liberacionesROVE.Quantity);

                    comando.Parameters.AddWithValue("@usuarioCreacion", liberacionesROVE.usuarioCreacion);


                    comando.Parameters.AddWithValue("@GPMReal", liberacionesROVE.GPMReal == null ? "" : liberacionesROVE.GPMReal);
                    comando.Parameters.AddWithValue("@Linea", liberacionesROVE.Linea);
                    comando.Parameters.AddWithValue("@Operador", liberacionesROVE.Operador);
                    comando.Parameters.AddWithValue("@Headcount2", liberacionesROVE.Headcount2);
                    comando.Parameters.AddWithValue("@Supervisor", liberacionesROVE.Supervisor == null ? "" : liberacionesROVE.Supervisor);
                    comando.Parameters.AddWithValue("@Headcount", liberacionesROVE.Headcount == null ? "" : liberacionesROVE.Headcount);

                    comando.Parameters.AddWithValue("@Comentarios", liberacionesROVE.Comentarios == null ? "" : liberacionesROVE.Comentarios);
                    comando.Parameters.AddWithValue("@VelocidadLinea", liberacionesROVE.VelocidadLinea == null ? "" : liberacionesROVE.VelocidadLinea);
                    comando.Parameters.AddWithValue("@Merma", liberacionesROVE.Merma == null ? "" : liberacionesROVE.Merma);
                    comando.Parameters.AddWithValue("@Peso", liberacionesROVE.Peso == null ? "" : liberacionesROVE.Peso);
                    comando.Parameters.AddWithValue("@Eficiencia", liberacionesROVE.Eficiencia == null ? "" : liberacionesROVE.Eficiencia);
                    comando.Parameters.AddWithValue("@MermaP", liberacionesROVE.MermaP == null ? "" : liberacionesROVE.MermaP);
                    comando.Parameters.AddWithValue("@Cajas", liberacionesROVE.Cajas == null ? "" : liberacionesROVE.Cajas);
                    comando.Parameters.AddWithValue("@Sobrepeso", liberacionesROVE.Sobrepeso == null ? "" : liberacionesROVE.Sobrepeso);
                    comando.CommandType = CommandType.Text;

                    idLiberacionROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idLiberacionROVE = -1;
            }

            return idLiberacionROVE;
        }

        //METODO PARA GUARDAR UNA LIBERACION
        public int guardarParosROVE(ParosROVE parosROVE)
        {
            int idParoROVE = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[parosROVE] " +
                   "([DocNum] " +
                   ",[inicioDateParo] " +
                   //",[inicioTimeParo] " +
                   ",[finDateParo] " +
                   ",[finTimeParo] " +
                   ",[areaParo] " +
                   ",[codigoParo] " +
                   ",[causaParo] " +
                   ",[supervisorParo] " +
                   ",[operadorParo] " +
                   ",[usuarioCreacion] " +
                   ",[lineaParo] " +
                   ",[estatusParo]) " +
                    "VALUES" +
                   "(@DocNum," +
                   "@inicioDateParo," +
                   //"@inicioTimeParo," +
                   "@finDateParo," +
                   "@finTimeParo," +
                   "@areaParo," +
                   "@codigoParo," +
                   "@causaParo," +
                   "@supervisorParo," +
                   "@operadorParo," +
                   "@usuarioCreacion," +
                   "@lineaParo," +
                   "@estatusParo);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", parosROVE.DocNum);
                    comando.Parameters.AddWithValue("@supervisorParo", parosROVE.supervisorParo);
                    comando.Parameters.AddWithValue("@operadorParo", parosROVE.operadorParo);
                    comando.Parameters.AddWithValue("@usuarioCreacion", parosROVE.usuarioCreacion);
                    comando.Parameters.AddWithValue("@lineaParo", parosROVE.lineaParo);
                    comando.Parameters.AddWithValue("@inicioDateParo", parosROVE.inicioDateParo);
                    //comando.Parameters.AddWithValue("@inicioTimeParo", parosROVE.inicioTimeParo);
                    comando.Parameters.AddWithValue("@finDateParo", parosROVE.finDateParo == null ? "" : parosROVE.finDateParo);
                    comando.Parameters.AddWithValue("@finTimeParo", parosROVE.finTimeParo == null ? "" : parosROVE.finTimeParo);
                    comando.Parameters.AddWithValue("@areaParo", parosROVE.areaParo == null ? "" : parosROVE.areaParo);
                    comando.Parameters.AddWithValue("@codigoParo", parosROVE.codigoParo == null ? "" : parosROVE.codigoParo);
                    comando.Parameters.AddWithValue("@causaParo", parosROVE.causaParo == null ? "" : parosROVE.causaParo);
                    comando.Parameters.AddWithValue("@estatusParo", parosROVE.estatusParo == null ? "" : parosROVE.estatusParo);
                    comando.CommandType = CommandType.Text;

                    idParoROVE = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idParoROVE = -1;
            }

            return idParoROVE;
        }

        public List<int> obtenerConsumos(string sociedad)
        {
            List<int> listaConsumos = new List<int>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select count(idConsumoROVE) as numero from  [DesarrolloWeb].[dbo].consumosROVE where Format(fechaCreacion, N'yyyy-MM-dd') = Format(getdate(), N'yyyy-MM-dd') union all select count(idConsumoROVE) as numero from  [DesarrolloWeb].[dbo].consumosROVE where MONTH(Format(fechaCreacion, N'yyyy-MM-dd')) = MONTH(getdate()) ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaConsumos.Add(
                                Convert.ToInt32(datareader["numero"].ToString())
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaConsumos = null;
            }

            return listaConsumos;
        }

        public List<int> obtenerLiberaciones(string sociedad)
        {
            List<int> listaLiberaciones = new List<int>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select count(idLiberacionROVE) as numero from [DesarrolloWeb].[dbo].liberacionesROVE where Format(fechaCreacion, N'yyyy-MM-dd') = Format(getdate(), N'yyyy-MM-dd') union all select count(idLiberacionROVE) as numero from [DesarrolloWeb].[dbo].liberacionesROVE where MONTH(Format(fechaCreacion, N'yyyy-MM-dd')) = MONTH(getdate())";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaLiberaciones.Add(
                                Convert.ToInt32(datareader["numero"].ToString())
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaLiberaciones = null;
            }

            return listaLiberaciones;
        }

        public List<int> obtenerParos(string sociedad)
        {
            List<int> listaParos = new List<int>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select count(idParo) as numero from [DesarrolloWeb].[dbo].parosROVE where Format(fechaCreacion, N'yyyy-MM-dd') = Format(getdate(), N'yyyy-MM-dd') union all select count(idParo) as numero from [DesarrolloWeb].[dbo].parosROVE where MONTH(Format(fechaCreacion, N'yyyy-MM-dd')) = MONTH(getdate()) ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaParos.Add(
                                Convert.ToInt32(datareader["numero"].ToString())
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaParos = null;
            }

            return listaParos;
        }

        public List<string> obtenerKgs(string sociedad)
        {
            List<string> listaKg = new List<string>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select CONCAT(planta,'-',kg,'-',Periodo COLLATE DATABASE_DEFAULT) as 'kgs' from [DesarrolloWeb].[dbo].resumenkgproducidos ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKg.Add((datareader["kgs"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKg = null;
            }

            return listaKg;
        }

        public List<string> obtenerKgs06(string sociedad)
        {
            List<string> listaKg = new List<string>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select CONCAT(quantity,'-',whscode COLLATE DATABASE_DEFAULT) as 'kgs' from [DesarrolloWeb].[dbo].pt06 ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKg.Add((datareader["kgs"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKg = null;
            }

            return listaKg;
        }

        public List<string> obtenerEstatusOF(string sociedad)
        {
            List<string> listaof = new List<string>();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select CONCAT(series,'-',status,'-',contador) as 'of' from [DesarrolloWeb].[dbo].estatusof ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaof.Add((datareader["of"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaof = null;
            }

            return listaof;
        }

        public List<OrdenFabricacion> obtenerOFAbiertasWeb(string sociedadVenta)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.DocEntry, T0.DocNum, T0.CmpltQty, CONCAT(T2.ItemCode, ' - ', T2.ItemName) AS ItemCode, T0.PlannedQty, T0.Type, T0.Status, T5.Consumos, T6.Liberaciones, T0.Comments " +
                        "FROM " + sociedadVenta + ".dbo.OWOR T0 " +
                        "JOIN " + sociedadVenta + ".dbo.OUSR T1 ON T0.UserSign=T1.USERID " +
                        "JOIN " + sociedadVenta + ".dbo.OITM T2 ON T2.ItemCode=T0.ItemCode AND Status NOT IN ('L','C') " +
                        "LEFT JOIN(select BASEREF, ROUND(SUM(LineTotal),2) AS 'Consumos' " +
                        "FROM " + sociedadVenta + ".dbo.IGE1 " +
                        "JOIN " + sociedadVenta + ".dbo.OIGE ON OIGE.DocEntry = IGE1.DocEntry " +
                        "GROUP BY BaseRef) T5 ON T5.BaseRef = T0.DocNum " +
                        "LEFT JOIN(select BASEREF, ROUND(SUM(LineTotal),2) AS 'Liberaciones' " +
                        "FROM " + sociedadVenta + ".dbo.IGN1 " +
                        "JOIN " + sociedadVenta + ".dbo.OIGN on OIGN.DocEntry = IGN1.DocEntry " +
                        "GROUP BY BaseRef) T6 ON T6.BaseRef = T0.DocNum ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                ReleasedQty = datareader["CmpltQty"].ToString(),
                                Type = datareader["Type"].ToString(),
                                Status = datareader["Status"].ToString(),
                                PFinalizado = Math.Round((Convert.ToDecimal(datareader["CmpltQty"].ToString()) / Convert.ToDecimal(datareader["PlannedQty"].ToString())) * 100, 4),
                                Sociedad = sociedadVenta,
                                Consumos = datareader["Consumos"].ToString() == "" ? "0.0000" : string.Format("{0:C}", Convert.ToDecimal(datareader["Consumos"].ToString())),
                                Liberaciones = datareader["Liberaciones"].ToString() == "" ? "0.0000" : string.Format("{0:C}", Convert.ToDecimal(datareader["Liberaciones"].ToString())),
                                Comments = datareader["Comments"].ToString(),
                                Desviacion = string.Format("{0:C}", (datareader["Liberaciones"].ToString() == "" ? 0 : Convert.ToDecimal(datareader["Liberaciones"])) - (datareader["Consumos"].ToString() == "" ? 0 : Convert.ToDecimal(datareader["Consumos"])))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> obtenerConsumosOFAbiertasWeb(string sociedadVenta)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select T0.DocEntry, T1.LineNum, T0.DocNum, T0.Warehouse, CONCAT(T2.ItemCode,' - ', T2.ItemName) AS ItemCode, CONCAT(T3.ItemCode,' - ', T3.ItemName) AS Code, T1.PlannedQty, T1.IssuedQty, T0.CmpltQty/T0.PlannedQty AS 'POF', T1.IssuedQty/T1.PlannedQty AS 'PCO', SUM(convert(float,T4.Quantity)) AS 'ConsumoRove'  " +
                        "FROM " + sociedadVenta + ".dbo.OWOR T0 " +
                        "JOIN " + sociedadVenta + ".dbo.WOR1 T1 ON T0.DocEntry = T1.DocEntry " +
                        "JOIN " + sociedadVenta + ".dbo.OITM T2 ON T2.ItemCode = T0.ItemCode " +
                        "JOIN " + sociedadVenta + ".dbo.OITM T3 ON T3.ItemCode = T1.ItemCode " +
                        "LEFT JOIN DesarrolloWeb.dbo.consumosROVE T4 ON T4.ItemCode COLLATE SQL_Latin1_General_CP1_CI_AS = T1.ItemCode  collate SQL_Latin1_General_CP1_CI_AS AND T4.DocNum  collate SQL_Latin1_General_CP1_CI_AS= T0.DocNum  " +
                        "WHERE T1.PlannedQty > 0 AND T0.Status NOT IN('L', 'C', 'P') AND T4.estatus=3 GROUP BY T0.DocEntry, T1.LineNum, T0.DocNum, T0.Warehouse, T2.ItemCode, T2.ItemName, T3.ItemCode, T3.ItemName, T1.PlannedQty, T1.IssuedQty, T0.CmpltQty, T0.PlannedQty , T1.IssuedQty ORDER BY T0.DocNum ASC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                WhsCode = datareader["Warehouse"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                Linea = datareader["LineNum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                Code = datareader["Code"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                IssuedQty = datareader["IssuedQty"].ToString(),
                                ConsumoRove = datareader["ConsumoRove"].ToString(),
                                POF = Math.Round(Convert.ToDecimal(datareader["POF"].ToString()) * 100, 4),
                                PCO = Math.Round(Convert.ToDecimal(datareader["PCO"].ToString()) * 100, 4),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> obtenerOFWeb(string ubicacion, string sociedad)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select DocNum from " + sociedad + ".dbo.OWOR where Status in ('R') ";

                    if (ubicacion == "1")
                    {
                        consulta = consulta + " and " + sociedad + ".dbo.OWOR.series IN (97) ";
                    }

                    if (ubicacion == "2")
                    {
                        consulta = consulta + " and " + sociedad + ".dbo.OWOR.series IN(77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "3")
                    {
                        consulta = consulta + " and " + sociedad + ".dbo.OWOR.series IN (77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "4")
                    {
                        consulta = consulta + " and " + sociedad + ".dbo.OWOR.series IN (81) ";
                    }

                    if (ubicacion == "5")
                    {
                        consulta = consulta + " and " + sociedad + ".dbo.OWOR.series IN (104,106,110) ";
                    }

                    consulta = consulta + " order by Docnum ASC ";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocNum = datareader["DocNum"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> obtenerComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.itemcode, T0.itemname, T11.uomname FROM " + ordenFabricacion.Sociedad + ".dbo.wor1 T0 " +
                                        "JOIN " + ordenFabricacion.Sociedad + ".dbo.OWOR T1 ON T1.docEntry = T0.docEntry " +
                                        "JOIN " + ordenFabricacion.Sociedad + ".dbo.OUOM T11 ON T11.uomcode = T0.uomcode " +
                                        "WHERE T1.docnum=@docnum and T0.itemcode is not null ORDER BY T0.ItemCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", ordenFabricacion.DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                ItemCode = datareader["itemcode"].ToString(),
                                ItemName = datareader["itemname"].ToString(),
                                UOM = datareader["uomname"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> obtenerCapacidades(string linea, string DocNum)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT concat (T0.ItemCode,' - ',  T0.ProdName, ' __ ' ,PlannedQty, ' - ' ,CmpltQty,' __ ',U_LINEA,' - ', T2.U_CAPACIDAD,' - ', T3.U_CAPACIDAD,' - ',T4.U_OPERADOR,' - ',T4.U_AYUDANTE,' - ',T3.U_GPM ) as ProdName " +
                        "FROM TSSL_NATURASOL.dbo.OWOR T0 " +
                        "LEFT JOIN TSSL_NATURASOL.dbo.[@CODIGOSMAQUINA] T1 ON T1.U_CODIGO = T0.ITEMCODE " +
                        "LEFT JOIN TSSL_NATURASOL.dbo.[@CAPACIDADESPROCESO] T2 ON T2.U_CODIGO = T0.ITEMCODE and T2.U_MAQUINA=T1.U_LINEA " +
                        "LEFT JOIN TSSL_NATURASOL.dbo.[@CAPACIDADESENVASADO] T3 ON T3.U_CODIGO = T0.ITEMCODE and T3.U_MAQUINA=T1.U_LINEA " +
                        "LEFT JOIN TSSL_NATURASOL.dbo.[@HEADCOUNT] T4 ON T1.U_LINEA = T4.U_MAQUINA " +
                        "WHERE T0.docnum=@docnum and T1.U_LINEA=@codigo";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", DocNum);
                    comando.Parameters.AddWithValue("@codigo", linea);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                ItemName = datareader["ProdName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> obtenerArticuloOFWeb(OrdenFabricacion ordenFabricacion)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT concat (T0.ItemCode,' - ',  T0.ProdName, ' __ ' ,PlannedQty, ' - ' ,CmpltQty,' __ ',U_LINEA,' - ', T2.U_CAPACIDAD,' - ', T3.U_CAPACIDAD,' - ',T4.U_OPERADOR,' - ',T4.U_AYUDANTE,' - ',T3.U_GPM ) as ProdName " +
                        "FROM " + ordenFabricacion.Sociedad + ".dbo.OWOR T0 " +
                        "LEFT JOIN " + ordenFabricacion.Sociedad + ".dbo.[@CODIGOSMAQUINA] T1 ON T1.U_CODIGO = T0.ITEMCODE " +
                        "LEFT JOIN " + ordenFabricacion.Sociedad + ".dbo.[@CAPACIDADESPROCESO] T2 ON T2.U_CODIGO = T0.ITEMCODE " +
                        "LEFT JOIN " + ordenFabricacion.Sociedad + ".dbo.[@CAPACIDADESENVASADO] T3 ON T3.U_CODIGO = T0.ITEMCODE " +
                        "LEFT JOIN " + ordenFabricacion.Sociedad + ".dbo.[@HEADCOUNT] T4 ON T1.U_LINEA = T4.U_MAQUINA " +
                                        "WHERE T0.docnum=@docnum";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", ordenFabricacion.DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                ItemName = datareader["ProdName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        public List<ParosROVE> obtenercodigoParo(string parosRove)
        {
            List<ParosROVE> listaDocumentos = new List<ParosROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT codigoParo, descripcionParo FROM [DesarrolloWeb].[dbo].[catalogoParos] where codigoParo LIKE '%" + parosRove + "%'";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ParosROVE()
                            {
                                descripcionParo = datareader["descripcionParo"].ToString(),
                                codigoParo = datareader["codigoParo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ParosROVE>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> obtenerLoteComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DistNumber, OnHandQty, t6.CntUnitMsr, T6.U_Familia FROM " + ordenFabricacion.Sociedad + ".dbo.obbq T0 " +
                                        "JOIN " + ordenFabricacion.Sociedad + ".dbo.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs " +
                                        "join " + ordenFabricacion.Sociedad + ".dbo.OITM t6 on t6.ItemCode = T0.ItemCode " +
                                        "join " + ordenFabricacion.Sociedad + ".dbo.WOR1 t7 on t7.ItemCode = t6.ItemCode " +
                                        "join " + ordenFabricacion.Sociedad + ".dbo.owor t8 on t8.DocEntry = t7.DocEntry " +
                                        "WHERE OnHandQty> 0 AND T0.ItemCode=LEFT(@itemcode,12) AND t8.docnum = @docnum and t8.Warehouse=T0.WhsCode ORDER BY T0.ItemCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", ordenFabricacion.DocNum);
                    comando.Parameters.AddWithValue("@ItemCode", ordenFabricacion.Code);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                DistNumber = datareader["DistNumber"].ToString(),
                                OnHandQty = datareader["OnHandQty"].ToString(),
                                Familia = datareader["U_Familia"].ToString(),
                                UOM = datareader["CntUnitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> obtenerMaquinasOFWeb(OrdenFabricacion ordenFabricacion)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT U_LINEA from  " + ordenFabricacion.Sociedad + ".dbo.[@CODIGOSMAQUINA] where U_CODIGO = (select ItemCode from  " + ordenFabricacion.Sociedad + ".dbo.owor where DocNum=@docnum) ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", ordenFabricacion.DocNum);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                U_LINEA = datareader["U_LINEA"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacionDetalle> obtenerUMComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            List<OrdenFabricacionDetalle> listaDocumentos = new List<OrdenFabricacionDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select InvntryUom FROM " + ordenFabricacion.Sociedad + ".dbo.OITM WHERE ItemCode=@itemcode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", ordenFabricacion.Code);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacionDetalle()
                            {
                                UOM = datareader["InvntryUom"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacionDetalle>();
            }

            return listaDocumentos;
        }

        public List<OrdenFabricacion> obtenerLiberacionesOFAbiertasWeb(string sociedadVenta)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.DocEntry, T0.DocNum, T0.CmpltQty, CONCAT(T2.ItemCode,' - ', T2.ItemName) AS ItemCode, T0.PlannedQty, T0.Type, T0.Status " +
                        "FROM " + sociedadVenta + ".dbo.OWOR T0 " +
                        "JOIN " + sociedadVenta + ".dbo.OUSR T1 ON T0.UserSign=T1.USERID " +
                        "JOIN " + sociedadVenta + ".dbo.OITM T2 ON T2.ItemCode=T0.ItemCode AND Status NOT IN ('L','C','P') ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                PlannedQty = datareader["PlannedQty"].ToString(),
                                ReleasedQty = datareader["CmpltQty"].ToString(),
                                Type = datareader["Type"].ToString(),
                                Status = datareader["Status"].ToString(),
                                PFinalizado = Math.Round((Convert.ToDecimal(datareader["CmpltQty"].ToString()) / Convert.ToDecimal(datareader["PlannedQty"].ToString())) * 100, 4),
                                Sociedad = sociedadVenta
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }


        public List<OrdenFabricacion> contabilizacion_stocks(string sociedad, string OF, string tipoBusqueda)
        {
            List<OrdenFabricacion> listaDocumentos = new List<OrdenFabricacion>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (tipoBusqueda == "1")
                    {
                        consulta = "select OIGE.DocNum, OIGE.DocDate, ItemCode, Dscription, Quantity, Price, Currency, LineTotal " +
                                            "from " + sociedad + ".dbo.IGE1 " +
                                            "join " + sociedad + ".dbo.oige on oige.DocEntry = IGE1.DocEntry " +
                                            "where BaseRef = @of " +
                                            "order by itemcode ";
                    }
                    else
                    {
                        consulta = "select OIGN.DocNum, OIGN.DocDate, ItemCode, Dscription, Quantity, Price, Currency, LineTotal " +
                                            "from " + sociedad + ".dbo.IGN1 " +
                                            "join " + sociedad + ".dbo.oIGN on oIGN.DocEntry = IGN1.DocEntry " +
                                            "where BaseRef = @of " +
                                            "order by itemcode ";
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@of", OF);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenFabricacion()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                createDate = datareader["DocDate"].ToString(),
                                ItemName = datareader["Dscription"].ToString(),
                                IssuedQty = datareader["Quantity"].ToString(),
                                CostoOF = string.Format("{0:C}", Math.Round(((datareader["LineTotal"].ToString() == "" ? 0 : Convert.ToDecimal(datareader["LineTotal"])) / Convert.ToDecimal(datareader["Quantity"].ToString())), 4).ToString() + " " + datareader["Currency"].ToString(), 4),
                                CostoLM = string.Format("{0:C}", Math.Round((datareader["LineTotal"].ToString() == "" ? 0 : Convert.ToDecimal(datareader["LineTotal"])), 4) + " " + datareader["Currency"].ToString(), 4),
                                Sociedad = sociedad
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<OrdenFabricacion>();
            }

            return listaDocumentos;
        }

        public List<ConsumosROVE> obtenerConsumosROVE(string ubicacion, string sociedadVenta)
        {
            List<ConsumosROVE> listaDocumentos = new List<ConsumosROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT TOP (3000) * FROM DESARROLLOWEB.DBO.[PRUEBACONSUMO] T2 ";

                    if (ubicacion == "1")
                    {
                        consulta = consulta + " where T2.series IN (97) ";
                    }

                    if (ubicacion == "2")
                    {
                        consulta = consulta + " where T2.series IN(77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "3")
                    {
                        consulta = consulta + " where T2.series IN (77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "5")
                    {
                        consulta = consulta + " where T2.series IN (104,106,110) ";
                    }

                    consulta = consulta + " order by T2.idConsumoROVE desc ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ConsumosROVE()
                            {
                                linea = datareader["OcrCode"].ToString() is null ? "0" : datareader["OcrCode"].ToString(),
                                cecos = datareader["Linea"].ToString() is null ? "0" : datareader["Linea"].ToString(),
                                DocNum = datareader["DocNum"].ToString() is null ? "0" : datareader["DocNum"].ToString(),
                                supervisor = datareader["supervisor"].ToString() is null ? "0" : datareader["supervisor"].ToString(),
                                operador = datareader["operador"].ToString() is null ? "-" : datareader["operador"].ToString(),
                                UM = datareader["InvntryUom"].ToString() is null ? "0" : datareader["InvntryUom"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString() is null ? "0" : datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString() is null ? "0" : datareader["ItemName"].ToString(),
                                BatchNumber = datareader["BatchNumber"].ToString() is null ? "0" : datareader["BatchNumber"].ToString(),
                                Quantity = datareader["Quantity"].ToString() is null ? "0" : datareader["Quantity"].ToString(),
                                Merma = datareader["Merma"].ToString() is null ? "0" : datareader["Merma"].ToString(),
                                CausaMerma = datareader["CausaMerma"].ToString() is null ? "0" : datareader["CausaMerma"].ToString(),
                                PNC = datareader["PNC"].ToString() is null ? "0" : datareader["PNC"].ToString(),
                                idConsumoROVE = datareader["idConsumoROVE"].ToString() is null ? "0" : datareader["idConsumoROVE"].ToString(),
                                Comentarios = datareader["Comentarios"].ToString() is null ? "0" : datareader["Comentarios"].ToString(),
                                estatus = datareader["estatus"].ToString() is null ? "0" : datareader["estatus"].ToString(),
                                turno = datareader["Turno"].ToString() is null ? "0" : datareader["Turno"].ToString(),
                                fechaCreacion = datareader["fechaCreacion"].ToString() is null ? DateTime.Now.ToString() : datareader["fechaCreacion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ConsumosROVE>();
            }

            return listaDocumentos;
        }

        public List<LiberacionesROVE> obtenerLiberacionesROVE(string ubicacion, string sociedadVenta)
        {
            List<LiberacionesROVE> listaDocumentos = new List<LiberacionesROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * from DESARROLLOWEB.DBO.[PRUEBALIBERACIONES] T1 ";

                    if (ubicacion == "1")
                    {
                        consulta = consulta + " WHERE T1.series IN (97) ";
                    }

                    if (ubicacion == "2")
                    {
                        consulta = consulta + " WHERE T1.series IN(77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "3")
                    {
                        consulta = consulta + " WHERE T1.series IN (77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "5")
                    {
                        consulta = consulta + " WHERE T1.series IN (104,106,110) ";
                    }

                    consulta = consulta + " order by idLiberacionROVE desc";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new LiberacionesROVE()
                            {
                                idLiberacionROVE = datareader["idLiberacionROVE"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                UOM = datareader["Uom"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ProdName"].ToString(),
                                Linea = datareader["ocrCode"].ToString(),
                                cecos = datareader["Linea"].ToString(),
                                BatchNumber = datareader["BatchNumber"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                Operador = datareader["Operador"].ToString(),
                                Supervisor = datareader["Supervisor"].ToString(),
                                VelocidadLinea = datareader["VelocidadLinea"].ToString(),
                                Headcount2 = datareader["Headcount2"].ToString(),
                                Headcount = datareader["Headcount"].ToString(),
                                estatus = datareader["estatus"].ToString(),
                                Peso = datareader["peso"].ToString(),
                                turno = datareader["Turno"].ToString(),
                                Merma = datareader["Merma"].ToString(),
                                Comentarios = datareader["Comentarios"].ToString(),
                                Eficiencia = datareader["Eficiencia"].ToString(),
                                fechaCreacion = datareader["fechaCreacion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<LiberacionesROVE>();
            }

            return listaDocumentos;
        }

        public List<ParosROVE> obtenerParosROVE(string ubicacion, string sociedadVenta)
        {
            List<ParosROVE> listaDocumentos = new List<ParosROVE>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT [idParo], T1.series, estatus, case when T1.ocrcode is null then '-' else T1.OcrCode end as 'ocrcode', case when T0.lineaParo is null then '-' else T0.lineaParo end as 'Linea', case when DATEPART(HOUR, fechaCreacion) between 6 and 13 then 'Turno 1' when DATEPART(HOUR, fechaCreacion) between 14 and 21 then 'Turno 2' else 'Turno 3' end as 'Turno' " +
                                      ",T0.[DocNum], T0.supervisorParo, T0.operadorParo " +
                                      ",CONVERT(datetime, [inicioDateParo] + ':00', 126) as 'inicioDateParo' " +
                                      //",case when inicioDateParo = '' then '' else cast(left(inicioDateParo,10) as date) end as 'inicioDateParo'  " +
                                      ",CONVERT(datetime, [finDateParo] + ':00', 126) as 'finDateParo' " +
                                      //",case when finDateParo = '' then '' else cast(left(finDateParo,10) as date ) end as 'finDateParo'  " +
                                      ",case [areaParo] WHEN 'M' THEN 'MANTENIMIENTO' WHEN 'R' THEN 'PROYECTOS' WHEN 'C' THEN 'CADENA DE SUMINISTRO' WHEN 'H' THEN 'RH' WHEN 'X' THEN 'EXTERNO' WHEN 'O' THEN 'COMPRAS' WHEN 'L' THEN 'CALIDAD' WHEN 'D' THEN 'CEIDENA' WHEN 'P' THEN 'PRODUCCIÓN 'WHEN 'U' THEN 'AUSENTISMO DE PERSONAL' WHEN 'N' THEN 'SANIDAD' WHEN 'S' THEN 'PROCESOS' WHEN 'G' THEN 'SEGURIDAD E HIGIENE' WHEN 'T' THEN 'SISTEMAS' END AS [areaParo] " +
                                      ",CASE [codigoParo] WHEN 'M1' THEN 'FALLA DE EQUIPOS, FALTA DE REFACCIONES, MANTENIMIENTOS NO PROGRAMADOS, MANTENIMIENTO CORRECTIVO' WHEN 'R1' THEN 'INSTALACIONES O MEJORAS DE EQUIPOS' WHEN 'C1' THEN 'CAMBIOS AL PROGRAMA' WHEN 'C2' THEN 'FALTA DE MATERIALES (ATRASO EN SURTIMIENTO, FUERA DE ESPECIFICACIÓN POR FALTA DE SEGUIMIENTO A PEPS)' WHEN 'C3' THEN 'FALTA DE MATERIALES (NO PLANEADOS)' WHEN 'H1' THEN 'FALTA DE PERSONAL POR PLANTILLA INCOMPLETA' WHEN 'X1' THEN 'FALTA DE ENERGIA ELECTRICA' WHEN 'X2' THEN 'DESASTRE NATURAL' WHEN 'O1' THEN 'FALTA DE SERVICIOS (AGUA, GAS, ETC.)' WHEN 'O2' THEN 'FALTA DE MATERIALES POR FALTA DE COMPRA' WHEN 'L1' THEN 'PNC POR FALTA DE CRITERIOS DE LIBERACIÓN' WHEN 'L2' THEN 'ATRASO EN LIBERACIÓN' WHEN 'D3' THEN 'PRUEBAS NO PLANEADAS POR DESARROLLO DE NUEVOS PRODUCTOS' WHEN 'P1' THEN 'PNC POR MAL AJUSTE OPERATIVO' WHEN 'P2' THEN 'PAROS OPERATIVOS' WHEN 'P3' THEN 'AJUSTES DE OPERACIÓN' WHEN 'U1' THEN 'FALTA DE PERSONAL POR AUSENTISMO' WHEN 'P4' THEN 'RELLENO DE ACEITE' WHEN 'P5' THEN 'ATRASO EN LIMPIEZA Y RETIRO DE LODOS' WHEN 'P6' THEN 'ATRASO POR LIMPIEZA' WHEN 'N1' THEN 'ATRASO EN TIEMPO DE LIMPIEZA POR SANIDAD' WHEN 'S1' THEN 'AJUSTES DE CONDICIONES DE OPERACIÓN, CAPACITACIÓN, MEJORAS DEL PROCESO' WHEN 'G1' THEN 'RIESGO DE TRABAJO' WHEN 'P7' THEN 'ARRANQUE DE PROCESO' WHEN 'P8' THEN 'FIN DE PROCESO' WHEN 'P9' THEN 'LIMPIEZA PROGRAMADA' WHEN 'P10' THEN 'TIEMPO PROGRAMADO PARA LIMPIEZA Y RETIRO DE LODOS' WHEN 'P11' THEN 'PESADO DE MATERIALES' WHEN 'T1' THEN 'FALTA DE SISTEMA SAP' END AS  [codigoParo] " +
                                      ",[causaParo] " +
                                      ",[estatusParo], " +
                                      "case when inicioDateParo like '%T%' then DATEDIFF(minute, CONVERT(datetime, [inicioDateParo] + ':00', 126), CONVERT(datetime, [finDateParo] + ':00', 126)) else '' end as 'tiempoMuerto' " +
                        "FROM desarrolloweb.dbo.[parosROVE] T0 " +
                        "JOIN TSSL_NATURASOL.dbo.OWOR T1 ON T1.DocNum = T0.DocNum COLLATE SQL_Latin1_General_CP850_CI_AS " +
                        "WHERE T1.Status='R' ";

                    if (ubicacion == "1")
                    {
                        consulta = consulta + " and T1.series IN (97) ";
                    }

                    if (ubicacion == "2")
                    {
                        consulta = consulta + " and T1.series IN(77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "3")
                    {
                        consulta = consulta + " and T1.series IN (77,78,33,79,80,94,99,101) ";
                    }

                    if (ubicacion == "5")
                    {
                        consulta = consulta + " and T1.series IN (104,106,110) ";
                    }


                    consulta = consulta + " order by idParo desc ";

                    //consulta = consulta + " AND idParo<2800  order by idParo desc";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new ParosROVE()
                            {
                                idParo = datareader["idParo"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                planta = datareader["ocrCode"].ToString() is null ? "" : datareader["ocrCode"].ToString(),
                                lineaParo = datareader["Linea"].ToString() is null ? "" : datareader["Linea"].ToString(),
                                supervisorParo = datareader["supervisorParo"].ToString() is null ? "" : datareader["supervisorParo"].ToString(),
                                operadorParo = datareader["operadorParo"].ToString() is null ? "" : datareader["operadorParo"].ToString(),
                                inicioDateParo = datareader["inicioDateParo"].ToString(),
                                finDateParo = datareader["finDateParo"].ToString(),
                                tiempoMuerto = datareader["tiempoMuerto"].ToString(),
                                areaParo = datareader["areaParo"].ToString(),
                                codigoParo = datareader["codigoParo"].ToString(),
                                causaParo = datareader["causaParo"].ToString(),
                                estatus = datareader["estatus"].ToString(),
                                turno = datareader["Turno"].ToString(),
                                estatusParo = datareader["estatusParo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<ParosROVE>();
            }

            return listaDocumentos;
        }
    }
}

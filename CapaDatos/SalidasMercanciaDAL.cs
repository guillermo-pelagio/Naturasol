using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class SalidasMercanciaDAL
    {
        public List<SalidasMercancia> listaMuestrasLiberacion(string sociedad)
        {
            List<SalidasMercancia> listaDocumentos = new List<SalidasMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.U_LiberacionOC, T0.Docnum, T0.DocEntry, T5.WddCode, T5.WtmCode " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "left JOIN  " + sociedad + ".dbo.OWDD T5 ON T0.DocEntry = T5.DraftEntry " +
                        "WHERE T0.ObjType = 60 AND U_INV_TIPOMOV=2 " +
                        "AND T0.U_LiberacionOC = 1 ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SalidasMercancia()
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
                listaDocumentos = new List<SalidasMercancia>();
            }

            return listaDocumentos;
        }

        public List<SalidasMercancia> listaBorradoresLiberacion(string sociedad)
        {
            List<SalidasMercancia> listaDocumentos = new List<SalidasMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.U_LiberacionOC, T0.Docnum, T0.DocEntry, T5.WddCode, T5.WtmCode " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "left JOIN  " + sociedad + ".dbo.OWDD T5 ON T0.DocEntry = T5.DraftEntry " +
                        "WHERE T0.ObjType = 22  " +
                        "AND T0.U_LiberacionOC <> 0 ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SalidasMercancia()
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
                listaDocumentos = new List<SalidasMercancia>();
            }

            return listaDocumentos;
        }

        public List<SalidasMercancia> listaBorradoresLiberacionDetalle(string sociedad, string docentry)
        {
            List<SalidasMercancia> listaDocumentos = new List<SalidasMercancia>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T0.DocEntry, DocNum, T0.DocDate, DocType, T0.CardCode, CardName, DocCur, DocRate, T0.VatSum, T0.VatSumFC, DocTotal, DocTotalFC, SlpName, T1.Email, T2.ItemCode, Dscription, T2.AcctCode, T3.AcctName, U_CantidadServicio, Quantity, Price, t2.Currency, LineTotal, TotalFrgn, TaxCode, T2.OcrCode, T2.OcrCode3, T4.OnHAND, t7.Consumo  " +
                        "FROM " + sociedad + ".dbo.odrf T0 " +
                        "JOIN " + sociedad + ".dbo.oslp T1 ON T1.SlpCode = T0.SlpCode " +
                        "JOIN " + sociedad + ".dbo.drf1 T2 ON T2.DocEntry = T0.DocEntry " +
                        "JOIN " + sociedad + ".dbo.oact T3 ON T2.AcctCode = T3.AcctCode " +
                        "LEFT JOIN  " + sociedad + ".dbo.oitm T4 ON T4.ItemCode = T2.ItemCode " +
                        "LEFT JOIN (SELECT IGE1.ItemCode, sum(Quantity) as 'Consumo' FROM " + sociedad + ".dbo.OIGE   " +
                                            "JOIN " + sociedad + ".dbo.IGE1 ON IGE1.DocEntry = oIGE.DocEntry " +
                                            "WHERE MONTH(OIGE.DocDate) = month(CAST(DATEADD(DAY, -84, GETDATE()) AS DATE)) " +
                                                 "AND ige1.BaseType = 202 " +
                                                 "GROUP BY ItemCode) T7 ON T7.ItemCode = T2.ItemCode " +
                        "WHERE T0.ObjType = 22  AND T0.docentry = @docentry ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.Parameters.AddWithValue("@docentry", docentry);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SalidasMercancia()
                            {
                                OcrCode = datareader["OcrCode"].ToString(),
                                OcrCode3 = datareader["OcrCode3"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),
                                DocRate = datareader["DocRate"].ToString(),
                                VatSum = datareader["VatSum"].ToString(),
                                VatSumFC = datareader["VatSumFC"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                OnHand = datareader["OnHAND"].ToString(),
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
                listaDocumentos = new List<SalidasMercancia>();
            }

            return listaDocumentos;
        }

        public List<SalidasMercancia> listaBorradoresLiberacionDetalleAnexos(string sociedad, string docentry)
        {
            List<SalidasMercancia> listaDocumentos = new List<SalidasMercancia>();
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
                            listaDocumentos.Add(new SalidasMercancia()
                            {
                                trgtPath = datareader["trgtPath"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SalidasMercancia>();
            }

            return listaDocumentos;
        }

        public int guardarSolicitudAutorizacionMuestras(SalidasMercancia borradorOrdenCompra)
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
        public int actualizarSolicitudAutorizacionMuestras(string wddcode, string estatus, string estatusArea)
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

        //METODO PARA ACTUALIZAR UN ACCESORIO
        public int actualizarMuestraAutorizada(string sociedad, string wddCode)
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
    }
}

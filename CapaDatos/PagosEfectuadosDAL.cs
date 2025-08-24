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
    public class PagosEfectuadosDAL
    {
        //METODO PARA OBTENER TODA LA INFO DE CXP - NOTIFICADOR AVISO -
        public List<PagosEfectuados> obtenerInfoCXP(string sociedad)
        {
            List<PagosEfectuados> listaSocios = new List<PagosEfectuados>();

            string empresa = sociedad.Contains("Naturasol") ? "NATURASOL" : "MIELMEX";

            string consulta = "SELECT DISTINCT T1.CardName, T1.E_Mail FROM " + sociedad + ".dbo.OCRD T1 " +
                " JOIN " + sociedad + ".dbo.OPOR ON OPOR.CardCode = T1.CardCode " +
                                "WHERE E_Mail IS NOT NULL AND CardType='S' AND OPOR.CreateDate >= '2023-01-01' ORDER BY CardName";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaSocios.Add(new PagosEfectuados()
                            {
                                sociedad = empresa,
                                E_Mail = datareader["E_Mail"].ToString(),
                                CardName = datareader["CardName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaSocios;
        }

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosEfectuados> obtenerPagosNotificarBasico(string sociedad)
        {
            List<PagosEfectuados> listaPagosEfectuados = new List<PagosEfectuados>();

            string empresa = sociedad.Contains("Naturasol") ? "NATURASOL" : "MIELMEX";

            string consulta = "SELECT T1.DocEntry, T1.PayNoDoc, T1.NoDocSum, T1.DocNum, T1.DocDate, T1.CardName, T1.DocTotal, T1.DocTotalFC, T2.E_Mail, T1.DocCurr, T1.DocRate FROM " + sociedad + ".dbo.OVPM T1 " +
                "JOIN " + sociedad + ".dbo.OCRD T2 ON T2.CardCode=T1.CardCode " +
                "WHERE T1.DocType = 'S' AND T1.U_EstatusPago IN ('Notificar') AND DocDate >= '2023-04-01' AND T1.Canceled='N' AND E_Mail IS NOT NULL ORDER BY DocNum ";
            //"WHERE T1.DocType = 'S' AND T1.DocNum = 78355 ";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPagosEfectuados.Add(new PagosEfectuados()
                            {
                                sociedad = empresa,
                                PayNoDoc = datareader["PayNoDoc"].ToString(),
                                NoDocSum = Convert.ToDecimal(datareader["NoDocSum"].ToString()),
                                E_Mail = datareader["E_Mail"].ToString(),
                                DocCur = datareader["DocCurr"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                DocRate = datareader["DocRate"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["DocDate"].ToString()),
                                CardName = datareader["CardName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaPagosEfectuados;
        }

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosEfectuados> obtenerPagosNotificarAvanzado(string sociedad, string docnum)
        {
            List<PagosEfectuados> listaPagosEfectuados = new List<PagosEfectuados>();

            string empresa = sociedad.Contains("Naturasol") ? "NATURASOL" : "MIELMEX";

            string consulta = "SELECT T1.PayNoDoc, T2.InvType, T1.DocNum, T1.DocDate, T3.E_Mail, T1.CardName, T1.DocTotal, T1.DocTotalFC, T1.DocCurr, T1.DocRate FROM " + sociedad + ".dbo.OVPM T1 " +
                "LEFT JOIN " + sociedad + ".dbo.VPM2 T2 ON T2.DocNum=T1.DocEntry " +
                "LEFT JOIN " + sociedad + ".dbo.OCRD T3 ON T3.CardCode=T1.CardCode " +
                "WHERE T1.DocType = 'S' AND T1.DocNum=@docnum";
            //"WHERE T1.DocType = 'S' AND T1.DocNum = 78355 ";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", docnum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPagosEfectuados.Add(new PagosEfectuados()
                            {
                                sociedad = empresa,
                                E_Mail = datareader["E_Mail"].ToString(),
                                PayNoDoc = datareader["PayNoDoc"].ToString(),
                                DocCur = datareader["DocCurr"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                DocRate = datareader["DocRate"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                InvType = datareader["InvType"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["DocDate"].ToString()),
                                CardName = datareader["CardName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaPagosEfectuados;
        }

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosEfectuados> obtenerPagosNotificarDetalle(string sociedad, string docnum, string invtype)
        {
            List<PagosEfectuados> listaPagosEfectuados = new List<PagosEfectuados>();
            string consulta = "";

            if (invtype == "18")
            {
                consulta = "SELECT T1.DocTotal, T1.DocTotalFC, T1.DocDate, T1.DocNum, T2.SumApplied, T2.AppliedFC, T3.DocCur, T2.DocRate, " +
                "T3.CardName, T3.NumAtCard, T3.EDocNum FROM " + sociedad + ".dbo.OVPM T1 " +
                "JOIN " + sociedad + ".dbo.VPM2 T2 ON T1.DocEntry = T2.DocNum " +
                "JOIN " + sociedad + ".dbo.OPCH T3 ON T3.DocEntry = T2.DocEntry " +
                "WHERE T1.DocType = 'S' AND T1.DocNum = @docnum; ";
            }
            if (invtype == "204")
            {
                consulta = "SELECT T1.DocTotal, T1.DocTotalFC, T1.DocDate, T1.DocNum, T2.SumApplied, T2.AppliedFC, T3.DocCur, T2.DocRate, " +
                "T3.CardName, T3.NumAtCard, T3.EDocNum FROM " + sociedad + ".dbo.OVPM T1 " +
                "LEFT JOIN " + sociedad + ".dbo.VPM2 T2 ON T1.DocEntry = T2.DocNum " +
                "LEFT JOIN " + sociedad + ".dbo.ODPO T3 ON T3.DocEntry = T2.DocEntry " +
                "WHERE T1.DocType = 'S' AND T1.DocNum = @docnum; ";
            }

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", docnum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPagosEfectuados.Add(new PagosEfectuados()
                            {
                                DocCur = datareader["DocCur"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["DocDate"].ToString()),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                SumApplied = datareader["SumApplied"].ToString(),
                                AppliedFC = datareader["AppliedFC"].ToString(),
                                DocRate = datareader["DocRate"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                EDocNum = datareader["EDocNum"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaPagosEfectuados;
        }
    }
}
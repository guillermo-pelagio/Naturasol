using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class PagosBancomerDAL
    {
        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosBancomer> obtenerPagos(int tipoPago, int sociedad, int usuario)
        {
            List<PagosBancomer> listaPagosBancomer = new List<PagosBancomer>();
            string formatoDocumento = "000000000";
            string formatoMonto = "0000000000000.##";
            string consulta = "";
            string filtroUsuario = "";
            string filtroTipoPago = "";

            //MISMO BANCO
            if (tipoPago == 1)
            {
                filtroTipoPago = " AND T1.[bankcode] = '0012' " + "AND T1.[DflAccount] <> '' ";
            }
            //INTERBANCARIO
            else if (tipoPago == 2)
            {
                filtroTipoPago = " AND T1.[bankcode] <> '0012' " + "AND T1.[DflSwift] <> '' ";
            }

            //MIELMEX
            if (sociedad == 1)
            {
                if (usuario == 1)
                {
                    filtroUsuario = " AND (T0.UserSign = 93 OR T0.UserSign = 65 OR T0.UserSign = 129 OR T0.UserSign = 139) ";
                }
                else if (usuario == 2)
                {
                    filtroUsuario = " AND (T0.UserSign = 93 OR T0.UserSign = 65 OR T0.UserSign = 129 OR T0.UserSign = 139) ";
                }

                if (tipoPago == 2)
                {
                    consulta = "SELECT T1.[DflSwift] AS 'Beneficiario'," +
                        "T1.[DflAccount]             AS 'Beneficiario2'," +
                        "'448134618'             AS 'Ordenante'," +
                        "T0.DocCurr              AS 'Divisa'," +
                        "T0.[TrsfrSum]           AS 'Importe'," +
                        "T0.[TrsfrSumfc]         AS 'Importe FC'," +
                        "LEFT(T0.[cardname], 30) AS 'Titular'," +
                        "'40'                    AS 'Cuenta'," +
                        "T1.[bankcode]           AS 'Banco', " +
                        "LEFT(T0.[trsfrref], 30) AS 'Pago'," +
                        "T0.[docnum]             AS 'Referencia'," +
                        "T0.[docentry]           AS 'DocEntry'," +
                        "T0.[comments]           AS 'Referencia2', " +
                        "T0.[DocDate]            AS 'Fecha'," +
                        "'H'                     AS 'Disponibilidad' " +
                "FROM TSSL_Mielmex.dbo.OVPM T0 " +
                        "INNER JOIN TSSL_Mielmex.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
                "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                        //"WHERE  T0.[u_estatuspago] = 'Actualizado' " +
                        "AND T0.[trsfracct] = '11101101' " +
                        filtroTipoPago +
                        "AND T0.Canceled = 'N' " +
                        //"AND T0.DocDate = '2022-06-01'" +
                        "AND T0.UserSign <> 8 " +
                        filtroUsuario +
                        "AND (T0.DocType = 'S' OR T0.DocType = 'C') ";

                    consulta =
                        consulta + " UNION ALL (" +
                        "SELECT CASE T1.AcctCode " +
                            "WHEN '11101162' THEN '002180700865157662'" +
                            "WHEN '11101117' THEN '002180700601789236'" +
                            "WHEN '11101115' THEN '002180094664934116'" +
                            "WHEN '11101113' THEN '002180486100883795'" +
                        "END AS 'Beneficiario2' " +
                        ", CASE T1.AcctCode " +
                            "WHEN '11101162' THEN '002180700865157662'" +
                            "WHEN '11101116' THEN '002180700601789236'" +
                            "WHEN '11101115' THEN '002180094664934116'" +
                            "WHEN '11101113' THEN '002180486100883795'" +
                        "END AS 'Beneficiario2' " +
                        ", '448134618'             AS 'Ordenante'" +
                        ", T0.DocCurr              AS 'Divisa'" +
                        ", T0.[TrsfrSum]           AS 'Importe'" +
                        ", T0.[TrsfrSumfc]         AS 'Importe FC'" +
                        ", LEFT(T0.[cardname], 30) AS 'Titular'" +
                        ", '40'                    AS 'Cuenta'" +
                        ", '0012'                  AS 'Banco'" +
                        ", LEFT(T0.[trsfrref], 30) AS 'Pago'" +
                        ", T0.[docnum]             AS 'Referencia'" +
                        ", T0.[docentry]           AS 'DocEntry'" +
                        ", T0.[comments]           AS 'Referencia2'" +
                        ", T0.[DocDate]            AS 'Fecha'" +
                        ", 'H'                     AS 'Disponibilidad' " +
                    "FROM TSSL_Mielmex.dbo.OVPM T0 " +
                    "INNER JOIN TSSL_Mielmex.dbo.VPM4 T1 ON T0.[DocEntry] = T1.DocNum " +
                    "WHERE T0.[u_estatuspago] = 'Por Procesar' " +
                    "AND T0.[trsfracct] = '11101101' " +
                    "AND T1.AcctCode IN( '11101162', '11101117', '11101115', '11101113') " +
                    "AND T0.Canceled = 'N' " +
                    "AND T0.UserSign <> 8 " +
                    filtroUsuario +
                    "AND T0.DocType = 'A' " +
                        ")";
                }

                if (tipoPago == 1)
                {
                    consulta =
                        consulta +
                        "SELECT '------------------'    AS 'Beneficiario'" +
                        ", CASE T1.AcctCode " +
                            "WHEN '11101109' THEN '0142906678'" +
                            "WHEN '11101104' THEN '0448134650'" +
                            "WHEN '11101173' THEN '0114121996'" +
                            "WHEN '11101182' THEN '0119636692'" +
                            "WHEN '11101177' THEN '0116612628'" +
                        "END AS 'Beneficiario2' " +
                        ", '448134618'             AS 'Ordenante'" +
                        ", T0.DocCurr              AS 'Divisa'" +
                        ", T0.[TrsfrSum]           AS 'Importe'" +
                        ", T0.[TrsfrSumfc]         AS 'Importe FC'" +
                        ", LEFT(T0.[cardname], 30) AS 'Titular'" +
                        ", '40'                    AS 'Cuenta'" +
                        ", '0012'                  AS 'Banco'" +
                        ", LEFT(T0.[trsfrref], 30) AS 'Pago'" +
                        ", T0.[docnum]             AS 'Referencia'" +
                        ", T0.[docentry]           AS 'DocEntry'" +
                        ", T0.[comments]           AS 'Referencia2'" +
                        ", T0.[DocDate]            AS 'Fecha'" +
                        ", 'H'                     AS 'Disponibilidad' " +
                    "FROM TSSL_Mielmex.dbo.OVPM T0 " +
                    "INNER JOIN TSSL_Mielmex.dbo.VPM4 T1 " +
                        "ON T0.[DocEntry] = T1.DocNum " +
                    "WHERE T0.[u_estatuspago] = 'Por Procesar' " +
                    "AND T1.AcctCode IN( '11101109', '11101104', '11101173', '11101177', '11101182') " +
                    "AND T0.Canceled = 'N' " +
                    "AND(T0.UserSign = 93 OR T0.UserSign = 65 OR T0.UserSign = 129 OR T0.UserSign = 139) " +
                    "AND T0.UserSign <> 8 " +
                    "AND T0.DocType = 'A' " +
                        "";

                    consulta =
                       consulta + " UNION ALL SELECT T1.[DflSwift] AS 'Beneficiario'," +
                       "T1.[DflAccount]             AS 'Beneficiario2'," +
                       "'448134618'            AS 'Ordenante'," +
                       "T0.DocCurr              AS 'Divisa'," +
                       "T0.[TrsfrSum]           AS 'Importe'," +
                       "T0.[TrsfrSumfc]         AS 'Importe FC'," +
                       "LEFT(T0.[cardname], 30) AS 'Titular'," +
                       "'40'                    AS 'Cuenta'," +
                       "T1.[bankcode]           AS 'Banco', " +
                       "LEFT(T0.[trsfrref], 30) AS 'Pago'," +
                       "T0.[docnum]             AS 'Referencia'," +
                       "T0.[docentry]           AS 'DocEntry'," +
                       "T0.[comments]          AS 'Referencia2', " +
                       "T0.[DocDate]            AS 'Fecha'," +
                       "'H'                     AS 'Disponibilidad' " +
               "FROM TSSL_Mielmex.dbo.OVPM T0 " +
                       "INNER JOIN TSSL_Mielmex.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
               "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                       "AND T0.[trsfracct] IN ('11101101','11101201') " +
                       filtroTipoPago +
                       "AND T0.Canceled = 'N' " +
                       "AND T0.UserSign <> 8 " +
                       filtroUsuario +
                       "AND (T0.DocType = 'S' OR T0.DocType = 'C') ";
                }

                consulta = consulta + " ORDER BY T0.[docnum] ";
            }
            else if (sociedad == 2)
            {
                if (usuario == 1)
                {
                    filtroUsuario = " AND (T0.UserSign = 103 OR T0.UserSign = 164 OR T0.UserSign = 134 OR T0.UserSign = 65) ";
                }
                else if (usuario == 2)
                {
                    filtroUsuario = " AND (T0.UserSign = 103 OR T0.UserSign = 164 OR T0.UserSign = 134 OR T0.UserSign = 65) ";
                }

                consulta = "SELECT T1.[DflSwift] AS 'Beneficiario'," +
                        "T1.[DflAccount]             AS 'Beneficiario2'," +
                        "'136217451'            AS 'Ordenante'," +
                        "T0.DocCurr              AS 'Divisa'," +
                        "T0.[TrsfrSum]           AS 'Importe'," +
                        "T0.[TrsfrSumfc]         AS 'Importe FC'," +
                        "LEFT(T0.[cardname], 30) AS 'Titular'," +
                        "'40'                    AS 'Cuenta'," +
                        "T1.[bankcode]           AS 'Banco', " +
                        "LEFT(T0.[trsfrref], 30) AS 'Pago'," +
                        "T0.[docnum]             AS 'Referencia'," +
                        "T0.[docentry]           AS 'DocEntry'," +
                        "T0.[comments]          AS 'Referencia2', " +
                        "T0.[DocDate]            AS 'Fecha'," +
                        "'H'                     AS 'Disponibilidad' " +
                "FROM TSSL_Naturasol.dbo.OVPM T0 " +
                        "INNER JOIN TSSL_Naturasol.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
                "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                //"WHERE  T0.[u_estatuspago] = 'Actualizado' " +
                        "AND T0.[trsfracct] IN ('11101138','11101202') " +
                        filtroTipoPago +
                        "AND T0.Canceled = 'N' " +
                        //"AND T0.DocDate = '2022-06-01'" +
                        "AND T0.UserSign <> 8 " +
                        filtroUsuario +
                        "AND (T0.DocType = 'S' OR T0.DocType = 'C') ORDER BY T0.[docnum]";
            }

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
                            DateTime fechaDocumento = Convert.ToDateTime(datareader["Fecha"].ToString());
                            DateTime fechaDias = DateTime.Now.AddDays(-14);
                            string motivoPago = "";

                            //MISMO BANCO
                            if (tipoPago == 1)
                            {
                                motivoPago = "PP" + Convert.ToInt32(datareader["Referencia"].ToString()).ToString(formatoDocumento) + (datareader["Referencia2"].ToString() == null ? "".PadRight(19) : datareader["Referencia2"].ToString().PadRight(19)).Substring(0, 19);
                            }
                            //INTERBANCARIO
                            else if (tipoPago == 2)
                            {
                                motivoPago = datareader["Pago"].ToString() == "" ? Convert.ToInt32(datareader["Referencia"].ToString()).ToString(formatoDocumento) + (datareader["Titular"].ToString() == null ? "".PadRight(19) : datareader["Titular"].ToString().PadRight(19)).Substring(0, 19) : datareader["Pago"].ToString().PadRight(30);
                            }

                            if (fechaDias <= fechaDocumento)
                            {
                                listaPagosBancomer.Add(new PagosBancomer()
                                {
                                    disponibilidad = datareader["Disponibilidad"].ToString(),
                                    motivoPago = motivoPago,
                                    numeroBanco = datareader["Banco"].ToString(),
                                    tipoCuenta = datareader["Cuenta"].ToString(),
                                    titular = datareader["Titular"].ToString() == null ? "".PadRight(30) : datareader["Titular"].ToString().PadRight(30),
                                    importe = Convert.ToDecimal(datareader["Importe"].ToString()).ToString("F").ToString().PadLeft(16, '0'),
                                    importeFC = Convert.ToDecimal(datareader["Importe FC"].ToString()).ToString("F").ToString().PadLeft(16, '0'),
                                    divisa = datareader["Divisa"].ToString(),
                                    docEntry = datareader["DocEntry"].ToString(),
                                    cuentaOrdenante = datareader["Ordenante"].ToString().PadLeft(18, '0'),
                                    cuentaBeneficiario = datareader["Beneficiario"].ToString().PadLeft(18, '0'),
                                    cuentaBeneficiario2 = datareader["Beneficiario2"].ToString().PadLeft(18, '0'),
                                    numeroDocumento = Convert.ToInt32(datareader["Referencia"].ToString()).ToString(formatoDocumento)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaPagosBancomer;
        }

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public String buscarClaveBanco(string numeroBanco)
        {
            string banco = "0";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "claveBanco " +
                        "FROM codigoBancosSAT " +
                        "WHERE codigoMielmex = @codigoMielmex";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@codigoMielmex", numeroBanco);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            banco = datareader["claveBanco"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return banco;
        }

        //METODO PARA GUARDAR UN PAGO EN LA TABLA DE DESARROLLO
        public int insertPagoIntermedio(int numeroDocumento, string sociedad)
        {
            int idAccesorio = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[pagosSAP]" +
                   "([numeroDocumento]," +
                   "[sociedad]," +
                   "[estatus]," +
                   "[fechaRegistro])" +
                    "VALUES" +
                   "(@numeroDocumento," +
                   "@sociedad," +
                   "@estatus," +
                   "@fechaRegistro);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@numeroDocumento", Convert.ToString(numeroDocumento));
                    comando.Parameters.AddWithValue("@sociedad", sociedad);
                    comando.Parameters.AddWithValue("@estatus", 1);
                    comando.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
                    comando.CommandType = CommandType.Text;

                    idAccesorio = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idAccesorio = 0;
            }

            return idAccesorio;
        }

        //S3-REQUERIDO-METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR DE LA BASE DE DESARROLLO
        public List<PagosBancomer> buscarPagosPendientesSAP()
        {
            List<PagosBancomer> listaPagosBancomer = new List<PagosBancomer>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT [numeroDocumento]," +
                        "[sociedad]," +
                        "[estatus]," +
                        "[fechaRegistro] " +
                        "FROM [DesarrolloWeb].[dbo].[pagosSAP] " +
                        "WHERE estatus = 1";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaPagosBancomer.Add(new PagosBancomer()
                            {
                                sociedad = datareader["sociedad"].ToString(),
                                numeroDocumento = datareader["numeroDocumento"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listaPagosBancomer;
        }

        //METODO PARA ACTUALIZAR EL PAGO EN LA BASE DE DESARROLLO
        public int updatePagoDesarrollo(string numeroDocumento, string sociedad)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [dbo].[pagosSAP]" +
                    "SET[estatus] = 0 " +
                    "WHERE numeroDocumento = @numeroDocumento " +
                    "AND sociedad = @sociedad";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
                    comando.Parameters.AddWithValue("@sociedad", sociedad);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }

            return resultado;
        }
    }
}
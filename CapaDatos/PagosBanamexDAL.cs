using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class PagosBanamexDAL
    {
        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosBanamex> obtenerPagos(int sociedad, int usuario)
        {
            List<PagosBanamex> listaPagosBanamex = new List<PagosBanamex>();
            string formatoDocumento = "0000000";
            string formatoMonto = "000000000000.##";
            string consulta = "";
            string filtroUsuario = "";

            if (sociedad == 1)
            {
                if (usuario == 1)
                {
                    filtroUsuario = " AND (T0.UserSign = 32 OR T0.UserSign = 82) ";
                }
                else if (usuario == 2)
                {
                    filtroUsuario = " AND (T0.UserSign = 87 OR T0.UserSign = 19) ";
                }

                consulta = "SELECT " +
                            "'03'                        AS 'Transaccion'," +
                            "'01'                        AS 'TipoCuentaOrigen'," +
                            "'4861'                      AS 'SucursalOrigen'," +
                            "'00000000000000084780'      AS 'CuentaOrigen'," +
                            "'01'                        AS 'TipoCuentaDestino'," +
                            "T1.[DflBranch]              AS 'SucursalCuentaDestino'," +
                            "T1.[DflAccount]             AS 'CuentaDestino'," +
                            "T0.[TrsfrSum]               AS 'Importe'," +
                            "'001'                       AS 'MonedaTransaccion'," +
                            "LEFT(CONCAT(T0.DocEntry, T0.[cardname] collate SQL_Latin1_General_CP1_CI_AS), 24) AS 'Descripcion'," +
                            "LEFT(CONCAT(T0.DocEntry, T0.[cardname] collate SQL_Latin1_General_CP1_CI_AS), 34) AS 'Concepto'," +
                            "T0.[docnum]                 AS 'Referencia'," +
                            "'000'                       AS 'Moneda'," +
                            "'000000'                    AS 'FechaAplicacion'," +
                            "'0000'                      AS 'HoraAplicacion'," +
                            "T0.DocType," +
                            "T0.[DocDate]                AS 'Fecha'," +
                            "T0.UserSign," +
                            "T0.BankCode," +
                            "T0.DocEntry                AS 'numeroDocumento'," +
                            "T0.U_EstatusPago " +
                            "FROM TSSL_Mielmex.dbo.OVPM T0 " +
                "INNER JOIN TSSL_Mielmex.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
                "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                //"WHERE  T0.[u_estatuspago] = 'Actualizado' " +
                        "AND T0.[trsfracct] = '11101112' " +
                        "AND T0.Canceled = 'N' " +
                        "AND T1.[bankcode] = '0002' " +
                        "AND T0.UserSign <> 8 " +
                        //"AND T0.DocDate = '2022-06-01'" +
                        "AND T1.[DflSwift] <> '' " +
                        filtroUsuario +
                        "AND (T0.DocType = 'S' OR T0.DocType = 'C') ORDER BY T0.[docnum]";
            }
            else if (sociedad == 2)
            {
                if (usuario == 1)
                {
                    filtroUsuario = " AND (T0.UserSign = 32 OR T0.UserSign = 84) ";
                }
                else if (usuario == 2)
                {
                    filtroUsuario = " AND (T0.UserSign = 89 OR T0.UserSign = 19) ";
                }

                consulta = "SELECT " +
                            "'03'                        AS 'Transaccion'," +
                            "'01'                        AS 'TipoCuentaOrigen'," +
                            "'0573'                      AS 'SucursalOrigen'," +
                            "'00000000000007947221'      AS 'CuentaOrigen'," +
                            "'01'                        AS 'TipoCuentaDestino'," +
                            "T1.[DflBranch]              AS 'SucursalCuentaDestino'," +
                            "T1.[DflAccount]             AS 'CuentaDestino'," +
                            "T0.[TrsfrSum]               AS 'Importe'," +
                            "'001'                       AS 'MonedaTransaccion'," +
                            "LEFT(CONCAT(T0.DocEntry, T0.[cardname] collate SQL_Latin1_General_CP1_CI_AS), 24) AS 'Descripcion'," +
                            "LEFT(CONCAT(T0.DocEntry, T0.[cardname] collate SQL_Latin1_General_CP1_CI_AS), 34) AS 'Concepto'," +
                            "T0.[docnum]                 AS 'Referencia'," +
                            "'000'                       AS 'Moneda'," +
                            "'000000'                    AS 'FechaAplicacion'," +
                            "'0000'                      AS 'HoraAplicacion'," +
                            "T0.[DocDate]                AS 'Fecha'," +
                            "T0.DocType," +
                            "T0.UserSign," +
                            "T0.BankCode," +
                            "T0.DocEntry                AS 'numeroDocumento'," +
                            "T0.U_EstatusPago " +
                "FROM TSSL_Naturasol.dbo.OVPM T0 " +
                        "INNER JOIN TSSL_Naturasol.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
                "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                //"WHERE  T0.[u_estatuspago] = 'Actualizado' " +
                        "AND T0.[trsfracct] = '11101140' " +
                        "AND T0.Canceled = 'N' " +
                        "AND T0.UserSign <> 8 " +
                        "AND T1.[bankcode] = '0002' " +
                        //"AND T0.DocDate = '2022-06-01'" +
                        "AND T1.[DflAccount] <> '' " +
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

                            if (fechaDias <= fechaDocumento)
                            {
                                listaPagosBanamex.Add(new PagosBanamex()
                                {
                                    sucursalCuentaDestino = datareader["SucursalCuentaDestino"].ToString().PadLeft(4, '0'),
                                    cuentaDestino = datareader["CuentaDestino"].ToString().PadLeft(20, '0'),
                                    importe = Convert.ToDecimal(datareader["Importe"].ToString()).ToString("F").ToString().PadLeft(14, '0').Replace(".", string.Empty).PadLeft(14, '0'),
                                    descripcion = datareader["Descripcion"].ToString().Replace(".", string.Empty).PadRight(24, ' '),
                                    concepto = datareader["Concepto"].ToString().Replace(".", string.Empty).PadRight(34, ' '),
                                    referencia = datareader["Referencia"].ToString().Replace(".", string.Empty).PadLeft(10, '0'),
                                    numeroDocumento = datareader["NumeroDocumento"].ToString(),
                                    transaccion = datareader["Transaccion"].ToString(),
                                    tipoCuentaOrigen = datareader["TipoCuentaOrigen"].ToString(),
                                    sucursalOrigen = datareader["SucursalOrigen"].ToString(),
                                    cuentaOrigen = datareader["CuentaOrigen"].ToString(),
                                    tipoCuentaDestino = datareader["TipoCuentaDestino"].ToString(),
                                    monedaTransaccion = datareader["MonedaTransaccion"].ToString(),
                                    moneda = datareader["Moneda"].ToString(),
                                    fechaAplicacion = datareader["FechaAplicacion"].ToString(),
                                    horaAplicacion = datareader["HoraAplicacion"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listaPagosBanamex;
        }

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public String buscarClaveBanco(string numeroBanco)
        {
            string banco = "0";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT claveBanco " +
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

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR DE LA BASE DE DESARROLLO
        public List<PagosBanamex> buscarPagosPendientesSAP()
        {
            List<PagosBanamex> listaPagosBanamex = new List<PagosBanamex>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "[numeroDocumento], " +
                        "[sociedad], " +
                        "[estatus], " +
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
                            listaPagosBanamex.Add(new PagosBanamex()
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

            return listaPagosBanamex;
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

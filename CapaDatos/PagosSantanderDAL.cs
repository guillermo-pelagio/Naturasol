using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos
{
    public class PagosSantanderDAL
    {
        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR
        public List<PagosSantander> obtenerPagos(int sociedad)
        {
            List<PagosSantander> listaPagosSantander = new List<PagosSantander>();
            string formatoDocumento = "0000000";
            string formatoMonto = "0000000000000.##";
            string consulta = "";
            string filtroUsuario = "";
            string filtroTipoPago = "";


            consulta = "SELECT T1.[DflSwift] AS 'Beneficiario'," +
                    "T1.[DflAccount]             AS 'Beneficiario2'," +
                    "'136217451'            AS 'Ordenante'," +
                    "'MXP'                   AS 'Divisa'," +
                    "T0.[TrsfrSum]           AS 'Importe'," +
                    "LEFT(T0.[cardname], 30) AS 'Titular'," +
                    "'40'                    AS 'Cuenta'," +
                    "T1.[bankcode]           AS 'Banco', " +
                    "LEFT(T0.[trsfrref], 30) AS 'Pago'," +
                    "T0.[docnum]             AS 'Referencia'," +
                    "T0.[comments]          AS 'Referencia2', " +
                    "T0.[DocDate]            AS 'Fecha'," +
                    "T1.LicTradNum          AS 'RFC'," +
                    "'H'                     AS 'Disponibilidad' " +
            "FROM TSSL_Naturasol.dbo.OVPM T0 " +
                    "INNER JOIN TSSL_Naturasol.dbo.OCRD T1 ON T0.[cardcode] = T1.[cardcode] " +
            "WHERE  T0.[u_estatuspago] = 'Por Procesar' " +
                    //"WHERE  T0.[u_estatuspago] = 'Actualizado' " +
                    "AND T0.[trsfracct] = '11101141' " +
                    "AND T0.Canceled = 'N' " +
                    //"AND T0.DocDate = '2022-06-01'" +
                    "AND (T0.DocType = 'S' OR T0.DocType = 'C') ORDER BY T0.[docnum]";

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

                            if (fechaDias <= fechaDocumento)
                            {
                                listaPagosSantander.Add(new PagosSantander()
                                {
                                    disponibilidad = datareader["Disponibilidad"].ToString(),
                                    numeroBanco = datareader["Banco"].ToString(),
                                    tipoCuenta = datareader["Cuenta"].ToString(),
                                    titular = datareader["Titular"].ToString() == null ? "".PadRight(40) : datareader["Titular"].ToString().PadRight(40),
                                    importe = Convert.ToDecimal(datareader["Importe"].ToString()).ToString("F").ToString().PadLeft(16, '0'),
                                    divisa = datareader["Divisa"].ToString(),
                                    cuentaOrdenante = datareader["Ordenante"].ToString().PadLeft(18, '0'),
                                    RFC = datareader["RFC"].ToString().PadLeft(14, ' '),
                                    cuentaBeneficiario = datareader["Beneficiario"].ToString().PadLeft(18, '0'),
                                    motivoPago = (datareader["Referencia2"].ToString() == null ? "".PadRight(60) : datareader["Referencia2"].ToString().PadRight(60)).Substring(0, 19),
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

            return listaPagosSantander;
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

        //METODO PARA OBTENER TODOS LOS PAGOS PENDIENTES POR PROCESAR DE LA BASE DE DESARROLLO
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
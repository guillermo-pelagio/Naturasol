using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ProveedoresDAL
    {
        //METODO DE BUSQUEDA DE USUARIO EN EL INICIO DE SESION
        public SesionProveedor inicioSesion(Proveedores proveedor)
        {
            SesionProveedor sesion = new SesionProveedor();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT OCPR.FirstName, OCPR.MiddleName, OCPR.E_MailL, OCPR.Password, OCRD.CardCode, OCRD.CardName, OCRD.LicTradNum FROM TSSL_NATURASOL.DBO.OCPR JOIN TSSL_NATURASOL.DBO.OCRD ON OCRD.CardCode = OCPR.CardCode WHERE U_TS_CuentaTerceros = 'Y' AND OCPR.E_MailL=@nameUsuario AND OCPR.Password=@contrasena";
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@nameUsuario", proveedor.nameUsuario);
                    comando.Parameters.AddWithValue("@contrasena", proveedor.contrasena);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesion.correo = Convert.ToString(datareader["E_MailL"].ToString());
                            sesion.nombreCompleto = datareader["FirstName"].ToString() + " " + datareader["MiddleName"].ToString();
                            sesion.contrasena = datareader["Password"].ToString();

                            sesion.numeroSocio = datareader["CardCode"].ToString();
                            sesion.nombreSocio = datareader["CardName"].ToString();
                            sesion.RFC = datareader["LicTradNum"].ToString();
                            sesion.estatus = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return sesion;
        }

        public void actualizarEstatus(string estatus, string idPortal)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [desarrolloWeb].[dbo].[proveedoresFacturas] " +
                    "SET estatus=@estatus where idregistro=@idRegistro ";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@estatus", estatus);
                    comando.Parameters.AddWithValue("@idRegistro", idPortal);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public List<EntradaMaterial> obtenerEntradas(string sociedad, string CardCode)
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select distinct T2.estatus, DocEntry, T1.DocNum, CONVERT(date, DocDate) as dates, CardCode, CardName, NumAtCard, DocTotal, DocTotalFC, VatSum, VatSumFC, DocCur from " + sociedad + ".dbo.OPDN T1 " +
                        "left join DesarrolloWeb.dbo.proveedoresFacturas t2 on T1.DocNum = T2.DocNum " +
                        "where (T1.docstatus='O') and CardCode=@CardCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@CardCode", CardCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["dates"].ToString()).ToShortDateString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                DocTotal = string.Format("{0:C}", Convert.ToDecimal(datareader["DocTotal"].ToString())),
                                DocTotalFC = string.Format("{0:C}", Convert.ToDecimal(datareader["DocTotalFC"].ToString())),
                                VatSum = string.Format("{0:C}", Convert.ToDecimal(datareader["VatSum"].ToString())),
                                VatSumFC = string.Format("{0:C}", Convert.ToDecimal(datareader["VatSumFC"].ToString())),
                                DocCur = datareader["DocCur"].ToString(),
                                estatus = datareader["estatus"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntradaMaterial>();
            }

            return listaDocumentos;
        }

        public List<EntradaMaterial> obtener_entradas_global(string sociedad)
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select distinct T2.estatus, DocEntry, T1.DocNum, CONVERT(date, DocDate) as dates, CardCode, CardName, NumAtCard, DocTotal, DocTotalFC, VatSum, VatSumFC, DocCur from " + sociedad + ".dbo.OPDN T1 " +
                        "left join DesarrolloWeb.dbo.proveedoresFacturas t2 on T1.DocNum = T2.DocNum " +
                        "where (T1.docstatus='O')";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);                    
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["dates"].ToString()).ToShortDateString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                DocTotal = string.Format("{0:C}", Convert.ToDecimal(datareader["DocTotal"].ToString())),
                                DocTotalFC = string.Format("{0:C}", Convert.ToDecimal(datareader["DocTotalFC"].ToString())),
                                VatSum = string.Format("{0:C}", Convert.ToDecimal(datareader["VatSum"].ToString())),
                                VatSumFC = string.Format("{0:C}", Convert.ToDecimal(datareader["VatSumFC"].ToString())),
                                DocCur = datareader["DocCur"].ToString(),
                                estatus = datareader["estatus"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntradaMaterial>();
            }

            return listaDocumentos;
        }
        

        public List<FacturaProveedor> obtenerFacturas(string sociedad, string CardCode)
        {
            List<FacturaProveedor> listaDocumentos = new List<FacturaProveedor>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select T2.DocDate AS 'FechaPago', T1.DocEntry, DOCSTATUS, T1.DocNum, CONVERT(date, T1.DocDate) as dates, T1.CardCode, T1.CardName, NumAtCard, T1.DocTotal, T1.DocTotalFC, DocCur, T1.DocTotal-PaidToDate AS 'Pendiente',T1.DocTotalFC-PaidFC as 'PendienteFC' " +
                        "from " + sociedad + ".dbo.OPCH T1 " +
                        "LEFT JOIN " + sociedad + ".dbo.OVPM T2 ON T2.DocEntry = T1.ReceiptNum " +
                        "where T1.CardCode = @CardCode AND T1.CANCELED NOT IN ('Y','C') ORDER BY T1.DocEntry DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@CardCode", CardCode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            string fecha = datareader["FechaPago"].ToString();
                            listaDocumentos.Add(new FacturaProveedor()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                DocStatus = datareader["DocStatus"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["dates"].ToString()).ToShortDateString(),
                                FechaPago = datareader["FechaPago"].ToString() == "" ? "-" : Convert.ToDateTime(datareader["FechaPago"].ToString()).ToShortDateString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                NumAtCard = datareader["NumAtCard"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                Pendiente = datareader["Pendiente"].ToString(),
                                PendienteFC = datareader["PendienteFC"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<FacturaProveedor>();
            }

            return listaDocumentos;
        }

        public int guardarRegistroProveedor(EntradaMaterial entradaMaterial)
        {
            int idRegistro = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[proveedoresFacturas] " +
                   "([DocNum] " +
                   ",[monto] " +
                   ",[moneda] " +
                   ",[estatus] " +
                   ",[archivo] " +
                   ") " +
                    "VALUES" +
                   "(@DocNum," +
                   "@monto," +
                   "@moneda," +
                   "@estatus," +
                   "@archivo" +
                   ");";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", entradaMaterial.DocNum);
                    comando.Parameters.AddWithValue("@monto", entradaMaterial.montoPortal);
                    comando.Parameters.AddWithValue("@moneda", entradaMaterial.monedaPortal);
                    comando.Parameters.AddWithValue("@estatus", "1");
                    comando.Parameters.AddWithValue("@archivo", entradaMaterial.archivoPortal);
                    comando.CommandType = CommandType.Text;

                    idRegistro = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idRegistro = -1;
            }

            return idRegistro;
        }

        public List<EntradaMaterial> obtenerPendientesRevisar()
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.idRegistro, T1.DocNum, T1.monto, T1.moneda, T1.estatus, T1.archivo, T2.DocCur, T2.DocTotal, T2.DocTotalFC, T2.VatSum, T2.VatSumFC, T2.LicTradNum from DesarrolloWeb.dbo.[proveedoresFacturas] T1 JOIN TSSL_NATURASOL.dbo.OPDN T2 ON T1.DOCNUM=T2.DOCNUM WHERE T1.estatus=1 ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                idPortal = datareader["idRegistro"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
                                montoPortal = datareader["monto"].ToString(),
                                monedaPortal = datareader["moneda"].ToString(),
                                estatus = datareader["estatus"].ToString(),
                                archivoPortal = datareader["archivo"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),
                                DocTotal = datareader["DocTotal"].ToString(),
                                DocTotalFC = datareader["DocTotalFC"].ToString(),
                                VatSum = datareader["VatSum"].ToString(),
                                VatSumFC = datareader["VatSumFC"].ToString(),
                                LicTradNum = datareader["LicTradNum"].ToString()

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntradaMaterial>();
            }

            return listaDocumentos;
        }
    }
}

using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class EntradasMaterialDAL
    {

        public List<PreciosEntrega> obtenerDataPEDetalle(string docEntry, string proveedor)
        {
            List<PreciosEntrega> listaDocumentos = new List<PreciosEntrega>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select T0.DocEntry, T1.CardCode, U_Inicio, u_fin, U_Referencia, U_Proveedor, RIGHT('00' + U_Concepto, 2) AS 'U_Concepto', U_Monto, U_Moneda  " +
                                         "from TSSL_NATURASOL.dbo.[@7_GASTOS_IMP] T0 " +
                                         "join TSSL_NATURASOL.dbo.OPOR T1 on T1.DocEntry = T0.U_OC " +
                                         "JOIN TSSL_NATURASOL.dbo.[@8_GASTOS_DET] T2 ON T2.DocEntry = T0.DocEntry " +
                                         "WHERE convert(date, DATEADD(DAY, 0, GETDATE()))>= U_Inicio AND U_Fin>= convert(date, DATEADD(DAY, 0, GETDATE())) AND T0.DocEntry = @DocEntry and U_Proveedor=@U_Proveedor";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocEntry", docEntry);
                    comando.Parameters.AddWithValue("@U_Proveedor", proveedor);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new PreciosEntrega()
                            {
                                docEntry = datareader["DocEntry"].ToString(),
                                fechaFin = datareader["u_fin"].ToString(),
                                fechaInicio = datareader["U_Inicio"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                referencia = datareader["U_Referencia"].ToString(),
                                proveedor = datareader["U_Proveedor"].ToString(),
                                monto = datareader["U_Monto"].ToString(),
                                moneda = datareader["U_Moneda"].ToString(),
                                concepto = datareader["U_Concepto"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<PreciosEntrega>();
            }

            return listaDocumentos;
        }

        public List<PreciosEntrega> obtenerDataPE(string docnum)
        {
            List<PreciosEntrega> listaDocumentos = new List<PreciosEntrega>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DISTINCT t0.DocEntry, T1.CardCode, U_Inicio, u_fin, U_Referencia, T1.DocNum, T2.Rate, T12.U_Proveedor, T12.U_Moneda " +
                                        "FROM  TSSL_NATURASOL.dbo.[@7_GASTOS_IMP] T0 " +
                                        "JOIN  TSSL_NATURASOL.dbo.OPOR T1 on T1.docentry = T0.U_OC " +
                                        "JOIN  TSSL_NATURASOL.dbo.PDN1 T8 on T8.BaseEntry = T1.DocEntry " +
                                        "JOIN  TSSL_NATURASOL.dbo.OPDN T9 on T9.DocEntry = T8.DocEntry " +
                                        "JOIN  TSSL_NATURASOL.dbo.ORTT T2 ON T2.RateDate = Convert(date, GETDATE()) AND T2.Currency = 'USD' " +
                                        "JOIN  TSSL_NATURASOL.dbo.[@8_GASTOS_DET] T12 ON T12.DocEntry = T0.DocEntry " +
                                        "WHERE convert(date, DATEADD(DAY, 0, GETDATE()))>= U_Inicio AND U_Fin>= convert(date, DATEADD(DAY, 0, GETDATE())) AND T9.docnum=@docnum AND T9.NumAtCard=U_Referencia";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", docnum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new PreciosEntrega()
                            {
                                docEntry = datareader["DocEntry"].ToString(),
                                fechaFin = datareader["u_fin"].ToString(),
                                TC = datareader["Rate"].ToString(),
                                fechaInicio = datareader["U_Inicio"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                referencia = datareader["U_Referencia"].ToString(),
                                OC = datareader["docnum"].ToString(),
                                moneda = datareader["U_Moneda"].ToString(),
                                proveedor = datareader["U_Proveedor"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<PreciosEntrega>();
            }

            return listaDocumentos;
        }

        public List<OrdenCompra> obtenerDataPEDetalleOC(string entrada)
        {
            List<OrdenCompra> listaDocumentos = new List<OrdenCompra>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.DocEntry, T1.DocCur, T4.LineNum, T4.Quantity, T4.WhsCode " +
                                         "from TSSL_NATURASOL.dbo.OPDN T1 " +
                                         "join TSSL_NATURASOL.dbo.PDN1 T4 on T4.DocEntry = T1.DocEntry " +
                                         "WHERE T1.docnum = @docnum ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docnum", entrada);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new OrdenCompra()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                LineNum = datareader["LineNum"].ToString(),
                                DocCur = datareader["DocCur"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString()
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

        public int actualizarEstatus(EntradaMaterial entradaMaterial)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";

                    string consulta = "update " + sociedad + ".DBO.OBTN set status=@estatus where  itemcode=@itemcode and distnumber = @distnumber";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.Parameters.AddWithValue("@estatus", entradaMaterial.Status);
                    comando.Parameters.AddWithValue("@itemcode", entradaMaterial.ItemCode);
                    comando.Parameters.AddWithValue("@distnumber", entradaMaterial.BatchNum);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    docEntry = 1;
                }
            }
            catch (Exception ex)
            {
                docEntry = 0;
            }

            return docEntry;
        }

        public int actualizar_lote(EntradaMaterial entradaMaterial)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";

                    string consulta = "update " + sociedad + ".DBO.OBTN set u_loteproveedor=@loteproveedor, expdate=@expdate where itemcode=@itemcode and distnumber = @distnumber";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.Parameters.AddWithValue("@loteproveedor", entradaMaterial.LoteProveedor);
                    comando.Parameters.AddWithValue("@itemcode", entradaMaterial.ItemCode);
                    comando.Parameters.AddWithValue("@expdate", entradaMaterial.ExpDate);
                    comando.Parameters.AddWithValue("@distnumber", entradaMaterial.BatchNum);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                    docEntry = 1;
                }
            }
            catch (Exception ex)
            {
                docEntry = 0;
            }

            return docEntry;
        }

        //METODO PARA OBTENER TODOS LA ENTRADA REALIZADA POR INTERCOMPANIA, PARA ACTUALIZAR LA ENTREGA
        public List<EntradaMaterial> obtenerEntradasIntercompania(EntregaMercancia entregaMercancia, string sociedadDestino)
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DocNum " +
                        "FROM " + sociedadDestino + ".dbo.OPDN " +
                        "WHERE U_NoCaja = @U_INT_SOREL";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@U_INT_SOREL", entregaMercancia.DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                Sociedad = sociedadDestino
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

        //METODO PARA OBTENER EL DETALLE DE UNA ENTRADA
        public List<EntradaMaterialDetalle> obtenerEntrada(string sociedad, string DocNum)
        {
            List<EntradaMaterialDetalle> listaDocumentos = new List<EntradaMaterialDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "OPDN.CardCode, " +
                        "OPDN.DocType, " +
                        "PDN1.DocEntry, " +
                        "PDN1.ItemCode, " +
                        "PDN1.Quantity, " +
                        "PDN1.TaxCode, " +
                        "PDN1.WhsCode, " +
                        "PDN1.LineNum, " +
                        "PDN1.Price, " +
                        "PDN1.Currency " +
                        "FROM " + sociedad + ".dbo.PDN1 " +
                        "JOIN " + sociedad + ".dbo.OPDN ON OPDN.DocEntry=PDN1.DocEntry " +
                        "WHERE OPDN.DocNum = @DocNum";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", DocNum);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterialDetalle()
                            {
                                DocEntry = datareader["DocEntry"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                DocType = datareader["DocType"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                TaxCode = datareader["TaxCode"].ToString(),
                                LineNum = datareader["LineNum"].ToString(),
                                Price = datareader["Price"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString(),
                                Currency = datareader["Currency"].ToString(),
                                CardCode = datareader["CardCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<EntradaMaterialDetalle>();
            }

            return listaDocumentos;
        }

        //S21-REQUERIDO-METODO PARA OBTENER ENTRADAS INTERNACIONALES
        public List<EntradaMaterial> obtenerEntradaInternacional()
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM  DesarrolloWeb.dbo.ENTRADAINTERNACIONAL T0 WHERE (T0.IndustryC = 7) AND (CAST(T0.DocDate AS DATE) = CAST(DATEADD(DAY, 0, GETDATE()) AS DATE)) AND (T0.U_FechaCobro IS NULL) ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                DocEntry = datareader["docEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
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
                listaDocumentos = new List<EntradaMaterial>();
            }

            return listaDocumentos;
        }

        public List<EntradaMaterial> obtenerEntradaInternacionalPE()
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DesarrolloWeb.dbo.ENTRADAINTERNACIONAL T0 WHERE (T0.IndustryC = 7) AND (CAST(T0.DocDate AS DATE) = CAST(DATEADD(DAY, -1, GETDATE()) AS DATE)) AND (T0.U_FechaCobro IS NOT NULL) AND T0.U_TS_DetalleErrorAdd IS NULL ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                DocEntry = datareader["docEntry"].ToString(),
                                DocNum = datareader["DocNum"].ToString(),
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
                listaDocumentos = new List<EntradaMaterial>();
            }

            return listaDocumentos;
        }


        public SesionEntradaMaterial validarOCArriboProveedor(SesionEntradaMaterial entradas)
        {
            SesionEntradaMaterial sesionEntradaMaterial = new SesionEntradaMaterial();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT " +
                        "OPOR.CardCode, " +
                        "OPOR.CardName, " +
                        "OPOR.DocNum " +
                        "FROM " + entradas.Sociedad + ".dbo.OPOR " +
                        "WHERE OPOR.DocNum = @DocNum";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", entradas.OCRelacionada);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            sesionEntradaMaterial.CardName = datareader["CardName"].ToString();
                            sesionEntradaMaterial.DocNum = datareader["DocNum"].ToString();
                            sesionEntradaMaterial.CardCode = datareader["CardCode"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sesionEntradaMaterial = new SesionEntradaMaterial();
            }

            return sesionEntradaMaterial;
        }

        public int confirmarOCArriboProveedor(SesionEntradaMaterial sesion)
        {
            int idUsuario = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [dbo].[arribosMercancia]" +
                   "([DocNum]" +
                   ",[CardCode]" +
                   ",[CardName]" +
                   ",[DocDate]" +
                   ",[DocTime]" +
                   ",[Ubicacion]" +
                   ",[Sociedad])" +
                    "VALUES" +
                   "(@DocNum," +
                   "@CardCode," +
                   "@CardName," +
                   "@DocDate," +
                   "@DocTime," +
                   "@Ubicacion," +
                   "@Sociedad);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@DocNum", sesion.DocNum);
                    comando.Parameters.AddWithValue("@CardCode", sesion.CardCode);
                    comando.Parameters.AddWithValue("@CardName", sesion.CardName);
                    comando.Parameters.AddWithValue("@DocDate", sesion.DocDate);
                    comando.Parameters.AddWithValue("@DocTime", sesion.DocTime);
                    comando.Parameters.AddWithValue("@Ubicacion", sesion.Ubicacion);
                    comando.Parameters.AddWithValue("@Sociedad", sesion.Sociedad);

                    comando.CommandType = CommandType.Text;

                    idUsuario = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idUsuario = 0;
            }

            return idUsuario;
        }

        public List<EntradaMaterial> obtenerLotesDetalle(string articulo, string descripcion, string almacen, string lote)
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (articulo == "" && almacen == "" && lote == "" && descripcion == "")
                    {
                        consulta = "" +
                            "SELECT DISTINCT TOP 0 " +
                            "'Artículo' = T0.ItemCode, " +
                            "'Descripción del Artículo' = T2.ItemName, " +
                            "'Lote' = T0.DistNumber, " +
                            "'Fecha de Caducidad' = T0.ExpDate, " +
                            "'Estatus lote' = T0.Status, " +
                            "'Lote proveedor' = T0.U_LOTEPROVEEDOR, " +
                            "'Almacén' = T6.WhsCode, " +
                            "'Nombre del Almacén' = T6.WhsName, " +
                            "'En Stock' = T3.OnHandQty, " +
                            "'UM' = T2.InvntryUom, " +
                            "'Tipo de Caducidad' = case  " +
                            "when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                            "when T0.ExpDate >= (GETDATE()) and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                            "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                            "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' " +
                            "else 'SIN CADUCIDAD'end, " +
                            "'Familia' = T5.ItmsGrpNam, " +
                            "T2.U_familia " +
                            "FROM " +
                            "TSSL_NATURASOL.dbo.OBTN T0 INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OWHS T6 ON T1.WhsCode = T6.WhsCode INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
                            "WHERE " +
                            "T6.Inactive = 'N' AND " +
                            "T1.Quantity > 0 AND " +
                            "T3.OnHandQty > 0 ";
                    }
                    else
                    {
                        consulta = "" +
                            "SELECT DISTINCT " +
                            "'Artículo' = T0.ItemCode, " +
                            "'Descripción del Artículo' = T2.ItemName, " +
                            "'Lote' = T0.DistNumber, " +
                            "'Fecha de Caducidad' = T0.ExpDate, " +
                            "'Estatus lote' = T0.Status, " +
                            "'Lote proveedor' = T0.U_LOTEPROVEEDOR, " +
                            "'Almacén' = T6.WhsCode, " +
                            "'Nombre del Almacén' = T6.WhsName, " +
                            "'En Stock' = T3.OnHandQty, " +
                            "'UM' = T2.InvntryUom, " +
                            "'Tipo de Caducidad' = case  " +
                            "when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                            "when T0.ExpDate >= (GETDATE()) and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                            "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                            "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' " +
                            "else 'SIN CADUCIDAD'end, " +
                            "'Familia' = T5.ItmsGrpNam, " +
                            "T2.U_familia " +
                            "FROM " +
                            "TSSL_NATURASOL.dbo.OBTN T0 INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OWHS T6 ON T1.WhsCode = T6.WhsCode INNER JOIN " +
                            "TSSL_NATURASOL.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
                            "WHERE " +
                            "T6.Inactive = 'N' AND " +
                            "T1.Quantity > 0 AND " +
                            "T3.OnHandQty > 0 " +
                            "AND T0.ItemCode LIKE '%" + articulo + "%' and T2.ItemName LIKE '%" + descripcion + "%' " +
                            "and T6.WhsName LIKE '%" + almacen + "%'  and T0.DistNumber LIKE '%" + lote + "%' ";
                    }



                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {
                                nombreAlmacen = datareader["Nombre del Almacén"].ToString(),
                                familia = datareader["Familia"].ToString(),
                                tipoCaducidad = datareader["Tipo de Caducidad"].ToString(),
                                um = datareader["UM"].ToString(),
                                ItemCode = datareader["Artículo"].ToString(),
                                ItemName = datareader["Descripción del Artículo"].ToString(),
                                WhsCode = datareader["Almacén"].ToString(),
                                BatchNum = datareader["Lote"].ToString(),
                                Quantity = datareader["En Stock"].ToString(),
                                ExpDate = Convert.ToDateTime(datareader["Fecha de Caducidad"].ToString()).ToString("yyyy-MM-dd"),
                                LoteProveedor = datareader["Lote proveedor"].ToString(),
                                Status = datareader["Estatus lote"].ToString()
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

        public List<EntradaMaterial> obtenerEntradasLote()
        {
            List<EntradaMaterial> listaDocumentos = new List<EntradaMaterial>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";                    
                        consulta =
                            "SELECT * FROM DESARROLLOWEB.DBO.[A7 - Entradas por compra con caducidad] WHERE DocDate>=DATEADD(DAY,-30,GETDATE()) ORDER BY DocNum DESC ";
                    
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new EntradaMaterial()
                            {                                
                                DocNum = datareader["DocNum"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["DocDate"].ToString()).ToString("yyyy-MM-dd"),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),

                                nombreAlmacen = datareader["WhsName"].ToString(),
                                ItemCode = datareader["ItemCode"].ToString(),
                                ItemName = datareader["ItemName"].ToString(),
                                WhsCode = datareader["WhsCode"].ToString(),
                                BatchNum = datareader["BatchNum"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                ExpDate = Convert.ToDateTime(datareader["ExpDate"].ToString()).ToString("yyyy-MM-dd"),
                                LoteProveedor = datareader["U_LoteProveedor"].ToString(),
                                analisisMicro = datareader["U_TS_STCCCode"].ToString(),                                
                                Status = datareader["Status"].ToString()
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
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class InventarioCaducoDAL
    {
        public List<InventarioCaduco> obtenerClientesKAM()
        {
            List<InventarioCaduco> listaClientes = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select DISTINCT U_CLIENTE, CardName, U_Correo1, U_Correo2, U_Correo3 from TSSL_NATURASOL.dbo.[@3_CLIENTES_CAD] T1 join TSSL_NATURASOL.dbo.OCRD T2 ON T2.CardCode = T1.U_Cliente ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaClientes.Add(new InventarioCaduco()
                            {
                                U_CLIENTE = datareader["U_CLIENTE"].ToString(),
                                cardname = datareader["cardname"].ToString(),
                                U_CORREO1 = datareader["U_CORREO1"].ToString(),
                                U_CORREO2 = datareader["U_CORREO2"].ToString(),
                                U_CORREO3 = datareader["U_CORREO3"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaClientes = new List<InventarioCaduco>();
            }

            return listaClientes;
        }

        public List<InventarioCaduco> obtenerAjustesMes(string basesDatos, int ajustes, int tipoAjustes)
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string tabla = "";
                    if (ajustes == 0)
                    {
                        tabla = "IGN";
                    }
                    else
                    {
                        tabla = "IGE";
                    }

                    string consulta = "select itemcode, dscription, sum(quantity) as Quantity, sum(T1.LineTotal) as Total, unitMsr" +
                        " from " + basesDatos + ".DBO." + tabla + "1 T1" +
                        " join " + basesDatos + ".DBO.o" + tabla + " T0 on T0.DocEntry = T1.DocEntry" +
                        " where T0.DocDate >= DATEADD(month, -1, getdate()) and BaseRef is null and U_INV_TIPOMOV = @tipoAjustes" +
                        " group by U_INV_TIPOMOV, itemcode, dscription, unitMsr order by quantity desc, ItemCode";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@tipoAjustes", tipoAjustes);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new InventarioCaduco()
                            {
                                Artículo = datareader["itemcode"].ToString(),
                                Descripción = datareader["dscription"].ToString(),
                                Stock = datareader["Quantity"].ToString(),
                                valorInventario = datareader["Total"].ToString(),
                                UM = datareader["unitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<InventarioCaduco>();
            }

            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerArticulosSinUbicacion()
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM ARTICULOSSINUBICACION ORDER BY ItemCode, WhsCode";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["ItemCode"].ToString(),
                                Descripción = datareader["ItemName"].ToString(),
                                Almacén = datareader["WhsCode"].ToString(),
                                NombreA = datareader["WhsName"].ToString(),
                                UM = datareader["InvntryUom"].ToString(),
                                Stock = datareader["Existencia"].ToString(),
                                Ubicación = datareader["BinCode"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<InventarioCaduco>();
            }

            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerArticulosCaducar(string u_CLIENTE)
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT U_ARTICULO, U_DIAS, U_PRIMERALERTA, U_SEGUNDAALERTA, U_CORREO1, U_CORREO2, U_CORREO3 FROM [TSSL_NATURASOL].[dbo].[@3_CLIENTES_CAD] WHERE U_CLIENTE = @cliente ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@cliente", u_CLIENTE);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaArticulos.Add(new InventarioCaduco()
                            {
                                U_CLIENTE = u_CLIENTE,
                                U_ARTICULO = datareader["U_ARTICULO"].ToString(),
                                U_DIAS = datareader["U_DIAS"].ToString(),
                                U_PRIMER = datareader["U_PRIMERALERTA"].ToString(),
                                U_SEGUNDA = datareader["U_SEGUNDAALERTA"].ToString(),
                                U_CORREO1 = datareader["U_CORREO1"].ToString(),
                                U_CORREO2 = datareader["U_CORREO2"].ToString(),
                                U_CORREO3 = datareader["U_CORREO3"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaArticulos = new List<InventarioCaduco>();
            }

            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducas(string u_ARTICULO, string dias)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * " +
    "FROM(SELECT DISTINCT 'Sociedad' = 'MIEL MEX', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                     "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end " +
         "FROM TSSL_MielMex.dbo.OBTN T0 " +
          "INNER JOIN TSSL_MielMex.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
          "INNER JOIN TSSL_MielMex.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
          "INNER JOIN TSSL_MielMex.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
          "INNER JOIN TSSL_MielMex.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
          "INNER JOIN TSSL_MielMex.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
          "INNER JOIN TSSL_MielMex.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
          "INNER JOIN TSSL_MielMex.dbo.OLCT T7 ON T6.Location = T7.Code " +
          "INNER JOIN TSSL_MielMex.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
     "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 " +
     "UNION ALL " +
     "SELECT DISTINCT 'Sociedad' = 'NATURASOL', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                      "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                  "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                  "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                  "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end " +
         "FROM TSSL_Naturasol.dbo.OBTN T0 " +
          "INNER JOIN TSSL_Naturasol.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
          "INNER JOIN TSSL_Naturasol.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
          "INNER JOIN TSSL_Naturasol.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
          "INNER JOIN TSSL_Naturasol.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
          "INNER JOIN TSSL_Naturasol.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
          "INNER JOIN TSSL_Naturasol.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
          "INNER JOIN TSSL_Naturasol.dbo.OLCT T7 ON T6.Location = T7.Code " +
          "INNER JOIN TSSL_Naturasol.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
     "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 " +
     "UNION ALL " +
     "SELECT DISTINCT 'Sociedad' = 'EVI', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                            "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                            "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                            "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end " +
         "FROM TSSL_distribuidora.dbo.OBTN T0 " +
          "INNER JOIN TSSL_distribuidora.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
          "INNER JOIN TSSL_distribuidora.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
          "INNER JOIN TSSL_distribuidora.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
          "INNER JOIN TSSL_distribuidora.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
          "INNER JOIN TSSL_distribuidora.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
          "INNER JOIN TSSL_distribuidora.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
          "INNER JOIN TSSL_distribuidora.dbo.OLCT T7 ON T6.Location = T7.Code " +
          "INNER JOIN TSSL_distribuidora.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
     "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 " +
     "UNION ALL " +
     "SELECT DISTINCT 'Sociedad' = 'NOVAL', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                  "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                              "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                              "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                              "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end " +
         "FROM TSSL_Noval.dbo.OBTN T0 " +
          "INNER JOIN TSSL_Noval.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
          "INNER JOIN TSSL_Noval.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
          "INNER JOIN TSSL_Noval.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
          "INNER JOIN TSSL_Noval.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
          "INNER JOIN TSSL_Noval.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
          "INNER JOIN TSSL_Noval.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
          "INNER JOIN TSSL_Noval.dbo.OLCT T7 ON T6.Location = T7.Code " +
          "INNER JOIN TSSL_Noval.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
     "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0)T_INVENTARIO " +
      "WHERE CAST(T_INVENTARIO.[Fecha de Caducidad] AS DATE) = CAST(DATEADD(DAY, @dias, GETDATE()) AS DATE) AND T_INVENTARIO.[Fecha de Caducidad]>= GETDATE() AND T_INVENTARIO.Artículo = @articulo";
                    //"WHERE CAST(T_INVENTARIO.[Fecha de Caducidad] AS DATE) <= CAST(DATEADD(DAY, @dias, GETDATE()) AS DATE) AND T_INVENTARIO.[Fecha de Caducidad]>= GETDATE() AND T_INVENTARIO.Artículo = @articulo";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@dias", Convert.ToInt32(dias));
                    comando.Parameters.AddWithValue("@articulo", u_ARTICULO);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Planta = datareader["Planta"].ToString(),
                                Artículo = datareader["Artículo"].ToString(),
                                Descripción = datareader["Descripcion"].ToString(),
                                Familia = datareader["Familia"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                NombreA = datareader["Nombre del Almacén"].ToString(),
                                Status = datareader["Status"].ToString(),
                                Lote = datareader["Lote"].ToString(),
                                Caducidad = datareader["Fecha de Caducidad"].ToString(),
                                Stock = datareader["En Stock"].ToString(),
                                Ubicación = datareader["Ubicación"].ToString(),
                                UM = datareader["UM"].ToString(),
                                TipoC = datareader["Tipo de Caducidad"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<InventarioCaduco>();
            }

            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducasMPST(int dias)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM CADUCIDADESMPST " +
                                      "WHERE CAST([Fecha de Caducidad] AS DATE) < CAST(DATEADD(DAY, @dias, GETDATE()) AS DATE) AND [Fecha de Caducidad]>= GETDATE() ORDER BY [Almacén]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@dias", Convert.ToInt32(dias));
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Planta = datareader["Planta"].ToString(),
                                Artículo = datareader["Artículo"].ToString(),
                                Descripción = datareader["Descripcion"].ToString(),
                                Familia = datareader["Familia"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                NombreA = datareader["Nombre del Almacén"].ToString(),
                                Status = datareader["Status"].ToString(),
                                Lote = datareader["Lote"].ToString(),
                                Caducidad = datareader["Fecha de Caducidad"].ToString(),
                                Stock = datareader["En Stock"].ToString(),
                                Ubicación = datareader["Ubicación"].ToString(),
                                UM = datareader["UM"].ToString(),
                                diasCaduco = datareader["dias"].ToString(),
                                TipoC = datareader["Tipo de Caducidad"].ToString(),
                                valorInventario = datareader["valorInventario"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<InventarioCaduco>();
            }

            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducadas(int tipoProducto)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * " +
                        "FROM(SELECT DISTINCT 'Sociedad' = 'MIEL MEX', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                     "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end, " +
                          "DATEDIFF(DAY, CAST(T0.EXPDATE AS DATE), GETDATE()) AS 'Dias', T3.OnHandQty * T8.AvgPrice AS 'valorInventario' " +
                          "FROM TSSL_MielMex.dbo.OBTN T0 " +
                          "INNER JOIN TSSL_MielMex.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
                          "INNER JOIN TSSL_MielMex.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
                          "INNER JOIN TSSL_MielMex.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
                          "INNER JOIN TSSL_MielMex.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
                          "INNER JOIN TSSL_MielMex.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
                          "INNER JOIN TSSL_MielMex.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
                          "INNER JOIN TSSL_MielMex.dbo.OLCT T7 ON T6.Location = T7.Code " +
                          "INNER JOIN TSSL_MielMex.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode ";
                    if (tipoProducto == 0)
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('ST','MP') ";
                    }
                    else
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('PT') ";
                    }
                    consulta = consulta + "UNION ALL " +
                    "SELECT DISTINCT 'Sociedad' = 'NATURASOL', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                                     "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                 "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end, " +
                          "DATEDIFF(DAY, CAST(T0.EXPDATE AS DATE), GETDATE()) AS 'Dias', T3.OnHandQty * T8.AvgPrice AS 'valorInventario'  " +
                        "FROM TSSL_Naturasol.dbo.OBTN T0 " +
                         "INNER JOIN TSSL_Naturasol.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
                         "INNER JOIN TSSL_Naturasol.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
                         "INNER JOIN TSSL_Naturasol.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
                         "INNER JOIN TSSL_Naturasol.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
                         "INNER JOIN TSSL_Naturasol.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
                         "INNER JOIN TSSL_Naturasol.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
                         "INNER JOIN TSSL_Naturasol.dbo.OLCT T7 ON T6.Location = T7.Code " +
                         "INNER JOIN TSSL_Naturasol.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode ";
                    if (tipoProducto == 0)
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('ST','MP') ";
                    }
                    else
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('PT') ";
                    }
                    consulta = consulta + "UNION ALL " +
                    "SELECT DISTINCT 'Sociedad' = 'EVI', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                               "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                           "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                           "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                           "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end, " +
                          "DATEDIFF(DAY, CAST(T0.EXPDATE AS DATE), GETDATE()) AS 'Dias', T3.OnHandQty * T8.AvgPrice AS 'valorInventario'  " +
                        "FROM TSSL_distribuidora.dbo.OBTN T0 " +
                         "INNER JOIN TSSL_distribuidora.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
                         "INNER JOIN TSSL_distribuidora.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
                         "INNER JOIN TSSL_distribuidora.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
                         "INNER JOIN TSSL_distribuidora.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
                         "INNER JOIN TSSL_distribuidora.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
                         "INNER JOIN TSSL_distribuidora.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
                         "INNER JOIN TSSL_distribuidora.dbo.OLCT T7 ON T6.Location = T7.Code " +
                         "INNER JOIN TSSL_distribuidora.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode " +
                    "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 " +
                    "AND T2.U_FAMILIA IN ('ST','MP') " +
                    "UNION ALL " +
                    "SELECT DISTINCT 'Sociedad' = 'NOVAL', 'Planta' = CASE WHEN T7.Location = 'NUEVA PLANTA MIEL' THEN 'PLANTA MIEL' " +
                                                                 "WHEN T7.Location = 'CEDIS' THEN 'PLANTA SEMILLAS' ELSE T7.Location END, 'Artículo' = T0.ItemCode, 'Descripcion' = T2.ItemName, 'Familia' = T5.ItmsGrpNam, 'Almacén' = T4.WhsCode, 'Nombre del Almacén' = T6.WhsName, 'Status' = T6.FedTaxId, 'Lote' = T0.DistNumber, 'Fecha de Caducidad' = CAST(T0.ExpDate AS DATE), 'Ubicación' = T4.BinCode, 'En Stock' = T3.OnHandQty, 'UM' = T2.InvntryUom, 'Tipo de Caducidad' =case when T0.ExpDate < (GETDATE()) then 'CADUCO' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                             "when T0.ExpDate >= (GETDATE())and(T0.ExpDate <= (GETDATE()) + 90) then 'POR CADUCAR' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                             "when T0.ExpDate > (GETDATE()) + 90 and(T0.ExpDate <= (GETDATE()) + 180) then 'DE 3 A 6 MESES' " +
                                                                                                                                                                                                                                                                                                                                                                                                                                                             "when T0.ExpDate > (GETDATE()) + 180 then 'MAYOR A 6 MESES' else 'SIN CADUCIDAD' end, " +
                         "DATEDIFF(DAY, CAST(T0.EXPDATE AS DATE), GETDATE()) AS 'Dias', T3.OnHandQty * T8.AvgPrice AS 'valorInventario'  " +
                         "FROM TSSL_Noval.dbo.OBTN T0 " +
                         "INNER JOIN TSSL_Noval.dbo.OBTQ T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber " +
                         "INNER JOIN TSSL_Noval.dbo.OITM T2 ON T0.ItemCode = T2.ItemCode " +
                         "INNER JOIN TSSL_Noval.dbo.OBBQ T3 ON T0.ItemCode = T3.ItemCode AND T0.AbsEntry = T3.SnBMDAbs " +
                         "INNER JOIN TSSL_Noval.dbo.OBIN T4 ON T3.BinAbs = T4.AbsEntry " +
                         "INNER JOIN TSSL_Noval.dbo.OITB T5 ON T2.ItmsGrpCod = T5.ItmsGrpCod " +
                         "INNER JOIN TSSL_Noval.dbo.OWHS T6 ON T4.WhsCode = T6.WhsCode AND T1.WhsCode = T6.WhsCode " +
                         "INNER JOIN TSSL_Noval.dbo.OLCT T7 ON T6.Location = T7.Code " +
                         "INNER JOIN TSSL_Noval.dbo.OITW T8 ON T6.WhsCode = T8.WhsCode AND T0.ItemCode = T8.ItemCode ";

                    if (tipoProducto == 0)
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('ST','MP') ";
                    }
                    else
                    {
                        consulta = consulta + "WHERE T6.Inactive = 'N' AND T1.Quantity > 0 AND T3.OnHandQty > 0 AND T2.U_FAMILIA IN ('PT') ";
                    }
                    consulta = consulta + " )T_INVENTARIO " +
                     "WHERE T_INVENTARIO.[Fecha de Caducidad]< GETDATE() ORDER BY T_INVENTARIO.[Almacén]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Planta = datareader["Planta"].ToString(),
                                Artículo = datareader["Artículo"].ToString(),
                                Descripción = datareader["Descripcion"].ToString(),
                                Familia = datareader["Familia"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                NombreA = datareader["Nombre del Almacén"].ToString(),
                                Status = datareader["Status"].ToString(),
                                Lote = datareader["Lote"].ToString(),
                                Caducidad = datareader["Fecha de Caducidad"].ToString(),
                                Stock = datareader["En Stock"].ToString(),
                                Ubicación = datareader["Ubicación"].ToString(),
                                UM = datareader["UM"].ToString(),
                                TipoC = datareader["Tipo de Caducidad"].ToString(),
                                diasCaduco = datareader["Dias"].ToString(),
                                valorInventario = datareader["valorInventario"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<InventarioCaduco>();
            }

            return listaExistencias;
        }

        //REVISADO
        public List<InventarioCaduco> obtenerPTSinMovimiento()
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.PTSINMOVIMIENTO ORDER BY ExpDate ASC;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["ItemCode"].ToString(),
                                Descripción = datareader["ItemName"].ToString(),
                                Almacén = datareader["WhsCode"].ToString(),
                                NombreA = datareader["WhsName"].ToString(),
                                UM = datareader["InvntryUom"].ToString(),
                                Lote = datareader["DistNumber"].ToString(),
                                Caducidad = datareader["ExpDate"].ToString(),
                                Stock = datareader["Quantity"].ToString(),
                                valorInventario = datareader["valor"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<InventarioCaduco>();
            }

            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerMPSinConsumo()
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * FROM DESARROLLOWEB.DBO.MPSTSINMOVIMIENTO ORDER BY ExpDate ASC";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new InventarioCaduco()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["ItemCode"].ToString(),
                                Descripción = datareader["ItemName"].ToString(),
                                Almacén = datareader["WhsCode"].ToString(),
                                NombreA = datareader["WhsName"].ToString(),
                                UM = datareader["InvntryUom"].ToString(),
                                Lote = datareader["DistNumber"].ToString(),
                                Caducidad = datareader["ExpDate"].ToString(),
                                Stock = datareader["OnHand"].ToString(),
                                valorInventario = datareader["valor"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<InventarioCaduco>();
            }

            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerSemanasStock()
        {
            List<InventarioCaduco> listaSemanasStock = new List<InventarioCaduco>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * FROM DESARROLLOWEB.DBO.SEMANASTOCK TA3 ORDER BY TA3.[Semanas existencia] ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaSemanasStock.Add(new InventarioCaduco()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["ItemCode"].ToString(),
                                Descripción = datareader["ItemName"].ToString(),
                                Stock = datareader["Existencia"].ToString(),
                                Consumo = datareader["Consumo"].ToString(),
                                UM = datareader["InvntryUom"].ToString(),
                                ConsumoSemanal = datareader["Consumo Semanal"].ToString(),
                                SemanasInventario = datareader["Semanas existencia"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaSemanasStock = new List<InventarioCaduco>();
            }

            return listaSemanasStock;
        }
    }
}

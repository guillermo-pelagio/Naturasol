using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class InventarioDAL
    {
        //S10-REQUERIDO
        public List<Inventario> obtenerClientesKAM()
        {
            List<Inventario> listaClientes = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select DISTINCT U_CLIENTE, CardName, U_Correo1, U_Correo2, U_Correo3 from TSSL_NATURASOL.dbo.[@3_CLIENTES_CAD] T1 join TSSL_NATURASOL.dbo.OCRD T2 ON T2.CardCode = T1.U_Cliente WHERE U_CORREO1 <> 'guillemo.pelagio@naturasol.com.mx' ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaClientes.Add(new Inventario()
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
                listaClientes = new List<Inventario>();
            }

            return listaClientes;
        }

        public List<Inventario> obtenerAjustesMes(string basesDatos, int ajustes, int tipoAjustes)
        {
            List<Inventario> listaArticulos = new List<Inventario>();
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
                            listaArticulos.Add(new Inventario()
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
                listaArticulos = new List<Inventario>();
            }

            return listaArticulos;
        }

        public DataTable obtenerExcel(Reportes reporte)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    using (SqlCommand cmd = new SqlCommand(reporte.queryReporte))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = conexionDB;
                            sda.SelectCommand = cmd;
                            using (dt)
                            {
                                sda.Fill(dt);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dt;

            /*
            using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
            {
                using (SqlCommand cmd = new SqlCommand(reporte.queryReporte, conexionDB))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = conexionDB;
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable("ReportableEvent"))
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt);
                                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                wb.Style.Font.Bold = true;

                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.Buffer = true;
                                HttpContext.Current.Response.Charset = "";
                                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                                    HttpContext.Current.Response.Flush();
                                    HttpContext.Current.Response.End();
                                }
                            }
                        }
                    }
                }            
            }
            */
        }

        public List<Inventario> obtenerArticulosSinUbicacion()
        {
            List<Inventario> listaArticulos = new List<Inventario>();
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
                            listaArticulos.Add(new Inventario()
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
                listaArticulos = new List<Inventario>();
            }

            return listaArticulos;
        }

        //S10-REQUERIDO
        public List<Inventario> obtenerArticulosCaducar(string u_CLIENTE)
        {
            List<Inventario> listaArticulos = new List<Inventario>();
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
                            listaArticulos.Add(new Inventario()
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
                listaArticulos = new List<Inventario>();
            }

            return listaArticulos;
        }

        //S10-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducas(string u_ARTICULO, string dias)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DesarrolloWeb.DBO.[A4 - Inventario] WHERE (Almacén LIKE '%02' OR Almacén LIKE '%06' OR Almacén LIKE '%12') AND CAST([Fecha de Caducidad] AS DATE) = CAST(DATEADD(DAY, @dias, GETDATE()) AS DATE) AND [Fecha de Caducidad]>= GETDATE() AND Artículo = @articulo";
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
                            listaExistencias.Add(new Inventario()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                Planta = datareader["Planta"].ToString(),
                                Artículo = datareader["Artículo"].ToString(),
                                Descripción = datareader["Descripción del Artículo"].ToString(),
                                Familia = datareader["Familia"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                NombreA = datareader["Nombre del Almacén"].ToString(),
                                Status = datareader["Estatus lote"].ToString(),
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
                listaExistencias = new List<Inventario>();
            }

            return listaExistencias;
        }

        //S9-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducasMPST(int dias)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM DESARROLLOWEB.DBO.[W9 - CaducidadesMPST] " +
                                      "WHERE CAST([Fecha de Caducidad] AS DATE) < CAST(DATEADD(DAY, @dias, GETDATE()) AS DATE) AND [Fecha de Caducidad]>= GETDATE() ORDER BY [Almacén]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@dias", Convert.ToInt32(dias));
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaExistencias.Add(new Inventario()
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
                listaExistencias = new List<Inventario>();
            }

            return listaExistencias;
        }

        public string buscarMovimientoSAP(string idSurtido)
        {
            string id = null;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * from tssl_naturasol.dbo.OWTR WHERE U_ListaPesos = @idSurtido";


                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@idSurtido", idSurtido);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            id = datareader["U_ListaPesos"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                id = null;
            }

            return id;
        }

        //S8-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducadas(int tipoProducto)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select *, DATEDIFF(DAY, CAST([Fecha de Caducidad] AS DATE), GETDATE()) AS 'Dias' from DesarrolloWeb.dbo.[A4 - Inventario] ";
                    consulta = consulta + " ORDER BY [Almacén]";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            if (tipoProducto == 0)
                            {
                                if ((datareader["U_FAMILIA"].ToString() == "MP") || (datareader["U_FAMILIA"].ToString() == "ST"))
                                {
                                    if (datareader["Tipo de Caducidad"].ToString() == "CADUCO")
                                    {
                                        listaExistencias.Add(new Inventario()
                                        {
                                            Sociedad = datareader["Sociedad"].ToString(),
                                            Planta = datareader["Planta"].ToString(),
                                            Artículo = datareader["Artículo"].ToString(),
                                            Descripción = datareader["Descripción del Artículo"].ToString(),
                                            Familia = datareader["Familia"].ToString(),
                                            Almacén = datareader["Almacén"].ToString(),
                                            NombreA = datareader["Nombre del Almacén"].ToString(),
                                            Status = datareader["Estatus lote"].ToString(),
                                            Lote = datareader["Lote"].ToString(),
                                            Caducidad = datareader["Fecha de Caducidad"].ToString(),
                                            Stock = datareader["En Stock"].ToString(),
                                            Ubicación = datareader["Ubicación"].ToString(),
                                            UM = datareader["UM"].ToString(),
                                            TipoC = datareader["Tipo de Caducidad"].ToString(),
                                            diasCaduco = datareader["Dias"].ToString(),
                                            valorInventario = datareader["Valor de Inventario"].ToString()
                                        });
                                    }
                                }
                            }
                            else
                            {
                                if ((datareader["U_FAMILIA"].ToString() == "PT"))
                                {
                                    if (datareader["Tipo de Caducidad"].ToString() == "CADUCO")
                                    {
                                        listaExistencias.Add(new Inventario()
                                        {
                                            Sociedad = datareader["Sociedad"].ToString(),
                                            Planta = datareader["Planta"].ToString(),
                                            Artículo = datareader["Artículo"].ToString(),
                                            Descripción = datareader["Descripción del Artículo"].ToString(),
                                            Familia = datareader["Familia"].ToString(),
                                            Almacén = datareader["Almacén"].ToString(),
                                            NombreA = datareader["Nombre del Almacén"].ToString(),
                                            Status = datareader["Estatus lote"].ToString(),
                                            Lote = datareader["Lote"].ToString(),
                                            Caducidad = datareader["Fecha de Caducidad"].ToString(),
                                            Stock = datareader["En Stock"].ToString(),
                                            Ubicación = datareader["Ubicación"].ToString(),
                                            UM = datareader["UM"].ToString(),
                                            TipoC = datareader["Tipo de Caducidad"].ToString(),
                                            diasCaduco = datareader["Dias"].ToString(),
                                            valorInventario = datareader["Valor de Inventario"].ToString()
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaExistencias = new List<Inventario>();
            }

            return listaExistencias;
        }

        //REVISADO
        public List<Inventario> obtenerPTSinMovimiento()
        {
            List<Inventario> listaExistencias = new List<Inventario>();
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
                            listaExistencias.Add(new Inventario()
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
                listaExistencias = new List<Inventario>();
            }

            return listaExistencias;
        }

        public List<Inventario> obtenerMPSinConsumo()
        {
            List<Inventario> listaExistencias = new List<Inventario>();
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
                            listaExistencias.Add(new Inventario()
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
                listaExistencias = new List<Inventario>();
            }

            return listaExistencias;
        }

        //S24-REQUERIDO
        public List<Inventario> obtenerSemanasStock()
        {
            List<Inventario> listaSemanasStock = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * FROM DESARROLLOWEB.DBO.[W24 - SemanasStock] TA3 ORDER BY TA3.[Semanas existencia] ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaSemanasStock.Add(new Inventario()
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
                listaSemanasStock = new List<Inventario>();
            }

            return listaSemanasStock;
        }

        public List<Inventario> obtenerInventarioActualWeb(string sociedad, string articulo, string descripcion, string almacen, string lote)
        {
            List<Inventario> listaSemanasStock = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (articulo == "" && almacen == "" && lote == "" && descripcion == "")
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[A4 - Inventario]";
                    }
                    else
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[A4 - Inventario] where [Artículo] LIKE '%" + articulo + "%' and [Descripción del Artículo] LIKE '%" + descripcion + "%' and [Almacén] LIKE '%" + almacen + "%'  and [Lote] LIKE '%" + lote + "%' ORDER BY [Artículo], [Almacén], [Fecha de Caducidad] ASC ";
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaSemanasStock.Add(new Inventario()
                            {
                                //Sociedad = datareader["Sociedad"].ToString(),
                                Artículo = datareader["Artículo"].ToString(),
                                Sociedad = datareader["Sociedad"].ToString(),
                                Descripción = datareader["Descripción del Artículo"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                Lote = datareader["Lote"].ToString(),
                                Caducidad = Convert.ToDateTime(datareader["Fecha de Caducidad"].ToString()).ToShortDateString(),
                                Stock = datareader["En Stock"].ToString(),
                                UM = datareader["UM"].ToString(),
                                Status = datareader["Estatus lote"].ToString(),
                                diasCaduco = datareader["Tipo de Caducidad"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaSemanasStock = new List<Inventario>();
            }

            return listaSemanasStock;
        }

        public List<Kardex> obtenerKardexWeb(string sociedad, string articulo, string descripcion, string almacen, string lote, string fechaInicio, string fechaFin)
        {
            List<Kardex> listaKardex = new List<Kardex>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (articulo == "" && almacen == "" && lote == "" && descripcion == "" && fechaInicio == "" && fechaFin == "")
                    {
                        consulta = "select TOP 0 * FROM DESARROLLOWEB.DBO.[KARDEX]";
                    }
                    else
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[KARDEX] where [ItemCode] LIKE '%" + articulo + "%' and [Dscription] LIKE '%" + descripcion + "%' and [Warehouse] LIKE '%" + almacen + "%'  and [BatchNum] LIKE '%" + lote + "%' and [Sociedad] LIKE '%" + (sociedad == "0" ? "Naturasol" : "Miel mex") + "%' ";

                        if (fechaInicio != "")
                        {
                            consulta = consulta + " and [DocDate] >= '" + fechaInicio + "' ";
                        }
                        if (fechaFin != "")
                        {
                            consulta = consulta + " and [DocDate] <= '" + fechaFin + "' ";
                        }
                        consulta = consulta + " ORDER BY DocDate DESC, [Tipo de Movimiento], [Número Documento] DESC";
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            string fecha;
                            string vigencia;
                            try
                            {
                                if (datareader["ExpDate"] == null)
                                {
                                    fecha = DateTime.Now.ToShortDateString();
                                }
                                else
                                {
                                    fecha = Convert.ToDateTime(datareader["DocDate"].ToString()).ToShortDateString();
                                }

                            }
                            catch (Exception)
                            {
                                fecha = DateTime.Now.ToShortDateString();
                            }

                            try
                            {
                                if (datareader["ExpDate"] == null)
                                {
                                    vigencia = DateTime.Now.ToShortDateString();
                                }
                                else
                                {
                                    vigencia = Convert.ToDateTime(datareader["ExpDate"].ToString()).ToShortDateString();
                                }
                            }
                            catch (Exception)
                            {
                                vigencia = DateTime.Now.ToShortDateString();
                            }

                            listaKardex.Add(new Kardex()
                            {

                                Sociedad = datareader["Sociedad"].ToString(),
                                DocNum = datareader["Número Documento"].ToString(),
                                Tipo = datareader["Tipo de Movimiento"].ToString(),
                                DocDate = fecha,
                                DocTime = datareader["Hora"].ToString(),
                                Usuario = datareader["Usuario"].ToString(),
                                Artículo = datareader["ItemCode"].ToString(),
                                Descripción = datareader["Dscription"].ToString(),
                                Lote = datareader["BatchNum"].ToString(),
                                Caducidad = vigencia,
                                CantidadP = datareader["InQty"].ToString(),
                                CantidadN = datareader["OutQty"].ToString(),
                                Almacen = datareader["Warehouse"].ToString(),
                                Comentarios1 = datareader["CardName"].ToString(),
                                Comentarios2 = datareader["Comments"].ToString(),
                                Comentarios3 = datareader["JrnlMemo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKardex = new List<Kardex>();
            }

            return listaKardex;
        }

        public List<SolicitudTrasladoDetalle> obtenerSurtidores(string ubicacion, string sociedad)
        {
            List<SolicitudTrasladoDetalle> listaDocumentos = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select idSurtidor, nombreSurtidor from desarrolloweb.dbo.surtidores ";

                    if (ubicacion != "0")
                    {
                        consulta = consulta + " where ubicacionSurtidor= " + ubicacion;
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTrasladoDetalle()
                            {
                                idSurtidor = datareader["idSurtidor"].ToString(),
                                nombreSurtidor = datareader["nombreSurtidor"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTrasladoDetalle>();
            }

            return listaDocumentos;
        }

        public List<SolicitudTrasladoDetalle> obtenerLoteST(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> listaDocumentos = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DistNumber, OnHandQty, t6.CntUnitMsr FROM TSSL_" + solicitudTrasladoDetalle.Sociedad + ".dbo.obbq T0 " +
                                        "JOIN TSSL_" + solicitudTrasladoDetalle.Sociedad + ".dbo.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs " +
                                        "join TSSL_" + solicitudTrasladoDetalle.Sociedad + ".dbo.OITM t6 on t6.ItemCode = T0.ItemCode " +
                                        "WHERE OnHandQty> 0 AND T0.ItemCode=LEFT(@itemcode,12) and T0.WhsCode=@almacen ORDER BY T0.ItemCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", solicitudTrasladoDetalle.ItemCodeST);
                    comando.Parameters.AddWithValue("@almacen", solicitudTrasladoDetalle.FillerST);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTrasladoDetalle()
                            {
                                DistNumber = datareader["DistNumber"].ToString(),
                                OnHandQty = datareader["OnHandQty"].ToString(),
                                UOM = datareader["CntUnitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTrasladoDetalle>();
            }

            return listaDocumentos;
        }

        public List<SolicitudTrasladoDetalle> confirmarExistenciaLote(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> listaDocumentos = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT DistNumber, OnHandQty, T6.CntUnitMsr FROM TSSL_NATURASOL.dbo.obbq T0 " +
                                        "JOIN TSSL_NATURASOL.dbo.OBTN T1 ON T1.AbsEntry = T0.SnBMDAbs " +
                                        "join TSSL_NATURASOL.dbo.OITM t6 on t6.ItemCode = T0.ItemCode " +
                                        "WHERE OnHandQty> 0 AND T0.ItemCode=LEFT(@itemcode,12) and T0.WhsCode=@almacen AND T1.DistNumber=@DistNumber AND T0.OnHandQty>=@quantity ORDER BY T0.ItemCode ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@itemcode", solicitudTrasladoDetalle.ItemCodeST);
                    comando.Parameters.AddWithValue("@almacen", solicitudTrasladoDetalle.FillerST);
                    comando.Parameters.AddWithValue("@quantity", solicitudTrasladoDetalle.QuantityST);
                    comando.Parameters.AddWithValue("@DistNumber", solicitudTrasladoDetalle.DistNumber);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTrasladoDetalle()
                            {
                                DistNumber = datareader["DistNumber"].ToString(),
                                OnHandQty = datareader["OnHandQty"].ToString(),
                                UOM = datareader["CntUnitMsr"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTrasladoDetalle>();
            }

            return listaDocumentos;
        }

        public List<SolicitudTrasladoDetalle> obtenerConfirmaciones(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> listaDocumentos = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * from DesarrolloWeb.dbo.Surtidoparcial " +
                        "WHERE docEntry =@docentry and lineNum =@linenum and estatus = 0 ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docentry", solicitudTrasladoDetalle.DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@lineNum", solicitudTrasladoDetalle.DocEntry.Split('-')[1]);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTrasladoDetalle()
                            {
                                ItemCodeST = datareader["itemcode"].ToString(),
                                QuantityST = datareader["quantity"].ToString(),
                                idSurtido = datareader["idSurtidoParcial"].ToString(),
                                DistNumber = datareader["batchnum"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTrasladoDetalle>();
            }

            return listaDocumentos;
        }

        public List<SolicitudTrasladoDetalle> obtenerConfirmaciones2(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> listaDocumentos = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select * from DesarrolloWeb.dbo.Surtidoparcial " +
                        "WHERE docEntry =@docentry and lineNum =@linenum and estatus = 2 ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@docentry", solicitudTrasladoDetalle.DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@lineNum", solicitudTrasladoDetalle.DocEntry.Split('-')[1]);

                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new SolicitudTrasladoDetalle()
                            {
                                ItemCodeST = datareader["itemcode"].ToString(),
                                idSurtido = datareader["idSurtidoParcial"].ToString(),
                                QuantityST = datareader["quantity"].ToString(),
                                DistNumber = datareader["batchnum"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<SolicitudTrasladoDetalle>();
            }

            return listaDocumentos;
        }

        public List<SolicitudTrasladoDetalle> obteneSTTS(string ubicacion, string puesto, string articulo, string descripcion, string almacen1, string almacen2, string fechaInicio, string fechaFin)
        {
            List<SolicitudTrasladoDetalle> listaKardex = new List<SolicitudTrasladoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    if ((puesto == "32"))
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[A1 - ST_TS2] where [Articulo ST] LIKE '%" + articulo + "%' and [Descripcion ST] LIKE '%" + descripcion + "%' and [Origen ST] LIKE '%" + almacen1 + "%'  and [Destino ST] LIKE '%" + almacen2 + "%' ";

                        if (fechaInicio != "")
                        {
                            consulta = consulta + " and [Fecha ST] >= '" + fechaInicio + "' ";
                        }
                        else
                        {
                            consulta = consulta + " AND [Fecha ST] >= DATEADD(DAY, -60 ,GETDATE()) ";
                        }
                        if (fechaFin != "")
                        {
                            consulta = consulta + " and [Fecha ST] <= '" + fechaFin + "' ";
                        }

                        if (ubicacion == "5")
                        {
                            consulta = consulta + " AND [U_listapesos] is not null AND [Estatus ST] ='O' AND (([Origen ST]='2303' AND [Destino ST]='2306') OR ([Origen ST]='2306' AND [Destino ST]='2302'))";
                        }
                        if ((ubicacion == "2") || (ubicacion == "3"))
                        {
                            consulta = consulta + " AND [U_listapesos] is not null AND [Estatus ST] ='O' AND(([Origen ST]='1703' AND [Destino ST] in ('1606','1706','1806','1906','2006','2106','2206','1106')) OR ([Origen ST] in ('1606','1706','1806','1906','2006','2106','2206','1106') AND [Destino ST]='1702'))";
                        }
                        if (ubicacion == "1")
                        {
                            consulta = consulta + " AND [U_listapesos] is not null AND [Estatus ST] ='O' AND(([Origen ST]='1403' AND [Destino ST]='1406') OR ([Origen ST]='1420' AND [Destino ST]='1406') OR ([Origen ST]='1405' AND [Destino ST]='1406') OR ([Origen ST]='1406' AND [Destino ST]='1402'))";
                        }
                        else
                        {
                            consulta = consulta + " AND [U_listapesos] is not null AND [Estatus ST] ='O' ";
                        }

                        consulta = consulta + " ORDER BY [Fecha ST] DESC, [Folio ST], [Sociedad] DESC";
                    }
                    else if ((puesto == "31") || (puesto == "33") || (puesto == "2") || (puesto == "1"))
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[A1 - ST_TS2] where [Articulo ST] LIKE '%" + articulo + "%' and [Descripcion ST] LIKE '%" + descripcion + "%' ";

                        if (fechaInicio != "")
                        {
                            consulta = consulta + " and [Fecha ST] >= '" + fechaInicio + "' ";
                        }
                        else
                        {
                            consulta = consulta + " AND [Fecha ST] >= DATEADD(DAY, -7 ,GETDATE()) ";
                        }
                        if (fechaFin != "")
                        {
                            consulta = consulta + " and [Fecha ST] <= '" + fechaFin + "' ";
                        }

                        if (ubicacion == "5")
                        {
                            //consulta = consulta + " AND ([U_listapesos] ='D' OR [U_listapesos] ='X') AND [Estatus ST] ='O' AND [Origen ST]='2303' ";
                            consulta = consulta + " AND (([Origen ST]='2303' AND [Destino ST]='2306') OR ([Origen ST]='2306' AND [Destino ST]='2302'))";
                        }
                        else if ((ubicacion == "2") || (ubicacion == "3"))
                        {
                            //consulta = consulta + " AND ([U_listapesos] ='D' OR [U_listapesos] ='X') AND [Estatus ST] ='O' AND [Origen ST]='1703' ";
                            consulta = consulta + " AND(([Origen ST]='1703' AND [Destino ST] in ('1606','1706','1806','1906','2006','2106','2206','1106')) OR ([Origen ST] in ('1606','1706','1806','1906','2006','2106','2206','1106') AND [Destino ST]='1702'))";
                        }
                        else if (ubicacion == "1")
                        {
                            //consulta = consulta + " AND ([U_listapesos] ='D' OR [U_listapesos] ='X') AND [Estatus ST] ='O' AND [Origen ST]='1703' ";
                            consulta = consulta + " AND(([Origen ST]='1403' AND [Destino ST]='1406') OR ([Origen ST]='1420' AND [Destino ST]='1406') OR ([Origen ST]='1405' AND [Destino ST]='1406') OR ([Origen ST]='1406' AND [Destino ST]='1402'))";
                        }
                        else
                        {
                            //consulta = consulta + " AND [U_listapesos] ='D' AND [Estatus ST] ='O' ";
                        }

                        consulta = consulta + "  AND [Estatus ST]='O' ORDER BY [Fecha ST] DESC, [Folio ST], [Sociedad] DESC";
                    }
                    else
                    {
                        if (articulo == "" && almacen1 == "" && almacen2 == "" && descripcion == "" && fechaInicio == "" && fechaFin == "")
                        {
                            consulta = "select TOP 0 * FROM DESARROLLOWEB.DBO.[A1 - ST_TS2]";
                        }
                        else
                        {
                            consulta = "select * FROM DESARROLLOWEB.DBO.[A1 - ST_TS2] where [Articulo ST] LIKE '%" + articulo + "%' and [Descripcion ST] LIKE '%" + descripcion + "%' and [Origen ST] LIKE '%" + almacen1 + "%'  and [Destino ST] LIKE '%" + almacen2 + "%' ";

                            if (fechaInicio != "")
                            {
                                consulta = consulta + " and [Fecha ST] >= '" + fechaInicio + "' ";
                            }
                            else
                            {
                                consulta = consulta + " AND [Fecha ST] >= DATEADD(DAY, -60 ,GETDATE()) ";
                            }
                            if (fechaFin != "")
                            {
                                consulta = consulta + " and [Fecha ST] <= '" + fechaFin + "' ";
                            }
                            //consulta = consulta + " AND [Estatus ST]='O' " +
                            consulta = consulta + " ORDER BY [Fecha ST] DESC, [Folio ST], [Sociedad] DESC";
                        }
                    }

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKardex.Add(new SolicitudTrasladoDetalle()
                            {
                                Sociedad = datareader["Sociedad"].ToString(),
                                DocNumST = datareader["Folio ST"].ToString(),
                                status = datareader["Estatus ST"].ToString(),
                                DocDateST = Convert.ToDateTime(datareader["Fecha ST"].ToString()).ToShortDateString(),
                                DocTimeST = datareader["Hora ST"].ToString(),
                                FillerST = datareader["Origen ST"].ToString(),
                                ToWhsCodeST = datareader["Destino ST"].ToString(),
                                OF = datareader["OF relacionada"].ToString(),
                                CommentsST = datareader["Comentarios ST"].ToString(),
                                UserST = datareader["Usuario ST"].ToString(),
                                CreatorST = datareader["Creador ST"].ToString(),
                                ItemCodeST = datareader["Articulo ST"].ToString(),
                                DscriptionST = datareader["Descripcion ST"].ToString(),
                                QuantityST = datareader["Cantidad ST"].ToString(),
                                OpenQuantityST = datareader["Pendiente ST"].ToString(),
                                UMST = datareader["UM ST"].ToString(),
                                //DocNumTS = datareader["Folio TS"].ToString(),
                                //DocDateTS = datareader["Fecha creacion TS"].ToString() == "" ? "-" : Convert.ToDateTime(datareader["Fecha creacion TS"].ToString()).ToShortDateString(),
                                //DocTimeTS = datareader["Hora creacion TS"].ToString(),
                                //CreatorTS = datareader["Creador TS"].ToString(),
                                //ItemCodeTS = datareader["Articulo TS"].ToString(),
                                //DscriptionTS = datareader["Descripcion TS"].ToString(),
                                //CantidadTS = datareader["Cantidad TS"].ToString(),
                                //UMTS = datareader["UM TS"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString(),
                                estatusSurtido = datareader["U_listapesos"].ToString(),
                                //FillerTS = datareader["Origen TS"].ToString(),
                                //ToWhsCodeTS = datareader["Destino TS"].ToString(),
                                FechaMC1 = datareader["u_fechamc1"].ToString(),
                                FechaMC2 = datareader["u_fechamc2"].ToString(),
                                //DistNumber = datareader["lote"].ToString(),
                                //ExpDate = datareader["ExpDate"].ToString() == "" ? "" : Convert.ToDateTime(datareader["ExpDate"].ToString()).ToShortDateString(),
                                FechaSurtir = datareader["u_fechasurtir"].ToString(),
                                FechaTraspaso = datareader["u_fechatraspaso"].ToString(),
                                FechaSurtido = datareader["u_fechasurtido"].ToString(),
                                Prioridad = datareader["u_prioridad"].ToString(),
                                planta = datareader["OCRCODE"].ToString(),
                                centroCosto = datareader["OCRCODE3"].ToString(),
                                nombreS = datareader["nombreSurtidor"].ToString(),
                                FechaSurtiendo = datareader["u_fechasurtiendo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKardex = new List<SolicitudTrasladoDetalle>();
            }

            return listaKardex;
        }

        public List<TransferenciaStockDetalle> obtenerTS(string folio, string itemcode)
        {
            List<TransferenciaStockDetalle> listaKardex = new List<TransferenciaStockDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT OWTR.DocNum, OWTR.DocDate, WTR1.ItemCode, ITL1.Quantity, OBTN.DistNumber, OBTN.ExpDate  " +
                        "FROM TSSL_NATURASOL.DBO.OWTR JOIN TSSL_NATURASOL.DBO.WTR1 ON WTR1.DocEntry=OWTR.DocEntry " +
                        "JOIN TSSL_NATURASOL.DBO.OITL ON OITL.DocNum=OWTR.DocNum AND OITL.DocType=67 AND OITL.LocCode=WTR1.WhsCode " +
                        "JOIN TSSL_NATURASOL.DBO.ITL1 ON OITL.LogEntry=ITL1.LogEntry AND ITL1.ItemCode=WTR1.ItemCode " +
                        "JOIN TSSL_NATURASOL.DBO.OBTN ON OBTN.SysNumber=ITL1.SysNumber AND OBTN.ItemCode=WTR1.ItemCode WHERE WTR1.BaseRef=@folio AND  WTR1.ItemCode=@itemcode";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@folio", folio);
                    comando.Parameters.AddWithValue("@itemcode", itemcode);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKardex.Add(new TransferenciaStockDetalle()
                            {
                                DocNum = datareader["DocNum"].ToString(),
                                Quantity = datareader["Quantity"].ToString(),
                                DistNumber = datareader["DistNumber"].ToString(),
                                DocDate = Convert.ToDateTime(datareader["DocDate"].ToString()).ToShortDateString(),
                                ExpDate = Convert.ToDateTime(datareader["ExpDate"].ToString()).ToShortDateString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKardex = new List<TransferenciaStockDetalle>();
            }

            return listaKardex;
        }

        public int actualizarEstatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    if (solicitudTraslado.Sociedad == "Mielmex")
                    {
                        sociedad = "TSSL_MIELMEX";
                    }
                    else
                    {
                        sociedad = "TSSL_NATURASOL";
                    }
                    string consulta = "update " + sociedad + ".DBO.WTQ1 set U_listapesos=@estatus ";

                    if (solicitudTraslado.estatusSurtido == "V")
                    {
                        consulta = consulta + ", u_fechamc1 = getdate(), u_usuariomc1=@usuario ";
                    }
                    if (solicitudTraslado.estatusSurtido == "P")
                    {
                        consulta = consulta + ", u_fechasurtir = getdate(), u_usuariosurtir=@usuario ";
                    }
                    if (solicitudTraslado.estatusSurtido == "T")
                    {
                        consulta = consulta + ", u_fechatraspaso = getdate(), u_usuariotraspaso=@usuario ";
                    }

                    consulta = consulta + " where DocEntry = @docentry and linenum=@linenum";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    string docentry = solicitudTraslado.DocEntry.Split('-')[0];
                    string linenum = solicitudTraslado.DocEntry.Split('-')[1];

                    comando.Parameters.AddWithValue("@estatus", solicitudTraslado.estatusSurtido);
                    comando.Parameters.AddWithValue("@usuario", solicitudTraslado.usuarioMovimiento);
                    comando.Parameters.AddWithValue("@docentry", solicitudTraslado.DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", solicitudTraslado.DocEntry.Split('-')[1]);

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

        public int regresarEstatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "update tssl_naturasol.DBO.WTQ1 set U_listapesos='S', u_fechasurtiendo=getdate(), u_usuariosurtiendo=@usuario ";

                    consulta = consulta + " where DocEntry = @docentry and linenum=@linenum";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    string docentry = solicitudTraslado.DocEntry.Split('-')[0];
                    string linenum = solicitudTraslado.DocEntry.Split('-')[1];

                    comando.Parameters.AddWithValue("@usuario", solicitudTraslado.usuarioMovimiento);
                    comando.Parameters.AddWithValue("@docentry", solicitudTraslado.DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", solicitudTraslado.DocEntry.Split('-')[1]);

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

        public int actualizarEstatus2(string DocEntry, string usuario)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";
                    string consulta = "update " + sociedad + ".DBO.WTQ1 set u_listapesos=@u_listapesos, ";

                    consulta = consulta + " U_proveedordiot=@idSurtidor, u_fechasurtiendo=getdate(), u_usuariosurtiendo=@usuario ";

                    consulta = consulta + "where DocEntry = @docentry and linenum=@linenum";


                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@docentry", DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", DocEntry.Split('-')[1]);
                    comando.Parameters.AddWithValue("@idSurtidor", DocEntry.Split('-')[2]);
                    comando.Parameters.AddWithValue("@u_listapesos", DocEntry.Split('-')[3]);

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

        public int actualizarEstatus4(string DocEntry, string usuario)
        {
            int docEntry = 0;
            string sociedad = "";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";
                    string consulta = "update " + sociedad + ".DBO.WTQ1 set ";

                    consulta = consulta + " u_fechamc2=getdate(), u_usuariomc2=@usuario ";

                    consulta = consulta + "where DocEntry = @docentry and linenum=@linenum";


                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);


                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@docentry", DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", DocEntry.Split('-')[1].Split('#')[0]);
                    comando.Parameters.AddWithValue("@itemcode", DocEntry.Split('#')[1]);
                    comando.Parameters.AddWithValue("@u_listapesos", DocEntry.Split('#')[2]);


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

        public int actualizarEstatus3(string DocEntry, string usuario)
        {
            int docEntry = 0;
            string sociedad = "";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";
                    string consulta = "update " + sociedad + ".DBO.WTQ1 set u_listapesos=@u_listapesos, ";

                    consulta = consulta + " u_fechasurtido=getdate(), u_usuariosurtido=@usuario ";

                    consulta = consulta + "where DocEntry = @docentry and linenum=@linenum";


                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);


                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@docentry", DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", DocEntry.Split('-')[1].Split('#')[0]);
                    comando.Parameters.AddWithValue("@itemcode", DocEntry.Split('#')[1]);
                    comando.Parameters.AddWithValue("@u_listapesos", DocEntry.Split('#')[2]);


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

        public int actualizarEstatusSurtidoParcial(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int docEntry = 0;
            string sociedad = "";

            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";
                    string consulta = "update DesarrolloWeb.dbo.surtidoParcial set estatus=@estatus, usuario=@usuario ";

                    if (solicitudTraslado.status == "2")
                    {
                        consulta = consulta + " ,quantity=@quantity ";
                    }

                    consulta = consulta + " where DocEntry = @docentry and linenum=@linenum ";
                    if (solicitudTraslado.status != "2")
                    {
                        consulta = consulta + " and quantity=@quantity ";
                    }

                    consulta = consulta + " and itemcode=@itemcode and batchnum=@batchnum ";


                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.Parameters.AddWithValue("@usuario", solicitudTraslado.usuarioMovimiento);
                    comando.Parameters.AddWithValue("@docentry", solicitudTraslado.DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", solicitudTraslado.DocEntry.Split('-')[1]);
                    comando.Parameters.AddWithValue("@itemcode", solicitudTraslado.ItemCodeST);
                    comando.Parameters.AddWithValue("@quantity", solicitudTraslado.QuantityST);
                    comando.Parameters.AddWithValue("@batchnum", solicitudTraslado.DistNumber);
                    comando.Parameters.AddWithValue("@estatus", solicitudTraslado.status);

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

        public int guardarSurtidoParcial(string DocEntry, string usuario)
        {
            int docEntry = 0;
            string sociedad = "";
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    sociedad = "TSSL_NATURASOL";
                    string consulta = "insert into desarrolloweb.dbo.surtidoparcial (docentry, linenum, itemcode, quantity, batchnum, estatus, usuarioCreacion) values (@docentry, @linenum, @itemcode, @quantity, @batchnum, @estatus, @usuario)";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    comando.Parameters.AddWithValue("@docentry", DocEntry.Split('-')[0]);
                    comando.Parameters.AddWithValue("@linenum", DocEntry.Split('-')[1].Split('#')[0]);
                    comando.Parameters.AddWithValue("@itemcode", DocEntry.Split('#')[1]);
                    comando.Parameters.AddWithValue("@quantity", DocEntry.Split('#')[2]);
                    comando.Parameters.AddWithValue("@batchnum", DocEntry.Split('#')[3]);
                    comando.Parameters.AddWithValue("@estatus", 0);
                    comando.Parameters.AddWithValue("@usuario", usuario);

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

        public List<BorradorDocumento> obtenerMuestrasAutorizar()
        {
            List<BorradorDocumento> listaKardex = new List<BorradorDocumento>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = " SELECT WddCode, ODRF.comments, ODRF.DocNum, ODRF.DocDate FROM TSSL_NATURASOL.DBO.ODRF " +
                                "JOIN TSSL_NATURASOL.DBO.OWDD ON OWDD.DraftEntry = ODRF.DocEntry " +
                                "WHERE ODRF.ObjType = 60 and U_INV_TIPOMOV = 2 AND ODRF.DocDate>'2025-03-09' " +
                                "AND OWDD.Status = 'W' ORDER BY Wddcode DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKardex.Add(new BorradorDocumento()
                            {
                                wddCode = Convert.ToInt32(datareader["WddCode"].ToString()),
                                numeroDocumento = Convert.ToInt32(datareader["DocNum"].ToString()),
                                fechaDocumento = datareader["DocDate"].ToString(),
                                comentarios = datareader["comments"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKardex = new List<BorradorDocumento>();
            }

            return listaKardex;
        }

        public List<BorradorDocumento> obtenerMuestrasAutorizarDetalle(int wddCode)
        {
            List<BorradorDocumento> listaKardex = new List<BorradorDocumento>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";

                    consulta = " SELECT WddCode, ODRF.comments, OITM.U_Familia, ODRF.DocNum, ODRF.DocDate, DRF1.Quantity, DRF1.ItemCode, DRF1.OcrCode, DRF1.Dscription, DRF1.WhsCode, DRF1.AcctCode, DRF1.OcrCode3, OITM.U_FT3_REGSANITARIOFA FROM TSSL_NATURASOL.DBO.ODRF " +
                                "JOIN TSSL_NATURASOL.DBO.DRF1 ON DRF1.DocEntry = ODRF.DocEntry " +
                                "JOIN TSSL_NATURASOL.DBO.OWDD ON OWDD.DraftEntry = ODRF.DocEntry " +
                                "JOIN TSSL_NATURASOL.DBO.OITM ON OITM.ItemCode=DRF1.ItemCode " +
                                "WHERE ODRF.ObjType = 60 and U_INV_TIPOMOV = 2 " +
                                "AND WddCode=@wddCode " +
                                "AND OWDD.Status = 'W' ORDER BY ODRF.DocDate DESC ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@wddCode", Convert.ToInt32(wddCode));
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaKardex.Add(new BorradorDocumento()
                            {
                                wddCode = Convert.ToInt32(datareader["WddCode"].ToString()),
                                numeroDocumento = Convert.ToInt32(datareader["DocNum"].ToString()),
                                fechaDocumento = datareader["DocDate"].ToString(),
                                cuentaContable = datareader["AcctCode"].ToString(),
                                centroCosto = datareader["OcrCode3"].ToString(),
                                planta = datareader["OcrCode"].ToString(),
                                cantidad = datareader["Quantity"].ToString(),
                                articulo = datareader["ItemCode"].ToString(),
                                descripcion = datareader["Dscription"].ToString(),
                                almacen = datareader["whsCode"].ToString(),
                                familia = datareader["U_Familia"].ToString(),
                                comentarios = datareader["comments"].ToString(),
                                maximoMuestra = datareader["U_FT3_REGSANITARIOFA"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaKardex = new List<BorradorDocumento>();
            }

            return listaKardex;
        }
    }
}

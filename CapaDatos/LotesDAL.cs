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
    public class LotesDAL
    {
        //METODO PARA OBTENER LOS LOTES DE UN ARTICULO
        public DataSet obtenerLotes(string sociedad, string articulo, string lote, string estatus)
        {
            DataSet dataset = new DataSet();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "SELECT " +
                        "OBTN.ItemCode AS Articulo, " +
                        "OITM.SalUnitMsr AS 'U.M.', " +
                        "OBTN.DistNumber AS Lote, " +
                        "OBTN.Status, " +
                        "OBTN.AbsEntry AS absEntry, " +
                        "OBBQ.OnHandQty AS Cantidad, " +
                        "OBBQ.WhsCode AS 'Código de almacen', " +
                        "OWHS.WhsName AS Almacen, " +
                        "OLCT.Location AS Ubicacion, " +
                        "OITM.ItemName AS 'Descripcion' " +
                        "FROM " + sociedad + ".dbo.OBTN " +
                        "JOIN " + sociedad + ".dbo.OITM ON OITM.ItemCode = OBTN.ItemCode " +
                        "JOIN " + sociedad + ".dbo.OBBQ ON OBBQ.ItemCode = OBTN.ItemCode " +
                        "JOIN " + sociedad + ".dbo.OWHS ON OBBQ.WhsCode = OWHS.WhsCode " +
                        "JOIN " + sociedad + ".dbo.OLCT ON OWHS.Location = OLCT.Code " +
                        "WHERE OBTN.ItemCode LIKE '%" + articulo + "%' " +
                        "AND OBTN.DistNumber LIKE '%" + lote + "%' " +
                        "AND OBTN.AbsEntry = OBBQ.SnBMDAbs " +
                        "AND OBTN.Status LIKE '%" + estatus + "%' " +
                        "AND OBBQ.OnHandQty>0 " +
                        "ORDER BY OBBQ.SnBMDAbs DESC;";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comando;
                    comando.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return dataset;
        }

        //METODO PARA OBTENER LOS ALMACENES PARA TRASPASAR LOS LOTES
        public DataSet almacenesDestinoLotes(string sociedad)
        {
            DataSet dataset = new DataSet();
            using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                try
                {
                    string consulta = "SELECT " +
                        "WhsCode, " +
                        "CONCAT(WhsCode collate SQL_Latin1_General_CP1_CI_AS,'-' collate SQL_Latin1_General_CP1_CI_AS, WhsName collate SQL_Latin1_General_CP1_CI_AS) AS WhsName " +
                    "FROM " + sociedad + ".dbo.OWHS " +
                        "WHERE (RIGHT(WhsCode, 2) = '12' OR RIGHT(WhsCode, 2) = '03'); ";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);

                    SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
                    SqlDataAdapter.SelectCommand = comando;
                    comando.CommandTimeout = 0;

                    dataset.Clear();
                    SqlDataAdapter.Fill(dataset);
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    conexionDB.Close();
                }

            return dataset;
        }

        //METODO PARA GUARDAR EN TALA INTERMEDIA LOS LOTES A ACTUALIZAR
        public int guardarCambioLotes(Lotes lote)
        {
            int id = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "INSERT INTO [DesarrolloWeb].[dbo].[lotesSAP]" +
                        "([absEntry]" +
                        ",[sociedad]" +
                        ",[articulo]" +
                        ",[lote]" +
                        ",[estatus]" +
                        ",[procesado]" +
                        ",[fechaRegistro]) VALUES" +
                        "(@absEntry," +
                        "@sociedad," +
                        "@articulo," +
                        "@lote," +
                        "@estatus," +
                        "@procesado," +
                        "@fechaRegistro);";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@absEntry", lote.absEntry);
                    comando.Parameters.AddWithValue("@sociedad", lote.sociedad);
                    comando.Parameters.AddWithValue("@articulo", lote.articulo);
                    comando.Parameters.AddWithValue("@lote", lote.lote);
                    comando.Parameters.AddWithValue("@estatus", lote.estatus);
                    comando.Parameters.AddWithValue("@procesado", lote.procesado);
                    comando.Parameters.AddWithValue("@fechaRegistro", lote.fechaRegistro);

                    comando.CommandType = CommandType.Text;

                    id = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                id = 0;
            }

            return id;
        }

        //METODO PARA OBTENER TODOS LOS DOCUMENTOS A ACTUALIZAR
        public List<Lotes> obtenerLotesActualizar()
        {
            List<Lotes> listaDocumentos = new List<Lotes>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * " +
                        "FROM lotesSAP " +
                        "WHERE procesado=0";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Lotes()
                            {
                                estatus = Convert.ToInt32(datareader["estatus"].ToString()),
                                sociedad = datareader["sociedad"].ToString(),
                                absEntry = Convert.ToInt32(datareader["absEntry"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Lotes>();
            }

            return listaDocumentos;
        }

        //METODO PARA ACTUALIZAR LOS LOTES
        public int actualizarSolicitudLote(Lotes lote)
        {
            int id = 0;
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "UPDATE [DesarrolloWeb].[dbo].[lotesSAP]" +
                        "SET " +
                        "procesado = @procesado, " +
                        "fechaProcesado = @fechaProcesado " +
                        "WHERE absEntry= @absEntry";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@absEntry", lote.absEntry);
                    comando.Parameters.AddWithValue("@procesado", lote.procesado);
                    comando.Parameters.AddWithValue("@fechaProcesado", lote.fechaProcesado);
                    comando.CommandType = CommandType.Text;

                    id = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                id = 0;
            }

            return id;
        }
    }
}

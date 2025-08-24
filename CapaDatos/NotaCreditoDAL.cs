using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class NotaCreditoDAL
    {
        //METODO PARA OBTENER LOS DETALLES DE LAS FACTURAS CON NOMBRE DE SOCIO INCORRECTO O VERSION INCORRECTA
        public List<NotaCreditoDetalle> obtenerFacturasCorreccionesV4(string sociedad)
        {
            List<NotaCreditoDetalle> listaDocumentos = new List<NotaCreditoDetalle>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT T1.U_FT3_IDVERSIONCFDI, T1.DocEntry, T2.CardName " +
                        "FROM " + sociedad + ".dbo.ORIN T1 " +
                        "JOIN " + sociedad + ".dbo.OCRD T2 ON T2.CardCode = T1.CardCode " +
                        "WHERE T1.DocDate >= '2023-04-01' " +
                        "AND(T2.CardName <> T1.CardName  OR T1.U_FT3_IDVERSIONCFDI <> 4); ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new NotaCreditoDetalle()
                            {
                                CardName = datareader["CardName"].ToString(),
                                DocEntry = datareader["DocEntry"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<NotaCreditoDetalle>();
            }

            return listaDocumentos;
        }

        //METODO PARA ACTUALIZAR UN ACCESORIO
        public int actualizarNotaCredito(string sociedad, string DocEntry, string Cardname)
        {
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.master))
                {
                    string consulta = "UPDATE " + sociedad + ".[dbo].[ORIN] " +
                    "SET[CardName] = @Cardname " +
                    ",[U_FT3_IDVERSIONCFDI] = 4 " +
                    "WHERE DocEntry = @DocEntry";

                    conexionDB.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.Parameters.AddWithValue("@Cardname", Cardname);
                    comando.Parameters.AddWithValue("@DocEntry", DocEntry);

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }

            return 1;
        }
    }
}

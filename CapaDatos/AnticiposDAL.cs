using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class AnticiposDAL
    {
        //S6-REQUERIDO
        public List<Anticipo> obtenerAnticiposAbiertas()
        {
            List<Anticipo> listaDocumentos = new List<Anticipo>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "SELECT * FROM [DesarrolloWeb].[dbo].[X6 - Anticipos por comprobar] ";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Anticipo()
                            {
                                NumeroAnticipo = datareader["Anticipo"].ToString(),
                                Moneda = datareader["Moneda"].ToString(),
                                TotalAnticipo = datareader["TotalAnticipo"].ToString(),
                                Impuesto = datareader["Impuesto"].ToString(),
                                Pendiente = datareader["Pendiente"].ToString(),
                                DocDate = datareader["DocDate"].ToString(),
                                CardCode = datareader["CardCode"].ToString(),
                                CardName = datareader["CardName"].ToString(),
                                SlpName = datareader["SlpName"].ToString(),
                                Email = datareader["Email"].ToString(),
                                Sociedad = datareader["Sociedad"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Anticipo>();
            }

            return listaDocumentos;
        }
    }
}

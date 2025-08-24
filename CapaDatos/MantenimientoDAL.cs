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
    public class MantenimientoDAL
    {
        public List<Inventario> obtenerInventarioMantenimiento(string sociedad, string articulo, string descripcion, string almacen)
        {
            List<Inventario> listaSemanasStock = new List<Inventario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "";
                    if (articulo == "" && almacen == "" && descripcion == "")
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[M1 - Inventario]";
                    }
                    else
                    {
                        consulta = "select * FROM DESARROLLOWEB.DBO.[M1 - Inventario] where [Artículo] LIKE '%" + articulo + "%' and [Descripción del Artículo] LIKE '%" + descripcion + "%' and [Almacén] LIKE '%" + almacen + "%'  ORDER BY [Artículo], [Almacén], [Fecha de Caducidad] ASC ";
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
                                Artículo = datareader["ArtIculo"].ToString(),
                                Sociedad = datareader["Sociedad"].ToString(),
                                Descripción = datareader["Descripción Articulo"].ToString(),
                                Almacén = datareader["Almacén"].ToString(),
                                Stock = datareader["En Stock"].ToString(),
                                UM = datareader["UM"].ToString()                                
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
    }
}

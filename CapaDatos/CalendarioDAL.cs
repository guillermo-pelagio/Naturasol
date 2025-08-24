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
    public class CalendarioDAL
    {
        public List<Calendario> obtenerEventos()
        {
            List<Calendario> listaDocumentos = new List<Calendario>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select distinct docnum, cardname, por1.ShipDate, por1.ItemCode, por1.Dscription,  case when por1.WhsCode like '%17%' then '#3498db' when WhsCode  like '%18%' then '#3498db' when WhsCode  like '%13%' then '#2ecc71' when WhsCode  like '%23%'  then '#f1c40f' when WhsCode  like '%12%' then '#9b59b6' else '#9b59b6' end as color from TSSL_NATURASOL.dbo.opor " +
                        "join TSSL_NATURASOL.dbo.por1 on opor.docentry = por1.docentry " +
                        "JOIN TSSL_NATURASOL.dbo.OITM ON OITM.ItemCode=POR1.ItemCode AND OITM.U_Familia IN ('MP', 'ME', 'BB', 'CR', 'EV', 'ET', 'EX', 'FR', 'IN', 'MD','TA', 'TR', 'TT', 'BO', 'ST', 'PT') " +
                        "where por1.LineStatus = 'O' and opor.DocStatus = 'O' and ShipDate is not null and POR1.ItemCode is not null ";

                    SqlCommand comando = new SqlCommand(consulta, conexionDB);
                    comando.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comando.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Calendario()
                            {
                                id = datareader["docnum"].ToString(),                                
                                end = Convert.ToDateTime(datareader["ShipDate"].ToString()).ToString("yyyy/MM/dd"),
                                someKey = datareader["ItemCode"].ToString(),
                                start = Convert.ToDateTime(datareader["ShipDate"].ToString()).ToString("yyyy/MM/dd"),
                                title = datareader["cardname"].ToString(),
                                color = datareader["color"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Calendario>();
            }

            return listaDocumentos;
        }
    }
}

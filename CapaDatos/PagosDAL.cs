using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class PagosDAL
    {
        public List<Pagos> obtenerPagosAbiertas()
        {
            List<Pagos> listaDocumentos = new List<Pagos>();
            try
            {
                using (SqlConnection conexionDB = new SqlConnection(ConexionDAL.conexionWeb))
                {
                    string consulta = "select distinct 'NATURASOL' as 'Sociedad', ovpm.DocNum as 'Folio SAP', docdate as 'Fecha', cardcode as 'Codigo socio', case when cardname is not null then cardname else CONCAT('PAGO A CUENTA ',Comments) END as 'Razon', case when DocCurr='MXP' then trsfrsum else trsfrsumfc end as 'Monto', doccurr as 'Moneda',case when ovpm.docrate = 0 then '1.000000' else ovpm.DocRate end as 'T.C.', trsfracct as 'Cuenta', oact.AcctName as 'Nombre cuenta' " +
                                        "from tssl_naturasol.dbo.ovpm " +
                                        "join tssl_naturasol.dbo.oact on oact.AcctCode = ovpm.TrsfrAcct " +
                                        "where Canceled = 'N' and ovpm.CreateDate = Convert(date, getdate())  " +
                                        "union all " +
                                        "select distinct 'MIEL MEX' as 'Sociedad', ovpm.DocNum as 'Folio SAP', docdate as 'Fecha', cardcode as 'Codigo socio', case when cardname is not null then cardname else CONCAT('PAGO A CUENTA ', Comments) END as 'Razon', case when DocCurr = 'MXP' then trsfrsum else trsfrsumfc end as 'Monto', doccurr as 'Moneda', case when ovpm.docrate = 0 then '1.000000' else ovpm.DocRate end as 'T.C.', trsfracct as 'Cuenta', oact.AcctName as 'Nombre cuenta' " +
                                        "from tssl_mielmex.dbo.ovpm " +
                                        "join tssl_mielmex.dbo.oact on oact.AcctCode = ovpm.TrsfrAcct " +
                                        "where Canceled = 'N' and ovpm.CreateDate = Convert(date, getdate())  " +
                                        "order by ovpm.docdate;";

                    SqlCommand comANDo = new SqlCommand(consulta, conexionDB);
                    comANDo.CommandType = CommandType.Text;
                    conexionDB.Open();

                    using (SqlDataReader datareader = comANDo.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDocumentos.Add(new Pagos()
                            {
                                NumeroPago = datareader["Folio SAP"].ToString(),
                                Moneda = datareader["Moneda"].ToString(),
                                TotalPago = datareader["Monto"].ToString(),
                                Cuenta = datareader["Cuenta"].ToString(),
                                DocRate = datareader["T.C."].ToString(),
                                DocDate = datareader["Fecha"].ToString(),
                                CardCode = datareader["Codigo socio"].ToString(),
                                CardName = datareader["Razon"].ToString(),
                                Sociedad = datareader["Sociedad"].ToString(),
                                NombreCuenta = datareader["Nombre cuenta"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDocumentos = new List<Pagos>();
            }

            return listaDocumentos;
        }
    }
}
using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class PagosBLL
    {
        PagosDAL PagosDAL = new PagosDAL();

        public List<Pagos> obtenerPagosAbiertas()
        {
            List<Pagos> listaPagos = new List<Pagos>();
            listaPagos = PagosDAL.obtenerPagosAbiertas();
            return listaPagos;
        }
    }
}

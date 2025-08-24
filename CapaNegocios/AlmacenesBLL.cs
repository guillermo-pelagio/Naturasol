using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class AlmacenesBLL
    {
        AlmacenesDAL almacenesDAL = new AlmacenesDAL();

        //S1-REQUERIDO-METODO PARA OBTENER LOS ALMACENES
        public List<Almacenes> obtenerAlmacenes(string sociedad)
        {
            List<Almacenes> listaAlmacenes = new List<Almacenes>();
            listaAlmacenes = almacenesDAL.listaAlmacenes(sociedad);
            return listaAlmacenes;
        }
    }
}

using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class AnticiposBLL
    {
        AnticiposDAL anticiposDAL = new AnticiposDAL();

        //S6-REQUERIDO
        public List<Anticipo> obtenerAnticiposAbiertas()
        {
            List<Anticipo> listaAnticipos = new List<Anticipo>();
            listaAnticipos = anticiposDAL.obtenerAnticiposAbiertas();
            return listaAnticipos;
        }
    }
}

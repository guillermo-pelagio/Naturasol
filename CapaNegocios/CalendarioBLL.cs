using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class CalendarioBLL
    {
        CalendarioDAL calendarioDAL = new CalendarioDAL();

        public List<Calendario> obtenerEventos()
        {
            return calendarioDAL.obtenerEventos();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TipoTicket
    {
        public int idTipoTicket
        {
            get; set;
        }
        public string descripcionTipoTicket
        {
            get; set;
        }
        public DateTime fechaActualizacion
        {
            get; set;
        }
        public DateTime fechaCreacion
        {
            get; set;
        }
        public int usuarioCreacion
        {
            get; set;
        }
    }
}

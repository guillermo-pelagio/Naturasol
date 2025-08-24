using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Modulos
    {
        public int idModulo
        {
            get; set;
        }
        public string nombreModulo
        {
            get; set;
        }
        public string descripcion
        {
            get; set;
        }
        public int estatusModulo
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


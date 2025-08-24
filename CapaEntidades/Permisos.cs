using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Permisos
    {
        public int idPermiso
        {
            get; set;
        }
        public string usuarioPermiso
        {
            get; set;
        }
        public string moduloPermiso
        {
            get; set;
        }
        public string tipoPermiso
        {
            get; set;
        }

        public string nameUsuario
        {
            get; set;
        }

        public string nombreModulo
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

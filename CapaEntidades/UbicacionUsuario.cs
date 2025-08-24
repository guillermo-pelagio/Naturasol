using System;

namespace CapaEntidades
{
    public class UbicacionUsuario
    {
        public int idUbicacion
        {
            get; set;
        }
        public string descripcionUbicacion
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

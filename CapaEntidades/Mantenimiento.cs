using System;

namespace CapaEntidades
{
    public class Mantenimiento
    {
        public int idMantenimiento
        {
            get; set;
        }

        public int idUsuario
        {
            get; set;
        }

        public int idTipoMtto
        {
            get; set;
        }

        public int idEquipoMtto
        {
            get; set;
        }

        public DateTime inicioMtto
        {
            get; set;
        }

        public DateTime finMtto
        {
            get; set;
        }

        public int estatusMantenimiento
        {
            get; set;
        }
        
        public string comentariosMtto
        {
            get; set;
        }

        public string acciones
        {
            get; set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Usuarios
    {
        public int idUsuario
        {
            get; set;
        }
        public string correo
        {
            get; set;
        }
        public string nameUsuario
        {
            get; set;
        }
        public string nombre
        {
            get; set;
        }
        public string apellidoPaterno
        {
            get; set;
        }
        public string apellidoMaterno
        {
            get; set;
        }
        public int ubicacion
        {
            get; set;
        }
        public int departamento
        {
            get; set;
        }
        public string puesto
        {
            get; set;
        }
        public string telefono
        {
            get; set;
        }
        public string extension
        {
            get; set;
        }
        public int estatus
        {
            get; set;
        }
        public string contrasena
        {
            get; set;
        }
        public DateTime fechaCreacion
        {
            get; set;
        }
        public DateTime fechaActualizacion
        {
            get; set;
        }

        public DateTime ultimoInicioSesion
        {
            get; set;
        }
        
        public int usuarioCreacion
        {
            get; set;
        }
        public string nombreCompleto
        {
            get; set;
        }
        public string accesoPublico
        {
            get; set;
        }

        public string maquina
        {
            get; set;
        }
    }
}

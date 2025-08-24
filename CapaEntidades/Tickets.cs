using System;

namespace CapaEntidades
{
    //OBJETO TICKETS Y SUS PROPIEDADES 
    public class Tickets
    {
        public int idTicket
        {
            get; set;
        }
        public int idUsuarioSolicita
        {
            get; set;
        }
        public string folio
        {
            get; set;
        }
        public int estatus
        {
            get; set;
        }
        public int idUsuarioFinalizo
        {
            get; set;
        }
        public DateTime horaSolicitud
        {
            get; set;
        }
        public DateTime horaFinalizacion
        {
            get; set;
        }
        public string comentarioFinalizacion
        {
            get; set;
        }
        public int departamento
        {
            get; set;
        }
        public string nombreCompleto
        {
            get; set;
        }


        public string correo
        {
            get; set;
        }
        public string nombreUsuario
        {
            get; set;
        }
        public string titulo
        {
            get; set;
        }
        public string descripcion
        {
            get; set;
        }
        public int tipo
        {
            get; set;
        }
        public int ubicacion
        {
            get; set;
        }
        public int prioridad
        {
            get; set;
        }
        public string departamentoTexto
        {
            get; set;
        }
        public string fechaSolicitud
        {
            get; set;
        }
    }
}

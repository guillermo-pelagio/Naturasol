using System;

namespace CapaEntidades
{
    public class BorradorOrdenCompraDetalle
    {
        public int wddCode
        {
            get; set;
        }
        public string sociedad
        {
            get; set;
        }
        public string total
        {
            get; set;
        }
        public string moneda
        {
            get; set;
        }
        public int estatus
        {
            get; set;
        }
        public DateTime fechaRegistro
        {
            get; set;
        }
        public DateTime fechaLiberacion
        {
            get; set;
        }
        public string folioSolicitud
        {
            get; set;
        }
        public string proveedor
        {
            get; set;
        }
        public string comprador
        {
            get; set;
        }
        public string fechaDocumento
        {
            get; set;
        }
    }
}
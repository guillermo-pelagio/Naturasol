using System;

namespace CapaEntidades
{
    public class Lotes
    {
        public int absEntry
        {
            get; set;
        }

        public string articulo
        {
            get; set;
        }

        public string sociedad
        {
            get; set;
        }

        public string lote
        {
            get; set;
        }

        public string descripcion
        {
            get; set;
        }

        public string almacen
        {
            get; set;
        }

        public string almacenDestinoTraslado
        {
            get; set;
        }

        public int procesado
        {
            get; set;
        }

        public int cantidadTraslado
        {
            get; set;
        }

        public int estatus
        {
            get; set;
        }

        public string estatusCadena
        {
            get; set;
        }

        public DateTime fechaRegistro
        {
            get; set;
        }

        public DateTime fechaProcesado
        {
            get; set;
        }
    }
}
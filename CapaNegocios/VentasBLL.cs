using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class VentasBLL
    {
        VentasDAL ventasDAL = new VentasDAL();

        public List<Cotizacion> obtenerCotizacionesWeb()
        {
            List<Cotizacion> inventario = new List<Cotizacion>();
            inventario = ventasDAL.obtenerCotizacionesWeb();
            return inventario;
        }

        public List<Cotizacion> obtenerEstatusCotizaciones()
        {
            List<Cotizacion> inventario = new List<Cotizacion>();
            inventario = ventasDAL.obtenerEstatusCotizaciones();
            return inventario;
        }

        public List<Cotizacion> obtenerEstatusCotizacionesKAM()
        {
            List<Cotizacion> inventario = new List<Cotizacion>();
            inventario = ventasDAL.obtenerEstatusCotizacionesKAM();
            return inventario;
        }

        public List<Cotizacion> obtenerMotivoRechazoCotizaciones()
        {
            List<Cotizacion> motivosRechazo = new List<Cotizacion>();
            motivosRechazo = ventasDAL.obtenerMotivoRechazoCotizaciones();
            return motivosRechazo;
        }

        public int guardarCotizacion(Cotizacion cotizacionWEB, int puesto)
        {
            return ventasDAL.guardarCotizacion(cotizacionWEB, puesto);
        }

        public List<string> obtener_numero_ventas()
        {
            return ventasDAL.obtener_numero_ventas();
        }
    }
}

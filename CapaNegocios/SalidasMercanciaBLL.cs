using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class SalidasMercanciaBLL
    {
        SalidasMercanciaDAL salidasMercanciaDAL = new SalidasMercanciaDAL();

        public List<SalidasMercancia> listaBorradoresLiberacion(string sociedad)
        {
            List<SalidasMercancia> listaCompradorOCAbiertas = new List<SalidasMercancia>();
            listaCompradorOCAbiertas = salidasMercanciaDAL.listaBorradoresLiberacion(sociedad);
            return listaCompradorOCAbiertas;
        }

        public List<SalidasMercancia> listaBorradoresLiberacionDetalle(string sociedad, string docentry)
        {
            List<SalidasMercancia> listaCompradorOCAbiertas = new List<SalidasMercancia>();
            listaCompradorOCAbiertas = salidasMercanciaDAL.listaBorradoresLiberacionDetalle(sociedad, docentry);
            return listaCompradorOCAbiertas;
        }

        public List<SalidasMercancia> listaBorradoresLiberacionDetalleAnexos(string sociedad, string docentry)
        {
            List<SalidasMercancia> listaCompradorOCAbiertas = new List<SalidasMercancia>();
            listaCompradorOCAbiertas = salidasMercanciaDAL.listaBorradoresLiberacionDetalleAnexos(sociedad, docentry);
            return listaCompradorOCAbiertas;
        }

        public int actualizarMuestraAutorizada(string sociedad, string wddCode)
        {
            return salidasMercanciaDAL.actualizarMuestraAutorizada(sociedad, wddCode);
        }

        public int guardarSolicitudAutorizacionMuestras(SalidasMercancia borradorOrdenCompra)
        {
            return salidasMercanciaDAL.guardarSolicitudAutorizacionMuestras(borradorOrdenCompra);
        }

    }
}

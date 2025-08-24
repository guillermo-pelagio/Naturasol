using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class SolicitudCompraBLL
    {
        SolicitudCompraDAL solicitudCompraDAL = new SolicitudCompraDAL();

        //S15-REQUERIDO
        public List<SolicitudCompra> obtenerSDCAbiertas(string sociedad, string email)
        {
            return solicitudCompraDAL.obtenerSDCAbiertas(sociedad, email);
        }

        //S15-ACTUALIZACION-METODO PARA CERRA LAS ST ABIERTAS DESPUES DE 2 DIAS
        public void cerrarSDCAbiertas(string DocEntry)
        {
            try
            {
                SAPbobsCOM.Documents oPurchaseRequest;
                oPurchaseRequest = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);
                oPurchaseRequest.GetByKey(Convert.ToInt32(DocEntry));
                oPurchaseRequest.Close();

                int respuesta = oPurchaseRequest.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("SDC actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la SDC " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la SDC " + ex);
            }

        }

        //S15-REQUERIDO
        public List<SolicitudCompra> obtenerSolicitanteSDCAbiertas(string sociedad)
        {
            List<SolicitudCompra> listaSolicitanteAbiertas = new List<SolicitudCompra>();
            listaSolicitanteAbiertas = solicitudCompraDAL.obtenerSolicitanteSDCAbiertas(sociedad);
            return listaSolicitanteAbiertas;
        }

        //S35-REQUERIDO        
        public List<SolicitudCompra> obtenerSDCDirectos(string sociedadVenta)
        {
            List<SolicitudCompra> listaSDCDirectos = new List<SolicitudCompra>();
            //listaSDCDirectos = solicitudCompraDAL.obtenerSDCDirectos(sociedadVenta);
            return listaSDCDirectos;
        }

    }
}

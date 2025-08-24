using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class PagosEfectuadosBLL
    {
        PagosEfectuadosDAL pagosEfectuadosDAL = new PagosEfectuadosDAL();

        public List<PagosEfectuados> obtenerPagosNotificarBasico(string sociedad)
        {
            return pagosEfectuadosDAL.obtenerPagosNotificarBasico(sociedad);
        }

        public List<PagosEfectuados> obtenerInfoCXP(string sociedad)
        {
            return pagosEfectuadosDAL.obtenerInfoCXP(sociedad);
        }

        //METODO DE OBTENER PAGOS A NOTIFICAR A PROVEEDOR
        public List<PagosEfectuados> obtenerPagosNotificarAvanzado(string sociedad, string docnum)
        {
            return pagosEfectuadosDAL.obtenerPagosNotificarAvanzado(sociedad, docnum);
        }

        //METODO DE OBTENER PAGOS A NOTIFICAR A PROVEEDOR
        public List<PagosEfectuados> obtenerPagosNotificarDetalle(string sociedad, string docnum, string invtype)
        {
            return pagosEfectuadosDAL.obtenerPagosNotificarDetalle(sociedad, docnum, invtype);
        }

        //METODO PARA ACTUALIZAR EL PAGO A SAP
        public static int updatePago(int numeroDocumento, string estatus)
        {            
            SAPbobsCOM.Payments oVendorPayments;
            oVendorPayments = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
            oVendorPayments.GetByKey(numeroDocumento);
            oVendorPayments.UserFields.Fields.Item("U_EstatusPago").Value = estatus;

            int respuesta = oVendorPayments.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Pago actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar el pago " + DIAPIDAL.company.GetLastErrorDescription());
            }

            return respuesta;
        }
    }
}
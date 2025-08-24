using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class PagosBancomerBLL
    {
        PagosBancomerDAL pagosBancomerDAL = new PagosBancomerDAL();

        //METODO DE OBTENER CORREOS
        public List<PagosBancomer> obtenerPagos(int tipoPago, int sociedad, int usuario)
        {
            return pagosBancomerDAL.obtenerPagos(tipoPago, sociedad, usuario);
        }

        //S3-ACTUALIZACION-METODO PARA ACTUALIZAR EL PAGO A SAP
        public static int updatePago(int numeroDocumento)
        {
            SAPbobsCOM.Payments oVendorPayments;
            oVendorPayments = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
            oVendorPayments.GetByKey(numeroDocumento);
            oVendorPayments.UserFields.Fields.Item("U_EstatusPago").Value = "Notificar";

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

        //METODO PARA GUARDAR EL PAGO EN DESARROLLO
        public static int insertPagoIntermedio(int numeroDocumento, string sociedad)
        {
            PagosBancomerDAL pagosBancomerDAL = new PagosBancomerDAL();
            return pagosBancomerDAL.insertPagoIntermedio(numeroDocumento, sociedad);
        }

        //S3-ACTUALIZACIONMETODO PARA ACTUALIZAR EL PAGO EN DESARROLLO
        public static int updatePagoDesarrollo(string numeroDocumento, string sociedad)
        {
            PagosBancomerDAL pagosBancomerDAL = new PagosBancomerDAL();
            return pagosBancomerDAL.updatePagoDesarrollo(numeroDocumento, sociedad);
        }

        //METODO PARA BUSCAR LA CLAVE DEL BANCO EN DESARROLLO
        public String buscarClaveBanco(string numeroBanco)
        {
            return pagosBancomerDAL.buscarClaveBanco(numeroBanco);
        }

        //S3-REQUERIDO-METODO PARA BUSCAR LOS PENDIENTES DE SAP
        public List<PagosBancomer> buscarPagosPendientesSAP()
        {
            return pagosBancomerDAL.buscarPagosPendientesSAP();
        }
    }
}
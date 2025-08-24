using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class PagosBanamexBLL
    {
        PagosBanamexDAL pagosBanamexDAL = new PagosBanamexDAL();

        //METODO DE OBTENER CORREOS
        public List<PagosBanamex> obtenerPagos(int sociedad, int usuario)
        {
            return pagosBanamexDAL.obtenerPagos(sociedad, usuario);
        }

        public static int updatePago(int numeroDocumento)
        {
            SAPbobsCOM.Payments oVendorPayments;
            oVendorPayments = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
            oVendorPayments.GetByKey(numeroDocumento);
            oVendorPayments.UserFields.Fields.Item("U_EstatusPago").Value = "Actualizado";

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

        //METODO PARA INSERTAR EL PAGO EN LA TABLA DESARROLLO
        public static int insertPagoIntermedio(int numeroDocumento, string sociedad)
        {
            PagosBanamexDAL pagosBanamexDAL = new PagosBanamexDAL();
            return pagosBanamexDAL.insertPagoIntermedio(numeroDocumento, sociedad);
        }
    }
}
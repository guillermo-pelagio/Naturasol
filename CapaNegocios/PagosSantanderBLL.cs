using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class PagosSantanderBLL
    {
        PagosSantanderDAL pagosSantanderDAL = new PagosSantanderDAL();

        //METODO DE OBTENER CORREOS
        public List<PagosSantander> obtenerPagos(int sociedad)
        {
            return pagosSantanderDAL.obtenerPagos(sociedad);
        }

        //METODO PARA ACTUALIZAR EL PAGO A SAP
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

        //METODO PARA GUARDAR EL PAGO EN DESARROLLO
        public static int insertPagoIntermedio(int numeroDocumento, string sociedad)
        {
            PagosSantanderDAL pagosSantanderDAL = new PagosSantanderDAL();
            return pagosSantanderDAL.insertPagoIntermedio(numeroDocumento, sociedad);
        }

        //METODO PARA ACTUALIZAR EL PAGO EN DESARROLLO
        public static int updatePagoDesarrollo(string numeroDocumento, string sociedad)
        {
            PagosSantanderDAL pagosSantanderDAL = new PagosSantanderDAL();
            return pagosSantanderDAL.updatePagoDesarrollo(numeroDocumento, sociedad);
        }

        //METODO PARA BUSCAR LA CLAVE DEL BANCO EN DESARROLLO
        public String buscarClaveBanco(string numeroBanco)
        {
            return pagosSantanderDAL.buscarClaveBanco(numeroBanco);
        }

        //METODO PARA BUSCAR LOS PENDIENTES DE SAP
        public List<PagosSantander> buscarPagosPendientesSAP()
        {
            //return pagosSantanderDAL.buscarPagosPendientesSAP();
            return null;
        }
    }
}
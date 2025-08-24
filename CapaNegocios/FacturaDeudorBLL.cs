using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class FacturaDeudorBLL
    {
        FacturaDeudorDAL facturaDeudorDAL = new FacturaDeudorDAL();

        //METODO DE BUSQUEDA DE FACTURAS DE DEUDOR INTERCOMPANIA
        public List<FacturaDeudor> obtenerFacturaDeudorIntercompania(string sociedad)
        {
            List<FacturaDeudor> listaFacturasDeudor = new List<FacturaDeudor>();
            listaFacturasDeudor = facturaDeudorDAL.obtenerFacturaDeudorIntercompania(sociedad);
            return listaFacturasDeudor;
        }

        //METODO PARA BUSCAR EL DETALLE DE UNA FACTURA DE DEUDOR INTERCOMPANIA
        public List<FacturaDeudorDetalle> obtenerFacturaDeudorDetalleIntercompania(string sociedad, FacturaDeudor facturaDeudor)
        {
            List<FacturaDeudorDetalle> listaFacturasDeudorDetalle = new List<FacturaDeudorDetalle>();
            listaFacturasDeudorDetalle = facturaDeudorDAL.obtenerFacturaDeudorDetalleIntercompania(sociedad, facturaDeudor);
            return listaFacturasDeudorDetalle;
        }

        //METODO PARA BUSCAR LAS FACTURAS PENDIENTES DE CORRECCION
        public List<FacturaDeudorDetalle> obtenerFacturasCorreccionesV4(string sociedad)
        {
            List<FacturaDeudorDetalle> listaFacturasDeudorDetalle = new List<FacturaDeudorDetalle>();
            listaFacturasDeudorDetalle = facturaDeudorDAL.obtenerFacturasCorreccionesV4(sociedad);
            return listaFacturasDeudorDetalle;
        }

        //REVISADO
        public List<FacturaDeudor> obtenerReservaSinEntrega()
        {
            List<FacturaDeudor> listaFacturasDeudorDetalle = new List<FacturaDeudor>();
            listaFacturasDeudorDetalle = facturaDeudorDAL.obtenerReservaSinEntrega();
            return listaFacturasDeudorDetalle;
        }        

        //METODO PARA ACTUALIZAR LA FACTURA CON NOMBRES DIFERENTES Y TIMBRADO
        public void actualizarFacturaDeudorCorrecciones(string sociedad, string DocEntry, string Cardname)
        {
            /*
            SAPbobsCOM.Documents oInvoices;
            oInvoices = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

            oInvoices.GetByKey(Convert.ToInt32(DocEntry));

            oInvoices.CardName = Cardname;
            //oInvoices.UserFields.Fields.Item("U_FT3_IDVERSIONCFDI").Value = "4";

            int respuesta = oInvoices.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Documento actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar el documento" + DIAPIDAL.company.GetLastErrorDescription());
            }
            */
            facturaDeudorDAL.actualizarFacturaDeudor(sociedad, DocEntry, Cardname);
        }

        //METODO PARA ACTUALIZAR LA NOTA DE CREDITO CON NOMBRES DIFERENTES Y TIMBRADO
        public void actualizarNotaCreditoDeudorCorrecciones(string DocEntry, string Cardname)
        {
            SAPbobsCOM.Documents oCreditNotes;
            oCreditNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

            oCreditNotes.GetByKey(Convert.ToInt32(DocEntry));

            oCreditNotes.CardName = Cardname;
            oCreditNotes.UserFields.Fields.Item("U_INT_GENERAR").Value = "4";

            int respuesta = oCreditNotes.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Documento actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar el documento" + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        //METODO PARA ACTUALIZAR LA FACTURA DE DEUDOR DESPUES DE CREAR LA DE PROVEEDOR
        public void actualizarFacturaDeudor(string DocEntry, string DocNum, string sociedadVenta, string mensaje)
        {
            SAPbobsCOM.Documents oInvoices;
            oInvoices = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
            oInvoices.GetByKey(Convert.ToInt32(DocEntry));

            if (mensaje.Equals(""))
            {
                oInvoices.UserFields.Fields.Item("U_INT_GENERAR").Value = "N";
                oInvoices.UserFields.Fields.Item("U_INT_DOCRE").Value = DocNum;
                oInvoices.UserFields.Fields.Item("U_INT_COMMENTS").Value = "Factura de proveedor creada en " + sociedadVenta + ", folio: " + DocNum;
            }
            else
            {
                oInvoices.UserFields.Fields.Item("U_INT_GENERAR").Value = "E";
                oInvoices.UserFields.Fields.Item("U_INT_COMMENTS").Value = mensaje;
            }

            int respuesta = oInvoices.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Factura de deudor actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la Factura de Deudor " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

    }
}

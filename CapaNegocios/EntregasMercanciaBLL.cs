using CapaDatos;
using CapaEnidades;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class EntregasMercanciaBLL
    {
        EntregaMercanciaDAL entregaMercanciaDAL = new EntregaMercanciaDAL();

        //METODO DE BUSQUEDA DE ENTREGAS INTERCOMPANIA
        public List<EntregaMercancia> obtenerEntregasIntercompania(string sociedad)
        {
            List<EntregaMercancia> listaOrdenesCompra = new List<EntregaMercancia>();
            listaOrdenesCompra = entregaMercanciaDAL.obtenerEntregasIntercompania(sociedad);
            return listaOrdenesCompra;
        }

        //S22-REQUERIDO
        public List<EntregaMercancia> obtenerEntregaSinFactura()
        {
            List<EntregaMercancia> listaEntregaSinFactura = new List<EntregaMercancia>();
            listaEntregaSinFactura = entregaMercanciaDAL.obtenerEntregaSinFactura();
            return listaEntregaSinFactura;
        }

        //BUSCAR EL DETALLE DE LAS ENTREGAS INTERCOMPANIA
        public List<EntregaMercanciaDetalle> obtenerEntregasDetalleIntercompania(string sociedad, EntregaMercancia orden)
        {
            List<EntregaMercanciaDetalle> listaOrdenesCompraDetalle = new List<EntregaMercanciaDetalle>();
            listaOrdenesCompraDetalle = entregaMercanciaDAL.obtenerEntregasDetalleIntercompania(sociedad, orden);
            return listaOrdenesCompraDetalle;
        }

        //OBTENER LOS LOTES DE LA ENTREGA
        public List<LoteEntregaMercancia> obtenerLotesEntregaDetalleIntercompania(string sociedad, EntregaMercanciaDetalle entregaMercanciaDetalle)
        {
            List<LoteEntregaMercancia> listaOrdenesCompraDetalle = new List<LoteEntregaMercancia>();
            listaOrdenesCompraDetalle = entregaMercanciaDAL.obtenerLotesEntregaDetalleIntercompania(sociedad, entregaMercanciaDetalle);
            return listaOrdenesCompraDetalle;
        }

        //ACTUALIZAR LA ENTREGA EN SAP
        public void actualizarEntrega(string docEntry, string docNum, string sociedadEntrega, string error)
        {
            SAPbobsCOM.Documents oDeliveryNotes;
            oDeliveryNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            oDeliveryNotes.GetByKey(Convert.ToInt32(docEntry));

            if (error.Equals(""))
            {
                oDeliveryNotes.UserFields.Fields.Item("U_INT_GENERAR").Value = "N";
                oDeliveryNotes.UserFields.Fields.Item("U_INT_DOCRE").Value = docNum;
                oDeliveryNotes.UserFields.Fields.Item("U_INT_COMMENTS").Value = "Entrada creada en " + sociedadEntrega + ", folio: " + docNum;
            }
            else
            {
                oDeliveryNotes.UserFields.Fields.Item("U_INT_GENERAR").Value = "E";
                oDeliveryNotes.UserFields.Fields.Item("U_INT_DOCRE").Value = "";
                oDeliveryNotes.UserFields.Fields.Item("U_INT_COMMENTS").Value = error;
            }

            int respuesta = oDeliveryNotes.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Entrada actualizada");
            }
            else
            {
                Console.WriteLine("Error al actualizar la entrada " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        public List<EntregaMercancia> obtenerEntradaIntercompania(EntregaMercancia entregaMercancia, string sociedadVenta)
        {
            return entregaMercanciaDAL.obtenerEntradaIntercompania(entregaMercancia, sociedadVenta);
        }
        //METODO PARA ACTUALIZAR LA OV DESPUES DE CREAR LA ORDEN DE COMPRA
        public void actualizarEntregaIntercompania(string DocEntry, string DocNum, string mensaje, string sociedadDestino)
        {
            SAPbobsCOM.Documents oDeliveryNotes;
            oDeliveryNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            oDeliveryNotes.GetByKey(Convert.ToInt32(DocEntry));

            if (mensaje.Equals(""))
            {
                //oDeliveryNotes.UserFields.Fields.Item("U_REPLICAR").Value = "0";
                oDeliveryNotes.UserFields.Fields.Item("U_NoCaja").Value = DocNum;
                oDeliveryNotes.NumAtCard = (sociedadDestino.Contains("MIELMEX") ? "MIE-" : sociedadDestino.Contains("NATURASOL") ? "NAT-" : sociedadDestino.Contains("NOVAL") ? "NOV-" : "EVI-") + DocNum;
            }
            else
            {
                //oDeliveryNotes.UserFields.Fields.Item("U_REPLICAR").Value = "0";
                //oDeliveryNotes.UserFields.Fields.Item("U_NoCaja").Value = DocNum;
                oDeliveryNotes.Comments = mensaje;
            }

            int respuesta = oDeliveryNotes.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OV actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OV " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        public List<EntregaMercanciaDetalle> obtenerEntradasDetalleIntercompania(string sociedad, EntregaMercancia orden)
        {
            List<EntregaMercanciaDetalle> listaOrdenesVentaDetalle = new List<EntregaMercanciaDetalle>();
            listaOrdenesVentaDetalle = entregaMercanciaDAL.obtenerOrdenesVentaDetalleIntercompania(sociedad, orden);
            return listaOrdenesVentaDetalle;
        }

    }
}
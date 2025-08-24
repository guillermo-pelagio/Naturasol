using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class FacturaProveedorBLL
    {
        FacturaProveedorDAL facturaProveedorDAL = new FacturaProveedorDAL();

        //METODO PARA CREAR LA FACTURA DE PROVEEDOR A SAP
        public string crearFacturaProveedor(EntradaMaterial entradaMercancia)
        {
            SAPbobsCOM.Documents oPurchaseInvoices;
            oPurchaseInvoices = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices);

            //SE OBTIENE LA ENTRADA
            string numeroEntrada = entradaMercancia.DocNum;
            EntradasBLL entradasBLL = new EntradasBLL();
            //OBTENER EL DETALLE DE LA ENTRADA
            List<EntradaMaterialDetalle> entradaMaterialDetalle = new List<EntradaMaterialDetalle>();
            entradaMaterialDetalle = entradasBLL.obtenerEntrada("TSSL_NATURASOL", numeroEntrada);

            //ENCABEZADO
            oPurchaseInvoices.CardCode = entradaMaterialDetalle[0].CardCode;
            oPurchaseInvoices.DocDate = DateTime.Now;
            oPurchaseInvoices.DocDueDate = DateTime.Now;
            oPurchaseInvoices.DocCurrency = entradaMaterialDetalle[0].Currency;
            oPurchaseInvoices.NumAtCard = entradaMercancia.UUID;
            oPurchaseInvoices.Comments = "Factura de proveedor basado en entrada : " + numeroEntrada ;
            oPurchaseInvoices.DocType = entradaMaterialDetalle[0].DocType == "I" ? SAPbobsCOM.BoDocumentTypes.dDocument_Items : SAPbobsCOM.BoDocumentTypes.dDocument_Service;

            for (int i = 0; i < entradaMaterialDetalle.Count; i++)
            {
                //DETALLE
                oPurchaseInvoices.Lines.BaseType = 20;
                oPurchaseInvoices.Lines.BaseEntry = Convert.ToInt32(entradaMaterialDetalle[i].DocEntry);
                oPurchaseInvoices.Lines.Quantity = Convert.ToDouble(entradaMaterialDetalle[i].Quantity);
                oPurchaseInvoices.Lines.BaseLine = Convert.ToInt32(entradaMaterialDetalle[i].LineNum);
                oPurchaseInvoices.Lines.TaxCode = entradaMaterialDetalle[i].TaxCode;
                oPurchaseInvoices.Lines.WarehouseCode = entradaMaterialDetalle[i].WhsCode;


                oPurchaseInvoices.Lines.Add();
            }

            int respuesta = oPurchaseInvoices.Add();
            string mensaje;

            if (respuesta == 0)
            {
                mensaje = "OK";
            }
            else
            {
                mensaje = DIAPIDAL.company.GetLastErrorDescription();
                Console.WriteLine("Error al crear la Factura de Proveedor " + DIAPIDAL.company.GetLastErrorDescription());
            }

            return mensaje;
        }

        //METODO PARA OBTENER LAS FACTURAS DE PROVEEDOR INTERCOMPANIA QUE SE CREARON
        public List<FacturaProveedor> obtenerFacturaProveedorIntercompania(FacturaDeudor facturaDeudor, string sociedadVenta)
        {
            List<FacturaProveedor> listaFacturasProveedor = new List<FacturaProveedor>();
            listaFacturasProveedor = facturaProveedorDAL.obtenerFacturaProveedorIntercompania(facturaDeudor, sociedadVenta);
            return listaFacturasProveedor;
        }
    }
}

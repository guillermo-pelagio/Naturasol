using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class InventarioBLL
    {
        InventarioDAL inventarioDAL = new InventarioDAL();

        //S10-REQUERIDO
        public List<Inventario> obtenerClientesKAM()
        {
            List<Inventario> listaClientes = new List<Inventario>();
            listaClientes = inventarioDAL.obtenerClientesKAM();
            return listaClientes;
        }

        //S10-REQUERIDO
        public List<Inventario> obtenerArticulosCaducar(string u_CLIENTE)
        {
            List<Inventario> listaArticulos = new List<Inventario>();
            listaArticulos = inventarioDAL.obtenerArticulosCaducar(u_CLIENTE);
            return listaArticulos;
        }

        public List<Inventario> obtenerAjustesMes(string basesDatos, int ajustes, int tipoAjustes)
        {
            List<Inventario> listaArticulos = new List<Inventario>();
            listaArticulos = inventarioDAL.obtenerAjustesMes(basesDatos, ajustes, tipoAjustes);
            return listaArticulos;
        }

        public List<Inventario> obtenerArticulosSinUbicacion()
        {
            List<Inventario> listaArticulos = new List<Inventario>();
            listaArticulos = inventarioDAL.obtenerArticulosSinUbicacion();
            return listaArticulos;
        }

        //S10-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducas(string u_ARTICULO, string dias)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            listaExistencias = inventarioDAL.obtenerExistenciasCaducas(u_ARTICULO, dias);
            return listaExistencias;
        }

        //S8-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducadas(int tipoProducto)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            listaExistencias = inventarioDAL.obtenerExistenciasCaducadas(tipoProducto);
            return listaExistencias;
        }

        //S9-REQUERIDO
        public List<Inventario> obtenerExistenciasCaducasMPST(int dias)
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            listaExistencias = inventarioDAL.obtenerExistenciasCaducasMPST(dias);
            return listaExistencias;
        }

        //REVISADO
        public List<Inventario> obtenerPTSinMovimiento()
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            listaExistencias = inventarioDAL.obtenerPTSinMovimiento();
            return listaExistencias;
        }

        //REVISADO
        public List<Inventario> obtenerMPSinConsumo()
        {
            List<Inventario> listaExistencias = new List<Inventario>();
            listaExistencias = inventarioDAL.obtenerMPSinConsumo();
            return listaExistencias;
        }

        public DataTable obtenerExcel(Reportes reporte)
        {
            return inventarioDAL.obtenerExcel(reporte);
        }

        //S24-REQUERIDO
        public List<Inventario> obtenerSemanasStock()
        {
            List<Inventario> listaSemanasStock = new List<Inventario>();
            listaSemanasStock = inventarioDAL.obtenerSemanasStock();
            return listaSemanasStock;
        }

        public List<Inventario> obtenerInventarioActualWeb(string sociedad, string articulo, string descripcion, string almacen, string lote)
        {
            List<Inventario> inventario = new List<Inventario>();
            inventario = inventarioDAL.obtenerInventarioActualWeb(sociedad, articulo, descripcion, almacen, lote);
            return inventario;
        }

        public List<Kardex> obtenerKardexWeb(string sociedad, string articulo, string descripcion, string almacen, string lote, string fechaInicio, string fechaFin)
        {
            List<Kardex> inventario = new List<Kardex>();
            inventario = inventarioDAL.obtenerKardexWeb(sociedad, articulo, descripcion, almacen, lote, fechaInicio, fechaFin);
            return inventario;
        }

        public List<SolicitudTrasladoDetalle> obtenerLoteST(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            inventario = inventarioDAL.obtenerLoteST(solicitudTrasladoDetalle);
            return inventario;
        }

        public List<SolicitudTrasladoDetalle> confirmarExistenciaLote(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            inventario = inventarioDAL.confirmarExistenciaLote(solicitudTrasladoDetalle);
            return inventario;
        }



        public List<SolicitudTrasladoDetalle> obtenerConfirmaciones(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            inventario = inventarioDAL.obtenerConfirmaciones(solicitudTrasladoDetalle);
            return inventario;
        }

        public List<SolicitudTrasladoDetalle> obtenerConfirmaciones2(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            inventario = inventarioDAL.obtenerConfirmaciones2(solicitudTrasladoDetalle);
            return inventario;
        }

        public List<SolicitudTrasladoDetalle> obtenerParcialConfirmado(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            //inventario = inventarioDAL.obtenerParcialConfirmado(solicitudTrasladoDetalle);
            return inventario;
        }

        public List<SolicitudTrasladoDetalle> obteneSTTS(string ubicacion, string puesto, string articulo, string descripcion, string almacen1, string almacen2, string fechaInicio, string fechaFin)
        {
            List<SolicitudTrasladoDetalle> inventario = new List<SolicitudTrasladoDetalle>();
            inventario = inventarioDAL.obteneSTTS(ubicacion, puesto, articulo, descripcion, almacen1, almacen2, fechaInicio, fechaFin);
            return inventario;
        }

        public List<TransferenciaStockDetalle> obtenerTS(string folio, string itemCode)
        {
            List<TransferenciaStockDetalle> transferencia = new List<TransferenciaStockDetalle>();
            transferencia = inventarioDAL.obtenerTS(folio, itemCode);
            return transferencia;
        }

        public List<SolicitudTrasladoDetalle> obtenerSurtidores(string ubicacion, string sociedad)
        {
            return inventarioDAL.obtenerSurtidores(ubicacion, sociedad);
        }

        public int guardarSurtidoParcial(string DocEntry, string usuario)
        {
            int resultado;
            resultado = inventarioDAL.guardarSurtidoParcial(DocEntry, usuario);
            return resultado;
        }

        public int actualizarEstatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int resultado;
            resultado = inventarioDAL.actualizarEstatus(solicitudTraslado);
            return resultado;
        }

        public int regresarEstatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int resultado;
            resultado = inventarioDAL.regresarEstatus(solicitudTraslado);
            return resultado;
        }

        public int actualizarEstatus2(string DocEntry, string usuario)
        {
            int resultado;
            resultado = inventarioDAL.actualizarEstatus2(DocEntry, usuario);
            return resultado;
        }
        public int actualizarEstatus3(string DocEntry, string usuario)
        {
            int resultado;
            resultado = inventarioDAL.actualizarEstatus3(DocEntry, usuario);
            return resultado;
        }

        public int actualizarEstatus4(string DocEntry, string usuario)
        {
            int resultado;
            resultado = inventarioDAL.actualizarEstatus4(DocEntry, usuario);
            return resultado;
        }

        public int actualizarEstatusSurtidoParcial(SolicitudTrasladoDetalle solicitudTraslado)
        {
            int resultado;
            resultado = inventarioDAL.actualizarEstatusSurtidoParcial(solicitudTraslado);
            return resultado;
        }

        public List<BorradorDocumento> obtenerMuestrasAutorizar()
        {
            List<BorradorDocumento> borradorMuestras = new List<BorradorDocumento>();
            borradorMuestras = inventarioDAL.obtenerMuestrasAutorizar();
            return borradorMuestras;
        }

        public List<BorradorDocumento> obtenerMuestrasAutorizarDetalle(int wddCode)
        {
            List<BorradorDocumento> borradorMuestras = new List<BorradorDocumento>();
            borradorMuestras = inventarioDAL.obtenerMuestrasAutorizarDetalle(wddCode);
            return borradorMuestras;
        }

        public int actualizarSalidaMercancia(int numeroDocumento, string estatus, string motivoRechazo)
        {
            int estatuss = 0;

            try
            {
                SAPbobsCOM.ApprovalRequestsService oApprovalRequestsService = null;
                SAPbobsCOM.ApprovalRequestParams oApprovalRequestParams = null;
                SAPbobsCOM.ApprovalRequest oApprovalRequest = null;
                SAPbobsCOM.ApprovalRequestDecision oApprovalRequestDecision = null;
                SAPbobsCOM.CompanyService oCompanyService = DIAPIDAL.company.GetCompanyService();

                oApprovalRequestsService =
                  (SAPbobsCOM.ApprovalRequestsService)oCompanyService.GetBusinessService(
                      SAPbobsCOM.ServiceTypes.ApprovalRequestsService);
                oApprovalRequestParams =
                  (SAPbobsCOM.ApprovalRequestParams)oApprovalRequestsService.GetDataInterface(
                      SAPbobsCOM.ApprovalRequestsServiceDataInterfaces.arsApprovalRequestParams);

                int wddCode = numeroDocumento;
                oApprovalRequestParams.Code = wddCode;
                oApprovalRequest = oApprovalRequestsService.GetApprovalRequest(oApprovalRequestParams);
                oApprovalRequestDecision = oApprovalRequest.ApprovalRequestDecisions.Add();

                if (estatus.Equals("ardApproved"))
                {
                    oApprovalRequestDecision.Status = SAPbobsCOM.BoApprovalRequestDecisionEnum.ardApproved;
                }
                else if (estatus.Equals("ardNotApproved"))
                {
                    //oApprovalRequestDecision.Status = SAPbobsCOM.BoApprovalRequestDecisionEnum.ardNotApproved;
                    oApprovalRequestDecision.Remarks = motivoRechazo;
                }

                oApprovalRequestsService.UpdateRequest(oApprovalRequest);
                estatuss = 0;
            }

            catch (Exception ex)
            {
                estatuss = 3;
                Console.WriteLine(ex);
            }
            return estatuss;

        }

        public string crearTransferencia(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            string respuestafinal = "0";
            string respuesta = "";
            OrdenFabricacionDAL ordenFabricacionDAL = new OrdenFabricacionDAL();

            try
            {
                DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                SAPbobsCOM.StockTransfer stockTransferPT;
                stockTransferPT = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                stockTransferPT.DocDate = DateTime.Now;
                stockTransferPT.PriceList = 12;

                stockTransferPT.ToWarehouse = solicitudTrasladoDetalle.ToWhsCodeST;
                stockTransferPT.FromWarehouse = solicitudTrasladoDetalle.FillerST;
                stockTransferPT.ShipToCode = null;
                stockTransferPT.UserFields.Fields.Item("U_ListaPesos").Value = solicitudTrasladoDetalle.idSurtido;

                stockTransferPT.Lines.ItemCode = solicitudTrasladoDetalle.ItemCodeST;
                stockTransferPT.Lines.BaseEntry = Convert.ToInt32(solicitudTrasladoDetalle.DocEntry.Split('-')[0]);
                stockTransferPT.Lines.BaseLine = Convert.ToInt32(solicitudTrasladoDetalle.DocEntry.Split('-')[1]);
                stockTransferPT.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                /*stockTransferPT.Lines.DistributionRule = "TULTEPK";
                stockTransferPT.Lines.DistributionRule3 = "MANUFAC";*/
                stockTransferPT.Lines.WarehouseCode = solicitudTrasladoDetalle.ToWhsCodeST;
                stockTransferPT.Lines.FromWarehouseCode = solicitudTrasladoDetalle.FillerST;
                stockTransferPT.Lines.Quantity = Convert.ToDouble(solicitudTrasladoDetalle.QuantityST);

                stockTransferPT.Lines.BatchNumbers.BatchNumber = solicitudTrasladoDetalle.DistNumber;
                stockTransferPT.Lines.BatchNumbers.Quantity = Convert.ToDouble(solicitudTrasladoDetalle.QuantityST);
                stockTransferPT.Lines.BatchNumbers.Add();

                stockTransferPT.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                stockTransferPT.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                stockTransferPT.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionOrigen(solicitudTrasladoDetalle.FillerST, solicitudTrasladoDetalle.DistNumber, solicitudTrasladoDetalle.ItemCodeST)[0].BinCode);
                stockTransferPT.Lines.BinAllocations.Quantity = Convert.ToDouble(solicitudTrasladoDetalle.QuantityST);
                stockTransferPT.Lines.BinAllocations.Add();


                stockTransferPT.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                stockTransferPT.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                stockTransferPT.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionDestino("TSSL_NATURASOL", solicitudTrasladoDetalle.ToWhsCodeST)[0].BinCode);
                stockTransferPT.Lines.BinAllocations.Quantity = Convert.ToDouble(solicitudTrasladoDetalle.QuantityST);
                stockTransferPT.Lines.BinAllocations.Add();


                stockTransferPT.Lines.Add();

                stockTransferPT.Comments = "TRANSFERENCIA INTRANET" + solicitudTrasladoDetalle.idSurtido;
                respuestafinal = Convert.ToString(stockTransferPT.Add());

                if (respuestafinal == "0")
                {
                    Console.WriteLine("TRANSFERENCIA CREADA");

                }
                else
                {
                    respuesta = DIAPIDAL.company.GetLastErrorDescription();
                    Console.WriteLine("Error al crear el TRASPASO a produccion " + DIAPIDAL.company.GetLastErrorDescription());
                    respuestafinal = respuesta;

                }
                DIAPIBLL.desconectarDIAPI();
            }
            catch (Exception ex)
            {

            }
            return respuestafinal;
        }

        public string buscarMovimientoSAP(string idSurtido)
        {
            string resultado;
            resultado = inventarioDAL.buscarMovimientoSAP(idSurtido);
            return resultado;
        }
    }
}

using CapaDatos;
using CapaEntidades;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocios
{
    public class LotesBLL
    {
        LotesDAL lotesDAL = new LotesDAL();

        //METODO DE OBTENCION DE LOTES POR ARTICULO/LOTE/ESTATUS
        public DataSet obtenerLotes(string basedatos, string articulo, string lote, string estatus)
        {
            DataSet dataset = new DataSet();
            dataset = lotesDAL.obtenerLotes(basedatos, articulo, lote, estatus);
            return dataset;
        }

        //METODO DE ALMACENES A TRASPASAR LOTES
        public DataSet almacenesDestinoLotes(string basedatos)
        {
            DataSet dataset = new DataSet();
            dataset = lotesDAL.almacenesDestinoLotes(basedatos);
            return dataset;
        }

        //METODO DE GUARDADO
        public int guardarCambioLotes(Lotes lote)
        {
            return lotesDAL.guardarCambioLotes(lote);
        }

        //METODO PARA CREAR UNA SOLICITUD DE TRASLADO
        public string solicitudTrasladoLote(Lotes lote)
        {
            string creado = "1";

            try
            {
                StockTransfer oTransfer = DIAPIDAL.company.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);
                oTransfer.DocObjectCode = BoObjectTypes.oInventoryTransferRequest;

                oTransfer.FromWarehouse = lote.almacen;
                oTransfer.DocDate = DateTime.Now.AddMonths(-2);
                oTransfer.TaxDate = DateTime.Now;

                oTransfer.Lines.ItemCode = lote.articulo;
                oTransfer.Lines.WarehouseCode = lote.almacenDestinoTraslado;
                oTransfer.Lines.Quantity = lote.cantidadTraslado;

                oTransfer.Lines.BatchNumbers.SetCurrentLine(0);
                oTransfer.Lines.BatchNumbers.Quantity = lote.cantidadTraslado;
                oTransfer.Lines.BatchNumbers.BatchNumber = lote.lote;
                oTransfer.Lines.BatchNumbers.Add();

                int err_code;
                string err_msg;

                if (oTransfer.Add() != 0)
                {
                    DIAPIDAL.company.GetLastError(out err_code, out err_msg);
                    creado = err_msg;
                }
                else
                {
                    creado = "1";
                }
            }
            catch (Exception ex)
            {
                creado = ex.Message.ToString();
                Console.WriteLine(ex.ToString());
            }

            return creado;
        }

        //METODO PARA OBTENER TODOS LAS OC YA PROCESADAS POR EL USUARIO Y A ACTUALIZAR A SAP
        public List<Lotes> obtenerLotesActualizar()
        {
            List<Lotes> listaLotes = new List<Lotes>();
            listaLotes = lotesDAL.obtenerLotesActualizar();
            return listaLotes;
        }

        //METODO PARA ACTUALIZAR EL PEDIDO A SAP
        public string updateLote(int numeroDocumento, int estatus)
        {
            string procesado = "";

            CompanyService oCompanyService = DIAPIDAL.company.GetCompanyService();
            BatchNumberDetailsService oBatchNumbersService;
            oBatchNumbersService = oCompanyService.GetBusinessService(ServiceTypes.BatchNumberDetailsService);
            BatchNumberDetailParams oBatchNumberDetailParams;
            oBatchNumberDetailParams = oBatchNumbersService.GetDataInterface(BatchNumberDetailsServiceDataInterfaces.bndsBatchNumberDetailParams);
            try
            {
                int docentry = numeroDocumento;
                oBatchNumberDetailParams.DocEntry = docentry;
                BatchNumberDetail oBatchNumberDetail;
                oBatchNumberDetail = oBatchNumbersService.Get(oBatchNumberDetailParams);

                if (estatus == 2)
                {
                    oBatchNumberDetail.Status = BoDefaultBatchStatus.dbs_Locked;
                }
                else if (estatus == 0)
                {
                    oBatchNumberDetail.Status = BoDefaultBatchStatus.dbs_Released;
                }
                else
                {
                    oBatchNumberDetail.Status = BoDefaultBatchStatus.dbs_NotAccessible;
                }

                oBatchNumbersService.Update(oBatchNumberDetail);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBatchNumberDetail);
                procesado = "1";
            }
            catch (Exception ex)
            {
                procesado = ex.Message.ToString();
                Console.WriteLine(ex);
            }
            return procesado;
        }

        //METODO DE ACTUALIZAR
        public int actualizarSolicitudLote(Lotes lote)
        {
            return lotesDAL.actualizarSolicitudLote(lote);
        }
    }
}

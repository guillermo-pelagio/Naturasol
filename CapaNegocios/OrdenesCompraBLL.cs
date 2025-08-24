using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocios
{
    public class OrdenesCompraBLL
    {
        OrdenesCompraDAL ordenesCompraDAL = new OrdenesCompraDAL();

        //METODO PARA CREAR EL PEDIDO DE COMPRA A SAP
        public string crearOC(OrdenVenta ordenVenta, List<OrdenVentaDetalle> ordenVentaDetalle, string sociedadVenta)
        {
            SAPbobsCOM.Documents oPurchaseOrders;
            oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

            //string[] codigosProveedor = { "2110100503", "2110100501", "2110100502", "2110100704" };

            //ENCABEZADO
            oPurchaseOrders.CardCode = ordenVenta.CardCodeEspejo;
            oPurchaseOrders.DocDate = DateTime.Now;
            oPurchaseOrders.DocDueDate = DateTime.Now.AddDays(30);
            oPurchaseOrders.DocCurrency = ordenVenta.DocCur;
            oPurchaseOrders.UserFields.Fields.Item("U_NoCaja").Value = ordenVenta.DocNum;
            oPurchaseOrders.NumAtCard = (ordenVenta.Sociedad.Contains("MIELMEX") ? "MM" : ordenVenta.Sociedad.Contains("NATURASOL") ? "NA" : ordenVenta.Sociedad.Contains("NOVAL") ? "NO" : "EV") + ordenVenta.DocNum;
            oPurchaseOrders.Comments = "Pedido Intercompania basado en OC: " + ordenVenta.DocNum + " de: " + ordenVenta.Sociedad;
            oPurchaseOrders.DocType = ordenVenta.DocType == "I" ? SAPbobsCOM.BoDocumentTypes.dDocument_Items : SAPbobsCOM.BoDocumentTypes.dDocument_Service;

            for (int i = 0; i < ordenVentaDetalle.Count; i++)
            {
                //DETALLE
                oPurchaseOrders.Lines.ItemCode = ordenVentaDetalle[i].ItemCode;
                oPurchaseOrders.Lines.Quantity = Convert.ToDouble(ordenVentaDetalle[i].Quantity);
                oPurchaseOrders.Lines.TaxCode = ordenVentaDetalle[i].TaxCode.Replace('T', 'A');
                oPurchaseOrders.Lines.Price = Convert.ToDouble(ordenVentaDetalle[i].Price);
                oPurchaseOrders.Lines.Currency = ordenVentaDetalle[i].Currency;
                oPurchaseOrders.Lines.CostingCode3 = "COMPRA";
                oPurchaseOrders.Lines.CostingCode = "GRU NAT";
                oPurchaseOrders.Lines.UoMEntry = Convert.ToInt32(ordenesCompraDAL.obtenerUM(ordenVentaDetalle[i].UM, sociedadVenta));
                oPurchaseOrders.Lines.Add();
            }

            int respuesta = oPurchaseOrders.Add();
            string mensaje;

            if (respuesta == 0)
            {
                mensaje = "OK";
                Console.WriteLine("Orden de venta creada correctamente");
            }
            else
            {
                mensaje = DIAPIDAL.company.GetLastErrorDescription();
                Console.WriteLine("Error al crear la Orden de Venta " + DIAPIDAL.company.GetLastErrorDescription());
            }

            return mensaje;
        }        

        /*
        //METODO PARA OBTENER LOS PEDIDOS DE COMPRA INTERCOMPANIA QUE SE CREARON
        public List<OrdenVenta> obtenerOCIntercompania(OrdenVenta ordenVenta, string sociedad)
        {
            List<OrdenVenta> listaBorradorOrdenVenta = new List<OrdenVenta>();
            listaBorradorOrdenVenta = ordenesCompraDAL.obtenerOCIntercompania(ordenVenta, sociedad);
            return listaBorradorOrdenVenta;
        }
        */

        //REVISADO - 
        public List<OrdenCompra> obtenerEntradasSinPE()
        {
            List<OrdenCompra> listaEntradas = new List<OrdenCompra>();
            listaEntradas = ordenesCompraDAL.obtenerEntradasSinPE();
            return listaEntradas;
        }

        //METODO PARA ACTUALIZAR EL PEDIDO DE COMPRA A SAP
        public int updatePedidoCompra(int numeroDocumento, string estatus, string motivoRechazo)
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
                    oApprovalRequestDecision.Status = SAPbobsCOM.BoApprovalRequestDecisionEnum.ardNotApproved;
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


        public List<Intercompania> obtenerSaldosIntercompanias(string sociedad)
        {
            List<Intercompania> saldosIntercompania = new List<Intercompania>();
            saldosIntercompania = ordenesCompraDAL.obtenerSaldosIntercompanias(sociedad);
            return saldosIntercompania;
        }

        //S12-REQUERIDO
        public List<OrdenCompra> obtenerCompradorOCAbiertas(string sociedad)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaCompradorOCAbiertas(sociedad);
            return listaCompradorOCAbiertas;
        }

        public List<OrdenCompra> listaBorradoresLiberacion(string sociedad)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaBorradoresLiberacion(sociedad);
            return listaCompradorOCAbiertas;
        }

        public List<OrdenCompra> listaBorradoresLiberacionDetalle(string sociedad, string docentry)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaBorradoresLiberacionDetalle(sociedad, docentry);
            return listaCompradorOCAbiertas;
        }

        public List<OrdenCompra> listaBorradoresCalculo(string sociedad, string itemcode, string fecha, string docentry)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaBorradoresCalculo(sociedad, itemcode, fecha, docentry);
            return listaCompradorOCAbiertas;
        }

        public List<OrdenCompra> listaBorradoresCalculoOC(string sociedad, string itemcode, string fecha)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaBorradoresCalculoOC(sociedad, itemcode, fecha);
            return listaCompradorOCAbiertas;
        }

        public List<OrdenCompra> semanasExistenciaLiberacionOC(string sociedad, string docEntry)
        {
            List<OrdenCompra> listaOCDetalle = new List<OrdenCompra>();
            listaOCDetalle = ordenesCompraDAL.semanasExistenciaLiberacionOC(sociedad, docEntry);
            return listaOCDetalle;
        }

        public List<OrdenCompra> listaBorradoresLiberacionDetalleAnexos(string sociedad, string docentry)
        {
            List<OrdenCompra> listaCompradorOCAbiertas = new List<OrdenCompra>();
            listaCompradorOCAbiertas = ordenesCompraDAL.listaBorradoresLiberacionDetalleAnexos(sociedad, docentry);
            return listaCompradorOCAbiertas;
        }

        //METODO PARA ACTUALIZAR LA OC DESPUES DE CREAR LA ORDEN DE VENTA
        public void actualizarOC(string DocEntry, string DocNum, string sociedadVenta, string mensaje)
        {
            SAPbobsCOM.Documents oPurchaseOrders;
            oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            oPurchaseOrders.GetByKey(Convert.ToInt32(DocEntry));

            if (mensaje.Equals(""))
            {
                oPurchaseOrders.UserFields.Fields.Item("U_INT_Generar").Value = "N";
                oPurchaseOrders.UserFields.Fields.Item("U_INT_COMMENTS").Value = "Orden de venta creada en " + sociedadVenta + ", folio: " + DocNum;
            }
            else
            {
                oPurchaseOrders.UserFields.Fields.Item("U_INT_Generar").Value = "E";
                oPurchaseOrders.UserFields.Fields.Item("U_INT_COMMENTS").Value = mensaje;
            }

            int respuesta = oPurchaseOrders.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OC actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OC " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        //METODO PARA CREAR EL PEDIDO DE COMPRA A SAP
        public string crearPedidoCompra()
        {
            SAPbobsCOM.Documents oPurchaseOrders;
            oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

            //ENCABEZADO
            oPurchaseOrders.CardCode = "2110100501";
            oPurchaseOrders.DocDate = DateTime.Now;
            oPurchaseOrders.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

            //DETALLE
            oPurchaseOrders.Lines.ItemCode = "1501-100-093";
            oPurchaseOrders.Lines.Quantity = 100;
            oPurchaseOrders.Lines.TaxCode = "IA0";
            oPurchaseOrders.Lines.Price = 30.0;
            oPurchaseOrders.Lines.Currency = "MXP";

            oPurchaseOrders.Add();

            int respuesta = oPurchaseOrders.Add();
            string mensaje;

            if (respuesta == 0)
            {
                mensaje = "OK";
                Console.WriteLine("OC creada correctamente");
            }
            else
            {
                mensaje = DIAPIDAL.company.GetLastErrorDescription();
                Console.WriteLine("Error al crear la oc " + DIAPIDAL.company.GetLastErrorDescription());
            }

            return mensaje;
        }

        //METODO DE BUSQUEDA DE PEDIDOS INTERCOMPANIA
        public List<OrdenCompra> obtenerOrdenesCompraIntercompania(string sociedad)
        {
            List<OrdenCompra> listaOrdenesCompra = new List<OrdenCompra>();
            listaOrdenesCompra = ordenesCompraDAL.obtenerOrdenesCompraIntercompania(sociedad);
            return listaOrdenesCompra;
        }

        //S12-REQUERIDO
        public List<OrdenCompra> obtenerOCAbiertas(string sociedadVenta, string slpname)
        {
            List<OrdenCompra> listaOCAbiertas = new List<OrdenCompra>();
            listaOCAbiertas = ordenesCompraDAL.obtenerOCAbiertas(sociedadVenta, slpname);
            return listaOCAbiertas;
        }

        
        //S11-REQUERIDO
        public List<OrdenCompra> obtenerBorradoresAbiertas(string sociedadVenta)
        {
            List<OrdenCompra> listaBorradoresAbiertas = new List<OrdenCompra>();
            listaBorradoresAbiertas = ordenesCompraDAL.obtenerBorradoresAbiertas(sociedadVenta);
            return listaBorradoresAbiertas;
        }        

        //METODO PARA OBTENER LOS PEDIDOS DE VENTA ABIERTOS
        public List<OrdenCompra> obtenerOCLiberar(string sociedadVenta, string slpname)
        {
            List<OrdenCompra> listaOCAbiertas = new List<OrdenCompra>();
            listaOCAbiertas = ordenesCompraDAL.obtenerOCLiberar(sociedadVenta, slpname);
            return listaOCAbiertas;
        }

        //METODO PARA CERRAR LOS PEDIDOS DE VENTA ABIERTOS
        public void cerrarOCAbiertas(string DocEntry)
        {
            try
            {
                SAPbobsCOM.Documents oPurchaseOrders;
                oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                oPurchaseOrders.GetByKey(Convert.ToInt32(DocEntry));
                oPurchaseOrders.Close();

                int respuesta = oPurchaseOrders.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("OC actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la OC " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la OC " + ex);
            }
        }

        //S11-ACTUALIZACION
        public void cerrarBorradorAbiertas(string DocEntry)
        {
            try
            {
                SAPbobsCOM.Documents oPurchaseOrders;
                oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                oPurchaseOrders.GetByKey(Convert.ToInt32(DocEntry));
                oPurchaseOrders.Close();

                int respuesta = oPurchaseOrders.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("OC actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la OC " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la OC " + ex);
            }
        }        

        //METODO PARA BUSCAR EL DETALLE DE UNA OC INTERCOMPANIA
        public List<OrdenCompraDetalle> obtenerOrdenesCompraDetalleIntercompania(string sociedad, OrdenCompra orden)
        {
            List<OrdenCompraDetalle> listaOrdenesCompraDetalle = new List<OrdenCompraDetalle>();
            listaOrdenesCompraDetalle = ordenesCompraDAL.obtenerOrdenesCompraDetalleIntercompania(sociedad, orden);
            return listaOrdenesCompraDetalle;
        }

        //OBTENER LA OC DE UNA ENTREGA POR INTERCOMPANIA
        public List<OrdenCompraDetalle> obtenerOrdenesCompraEntregaDetalleIntercompania(string sociedad, string DocEntry)
        {
            List<OrdenCompraDetalle> listaOrdenesCompraDetalle = new List<OrdenCompraDetalle>();
            listaOrdenesCompraDetalle = ordenesCompraDAL.obtenerOrdenesCompraEntregaDetalleIntercompania(sociedad, DocEntry);
            return listaOrdenesCompraDetalle;
        }

        //METODO PARA OBTENER TODOS LAS OC YA PROCESADAS POR EL USUARIO Y A ACTUALIZAR A SAP
        public List<BorradorDocumento> obtenerOrdenesCompraActualizar()
        {
            List<BorradorDocumento> listaBorradorOrdenCompra = new List<BorradorDocumento>();
            listaBorradorOrdenCompra = ordenesCompraDAL.obtenerDocumentos();
            return listaBorradorOrdenCompra;
        }

        //METODO PARA OBTENER TODOS LAS OC SEGUN UN FOLIO A AUTORIZAR DESDE OUTLOOK
        public DataSet obtenerOrdenesCompra(string folio)
        {
            DataSet finalDataset = new DataSet();
            List<BorradorDocumento> listaDocumentos = new List<BorradorDocumento>();
            listaDocumentos = ordenesCompraDAL.obtenerDocumentos();
            foreach (var documento in listaDocumentos)
            {
                DataSet dataset = new DataSet();
                dataset = ordenesCompraDAL.obtenerOrdenesCompra(documento.sociedad, documento.wddCode);
                finalDataset.Merge(dataset);
            }
            return finalDataset;
        }

        //METODO PARA OBTENER TODOS LAS OC A AUTORIZAR DESDE OUTLOOK
        public DataSet obtenerDetalleOrdenesCompra(string sociedad, string docentry)
        {
            DataSet dataset = new DataSet();
            dataset = ordenesCompraDAL.obtenerDetalleOrdenesCompra(sociedad, docentry);
            return dataset;
        }

        //METODO PARA OBTENER TODOS LAS OC
        public DataSet obtenerOrdenesCompra(string sociedad, string comprador)
        {
            return ordenesCompraDAL.obtenerOrdenesCompra(sociedad, comprador);
        }

        //CONSULTA QUE OBTIENE LOS USUARIOS DE COMPRAS
        public DataSet obtenerUsuariosCompras(string sociedad)
        {
            return ordenesCompraDAL.obtenerUsuariosCompras(sociedad);
        }

        //METODO PARA ACTUALIZAR LA OC EN DESARROLLO
        public int actualizarFolioAutorizacionOC()
        {
            return ordenesCompraDAL.actualizarFolioAutorizacionOC();
        }

        //METODO DE OBTENER EL ULTOMO FOLIO DE UN MOVIMIENTO
        public int obtenerFolioAutorizacionOC(int tipoMovimiento)
        {
            return ordenesCompraDAL.obtenerFolioAutorizacionOC(tipoMovimiento);
        }

        //METODO DE GUARDADO EN DESARROLLO
        public int guardarSolicitudAutorizacionOC(OrdenCompra borradorOrdenCompra)
        {
            return ordenesCompraDAL.guardarSolicitudAutorizacionOC(borradorOrdenCompra);
        }

        //METODO DE ACTUALIZAR EN DESARROLLO
        public int actualizarSolicitudAutorizacionOC(string wddcode, string estatus, string estatusArea)
        {
            return ordenesCompraDAL.actualizarSolicitudAutorizacionOC(wddcode, estatus, estatusArea);
        }

        //METODO DE ACTUALIZAR EN DESARROLLO
        public List<OrdenCompra> obtenerDocumentosAutorizarPendiente(string sociedad)
        {
            return ordenesCompraDAL.obtenerDocumentosAutorizarPendiente(sociedad);
        }

        //METODO PARA ACTUALIZAR LA OC DESPUES DE CREAR LA ORDEN DE VENTA
        public void actualizarOCPendientes(string DocEntry)
        {
            SAPbobsCOM.Documents oPurchaseOrders;
            oPurchaseOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            oPurchaseOrders.GetByKey(Convert.ToInt32(DocEntry));
            //oPurchaseOrders.Close();
            oPurchaseOrders.Confirmed = SAPbobsCOM.BoYesNoEnum.tYES;

            int respuesta = oPurchaseOrders.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OC actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OC " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        public int actualizarOCAutorizada(string sociedad, string wddCode)
        {
            return ordenesCompraDAL.actualizarOCAutorizada(sociedad, wddCode);
        }

        public List<OrdenCompra> obtenerOCIntercompania(OrdenVenta ordenVenta, string sociedadVenta)
        {
            return ordenesCompraDAL.obtenerOCIntercompania(ordenVenta, sociedadVenta);
        }
    }
}
using CapaDatos;
using CapaEnidades;
using CapaEntidades;
using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class EntradasBLL
    {
        //METODO PARA CREAR UNA ENTRADA A SAP
        public List<PreciosEntrega> obtenerDataPE(string docnum)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<PreciosEntrega> listaPE = new List<PreciosEntrega>();
            listaPE = entradasMaterialDAL.obtenerDataPE(docnum);
            return listaPE;
        }

        public List<OrdenCompra> obtenerDataPEDetalleOC(string entrada)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<OrdenCompra> listaPEOC = new List<OrdenCompra>();
            listaPEOC = entradasMaterialDAL.obtenerDataPEDetalleOC(entrada);
            return listaPEOC;
        }

        public List<PreciosEntrega> obtenerDataPEDetalle(string docEntry, string proveedor)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<PreciosEntrega> listaPE = new List<PreciosEntrega>();
            listaPE = entradasMaterialDAL.obtenerDataPEDetalle(docEntry, proveedor);
            return listaPE;
        }

        //METODO PARA CREAR UNA ENTRADA A SAP
        public void crearPrecioEntrega(string docnum)
        {
            EntregasMercanciaBLL entregasMercanciaBLL = new EntregasMercanciaBLL();
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();

            List<PreciosEntrega> listaPreciosEntrega = new List<PreciosEntrega>();
            listaPreciosEntrega = obtenerDataPE(docnum);

            for (int i = 0; i < listaPreciosEntrega.Count; i++)
            {
                if (listaPreciosEntrega[i].proveedor != "")
                {
                    ////////////////////////INICIALIZACION
                    SAPbobsCOM.LandedCostsService oLandedCostService;
                    oLandedCostService = DIAPIDAL.company.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.LandedCostsService);
                    SAPbobsCOM.LandedCost oLandedCost;
                    oLandedCost = oLandedCostService.GetDataInterface(SAPbobsCOM.LandedCostsServiceDataInterfaces.lcsLandedCost);
                    SAPbobsCOM.LandedCost_ItemLine oLandedItemLine;

                    ///////////////////////
                    List<OrdenCompra> listaPreciosEntregaDetalleOC = new List<OrdenCompra>();
                    listaPreciosEntregaDetalleOC = obtenerDataPEDetalleOC(docnum);

                    List<PreciosEntrega> listaPreciosEntregaDetalle = new List<PreciosEntrega>();
                    listaPreciosEntregaDetalle = obtenerDataPEDetalle(listaPreciosEntrega[i].docEntry, listaPreciosEntrega[i].proveedor);

                    //////////////////ENCABEZADO DEL PE
                    oLandedCost.DocumentCurrency = listaPreciosEntrega[i].moneda;
                    oLandedCost.PostingDate = DateTime.Today;
                    oLandedCost.Broker = listaPreciosEntrega[i].proveedor;

                    oLandedItemLine = oLandedCost.LandedCost_ItemLines.Add();
                    oLandedItemLine.BaseDocumentType = SAPbobsCOM.LandedCostBaseDocumentTypeEnum.asGoodsReceiptPO;
                    /////////////////LINEAS DEL PE
                    oLandedItemLine.BaseEntry = Convert.ToInt32(listaPreciosEntregaDetalleOC[0].DocEntry);
                    oLandedItemLine.BaseLine = Convert.ToInt32(listaPreciosEntregaDetalleOC[0].LineNum);
                    oLandedItemLine.Quantity = Convert.ToDouble(listaPreciosEntregaDetalleOC[0].Quantity);

                    oLandedItemLine.Warehouse = listaPreciosEntregaDetalleOC[0].WhsCode;

                    for (int k = 0; k < listaPreciosEntregaDetalle.Count; k++)
                    {
                        oLandedItemLine.Currency = listaPreciosEntregaDetalle[k].moneda;
                        ////////////////////////////////////////////////////////////////////
                        SAPbobsCOM.LandedCost_CostLine oLandedCostLine;
                        oLandedCostLine = oLandedCost.LandedCost_CostLines.Add();

                        oLandedCostLine.LandedCostCode = listaPreciosEntregaDetalle[k].concepto;
                        if (listaPreciosEntregaDetalle[k].moneda == "USD")
                        {
                            oLandedCostLine.AmountFC = Convert.ToDouble(listaPreciosEntregaDetalle[k].monto);
                            oLandedCostLine.amount = Convert.ToDouble(listaPreciosEntregaDetalle[k].monto) * Convert.ToDouble(listaPreciosEntrega[i].TC);
                        }
                        else
                        {
                            oLandedCostLine.amount = Convert.ToDouble(listaPreciosEntregaDetalle[k].monto);
                            oLandedCostLine.AmountFC = Convert.ToDouble(listaPreciosEntregaDetalle[k].monto) / (Convert.ToDouble(listaPreciosEntrega[i].TC));
                        }

                        oLandedCostLine.Broker = listaPreciosEntregaDetalle[k].proveedor;
                    }

                    SAPbobsCOM.LandedCostParams oLandedCostParams;
                    oLandedCostParams = oLandedCostService.GetDataInterface(SAPbobsCOM.LandedCostsServiceDataInterfaces.lcsLandedCostParams);
                    oLandedCostParams = oLandedCostService.AddLandedCost(oLandedCost);

                    int respuesta = oLandedCostParams.LandedCostNumber;


                    /////////////////////////////////////////////ACTUALIZA LA ENTRADA
                    List<OrdenCompra> listaDetalleEntrada = new List<OrdenCompra>();
                    listaDetalleEntrada = obtenerDataPEDetalleOC(docnum);
                    SAPbobsCOM.Documents oPurchaseDeliveryNotes;
                    oPurchaseDeliveryNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                    oPurchaseDeliveryNotes.GetByKey(Convert.ToInt32(listaDetalleEntrada[0].DocEntry));

                    oPurchaseDeliveryNotes.OpenForLandedCosts = BoYesNoEnum.tYES;

                    int respuestaEntrada = oPurchaseDeliveryNotes.Update();

                    if (respuestaEntrada == 0)
                    {
                        Console.WriteLine("Entrada actualizada");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar la entrada " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
            }
        }

        public int actualizarEstatus(EntradaMaterial entradaMaterial)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            int resultado;
            resultado = entradasMaterialDAL.actualizarEstatus(entradaMaterial);
            return resultado;
        }

        public int actualizar_lote(EntradaMaterial entradaMaterial)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            int resultado;
            resultado = entradasMaterialDAL.actualizar_lote(entradaMaterial);
            return resultado;
        }

        private int crearPE(LandedCostsService oLandedCostService, LandedCost oLandedCost)
        {
            SAPbobsCOM.LandedCostParams oLandedCostParams;
            oLandedCostParams = oLandedCostService.GetDataInterface(SAPbobsCOM.LandedCostsServiceDataInterfaces.lcsLandedCostParams);
            oLandedCostParams = oLandedCostService.AddLandedCost(oLandedCost);

            int respuesta = oLandedCostParams.LandedCostNumber;

            return respuesta;
        }

        //METODO PARA CREAR UNA ENTRADA A SAP
        public string crearEntrada(EntregaMercancia entregaMercancia, List<EntregaMercanciaDetalle> entregaMercanciaDetalles, string sociedadDestino)
        {
            EntregasMercanciaBLL entregasMercanciaBLL = new EntregasMercanciaBLL();
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            SAPbobsCOM.Documents oPurchaseDeliveryNotes;
            oPurchaseDeliveryNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

            //SE OBTIENE LA OC
            string numeroOV = entregaMercancia.U_NoCaja;

            //OBTENER EL DETALLE DE LA OC
            List<OrdenCompraDetalle> ordenCompraDetalle = new List<OrdenCompraDetalle>();
            ordenCompraDetalle = ordenesCompraBLL.obtenerOrdenesCompraEntregaDetalleIntercompania(sociedadDestino, numeroOV);

            //ENCABEZADO
            oPurchaseDeliveryNotes.CardCode = ordenCompraDetalle[0].CardCode;
            oPurchaseDeliveryNotes.DocDate = DateTime.Now;
            oPurchaseDeliveryNotes.DocDueDate = DateTime.Now;
            oPurchaseDeliveryNotes.DocCurrency = entregaMercancia.DocCur;
            oPurchaseDeliveryNotes.UserFields.Fields.Item("U_NoCaja").Value = entregaMercancia.DocNum;
            oPurchaseDeliveryNotes.NumAtCard = (entregaMercancia.Sociedad.Contains("MIELMEX") ? "MIE-" : entregaMercancia.Sociedad.Contains("NATURASOL") ? "NAT-" : entregaMercancia.Sociedad.Contains("NOVAL") ? "NOV-" : "EVI-") + entregaMercancia.DocNum;
            oPurchaseDeliveryNotes.Comments = "Pedido Intercompania basado en OC: " + entregaMercancia.DocNum + " de: " + entregaMercancia.Sociedad;
            oPurchaseDeliveryNotes.DocType = entregaMercancia.DocType == "I" ? SAPbobsCOM.BoDocumentTypes.dDocument_Items : SAPbobsCOM.BoDocumentTypes.dDocument_Service;
            int aplicarInter = 0;

            for (int i = 0; i < ordenCompraDetalle.Count; i++)
            {
                for (int j = 0; j < entregaMercanciaDetalles.Count; j++)
                {
                    if ((ordenCompraDetalle[i].ItemCode == entregaMercanciaDetalles[j].ItemCode) && (Convert.ToDouble(entregaMercanciaDetalles[j].Quantity) > 0))
                    {
                        if (ordenCompraDetalle[i].LineNum == entregaMercanciaDetalles[j].LineNum)
                        {
                            //DETALLE
                            oPurchaseDeliveryNotes.Lines.BaseType = 22;
                            oPurchaseDeliveryNotes.Lines.BaseEntry = Convert.ToInt32(ordenCompraDetalle[i].DocEntry);
                            oPurchaseDeliveryNotes.Lines.BaseLine = Convert.ToInt32(ordenCompraDetalle[i].LineNum);

                            if (entregaMercancia.U_REPLICAR != null)
                            {
                                if (entregaMercancia.U_REPLICAR.Equals("0") || entregaMercancia.U_REPLICAR.Equals("2") || entregaMercancia.U_REPLICAR.Equals("3"))
                                {
                                    aplicarInter = 1;
                                    oPurchaseDeliveryNotes.Lines.WarehouseCode = entregaMercancia.U_REPLICAR.Equals("0") ? "1202" : entregaMercancia.U_REPLICAR.Equals("1") ? "1302" : entregaMercancia.U_REPLICAR.Equals("2") ? "1502" : entregaMercancia.U_REPLICAR.Equals("3") ? "1702" : entregaMercancia.U_REPLICAR.Equals("4") ? "1323" : sociedadDestino == "TSSL_NOVAL" ? "2312" : "2302";


                                    oPurchaseDeliveryNotes.Lines.Quantity = Convert.ToDouble(entregaMercanciaDetalles[j].Quantity);
                                    oPurchaseDeliveryNotes.Lines.TaxCode = ordenCompraDetalle[i].TaxCode;

                                    /*
                                      oPurchaseOrders.Lines.ItemCode = ordenVen000taDetalle[i].ItemCode;
                                        oPurchaseOrders.Lines.Quantity = Convert.ToDouble(ordenVentaDetalle[i].Quantity);
                                        oPurchaseOrders.Lines.TaxCode = ordenVentaDetalle[i].TaxCode.Replace('T', 'A');
                                        oPurchaseOrders.Lines.Price = Convert.ToDouble(ordenVentaDetalle[i].Price);
                                        oPurchaseOrders.Lines.Currency = "MXP";
                                        oPurchaseOrders.Lines.CostingCode3 = "COMPRA";
                                        oPurchaseOrders.Lines.CostingCode = "GRU NAT";
                                        oPurchaseOrders.Lines.UoMEntry = Convert.ToInt32(ordenesCompraDAL.obtenerUM(ordenVentaDetalle[i].UM, sociedadVenta));
                                        oPurchaseOrders.Lines.Add();
                                                */
                                    //LOTES DE LA ENTREGA PLASMADOS EN LA ENTRADA

                                    List<LoteEntregaMercancia> listaLotes = new List<LoteEntregaMercancia>();
                                    listaLotes = entregasMercanciaBLL.obtenerLotesEntregaDetalleIntercompania(entregaMercancia.Sociedad, entregaMercanciaDetalles[j]);
                                    double sumaCantidadLotes = 0;
                                    for (int k = 0; k < listaLotes.Count; k++)
                                    {
                                        sumaCantidadLotes = sumaCantidadLotes + Convert.ToDouble(listaLotes[k].Quantity);
                                        if (k > 0)
                                        {
                                            double resta = 0;// Convert.ToDouble(entregaMercanciaDetalles[j].Quantity) - Convert.ToDouble(sumaCantidadLotes);
                                            if (resta >= -0.1)
                                            {
                                                oPurchaseDeliveryNotes.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaLotes[k].Quantity);
                                                oPurchaseDeliveryNotes.Lines.BatchNumbers.BatchNumber = listaLotes[k].BatchNum;
                                                oPurchaseDeliveryNotes.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(listaLotes[k].ExpDate);
                                                oPurchaseDeliveryNotes.Lines.BatchNumbers.Add();

                                                OrdenFabricacionDAL ordenFabricacionDAL = new OrdenFabricacionDAL();

                                                oPurchaseDeliveryNotes.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = k;
                                                oPurchaseDeliveryNotes.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionDestino(sociedadDestino, entregaMercancia.U_REPLICAR.Equals("0") ? "1202" : entregaMercancia.U_REPLICAR.Equals("1") ? "1302" : entregaMercancia.U_REPLICAR.Equals("2") ? "1502" : entregaMercancia.U_REPLICAR.Equals("3") ? "1702" : entregaMercancia.U_REPLICAR.Equals("4") ? "1323" : sociedadDestino == "TSSL_NOVAL" ? "2312" : "2302")[0].BinCode);
                                                oPurchaseDeliveryNotes.Lines.BinAllocations.Quantity = Convert.ToDouble(listaLotes[k].Quantity);
                                                oPurchaseDeliveryNotes.Lines.BinAllocations.Add();
                                            }
                                        }
                                        else
                                        {
                                            oPurchaseDeliveryNotes.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaLotes[k].Quantity);
                                            oPurchaseDeliveryNotes.Lines.BatchNumbers.BatchNumber = listaLotes[k].BatchNum;
                                            oPurchaseDeliveryNotes.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(listaLotes[k].ExpDate);
                                            oPurchaseDeliveryNotes.Lines.BatchNumbers.Add();

                                            OrdenFabricacionDAL ordenFabricacionDAL = new OrdenFabricacionDAL();

                                            oPurchaseDeliveryNotes.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = k;
                                            oPurchaseDeliveryNotes.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionDestino(sociedadDestino, entregaMercancia.U_REPLICAR.Equals("0") ? "1202" : entregaMercancia.U_REPLICAR.Equals("1") ? "1302" : entregaMercancia.U_REPLICAR.Equals("2") ? "1502" : entregaMercancia.U_REPLICAR.Equals("3") ? "1702" : entregaMercancia.U_REPLICAR.Equals("4") ? "1323" : sociedadDestino == "TSSL_NOVAL" ? "2312" : "2302")[0].BinCode);
                                            oPurchaseDeliveryNotes.Lines.BinAllocations.Quantity = Convert.ToDouble(listaLotes[k].Quantity);
                                            oPurchaseDeliveryNotes.Lines.BinAllocations.Add();
                                        }
                                    }

                                    oPurchaseDeliveryNotes.Lines.Add();
                                    oPurchaseDeliveryNotes.Lines.BatchNumbers.Add();

                                }
                                else
                                {
                                    aplicarInter = 0;
                                }
                            }
                        }
                    }
                }
            }

            string mensaje;
            if (aplicarInter == 1)
            {
                int respuesta = oPurchaseDeliveryNotes.Add();

                if (respuesta == 0)
                {
                    mensaje = "OK";
                }
                else
                {
                    mensaje = DIAPIDAL.company.GetLastErrorDescription();
                    Console.WriteLine("Error al crear la Orden de Venta " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            else
            {
                mensaje = "";
            }

            return mensaje;
        }

        //OBTENER DETALLE DE LA ENTRADA REALIZADA POR EL SISTEMA
        public List<EntradaMaterial> obtenerEntradaIntercompania(EntregaMercancia entregaMercancia, string sociedadDestino)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterial> listaBorradorEntrada = new List<EntradaMaterial>();
            listaBorradorEntrada = entradasMaterialDAL.obtenerEntradasIntercompania(entregaMercancia, sociedadDestino);
            return listaBorradorEntrada;
        }

        //S21-REQUERIDO
        public List<EntradaMaterial> obtenerEntradaInternacional()
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterial> listaBorradorEntrada = new List<EntradaMaterial>();
            listaBorradorEntrada = entradasMaterialDAL.obtenerEntradaInternacional();
            return listaBorradorEntrada;
        }

        //OBTENER DETALLE DE LA ENTRADA INTERNACIONAL PE
        public List<EntradaMaterial> obtenerEntradaInternacionalPE()
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterial> listaEntradaPE = new List<EntradaMaterial>();
            listaEntradaPE = entradasMaterialDAL.obtenerEntradaInternacionalPE();
            return listaEntradaPE;
        }

        //OBTENER LA ENTRADA DE UNA FACTURA PROVEEDOR POR INTERCOMPANIA
        public List<EntradaMaterialDetalle> obtenerEntrada(string sociedad, string DocNum)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterialDetalle> listaEntradaMaterialDetalle = new List<EntradaMaterialDetalle>();
            listaEntradaMaterialDetalle = entradasMaterialDAL.obtenerEntrada(sociedad, DocNum);
            return listaEntradaMaterialDetalle;
        }

        //VALIDAR OC Y HORARIO DE ARRIBO DE MATERIAL
        public SesionEntradaMaterial validarOCArriboProveedor(SesionEntradaMaterial entradas)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            SesionEntradaMaterial listaEntradaMaterialDetalle = new SesionEntradaMaterial();
            listaEntradaMaterialDetalle = entradasMaterialDAL.validarOCArriboProveedor(entradas);
            return listaEntradaMaterialDetalle;
        }

        //S21-ACTUALIZACION-ACTUALIZAR LA ENTREGA EN SAP
        public void actualizarEntrada(string docEntry, string proceso)
        {
            DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
            SAPbobsCOM.Documents oPurchaseDeliveryNotes;
            oPurchaseDeliveryNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
            oPurchaseDeliveryNotes.GetByKey(Convert.ToInt32(docEntry));

            if (proceso == "1")
            {
                oPurchaseDeliveryNotes.UserFields.Fields.Item("U_FechaCobro").Value = DateTime.Now.ToShortDateString();
            }
            else
            {
                oPurchaseDeliveryNotes.UserFields.Fields.Item("U_TS_DetalleErrorAdd").Value = DateTime.Now.ToShortDateString();
            }

            int respuesta = oPurchaseDeliveryNotes.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Entrada actualizada");
            }
            else
            {
                Console.WriteLine("Error al actualizar la entrada " + DIAPIDAL.company.GetLastErrorDescription());
            }

            DIAPIBLL.desconectarDIAPI();
        }

        //VALIDAR OC Y HORARIO DE ARRIBO DE MATERIAL        
        public int confirmarOCArriboProveedor(SesionEntradaMaterial sesion)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            int resultado = 0;
            resultado = entradasMaterialDAL.confirmarOCArriboProveedor(sesion);
            return resultado;
        }

        public List<EntradaMaterial> obtenerLotesDetalle(string articulo, string descripcion, string almacen, string lote)
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterial> listaEntradaMaterialDetalle = new List<EntradaMaterial>();
            listaEntradaMaterialDetalle = entradasMaterialDAL.obtenerLotesDetalle(articulo, descripcion, almacen, lote);
            return listaEntradaMaterialDetalle;
        }

        //OBTENER LA ENTRADA DE UNA FACTURA PROVEEDOR POR INTERCOMPANIA
        public List<EntradaMaterial> obtenerEntradasLote()
        {
            EntradasMaterialDAL entradasMaterialDAL = new EntradasMaterialDAL();
            List<EntradaMaterial> listaEntradaMaterialDetalle = new List<EntradaMaterial>();
            listaEntradaMaterialDetalle = entradasMaterialDAL.obtenerEntradasLote();
            return listaEntradaMaterialDetalle;
        }
    }
}

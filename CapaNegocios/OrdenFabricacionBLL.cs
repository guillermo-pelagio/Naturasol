using CapaDatos;
using CapaEntidades;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaNegocios
{
    public class OrdenFabricacionBLL
    {
        OrdenFabricacionDAL ordenFabricacionDAL = new OrdenFabricacionDAL();

        public List<OrdenFabricacion> buscarMaterialMayoreo(string sociedad)
        {
            return ordenFabricacionDAL.buscarMaterialMayoreo(sociedad);
        }

        public void completarConsumoOF()
        {
            /*
            try
            {
                //string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> materialesTS = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    Console.WriteLine("OF ACTUALIZADA");
                    List<OrdenFabricacion> materialesExistencia = new List<OrdenFabricacion>();
                    materialesExistencia = ordenFabricacionDAL.buscarInventarioProduccion(basesDatos[i]);

                    double contadorOriginal = 0;

                    contadorOriginal = Convert.ToDouble(materialesTS[j].PlannedQty);

                    if (materialesExistencia.Count > 0)
                    {
                        int lineaContador = 0;
                        for (int x = 0; x < materialesExistencia.Count; x++)
                        {
                            if (contadorOriginal > 0)
                            {
                                if (Convert.ToDouble(materialesExistencia[x].ReleasedQty) - contadorOriginal >= 0)
                                {
                                    DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                                    Documents oInventoryGenExit;
                                    oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                                    oInventoryGenExit.Comments = "CONSUMO BASADO EN LA TS " + materialesTS[j].DocNum;
                                    oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);

                                    oInventoryGenExit.Lines.BaseType = 202;
                                    oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(materialesTS[j].DocEntry);
                                    oInventoryGenExit.Lines.BaseLine = Convert.ToInt32(materialesTS[j].Linea);
                                    oInventoryGenExit.Lines.WarehouseCode = materialesTS[j].WhsCode;
                                    oInventoryGenExit.Lines.Quantity = contadorOriginal;

                                    oInventoryGenExit.Lines.BatchNumbers.BatchNumber = materialesExistencia[x].DistNumber;
                                    oInventoryGenExit.Lines.BatchNumbers.Quantity = contadorOriginal;
                                    //oInventoryGenExit.Lines.BatchNumbers.Location = materialesExistencia[x].BinCode;
                                    oInventoryGenExit.Lines.BatchNumbers.Add();

                                   
                                    oInventoryGenExit.Lines.Add();

                                    int respuestaC = oInventoryGenExit.Add();

                                    if (respuestaC == 0)
                                    {
                                        Console.WriteLine(DateTime.Now + " CONSUMO CREADO EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);

                                        actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                        contadorOriginal = 0;
                                        lineaContador = lineaContador + 1;
                                    }
                                    else
                                    {
                                        Console.WriteLine(DateTime.Now + " Error al crear el CONSUMO " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);
                                    }
                                    DIAPIBLL.desconectarDIAPI();

                                }
                                else
                                {


                                    DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                                    Documents oInventoryGenExit;
                                    oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                                    oInventoryGenExit.Comments = "CONSUMO BASADO EN LA TS " + materialesTS[j].DocNum;
                                    oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);

                                    oInventoryGenExit.Lines.BaseType = 202;
                                    oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(materialesTS[j].DocEntry);
                                    oInventoryGenExit.Lines.BaseLine = Convert.ToInt32(materialesTS[j].Linea);
                                    oInventoryGenExit.Lines.WarehouseCode = materialesTS[j].WhsCode;
                                    oInventoryGenExit.Lines.Quantity = Convert.ToDouble(materialesExistencia[x].ReleasedQty);

                                    oInventoryGenExit.Lines.BatchNumbers.BatchNumber = materialesExistencia[x].DistNumber;
                                    oInventoryGenExit.Lines.BatchNumbers.Quantity = Convert.ToDouble(materialesExistencia[x].ReleasedQty);
                                    oInventoryGenExit.Lines.BatchNumbers.Location = materialesExistencia[x].BinCode;
                                    oInventoryGenExit.Lines.BatchNumbers.Add();
                                    
                                    oInventoryGenExit.Lines.Add();

                                    int respuestaC = oInventoryGenExit.Add();

                                    if (respuestaC == 0)
                                    {
                                        contadorOriginal = Convert.ToDouble(materialesTS[j].PlannedQty) - Convert.ToDouble(materialesExistencia[x].ReleasedQty);
                                        Console.WriteLine(DateTime.Now + " CONSUMO CREADO EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);
                                        lineaContador = lineaContador + 1;
                                        actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                    }
                                    else
                                    {
                                        Console.WriteLine(DateTime.Now + " Error al crear el CONSUMO " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);
                                    }

                                    DIAPIBLL.desconectarDIAPI();
                                }
                            }


                        }
                    }
                    else
                    {
                        actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
*/
        }

        //S17-REQUERIDO+ACTUALIZACION
        public void crearConsumoAutomaticoROVE()
        {
            try
            {
                //string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> materialesROVE = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    materialesROVE = ordenFabricacionDAL.buscarConsumosROVEAutorizados();
                    DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                    for (int j = 0; j < materialesROVE.Count; j++)
                    {
                        Console.WriteLine("OF ACTUALIZADA");

                        
                        Documents oInventoryGenExit;
                        oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                        oInventoryGenExit.Comments = "CONSUMO BASADO EN ROVE " + materialesROVE[j].DocNum;
                        oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);
                        oInventoryGenExit.UserFields.Fields.Item("U_Cita").Value = materialesROVE[j].U_LINEA;

                        oInventoryGenExit.Lines.BaseType = 202;
                        oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(materialesROVE[j].DocEntry);
                        oInventoryGenExit.Lines.BaseLine = Convert.ToInt32(materialesROVE[j].Linea);
                        oInventoryGenExit.Lines.WarehouseCode = materialesROVE[j].WhsCode;
                        oInventoryGenExit.Lines.Quantity = Convert.ToDouble(materialesROVE[j].IssuedQty);

                        oInventoryGenExit.Lines.BatchNumbers.BatchNumber = materialesROVE[j].DistNumber;
                        oInventoryGenExit.Lines.BatchNumbers.Quantity = Convert.ToDouble(materialesROVE[j].IssuedQty);
                        oInventoryGenExit.Lines.BatchNumbers.Location = materialesROVE[j].BinCode;
                        oInventoryGenExit.Lines.BatchNumbers.Add();

                        oInventoryGenExit.Lines.Add();

                        int respuestaC = oInventoryGenExit.Add();

                        ConsumosROVE consumosROVE = new ConsumosROVE();

                        if (respuestaC == 0)
                        {
                            Console.WriteLine(DateTime.Now + " CONSUMO CREADO EN LA OF " + materialesROVE[j].U_LINEA);
                            consumosROVE.estatus = "3";
                            consumosROVE.idConsumoROVE = materialesROVE[j].U_LINEA;
                            validarConsumoROVE(consumosROVE);
                        }
                        else
                        {

                            Console.WriteLine(DateTime.Now + " Error al crear el CONSUMO " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + materialesROVE[j].DocEntry);
                            if (DIAPIDAL.company.GetLastErrorDescription().Contains("P053"))
                            {
                                consumosROVE.estatus = "3";
                                consumosROVE.idConsumoROVE = materialesROVE[j].U_LINEA;
                                validarConsumoROVE(consumosROVE);
                            }
                            else
                            {
                                string error = DIAPIDAL.company.GetLastErrorDescription();
                                consumosROVE.Comentarios = error;
                                consumosROVE.estatus = "4";
                                consumosROVE.idConsumoROVE = materialesROVE[j].U_LINEA;
                                validarConsumoROVE(consumosROVE);
                            }
                        }
                        
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        //S17-REQUERIDO+ACTUALIZACION
        public void crearGeneracionAutomaticoROVE()
        {
            try
            {
                //string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> ordenFabricacion = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    ordenFabricacion = ordenFabricacionDAL.buscarGeneracionesROVEAutorizados();

                    DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                    for (int j = 0; j < ordenFabricacion.Count; j++)
                    {
                        Console.WriteLine("OF ACTUALIZADA");

                        
                        SAPbobsCOM.Documents inventoryGenEntry;
                        inventoryGenEntry = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

                        inventoryGenEntry.Comments = "GENERACION BASADA EN ROVE";
                        inventoryGenEntry.DocDueDate = DateTime.Now.AddDays(0);
                        //inventoryGenEntry.UserFields.Fields.Item("U_Maquina").Value = Convert.ToString("32");
                        inventoryGenEntry.UserFields.Fields.Item("U_Cita").Value = ordenFabricacion[j].U_LINEA;

                        inventoryGenEntry.Lines.Quantity = Convert.ToDouble(ordenFabricacion[j].IssuedQty);
                        inventoryGenEntry.Lines.BaseEntry = Convert.ToInt32(ordenFabricacion[j].DocEntry);
                        //inventoryGenEntry.Lines.CostingCode = "CEDIS SF";
                        //inventoryGenEntry.Lines.CostingCode3 = "MANUFAC";
                        inventoryGenEntry.Lines.BatchNumbers.BatchNumber = ordenFabricacion[j].DistNumber;
                        inventoryGenEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(ordenFabricacion[j].IssuedQty);
                        int diasVidaUtil = ordenFabricacionDAL.buscarVidaUtil(ordenFabricacion[j].DocEntry)[0].ValidComm == null ? 0 : Convert.ToInt32(ordenFabricacionDAL.buscarVidaUtil(ordenFabricacion[j].DocEntry)[0].ValidComm);
                        inventoryGenEntry.Lines.BatchNumbers.ExpiryDate = DateTime.Now.AddDays(diasVidaUtil);
                        inventoryGenEntry.Lines.BatchNumbers.Add();

                        inventoryGenEntry.Lines.Add();

                        int respuestaL = inventoryGenEntry.Add();

                        LiberacionesROVE liberacionesROVE = new LiberacionesROVE();
                        if (respuestaL == 0)
                        {
                            Console.WriteLine("LIBERACION CREADA");

                            Console.WriteLine(DateTime.Now + " GENERACION BASADA EN ROVE");
                            liberacionesROVE.estatus = "3";
                            liberacionesROVE.idLiberacionROVE = ordenFabricacion[j].U_LINEA;
                            validarGeneracionROVE(liberacionesROVE);
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now + " Error al crear la GENERACION " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + ordenFabricacion[j].DocEntry);

                            if (DIAPIDAL.company.GetLastErrorDescription().Contains("P054"))
                            {
                                liberacionesROVE.estatus = "3";
                                liberacionesROVE.idLiberacionROVE = ordenFabricacion[j].U_LINEA;
                                validarGeneracionROVE(liberacionesROVE);
                            }
                            else
                            {
                                string error = DIAPIDAL.company.GetLastErrorDescription();
                                liberacionesROVE.Comentarios = error;
                                liberacionesROVE.estatus = "4";
                                liberacionesROVE.idLiberacionROVE = ordenFabricacion[j].U_LINEA;
                                validarGeneracionROVE(liberacionesROVE);
                            }
                        }
                        
                    }
                    DIAPIBLL.desconectarDIAPI();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        public void crearConsumoAutomatico()
        {
            try
            {
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> materialesTS = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    materialesTS = ordenFabricacionDAL.buscarTSOF(basesDatos[i]);

                    if (materialesTS.Count > 0)
                    {

                        for (int j = 0; j < materialesTS.Count; j++)
                        {
                            Console.WriteLine("OF ACTUALIZADA");
                            List<OrdenFabricacion> materialesExistencia = new List<OrdenFabricacion>();
                            materialesExistencia = ordenFabricacionDAL.buscarInventarioOF(basesDatos[i], materialesTS[j].ItemCode, materialesTS[j].WhsCode);

                            double contadorOriginal = 0;

                            contadorOriginal = Convert.ToDouble(materialesTS[j].PlannedQty);

                            if (materialesExistencia.Count > 0)
                            {
                                int lineaContador = 0;
                                for (int x = 0; x < materialesExistencia.Count; x++)
                                {
                                    if (contadorOriginal > 0)
                                    {
                                        if (Convert.ToDouble(materialesExistencia[x].ReleasedQty) - contadorOriginal >= 0)
                                        {
                                            DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                                            Documents oInventoryGenExit;
                                            oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                                            oInventoryGenExit.Comments = "CONSUMO BASADO EN LA TS " + materialesTS[j].DocNum;
                                            oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);

                                            oInventoryGenExit.Lines.BaseType = 202;
                                            oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(materialesTS[j].DocEntry);
                                            oInventoryGenExit.Lines.BaseLine = Convert.ToInt32(materialesTS[j].Linea);
                                            oInventoryGenExit.Lines.WarehouseCode = materialesTS[j].WhsCode;
                                            oInventoryGenExit.Lines.Quantity = contadorOriginal;

                                            oInventoryGenExit.Lines.BatchNumbers.BatchNumber = materialesExistencia[x].DistNumber;
                                            oInventoryGenExit.Lines.BatchNumbers.Quantity = contadorOriginal;
                                            //oInventoryGenExit.Lines.BatchNumbers.Location = materialesExistencia[x].BinCode;
                                            oInventoryGenExit.Lines.BatchNumbers.Add();

                                            /*
                                            oInventoryGenExit.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = lineaContador;
                                            oInventoryGenExit.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(materialesExistencia[x].BinCode);
                                            oInventoryGenExit.Lines.BinAllocations.Quantity = contadorOriginal;
                                            oInventoryGenExit.Lines.BinAllocations.Add();
                                            */

                                            oInventoryGenExit.Lines.Add();

                                            int respuestaC = oInventoryGenExit.Add();

                                            if (respuestaC == 0)
                                            {
                                                Console.WriteLine(DateTime.Now + " CONSUMO CREADO EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);

                                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                                contadorOriginal = 0;
                                                lineaContador = lineaContador + 1;
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now + " Error al crear el CONSUMO " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);
                                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                                contadorOriginal = 0;
                                                lineaContador = lineaContador + 1;
                                            }
                                            DIAPIBLL.desconectarDIAPI();

                                        }
                                        else
                                        {


                                            DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                                            Documents oInventoryGenExit;
                                            oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                                            oInventoryGenExit.Comments = "CONSUMO BASADO EN LA TS " + materialesTS[j].DocNum;
                                            oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);

                                            oInventoryGenExit.Lines.BaseType = 202;
                                            oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(materialesTS[j].DocEntry);
                                            oInventoryGenExit.Lines.BaseLine = Convert.ToInt32(materialesTS[j].Linea);
                                            oInventoryGenExit.Lines.WarehouseCode = materialesTS[j].WhsCode;
                                            oInventoryGenExit.Lines.Quantity = Convert.ToDouble(materialesExistencia[x].ReleasedQty);

                                            oInventoryGenExit.Lines.BatchNumbers.BatchNumber = materialesExistencia[x].DistNumber;
                                            oInventoryGenExit.Lines.BatchNumbers.Quantity = Convert.ToDouble(materialesExistencia[x].ReleasedQty);
                                            oInventoryGenExit.Lines.BatchNumbers.Location = materialesExistencia[x].BinCode;
                                            oInventoryGenExit.Lines.BatchNumbers.Add();
                                            /*
                                            oInventoryGenExit.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = lineaContador;
                                            oInventoryGenExit.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(materialesExistencia[x].BinCode);
                                            oInventoryGenExit.Lines.BinAllocations.Quantity = contadorOriginal;
                                            oInventoryGenExit.Lines.BinAllocations.Add();
                                            */
                                            oInventoryGenExit.Lines.Add();

                                            int respuestaC = oInventoryGenExit.Add();

                                            if (respuestaC == 0)
                                            {
                                                contadorOriginal = Convert.ToDouble(materialesTS[j].PlannedQty) - Convert.ToDouble(materialesExistencia[x].ReleasedQty);
                                                Console.WriteLine(DateTime.Now + " CONSUMO CREADO EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);
                                                lineaContador = lineaContador + 1;
                                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                            }
                                            else
                                            {
                                                Console.WriteLine(DateTime.Now + " Error al crear el CONSUMO " + DIAPIDAL.company.GetLastErrorDescription() + " EN LA OF " + materialesTS[j].DocEntry + " DE LA TS " + materialesTS[j].DocNum);

                                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                                lineaContador = lineaContador + 1;
                                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                                            }

                                            DIAPIBLL.desconectarDIAPI();
                                        }
                                    }


                                }
                            }
                            else
                            {
                                actualizarTransferencia(basesDatos[i], materialesTS[j].DocNum, materialesTS[j].Linea2);
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        public void actualizarTransferencia(string sociedad, string docNum, string linea2)
        {
            ordenFabricacionDAL.actualizarTransferencia(sociedad, docNum, linea2);
        }

        public List<OrdenFabricacion> buscarOFCreada(string sociedad, string itemcode, string whscode, string quantity)
        {
            return ordenFabricacionDAL.buscarOFCreada(sociedad, itemcode, whscode, quantity);
        }

        //S19-REQUERIDO
        public List<OrdenFabricacion> obtenerLiberacionesCosto()
        {
            List<OrdenFabricacion> listaPT = new List<OrdenFabricacion>();
            listaPT = ordenFabricacionDAL.obtenerLiberacionesCosto();
            return listaPT;
        }

        public List<OrdenFabricacion> buscarUbicacionDestino(string sociedad, string whscode)
        {
            return ordenFabricacionDAL.buscarUbicacionDestino(sociedad, whscode);
        }

        //METODO DE OBTENER LAS OF PENDIENTES
        public List<OrdenFabricacion> buscarOFAbiertas(string sociedad)
        {
            return ordenFabricacionDAL.buscarOFAbiertas(sociedad);
        }

        public List<OrdenFabricacion> obtenerOFAbiertasWeb(string sociedad)
        {
            return ordenFabricacionDAL.obtenerOFAbiertasWeb(sociedad);
        }

        public List<ConsumosROVE> obtenerConsumosROVE(string ubicacion, string sociedad)
        {
            return ordenFabricacionDAL.obtenerConsumosROVE(ubicacion, sociedad);
        }

        public List<LiberacionesROVE> obtenerLiberacionesROVE(string ubicacion, string sociedad)
        {
            return ordenFabricacionDAL.obtenerLiberacionesROVE(ubicacion, sociedad);
        }

        public int guardarConsumoROVE(ConsumosROVE consumoROVE)
        {
            return ordenFabricacionDAL.guardarConsumoROVE(consumoROVE);
        }

        //S17-REQUERIDO
        public int validarConsumoROVE(ConsumosROVE consumoROVE)
        {
            return ordenFabricacionDAL.validarConsumoROVE(consumoROVE);
        }

        //S17-REQUERIDO
        public int validarGeneracionROVE(LiberacionesROVE liberacionesROVE)
        {
            return ordenFabricacionDAL.validarGeneracionROVE(liberacionesROVE);
        }

        public List<ConsumosROVE> confirmarExistenciaLote(ConsumosROVE consumoROVE)
        {
            return ordenFabricacionDAL.confirmarExistenciaLote(consumoROVE);
        }

        public List<ConsumosROVE> confirmarExistenciaLote2(ConsumosROVE consumoROVE)
        {
            return ordenFabricacionDAL.confirmarExistenciaLote2(consumoROVE);
        }

        public int validarLiberacionROVE(LiberacionesROVE liberacionesROVE)
        {
            return ordenFabricacionDAL.validarLiberacionROVE(liberacionesROVE);
        }

        public int validarParoROVE(ParosROVE parosROVE)
        {
            return ordenFabricacionDAL.validarParoROVE(parosROVE);
        }

        public List<OrdenFabricacion> obtenerConsumosOFAbiertasWeb(string sociedad)
        {
            return ordenFabricacionDAL.obtenerConsumosOFAbiertasWeb(sociedad);
        }

        public int guardarLiberacionesROVE(LiberacionesROVE liberacionesROVE)
        {
            return ordenFabricacionDAL.guardarLiberacionesROVE(liberacionesROVE);
        }

        public List<OrdenFabricacion> obtenerLiberacionesOFAbiertasWeb(string sociedad)
        {
            return ordenFabricacionDAL.obtenerLiberacionesOFAbiertasWeb(sociedad);
        }

        public int guardarParosROVE(ParosROVE parosROVE)
        {
            return ordenFabricacionDAL.guardarParosROVE(parosROVE);
        }

        public List<ParosROVE> obtenerParosROVE(string ubicacion, string sociedad)
        {
            return ordenFabricacionDAL.obtenerParosROVE(ubicacion, sociedad);
        }

        public List<ParosROVE> obtenercodigoParo(string areaParo)
        {
            return ordenFabricacionDAL.obtenercodigoParo(areaParo);
        }

        public List<OrdenFabricacion> obtenerOFWeb(string ubicacion, string sociedad)
        {
            return ordenFabricacionDAL.obtenerOFWeb(ubicacion, sociedad);
        }

        public List<OrdenFabricacionDetalle> obtenerCapacidades(string linea, string DocNum)
        {
            return ordenFabricacionDAL.obtenerCapacidades(linea, DocNum);
        }

        public List<OrdenFabricacionDetalle> obtenerArticuloOFWeb(OrdenFabricacion ordenFabricacion)
        {
            return ordenFabricacionDAL.obtenerArticuloOFWeb(ordenFabricacion);
        }

        public List<OrdenFabricacionDetalle> obtenerComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            return ordenFabricacionDAL.obtenerComponenteOFWeb(ordenFabricacion);
        }

        public List<OrdenFabricacionDetalle> obtenerLoteComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            return ordenFabricacionDAL.obtenerLoteComponenteOFWeb(ordenFabricacion);
        }

        public List<OrdenFabricacion> obtenerMaquinasOFWeb(OrdenFabricacion ordenFabricacion)
        {
            return ordenFabricacionDAL.obtenerMaquinasOFWeb(ordenFabricacion);
        }

        public List<OrdenFabricacion> contabilizacion_stocks(string sociedad, string OF, string tipoBusqueda)
        {
            return ordenFabricacionDAL.contabilizacion_stocks(sociedad, OF, tipoBusqueda);
        }

        //METODO DE VALIDAR LAS OF CON LA LISTA DE MATERIALES
        public List<ListaMateriales> listaMaterialesOF(string sociedad, string ItemCode)
        {
            return ordenFabricacionDAL.listaMaterialesOF(sociedad, ItemCode);
        }

        //METODO DE VALIDAR LAS OF CON LA LISTA DE MATERIALES
        public List<OrdenFabricacionDetalle> detalleOF(string sociedad, string DocEntry)
        {
            return ordenFabricacionDAL.detalleOF(sociedad, DocEntry);
        }

        //METODO DE OBTENER LAS OF PENDIENTES
        public List<OrdenFabricacionDetalle> buscarOFPartidasEliminadas(string sociedad, string docnum)
        {
            return ordenFabricacionDAL.buscarOFPartidasEliminadas(sociedad, docnum);
        }

        //REVISADO
        public List<OrdenFabricacion> OFSinCierre()
        {
            return ordenFabricacionDAL.OFSinCierre();
        }

        //S13-REQUERIDO
        public List<OrdenFabricacion> obtenerOFAbiertas(string sociedadVenta)
        {
            List<OrdenFabricacion> listaOFAbiertas = new List<OrdenFabricacion>();
            listaOFAbiertas = ordenFabricacionDAL.obtenerOFAbiertas(sociedadVenta);
            return listaOFAbiertas;
        }

        //S13-ACTUALIZACION        
        public void cerrarOFAbiertas(string DocEntry)
        {
            try
            {
                SAPbobsCOM.ProductionOrders productionOrders;
                productionOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                productionOrders.GetByKey(Convert.ToInt32(DocEntry));
                productionOrders.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposCancelled;
                //productionOrders.StartDate = DateTime.Now.Date;

                int respuesta = productionOrders.Cancel();

                if (respuesta == 0)
                {
                    Console.WriteLine("OF actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la OF " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la OF " + ex);
            }
        }

        public void crearOFAutomatica()
        {
            try
            {
                //string[] basesDatos = { "TSSL_Naturasol", "TSSL_Mielmex", "TSSL_Noval" };
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> listaOFCrear = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    listaOFCrear = ordenFabricacionDAL.buscarMaterialMayoreo(basesDatos[i]);

                    if (listaOFCrear.Count > 0)
                    {
                        DIAPIBLL.conectarDIAPI(basesDatos[i]);

                        for (int j = 0; j < listaOFCrear.Count; j++)
                        {
                            StockTransfer stockTransfer;
                            stockTransfer = DIAPIDAL.company.GetBusinessObject(BoObjectTypes.oStockTransfer);

                            stockTransfer.DocDate = DateTime.Now;
                            stockTransfer.FromWarehouse = listaOFCrear[j].WhsCode;
                            stockTransfer.PriceList = 12;
                            stockTransfer.ToWarehouse = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);

                            stockTransfer.Lines.ItemCode = listaOFCrear[j].ItemCode;
                            stockTransfer.Lines.FromWarehouseCode = listaOFCrear[j].WhsCode;
                            stockTransfer.Lines.WarehouseCode = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);
                            stockTransfer.Lines.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                            stockTransfer.Lines.DistributionRule = "CEDIS SF";
                            stockTransfer.Lines.DistributionRule3 = "MANUFAC";

                            stockTransfer.Lines.BatchNumbers.BatchNumber = listaOFCrear[j].DistNumber;
                            stockTransfer.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                            stockTransfer.Lines.BatchNumbers.Add();

                            stockTransfer.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batFromWarehouse;
                            stockTransfer.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                            stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(listaOFCrear[j].BinCode);
                            stockTransfer.Lines.BinAllocations.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                            stockTransfer.Lines.BinAllocations.Add();

                            stockTransfer.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batToWarehouse;
                            stockTransfer.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;

                            stockTransfer.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionDestino(basesDatos[i], Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14))[0].BinCode);
                            stockTransfer.Lines.BinAllocations.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                            stockTransfer.Lines.BinAllocations.Add();

                            stockTransfer.Lines.Add();

                            stockTransfer.Comments = "TRANSFERENCIA DE MAYOREO A PRODUCCION";
                            int respuesta = stockTransfer.Add();

                            if (respuesta == 0)
                            {
                                Console.WriteLine("Error al crear la OF " + DIAPIDAL.company.GetLastErrorDescription());
                                string codigoPT = "";
                                switch (listaOFCrear[j].ItemCode)
                                {
                                    case "2001-081-006":
                                        codigoPT = "2004-081-022";
                                        break;
                                    case "2001-081-019":
                                        codigoPT = "2004-081-025";
                                        break;
                                    case "2001-081-017":
                                        codigoPT = "2004-081-023";
                                        break;
                                    case "1501-100-099":
                                        codigoPT = "2004-081-019";
                                        break;
                                    case "1501-050-056":
                                        codigoPT = "2004-081-017";
                                        break;
                                    case "2001-081-016":
                                        codigoPT = "2004-081-024";
                                        break;
                                    case "2001-082-003":
                                        codigoPT = "2004-082-006";
                                        break;
                                    case "1501-100-086":
                                        codigoPT = "2004-083-087";
                                        break;
                                    case "1501-100-093":
                                        codigoPT = "2004-083-086";
                                        break;
                                    case "1501-100-097":
                                        codigoPT = "2004-083-088";
                                        break;
                                    case "2001-100-001":
                                        codigoPT = "2004-100-004";
                                        break;
                                    case "2001-085-008":
                                        codigoPT = "2004-085-004";
                                        break;
                                    case "2001-085-009":
                                        codigoPT = "2004-085-016";
                                        break;
                                    case "2001-150-002":
                                        codigoPT = "2004-150-057";
                                        break;
                                    case "2001-150-001":
                                        codigoPT = "2004-150-056";
                                        break;
                                    case "1501-100-015":
                                        codigoPT = "2004-070-005";
                                        break;
                                    case "1501-100-053":
                                        codigoPT = "2004-080-002";
                                        break;
                                    case "2001-086-003":
                                        codigoPT = "2004-086-004";
                                        break;
                                    case "2001-085-004":
                                        codigoPT = "2004-085-010";
                                        break;
                                    case "2001-085-002":
                                        codigoPT = "2004-085-007";
                                        break;
                                    case "2001-082-007":
                                        codigoPT = "2004-082-009";
                                        break;
                                    case "1501-050-057":
                                        codigoPT = "2004-081-027";
                                        break;
                                    case "2001-085-005":
                                        codigoPT = "2004-085-011";
                                        break;
                                    case "1501-100-071":
                                        codigoPT = "2004-083-088";
                                        break;
                                    case "2001-085-003":
                                        codigoPT = "2004-085-009";
                                        break;
                                    case "2001-082-006":
                                        codigoPT = "2004-082-010";
                                        break;
                                    case "1501-100-047":
                                        codigoPT = "2004-080-003";
                                        break;
                                    case "2001-081-024":
                                        codigoPT = "2004-081-028";
                                        break;
                                    case "1501-070-009":
                                        codigoPT = "2004-085-012";
                                        break;
                                    case "2001-083-041":
                                        codigoPT = "2004-083-173";
                                        break;
                                    case "1501-100-045":
                                        codigoPT = "2004-070-007";
                                        break;
                                }

                                ProductionOrders productionOrders;
                                productionOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                                productionOrders.ItemNo = codigoPT;
                                productionOrders.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotSpecial;
                                productionOrders.DueDate = DateTime.Now.AddDays(0);
                                productionOrders.PostingDate = DateTime.Now.AddDays(-1);
                                productionOrders.ProductionOrderStatus = BoProductionOrderStatusEnum.boposPlanned;
                                productionOrders.PlannedQuantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                productionOrders.Warehouse = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);

                                string serie = "";

                                switch (Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14))
                                {
                                    case "1306":
                                        serie = "81";
                                        break;
                                    case "1706":
                                        serie = "77";
                                        break;
                                    case "1806":
                                        serie = "33";
                                        break;
                                }

                                productionOrders.Series = Convert.ToInt32(serie);

                                productionOrders.Lines.ItemNo = listaOFCrear[j].ItemCode;
                                productionOrders.Lines.PlannedQuantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                productionOrders.Lines.Warehouse = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);
                                productionOrders.Lines.DistributionRule = "CEDIS SF";
                                productionOrders.Lines.DistributionRule3 = "MANUFAC";
                                productionOrders.Lines.DistributionRule4 = "MAT PRI";
                                productionOrders.Lines.Add();

                                int respuestaOF = productionOrders.Add();

                                if (respuestaOF == 0)
                                {
                                    Console.WriteLine("OF CREADA");

                                    ProductionOrders updateProductionOrders;
                                    updateProductionOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                                    updateProductionOrders.GetByKey(Convert.ToInt32(Convert.ToInt32(ordenFabricacionDAL.buscarOFCreada(basesDatos[i], codigoPT, Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14), (listaOFCrear[j].PlannedQty))[0].DocEntry)));
                                    updateProductionOrders.ProductionOrderStatus = BoProductionOrderStatusEnum.boposReleased;

                                    int respuestaOFActualizar = updateProductionOrders.Update();
                                    if (respuestaOFActualizar == 0)
                                    {
                                        Console.WriteLine("OF ACTUALIZADA");
                                        Documents oInventoryGenExit;
                                        oInventoryGenExit = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                                        oInventoryGenExit.Comments = "CONSUMO DE ORDEN MAYOREO";
                                        oInventoryGenExit.DocDueDate = DateTime.Now.AddDays(0);

                                        oInventoryGenExit.Lines.BaseType = 202;
                                        oInventoryGenExit.Lines.BaseEntry = Convert.ToInt32(ordenFabricacionDAL.buscarOFCreada(basesDatos[i], codigoPT, Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14), (listaOFCrear[j].PlannedQty))[0].DocEntry);
                                        oInventoryGenExit.Lines.BaseLine = 0;

                                        oInventoryGenExit.Lines.BatchNumbers.BatchNumber = listaOFCrear[j].DistNumber;
                                        oInventoryGenExit.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                        oInventoryGenExit.Lines.BatchNumbers.Add();

                                        oInventoryGenExit.Lines.Add();

                                        int respuestaC = oInventoryGenExit.Add();

                                        if (respuestaC == 0)
                                        {
                                            Console.WriteLine("CONSUMO CREADO");

                                            SAPbobsCOM.Documents inventoryGenEntry;
                                            inventoryGenEntry = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

                                            inventoryGenEntry.Comments = "LIBERACION DE ORDEN DE MAYOREO";
                                            inventoryGenEntry.DocDueDate = DateTime.Now.AddDays(0);
                                            inventoryGenEntry.UserFields.Fields.Item("U_Maquina").Value = Convert.ToString("32");

                                            inventoryGenEntry.Lines.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                            inventoryGenEntry.Lines.BaseEntry = Convert.ToInt32(ordenFabricacionDAL.buscarOFCreada(basesDatos[i], codigoPT, Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14), (listaOFCrear[j].PlannedQty))[0].DocEntry);
                                            inventoryGenEntry.Lines.CostingCode = "CEDIS SF";
                                            inventoryGenEntry.Lines.CostingCode3 = "MANUFAC";
                                            inventoryGenEntry.Lines.BatchNumbers.BatchNumber = listaOFCrear[j].DistNumber;
                                            inventoryGenEntry.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                            inventoryGenEntry.Lines.BatchNumbers.ExpiryDate = DateTime.Now.AddDays(listaOFCrear[j].ValidComm == "" ? 0 : Convert.ToDouble(listaOFCrear[j].ValidComm));
                                            inventoryGenEntry.Lines.BatchNumbers.Add();

                                            inventoryGenEntry.Lines.Add();

                                            int respuestaL = inventoryGenEntry.Add();

                                            if (respuestaL == 0)
                                            {
                                                Console.WriteLine("LIBERACION CREADA");

                                                ProductionOrders updateProductionOrders2;
                                                updateProductionOrders2 = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                                                updateProductionOrders2.GetByKey(Convert.ToInt32(Convert.ToInt32(ordenFabricacionDAL.buscarOFCreada(basesDatos[i], codigoPT, Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14), (listaOFCrear[j].PlannedQty))[0].DocEntry)));
                                                updateProductionOrders2.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;

                                                int respuestaOFActualizar2 = updateProductionOrders2.Update();

                                                if (respuestaOFActualizar2 == 0)
                                                {
                                                    Console.WriteLine("OF ACTUALIZADA");
                                                    StockTransfer stockTransferPT;
                                                    stockTransferPT = DIAPIDAL.company.GetBusinessObject(BoObjectTypes.oStockTransfer);

                                                    stockTransferPT.DocDate = DateTime.Now;
                                                    stockTransferPT.PriceList = 12;

                                                    stockTransferPT.ToWarehouse = listaOFCrear[j].WhsCode;
                                                    stockTransferPT.FromWarehouse = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);

                                                    stockTransferPT.Lines.ItemCode = codigoPT;
                                                    stockTransferPT.Lines.DistributionRule = "CEDIS SF";
                                                    stockTransferPT.Lines.DistributionRule3 = "MANUFAC";
                                                    stockTransferPT.Lines.WarehouseCode = listaOFCrear[j].WhsCode;
                                                    stockTransferPT.Lines.FromWarehouseCode = Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14);
                                                    stockTransferPT.Lines.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);

                                                    stockTransferPT.Lines.BatchNumbers.BatchNumber = listaOFCrear[j].DistNumber;
                                                    stockTransferPT.Lines.BatchNumbers.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                                    stockTransferPT.Lines.BatchNumbers.Add();

                                                    stockTransferPT.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batFromWarehouse;
                                                    stockTransferPT.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                                                    stockTransferPT.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(ordenFabricacionDAL.buscarUbicacionDestino(basesDatos[i], Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14) == "1206" ? "1806" : Convert.ToString(Convert.ToInt32(listaOFCrear[j].WhsCode) - 14))[0].BinCode);
                                                    stockTransferPT.Lines.BinAllocations.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                                    stockTransferPT.Lines.BinAllocations.Add();

                                                    stockTransferPT.Lines.BinAllocations.BinActionType = BinActionTypeEnum.batToWarehouse;
                                                    stockTransferPT.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;

                                                    stockTransferPT.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(listaOFCrear[j].BinCode);
                                                    stockTransferPT.Lines.BinAllocations.Quantity = Convert.ToDouble(listaOFCrear[j].PlannedQty);
                                                    stockTransferPT.Lines.BinAllocations.Add();

                                                    stockTransferPT.Lines.Add();

                                                    stockTransferPT.Comments = "TRANSFERENCIA DE PRODUCCION A MAYOREO";
                                                    int respuestafinal = stockTransferPT.Add();

                                                    if (respuestafinal == 0)
                                                    {
                                                        Console.WriteLine("TRANSFERENCIA CREADA");

                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Error al crear el TRASPASO a produccion " + DIAPIDAL.company.GetLastErrorDescription());

                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Error al CERRAR LA OF a produccion " + DIAPIDAL.company.GetLastErrorDescription());
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error al crear la LIBERACION " + codigoPT + DIAPIDAL.company.GetLastErrorDescription());
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error al crear el CONSUMO " + listaOFCrear[j].ItemCode + DIAPIDAL.company.GetLastErrorDescription());
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error al crear la OF " + DIAPIDAL.company.GetLastErrorDescription());
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error al crear la OF " + DIAPIDAL.company.GetLastErrorDescription());
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error al crear el TRASPASO a produccion " + DIAPIDAL.company.GetLastErrorDescription());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        public void crearOFMasivo()
        {
            try
            {
                string[] basesDatos = { "TSSL_NATURASOL" };
                List<OrdenFabricacion> listaOFCrear = new List<OrdenFabricacion>();

                for (int i = 0; i < basesDatos.Length; i++)
                {
                    listaOFCrear = ordenFabricacionDAL.buscarOFMasiva(basesDatos[i]);

                    for (int x = 0; x < listaOFCrear.Count; x++)
                    {
                        if (listaOFCrear[x].DocNum == "" && listaOFCrear[x].ItemCode != "")
                        {
                            ProductionOrders productionOrders;
                            productionOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                            productionOrders.ItemNo = listaOFCrear[x].ItemCode;
                            productionOrders.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotStandard;
                            productionOrders.DueDate = Convert.ToDateTime(listaOFCrear[x].DueDate);
                            productionOrders.PostingDate = DateTime.Now.AddDays(-1);
                            productionOrders.ProductionOrderStatus = BoProductionOrderStatusEnum.boposPlanned;
                            productionOrders.PlannedQuantity = Convert.ToDouble(listaOFCrear[x].PlannedQty);
                            productionOrders.Warehouse = Convert.ToString(listaOFCrear[x].WhsCode);
                            productionOrders.DistributionRule = listaOFCrear[x].Linea;
                            productionOrders.DistributionRule3 = listaOFCrear[x].Linea2;

                            string serie = "";

                            switch (Convert.ToString(listaOFCrear[x].Series))
                            {
                                case "BARRAS":
                                    serie = "77";
                                    break;
                                case "CEDIS":
                                    serie = "81";
                                    break;
                                case "CREMAS":
                                    serie = "78";
                                    break;
                                case "ENVASADO":
                                    serie = "101";
                                    break;
                                case "MAIZ":
                                    serie = "94";
                                    break;
                                case "PAPAS":
                                    serie = "79";
                                    break;
                                case "SAZONA":
                                    serie = "80";
                                    break;
                                case "SEMILLAS":
                                    serie = "33";
                                    break;
                                case "TPK-MAIZ":
                                    serie = "106";
                                    break;
                                case "TPK-MEZC":
                                    serie = "110";
                                    break;
                                case "TPK-PAP":
                                    serie = "104";
                                    break;
                                case "UNTABLES":
                                    serie = "97";
                                    break;
                                case "VITROLER":
                                    serie = "99";
                                    break;
                            }

                            productionOrders.Series = Convert.ToInt32(serie);
                            /*
                            productionOrders.Lines.Warehouse = Convert.ToString(listaOFCrear[x].WhsCode);
                            productionOrders.Lines.DistributionRule = listaOFCrear[x].Linea;
                            productionOrders.Lines.DistributionRule3 = listaOFCrear[x].Linea2;
                            productionOrders.Lines.Add();
                            */

                            int respuestaOF = productionOrders.Add();

                            if (respuestaOF == 0)
                            {
                                ordenFabricacionDAL.actualizarTablaOFMasiva(listaOFCrear[x].Code, DIAPIDAL.company.GetNewObjectKey());
                                Console.WriteLine("OF CREADA");
                            }
                            else
                            {
                                Console.WriteLine("Error al crear la OF " + DIAPIDAL.company.GetLastErrorDescription());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        public List<OrdenFabricacionDetalle> obtenerUMComponenteOFWeb(OrdenFabricacion ordenFabricacion)
        {
            return ordenFabricacionDAL.obtenerUMComponenteOFWeb(ordenFabricacion);
        }

        public List<int> obtenerConsumos(string sociedad)
        {
            return ordenFabricacionDAL.obtenerConsumos(sociedad);
        }

        public List<int> obtenerLiberaciones(string sociedad)
        {
            return ordenFabricacionDAL.obtenerLiberaciones(sociedad);
        }

        public List<int> obtenerParos(string sociedad)
        {
            return ordenFabricacionDAL.obtenerParos(sociedad);
        }

        public List<string> obtenerKgs(string sociedad)
        {
            return ordenFabricacionDAL.obtenerKgs(sociedad);
        }

        public List<string> obtenerEstatusOF(string sociedad)
        {
            return ordenFabricacionDAL.obtenerEstatusOF(sociedad);
        }

        public List<string> obtenerKgs06(string sociedad)
        {
            return ordenFabricacionDAL.obtenerKgs06(sociedad);
        }

        //METODO PARA CAMBIAR EL ESTATUS DE REVISADO DE UNA OF
        public void actualizarOF(string DocEntry, string planeado, string original, string VisOrder, string U_BOM)
        {
            try
            {
                Console.WriteLine(Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                /*
                SAPbobsCOM.CompanyService oCompanyService = DIAPIDAL.company.GetCompanyService();
                oCompanyService.GetFinancePeriods(oCompanyService.GetPeriods().Item(1)).Item(0).Locked = SAPbobsCOM.BoYesNoEnum.tYES;
                oCompanyService.GetFinancePeriod( (oFinancePeriod)
                */
                SAPbobsCOM.ProductionOrders productionOrders;
                productionOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                productionOrders.GetByKey(Convert.ToInt32(DocEntry));
                productionOrders.UserFields.Fields.Item("U_Fecha_Revision").Value = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                productionOrders.UserFields.Fields.Item("U_Revisada").Value = Convert.ToString('1');

                if (original == null || original == "")
                {
                    productionOrders.UserFields.Fields.Item("U_Planificado_Original").Value = planeado;
                }

                if (VisOrder != null)
                {
                    for (int i = 0; i < productionOrders.Lines.Count; i++)
                    {
                        if (i == Convert.ToInt32(VisOrder))
                        {
                            productionOrders.Lines.SetCurrentLine(i);
                            //DETALLE
                            productionOrders.Lines.UserFields.Fields.Item("U_BOM").Value = U_BOM;
                        }
                    }
                }

                int respuesta = productionOrders.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("OF actualizada");
                }
                else
                {
                    Console.WriteLine("Error al actualizar la OF " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actulizar la OF " + ex);
            }

        }

        //S19-ACTUALIZACION
        public void actualizarOFCosto(string DocEntry, string linea)
        {
            try
            {
                Console.WriteLine(Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                SAPbobsCOM.Documents inventoryGenEntry;
                inventoryGenEntry = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                inventoryGenEntry.GetByKey(Convert.ToInt32(DocEntry));
                inventoryGenEntry.UserFields.Fields.Item("U_FT3_LEYENDAPROC").Value = Convert.ToString('1');

                int respuesta = inventoryGenEntry.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("OF actualizada");
                }
                else
                {
                    Console.WriteLine("Error al actualizar la OF " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actulizar la OF " + ex);
            }
        }
    }
}

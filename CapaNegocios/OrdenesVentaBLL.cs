using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocios
{
    public class OrdenesVentaBLL
    {
        OrdenesVentaDAL ordenesVentaDAL = new OrdenesVentaDAL();

        //METODO PARA CREAR EL PEDIDO DE VENTA A SAP
        public string crearPedidoVenta(OrdenCompra ordenCompra, List<OrdenCompraDetalle> ordenCompraDetalle, string sociedadVenta)
        {
            SAPbobsCOM.Documents oOrders;
            oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

            string[] codigosProveedor = { "2110100503", "2110100501", "2110100502", "2110100704" };

            //ENCABEZADO
            oOrders.CardCode = ordenCompra.CardCode;
            oOrders.DocDate = DateTime.Now;
            oOrders.DocDueDate = DateTime.Now;
            oOrders.UserFields.Fields.Item("U_INT_SOREL").Value = ordenCompra.Sociedad == "TestMielmex" ? "MM" : (ordenCompra.Sociedad == "TestNaturasol" ? "NA" : (ordenCompra.Sociedad == "TestEvi") ? "EV" : "NO");
            oOrders.UserFields.Fields.Item("U_INT_DOCRE").Value = ordenCompra.DocNum;
            oOrders.Comments = "Pedido Intercompania basado en OC: " + ordenCompra.DocNum + " de: " + ordenCompra.Sociedad;
            oOrders.DocType = ordenCompra.DocType == "I" ? SAPbobsCOM.BoDocumentTypes.dDocument_Items : SAPbobsCOM.BoDocumentTypes.dDocument_Service;

            for (int i = 0; i < ordenCompraDetalle.Count; i++)
            {
                //DETALLE
                oOrders.Lines.ItemCode = ordenCompraDetalle[i].ItemCode;
                oOrders.Lines.Quantity = Convert.ToDouble(ordenCompraDetalle[i].Quantity);
                oOrders.Lines.TaxCode = ordenCompraDetalle[i].TaxCode.Replace('A', 'T');
                oOrders.Lines.Price = Convert.ToDouble(ordenCompraDetalle[i].Price);
                oOrders.Lines.Currency = ordenCompraDetalle[i].Currency;
                oOrders.Lines.UoMEntry = ordenesVentaDAL.obtenerUM(ordenCompraDetalle[i].UM, sociedadVenta);
                oOrders.Lines.Add();
            }

            int respuesta = oOrders.Add();
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

        //METODO PARA BUSCAR EL DETALLE DE UNA OC INTERCOMPANIA
        public List<OrdenVentaDetalle> obtenerOrdenesCompraDetalleIntercompania(string sociedad, OrdenVenta orden)
        {
            List<OrdenVentaDetalle> listaOrdenesVentaDetalle = new List<OrdenVentaDetalle>();
            listaOrdenesVentaDetalle = ordenesVentaDAL.obtenerOrdenesVentaDetalleIntercompania(sociedad, orden);
            return listaOrdenesVentaDetalle;
        }

        //METODO DE BUSQUEDA DE PEDIDOS INTERCOMPANIA
        public List<OrdenVenta> obtenerOrdenesVentaIntercompania(string sociedad)
        {
            List<OrdenVenta> listaOrdenesCompra = new List<OrdenVenta>();
            listaOrdenesCompra = ordenesVentaDAL.obtenerOrdenesVentaIntercompania(sociedad);
            return listaOrdenesCompra;
        }

        //METODO PARA OBTENER LOS PEDIDOS DE VENTA INTERCOMPANIA QUE SE CREARON
        public List<OrdenVenta> obtenerPedidoVentaIntercompania(OrdenCompra OrdenCompra, string sociedadVenta)
        {
            List<OrdenVenta> listaBorradorOrdenVenta = new List<OrdenVenta>();
            listaBorradorOrdenVenta = ordenesVentaDAL.obtenerPedidoVentaIntercompania(OrdenCompra, sociedadVenta);
            return listaBorradorOrdenVenta;
        }

        //S14-REQUERIDO-METODO PARA OBTENER LOS PEDIDOS DE VENTA ABIERTOS
        public List<OrdenVenta> obtenerElaboradorOVAbiertas(string sociedadVenta)
        {
            List<OrdenVenta> listaElaboradores = new List<OrdenVenta>();
            listaElaboradores = ordenesVentaDAL.obtenerElaboradorOVAbiertas(sociedadVenta);
            return listaElaboradores;
        }

        public List<OrdenVenta> obtenerPedidoVenta(string OrdenCompra)
        {
            List<OrdenVenta> listaBorradorOrdenVenta = new List<OrdenVenta>();
            listaBorradorOrdenVenta = ordenesVentaDAL.obtenerPedidoVenta(OrdenCompra);
            return listaBorradorOrdenVenta;
        }
        //S14-REQUERIDO-METODO PARA OBTENER LOS PEDIDOS DE VENTA ABIERTOS
        public List<OrdenVenta> obtenerOVAbiertas(string sociedadVenta, string email)
        {
            List<OrdenVenta> listaOVAbiertas = new List<OrdenVenta>();
            listaOVAbiertas = ordenesVentaDAL.obtenerOVAbiertas(sociedadVenta, email);
            return listaOVAbiertas;
        }

        //S14-ACTUALIZACION-METODO PARA CERRAR LOS PEDIDOS DE VENTA ABIERTOS
        public void cerrarOVAbiertas(string DocEntry)
        {
            try
            {
                SAPbobsCOM.Documents oOrders;
                oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                oOrders.GetByKey(Convert.ToInt32(DocEntry));
                oOrders.Close();

                int respuesta = oOrders.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("OV actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la OV " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la OV " + ex);
            }
        }

        //METODO PARA ACTUALIZAR LA OV DESPUES DE CREAR LA ORDEN DE COMPRA
        public void actualizarOV(string DocEntry, string DocNum, string mensaje)
        {
            SAPbobsCOM.Documents oOrders;
            oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            oOrders.GetByKey(Convert.ToInt32(DocEntry));

            if (mensaje.Equals(""))
            {
                //oOrders.UserFields.Fields.Item("U_REPLICAR").Value = "0";
                oOrders.UserFields.Fields.Item("U_NoCaja").Value = DocNum;
                oOrders.NumAtCard = DocNum;
            }
            else
            {
                //oOrders.UserFields.Fields.Item("U_REPLICAR").Value = "0";
                oOrders.UserFields.Fields.Item("U_NoCaja").Value = DocNum;
                oOrders.Comments = mensaje;
            }

            int respuesta = oOrders.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OV actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OV " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        //METODO PARA ACTUALIZAR LA OV DESPUES DE CREAR LA ORDEN DE COMPRA
        public void actualizarFechaCitaOV2(string ordenCompra, string fecha, string hora, string cita)
        //public void actualizarFechaCitaOV2(DataTable dt)
        {
            //DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
            
            ordenesVentaDAL.actualizarFechaCitaOV2(ordenCompra, fecha, hora, cita);
            /*
            for (int g = 0; g < dt.Rows.Count; g++)
            {
                SAPbobsCOM.Documents oOrders;
                oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                List<OrdenVenta> listaOV = obtenerPedidoVenta(Convert.ToString(dt.Rows[g]["OC"]));

                for (int k = 0; k < listaOV.Count; k++)
                {
                    oOrders.GetByKey(Convert.ToInt32(listaOV[k].DocEntry));

                    oOrders.UserFields.Fields.Item("U_HoraEntrega").Value = Convert.ToString(dt.Rows[g]["OC"]);
                    oOrders.UserFields.Fields.Item("U_ETABodega").Value = Convert.ToString(dt.Rows[g]["Fecha y Hora Cita Destino"].ToString().Split(' ')[0]);
                    oOrders.UserFields.Fields.Item("U_Observaciones").Value = Convert.ToString(dt.Rows[g]["Fecha y Hora Cita Destino"].ToString().Split(' ')[1]);

                    int respuesta = oOrders.Update();

                    if (respuesta == 0)
                    {
                        Console.WriteLine("OV actualizado");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar la OV " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
            }
            DIAPIBLL.desconectarDIAPI();
            */
        }

        //METODO PARA ACTUALIZAR LA OC DESPUES DE CREAR LA ORDEN DE VENTA
        public void actualizarOV(string itemcode)
        {
            /*
            SAPbobsCOM.Documents oOrders;
            oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);            
            oOrders.GetByKey(Convert.ToInt32(DocEntry));
            oOrders.CardName = "";
            //oOrders.Lines.UserFields.Fields.Item("U_MOT_NO_ENTREGA").Value = "";
            oOrders.Comments = "Orden de venta actualizada desde DIAPI";

            int respuesta = oOrders.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OV actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OV " + DIAPIDAL.company.GetLastErrorDescription());
            }
            */
            SAPbobsCOM.ProductTrees oProductTrees;
            oProductTrees = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);
            oProductTrees.GetByKey(itemcode);

            Console.WriteLine("OV" + oProductTrees.Items.Count);
            oProductTrees.Items.ItemName = "jijiji";

            int respuesta = oProductTrees.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("OV actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar la OV " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }
    }
}
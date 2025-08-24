using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class ListaPreciosBLL
    {
        ListaPreciosDAL listaPreciosDAL = new ListaPreciosDAL();
        OrdenesVentaDAL ordenesVentaDAL = new OrdenesVentaDAL();

        public List<ListaPrecios> obtenerDocumentosVenta(string sociedad)
        {
            List<ListaPrecios> listaArticulos = new List<ListaPrecios>();
            listaArticulos = listaPreciosDAL.obtenerDocumentosVenta(sociedad);
            return listaArticulos;
        }

        public List<ListaPrecios> obtenerDocumentosCompra(string sociedad)
        {
            List<ListaPrecios> listaArticulos = new List<ListaPrecios>();
            listaArticulos = listaPreciosDAL.obtenerDocumentosCompra(sociedad);
            return listaArticulos;
        }

        public void actualizarDocumentosVenta(string DocEntry, string VisOrder, string item)
        {
            //return ordenesVentaDAL.actualizarDocumentosVenta(sociedad, DocEntry, linea, item);

            SAPbobsCOM.Documents oOrders;
            oOrders = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            oOrders.GetByKey(Convert.ToInt32(DocEntry));

            for (int i = 0; i < oOrders.Lines.Count; i++)
            {
                if (i == Convert.ToInt32(VisOrder))
                {
                    oOrders.Lines.SetCurrentLine(Convert.ToInt32(VisOrder));
                    oOrders.Lines.UserFields.Fields.Item("U_FechaRevision").Value = item;

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
        }

        public void actualizarDocumentosCompra(string DocEntry, string VisOrder, string item, int error)
        {            
            SAPbobsCOM.Documents oDrafts;
            oDrafts = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
            oDrafts.GetByKey(Convert.ToInt32(DocEntry));
            

            if (error == 0)
            {
                oDrafts.UserFields.Fields.Item("U_Observaciones").Value = "NO AUTORIZADO POR PRECIO";
            }

            for (int i = 0; i < oDrafts.Lines.Count; i++)
            {
                if (i == Convert.ToInt32(VisOrder))
                {
                    oDrafts.Lines.SetCurrentLine(Convert.ToInt32(VisOrder));
                    oDrafts.Lines.UserFields.Fields.Item("U_FechaRevision").Value = item;

                    int respuesta = oDrafts.Update();

                    if (respuesta == 0)
                    {
                        Console.WriteLine("OC actualizado");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar la OC " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
            }
        }
    }
}

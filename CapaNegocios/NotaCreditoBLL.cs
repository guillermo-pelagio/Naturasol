using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class NotaCreditoBLL
    {
        NotaCreditoDAL notaCreditoDAL = new NotaCreditoDAL();

        //METODO PARA BUSCAR LAS FACTURAS PENDIENTES DE CORRECCION
        public List<NotaCreditoDetalle> obtenerNotasCreditoCorreccionesV4(string sociedad)
        {
            List<NotaCreditoDetalle> listaFacturasDeudorDetalle = new List<NotaCreditoDetalle>();
            listaFacturasDeudorDetalle = notaCreditoDAL.obtenerFacturasCorreccionesV4(sociedad);
            return listaFacturasDeudorDetalle;
        }

        //METODO PARA ACTUALIZAR LA NOTA DE CREDITO CON NOMBRES DIFERENTES Y TIMBRADO
        public void actualizarNotaCreditoDeudorCorrecciones(string sociedad, string DocEntry, string Cardname)
        {
            /*
            SAPbobsCOM.Documents oCreditNotes;
            oCreditNotes = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

            oCreditNotes.GetByKey(Convert.ToInt32(DocEntry));

            oCreditNotes.CardName = Cardname;
            oCreditNotes.UserFields.Fields.Item("U_FT3_IDVERSIONCFDI").Value = "4";

            int respuesta = oCreditNotes.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Documento actualizado");
            }
            else
            {
                Console.WriteLine("Error al actualizar el documento" + DIAPIDAL.company.GetLastErrorDescription());
            }
            */
            notaCreditoDAL.actualizarNotaCredito(sociedad, DocEntry, Cardname);
        }
    }
}

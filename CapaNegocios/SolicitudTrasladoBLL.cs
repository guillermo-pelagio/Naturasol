using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class SolicitudTrasladoBLL
    {
        SolicitudTrasladoDAL solicitudTrasladoDAL = new SolicitudTrasladoDAL();

        //S16-REQUERIDO-METODO DE OBTENER LAS ST PENDIENTES
        public List<SolicitudTraslado> obtenerSTAbiertas(string sociedad)
        {
            return solicitudTrasladoDAL.obtenerSTAbiertas(sociedad);
        }

        //S16-ACTUALIZACION-METODO PARA CERRA LAS ST ABIERTAS DESPUES DE 2 DIAS
        public void cerrarSTAbiertas(string DocEntry)
        {            
            try
            {
                DIAPIBLL.conectarDIAPI("TSSL_NATURASOL");
                SAPbobsCOM.StockTransfer oStock;
                oStock = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);
                oStock.GetByKey(Convert.ToInt32(DocEntry));
                oStock.Close();

                int respuesta = oStock.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("ST actualizado");
                }
                else
                {
                    Console.WriteLine("Error al cerrar la ST " + DIAPIDAL.company.GetLastErrorDescription());
                }
                DIAPIBLL.desconectarDIAPI();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la ST " + ex);
            }            
        }
    }
}

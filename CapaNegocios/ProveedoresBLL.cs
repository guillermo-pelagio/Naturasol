using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class ProveedoresBLL
    {
        ProveedoresDAL proveedoresDAL = new ProveedoresDAL();

        //METODO DE VALIDACION DE INICIO DE SESION
        public SesionProveedor inicioSesion(Proveedores proveedor)
        {
            return proveedoresDAL.inicioSesion(proveedor);
        }

        public List<EntradaMaterial> obtenerEntradas(string sociedad, string cardcode)
        {
            return proveedoresDAL.obtenerEntradas(sociedad, cardcode);
        }

        public List<EntradaMaterial> obtener_entradas_global(string sociedad)
        {
            return proveedoresDAL.obtener_entradas_global(sociedad);
        }
        
        public List<FacturaProveedor> obtenerFacturas(string sociedad, string cardcode)
        {
            return proveedoresDAL.obtenerFacturas(sociedad, cardcode);
        }

        public int guardarRegistroProveedor(EntradaMaterial entradaMaterial)
        {
            return proveedoresDAL.guardarRegistroProveedor(entradaMaterial);
        }
        
        public List<EntradaMaterial> obtenerPendientesRevisar()
        {
            return proveedoresDAL.obtenerPendientesRevisar();
        }

        public void actualizarEstatus(string estatus, string idRegistro)
        {
            proveedoresDAL.actualizarEstatus(estatus, idRegistro);
        }
    }
}

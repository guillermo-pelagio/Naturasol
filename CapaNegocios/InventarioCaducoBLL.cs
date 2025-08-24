using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class InventarioCaducoBLL
    {
        InventarioCaducoDAL inventarioCaducoDAL = new InventarioCaducoDAL();

        public List<InventarioCaduco> obtenerClientesKAM()
        {
            List<InventarioCaduco> listaClientes = new List<InventarioCaduco>();
            listaClientes = inventarioCaducoDAL.obtenerClientesKAM();
            return listaClientes;
        }

        public List<InventarioCaduco> obtenerArticulosCaducar(string u_CLIENTE)
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            listaArticulos = inventarioCaducoDAL.obtenerArticulosCaducar(u_CLIENTE);
            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerAjustesMes(string basesDatos, int ajustes, int tipoAjustes)
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            listaArticulos = inventarioCaducoDAL.obtenerAjustesMes(basesDatos, ajustes, tipoAjustes);
            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerArticulosSinUbicacion()
        {
            List<InventarioCaduco> listaArticulos = new List<InventarioCaduco>();
            listaArticulos = inventarioCaducoDAL.obtenerArticulosSinUbicacion();
            return listaArticulos;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducas(string u_ARTICULO, string dias)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            listaExistencias = inventarioCaducoDAL.obtenerExistenciasCaducas(u_ARTICULO, dias);
            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducadas(int tipoProducto)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            listaExistencias = inventarioCaducoDAL.obtenerExistenciasCaducadas(tipoProducto);
            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerExistenciasCaducasMPST(int dias)
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            listaExistencias = inventarioCaducoDAL.obtenerExistenciasCaducasMPST(dias);
            return listaExistencias;
        }

        //REVISADO
        public List<InventarioCaduco> obtenerPTSinMovimiento()
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            listaExistencias = inventarioCaducoDAL.obtenerPTSinMovimiento();
            return listaExistencias;
        }

        //REVISADO
        public List<InventarioCaduco> obtenerMPSinConsumo()
        {
            List<InventarioCaduco> listaExistencias = new List<InventarioCaduco>();
            listaExistencias = inventarioCaducoDAL.obtenerMPSinConsumo();
            return listaExistencias;
        }

        public List<InventarioCaduco> obtenerSemanasStock()
        {
            List<InventarioCaduco> listaSemanasStock = new List<InventarioCaduco>();
            listaSemanasStock = inventarioCaducoDAL.obtenerSemanasStock();
            return listaSemanasStock;
        }
    }
}

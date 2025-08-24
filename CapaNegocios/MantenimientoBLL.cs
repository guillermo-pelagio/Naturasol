using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class MantenimientoBLL
    {
        MantenimientoDAL mantenimientoDAL = new MantenimientoDAL();

        public List<Inventario> obtenerInventarioMantenimiento(string sociedad, string articulo, string descripcion, string almacen)
        {
            List<Inventario> inventario = new List<Inventario>();
            inventario = mantenimientoDAL.obtenerInventarioMantenimiento(sociedad, articulo, descripcion, almacen);
            return inventario;
        }
    }
}

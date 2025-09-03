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

        public int guardarLinea(LineaMtto cotizacionWEB)
        {
            return mantenimientoDAL.guardarLinea(cotizacionWEB);
        }

        public List<LineaMtto> obtenerLineas()
        {
            List<LineaMtto> lineas = new List<LineaMtto>();
            lineas = mantenimientoDAL.obtenerLineas();
            return lineas;
        }

        public int guardarMaquina(MaquinasMtto cotizacionWEB)
        {
            return mantenimientoDAL.guardarMaquina(cotizacionWEB);
        }

        public int guardarPersonal(PersonalMtto cotizacionWEB)
        {
            return mantenimientoDAL.guardarPersonal(cotizacionWEB);
        }

        public List<MaquinasMtto> obtenerMaquinas()
        {
            List<MaquinasMtto> lineas = new List<MaquinasMtto>();
            lineas = mantenimientoDAL.obtenerMaquinas();
            return lineas;
        }

        public List<PersonalMtto> obtenerPersonal()
        {
            List<PersonalMtto> lineas = new List<PersonalMtto>();
            lineas = mantenimientoDAL.obtenerPersonal();
            return lineas;
        }

        public int guardarMantenimiento(Mantenimiento mantenimientos)
        {
            return mantenimientoDAL.guardarMantenimiento(mantenimientos);
        }

        public List<Mantenimiento> obtenerMantenimiento()
        {
            List<Mantenimiento> lineas = new List<Mantenimiento>();
            lineas = mantenimientoDAL.obtenerMantenimiento();
            return lineas;
        }
    }
}

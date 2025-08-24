using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class LogisticaBLL
    {
        LogisticaDAL logisticaDAL = new LogisticaDAL();

        public bool verificarRegistro(object oc, object cliente, object planta, object upc, object codigo, object descripcion, object cantidad, object um, object fecha, object confirmacion, object observaciones, object color)
        {
            bool existeRegistro = logisticaDAL.verificarRegistro( oc,  cliente,  planta,  upc,  codigo,  descripcion,  cantidad,  um,  fecha,  confirmacion,  observaciones,  color);
            return existeRegistro;
        }

        public int insertarRegistro(object oc, object cliente, object planta, object upc, object codigo, object descripcion, object cantidad, object um, object fecha, object confirmacion, object observaciones, object color)
        {
            return logisticaDAL.insertarRegistro(oc, cliente, planta, upc, codigo, descripcion, cantidad, um, fecha, confirmacion, observaciones, color);
            
        }

        public int actualizarRegistro(object oc, object cliente, object planta, object upc, object codigo, object descripcion, object cantidad, object um, object fecha, object confirmacion, object observaciones, object color)
        {
            return logisticaDAL.actualizarRegistro(oc, cliente, planta, upc, codigo, descripcion, cantidad, um, fecha, confirmacion, observaciones, color);            
        }

        public List<CalendarioEntrega> obtenerCalendario(string oc, string fecha1, string fecha2, string codigo, string cliente, string descripcion)
        {
            return logisticaDAL.obtenerCalendario( oc,  fecha1,  fecha2,  codigo,  cliente,  descripcion);
        }
    }
}

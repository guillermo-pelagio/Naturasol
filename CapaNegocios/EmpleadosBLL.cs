using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class EmpleadosBLL
    {
        EmpleadosDAL empleadosDAL = new EmpleadosDAL();

        //METODO DE GUARDADO DE ASISTENCIA
        public int guardarAsistencia(Empleado empleado)
        {
            return empleadosDAL.guardarAsistencia(empleado);
        }

        public int guardarAusencia(Empleado empleado)
        {
            return empleadosDAL.guardarAusencia(empleado);
        }

        public List<Empleado> obtenerAsistencia()
        {
            return empleadosDAL.obtenerAsistencia();
        }

        public List<Ausencia> obtenerAusencia()
        {
            return empleadosDAL.obtenerAusencia();
        }
    }
}

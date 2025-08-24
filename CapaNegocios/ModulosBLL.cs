using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class ModulosBLL
    {
        ModulosDAL modulosDAL = new ModulosDAL();

        //METODO DE OBTENER MODULOS
        public List<Modulos> obtenerModulos()
        {
            return modulosDAL.obtenerModulos();
        }

        public List<Permisos> obtenerPermisos()
        {
            return modulosDAL.obtenerPermisos();
        }

        //METODO DE GUARDADO DEL MODULO
        public int guardarModulo(Modulos modulo)
        {
            return modulosDAL.guardarModulo(modulo);
        }

        //METODO DE ACTUALIZADO DEL MODULO
        public int actualizarModulo(Modulos modulo)
        {
            return modulosDAL.actualizarModulo(modulo);
        }

        public int guardarAcceso(Permisos permisos)
        {
            return modulosDAL.guardarAcceso(permisos);
        }

        public int eliminarAcceso(Permisos permisos)
        {
            return modulosDAL.eliminarAcceso(permisos);
        }

        //METODO DE ACTUALIZADO DEL MODULO
        public int actualizarAcceso(Permisos permisos)
        {
            return modulosDAL.actualizarAcceso(permisos);
        }

    }
}

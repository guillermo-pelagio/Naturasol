using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    //PROPIEDADES DE UN USUARIO AL CREAR SESION
    public class SesionEntradaMaterial
    {
        public string CardCode
        {
            get; set;
        }

        public string DocDate
        {
            get; set;
        }

        public string DocNum
        {
            get; set;
        }

        public string Sociedad
        {
            get; set;
        }

        public string CardName
        {
            get; set;
        }

        public string OCRelacionada
        {
            get; set;
        }

        public string Ubicacion
        {
            get; set;
        }

        public string DocTime
        {
            get; set;
        }
    }
}
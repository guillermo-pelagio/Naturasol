using SAPbobsCOM;
using System;

namespace CapaDatos
{
    public class Companias
    {
        public static Company oCompany;
        public static int retval;
        public static string retStr;

        //PARA CONECTARSE A UNA BD
        private static Company getConexionBD(String Base)
        {
            string pass = "";
            if (Base.Contains("TSSL"))
            {
                pass = "Satelite1";
            }
            else
            {
                pass = "Satelite1";
            }
            oCompany = new Company
            {
                DbServerType = BoDataServerTypes.dst_MSSQL2019,
                DbUserName = "sa",
                DbPassword = "Sql@dmin1",
                Server = "powerbisvr",
                CompanyDB = Base,
                UserName = "manager",
                Password = pass,
                language = BoSuppLangs.ln_Spanish,
                UseTrusted = false,
                AddonIdentifier = String.Empty
            };

            return oCompany;
        }

        private static Company getConexionBDContabilidad(String Base)
        {
            string pass = "";
            if (Base.Contains("TSSL"))
            {
                pass = "JMdjP2";
            }
            else
            {
                pass = "JMdjP2";
            }
            oCompany = new Company
            {
                DbServerType = BoDataServerTypes.dst_MSSQL2019,
                DbUserName = "sa",
                DbPassword = "Sql@dmin1",
                Server = "powerbisvr",
                CompanyDB = Base,
                UserName = "CCON003",
                Password = pass,
                language = BoSuppLangs.ln_Spanish,
                UseTrusted = false,
                AddonIdentifier = String.Empty
            };

            return oCompany;
        }

        public static Company conexionBD(String Base)
        {
            return getConexionBD(Base);
        }

        public static Company conexionBDContabilidad(String Base)
        {
            return getConexionBDContabilidad(Base);
        }
    }
}

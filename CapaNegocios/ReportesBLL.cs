using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocios
{
    public class ReportesBLL
    {
        ReportesDAL reportesDAL = new ReportesDAL();

        public List<Reportes> obtenerReportes(string sociedad)
        {
            List<Reportes> reporte = new List<Reportes>();
            reporte = reportesDAL.obtenerReportes(sociedad);
            return reporte;
        }

        public List<Formatos> obtenerFormatos()
        {
            List<Formatos> formato = new List<Formatos>();
            formato = reportesDAL.obtenerFormatos();
            return formato;
        }

        public List<Reportes> obtenerQuery(string idReporte)
        {
            List<Reportes> reporte = new List<Reportes>();
            reporte = reportesDAL.obtenerQuery(idReporte);
            return reporte;
        }

        public DataTable obtenerResultadosQuery(string query)
        {
            DataTable reporte = new DataTable();
            reporte = reportesDAL.obtenerResultadosQuery(query);
            return reporte;
        }
    }
}

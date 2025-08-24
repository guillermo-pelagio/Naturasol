namespace CapaNegocios
{
    public class ExcelBLL
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////NO ACTIVAR
        /*
        //CLASE ENVÍO DE REPORTE DE CORREO.
        public static string exportarReportesCorreo(System.Data.DataTable dataTable, string nombreReporte = "Tickets", string rutaArchivo = null, string color = "royalblue", int columnas = 0)
        {
            string nombreFinalArchivo;
            string filess = null;

            if (dataTable.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                    "<html> <head> <meta charset=\"UTF-8\"/>  <style type=\"text/css\"> .tabla { border-style: solid; border-width: thin; border-color: #008080; }");
                sb.Append(".encabezado{font-weight: bold; background-color:" + color +
                          "; color: white; font-size: 14px;}");
                sb.Append(".columna{text-align:center;white-space:nowrap;}</style>");
                sb.Append("</head> <body> <table class=\"tabla\"> ");

                sb.Append("<tr>");

                for (int a = 0; a < (columnas == 0 ? dataTable.Columns.Count : columnas); a++)
                {
                    sb.Append("<td class=\"encabezado\">" + dataTable.Columns[a].ColumnName + "</td>");
                }

                sb.Append("</tr>");

                for (int i = 0; i < (columnas == 0 ? dataTable.Rows.Count : 1); i++)
                {
                    sb.Append("<tr class=\"columna\">");
                    for (int j = 0; j < (columnas == 0 ? dataTable.Columns.Count : columnas); j++)
                    {
                        sb.Append("<td>" + dataTable.Rows[i][j] + "</td>");
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table> </body> </html>");

                if (nombreReporte == null)
                {
                    return sb.ToString();
                }

                nombreFinalArchivo = nombreReporte + "-" + Regex.Replace(DateTime.Now.ToShortDateString(), "/", "-") + ".xls";
                var fs = new FileStream("C:\\Users\\PROGRAMADOR\\Downloads\\" + nombreFinalArchivo, FileMode.Create, FileAccess.ReadWrite);
                var w = new StreamWriter(fs);
                w.Write(sb.ToString());
                w.Close();
                filess = rutaArchivo + nombreFinalArchivo;

                return filess;
            }

            return filess;
        }
        */
    }
}

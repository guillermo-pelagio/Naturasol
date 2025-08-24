using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;

namespace ServicioLecturaCalendario
{
    class Program
    {
        static void Main(string[] args)
        {
            OrdenesVentaBLL ordenesVentaBLL = new OrdenesVentaBLL();
            LogisticaBLL logisticaBLL = new LogisticaBLL();
            Console.WriteLine("Enter Excel path");

            string filePath = "C:\\Logistica\\CALENDARIO Y PROGRAMACION UNIDADES\\CALENDARIO\\CALENDARIO DE ENTREGAS.xlsx";
            //string filePath = "C:\\Logistica\\CALENDARIO_1.xlsx";
            string filePath2 = "C:\\Logistica\\calendario_sap.xlsx";

            try
            {
                // Check if file exists with its full path
                if (File.Exists(filePath2))
                {
                    // If file found, delete it
                    File.Delete(filePath2);
                    Console.WriteLine("Archivo eliminado");
                }
                else
                {
                    Console.WriteLine("Archivo no encontrado");
                }

                File.Copy(filePath, filePath2);
                DataTable dt = GetExcelDataTable(filePath2);

                DataTable dt2 = new DataTable();
                //foreach (DataRow dataRow in dt.Rows)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //for (int j = 0; j < dt.Rows[i].ItemArray.Length; j++)
                    //foreach (var item in dataRow.ItemArray)
                    if (dt.Rows[i]["DESCRIPCION SAP"] != "")
                    {
                        if (dt.Rows[i]["OC"] != "" && dt.Rows[i]["Fecha y Hora Cita Destino"] != "")
                        {
                            //dt2.ImportRow(dt.Rows[i]);
                            try
                            {
                                Console.WriteLine(dt.Rows[i]["OC"]);
                                Console.WriteLine(dt.Rows[i]["Fecha y Hora Cita Destino"].ToString().Split(' ')[0]);
                                Console.WriteLine(dt.Rows[i]["Fecha y Hora Cita Destino"].ToString().Split(' ')[1]);
                                Console.WriteLine(dt.Rows[i]["CONFIRMACION"]);
                                ordenesVentaBLL.actualizarFechaCitaOV2(dt.Rows[i]["OC"].ToString(), dt.Rows[i]["Fecha y Hora Cita Destino"].ToString().Split(' ')[0], dt.Rows[i]["Fecha y Hora Cita Destino"].ToString().Split(' ')[1], dt.Rows[i]["CONFIRMACION"].ToString());
                            }
                            catch (IndexOutOfRangeException ex)
                            {

                            }
                        }
                        bool existeRegistro = false;
                        if (dt.Rows[i]["DESCRIPCION SAP"].ToString().Contains("TOTAL"))
                        {
                            dt.Rows[i]["OC"] = dt.Rows[i - 1]["OC"];
                            dt.Rows[i]["CLIENTE"] = dt.Rows[i - 1]["CLIENTE"];
                        }

                        existeRegistro = logisticaBLL.verificarRegistro(dt.Rows[i]["OC"], dt.Rows[i]["CLIENTE"], dt.Rows[i]["PLANTA"], dt.Rows[i]["UPC"], dt.Rows[i]["CODIGO SAP"], dt.Rows[i]["DESCRIPCION SAP"], dt.Rows[i]["CANTIDAD SOLICITADA"], dt.Rows[i]["UM"], dt.Rows[i]["Fecha y Hora Cita Destino"], dt.Rows[i]["CONFIRMACION"], dt.Rows[i]["OBSERVACIONES"], dt.Rows[i]["COLOR"]);

                        if (!existeRegistro)
                        {
                            logisticaBLL.insertarRegistro(dt.Rows[i]["OC"], dt.Rows[i]["CLIENTE"], dt.Rows[i]["PLANTA"], dt.Rows[i]["UPC"], dt.Rows[i]["CODIGO SAP"], dt.Rows[i]["DESCRIPCION SAP"], dt.Rows[i]["CANTIDAD SOLICITADA"], dt.Rows[i]["UM"], dt.Rows[i]["Fecha y Hora Cita Destino"], dt.Rows[i]["CONFIRMACION"], dt.Rows[i]["OBSERVACIONES"], dt.Rows[i]["COLOR"]);
                        }


                        else
                        {
                            if (dt.Rows[i]["OC"] != null)
                            {
                                logisticaBLL.actualizarRegistro(dt.Rows[i]["OC"], dt.Rows[i]["CLIENTE"], dt.Rows[i]["PLANTA"], dt.Rows[i]["UPC"], dt.Rows[i]["CODIGO SAP"], dt.Rows[i]["DESCRIPCION SAP"], dt.Rows[i]["CANTIDAD SOLICITADA"], dt.Rows[i]["UM"], dt.Rows[i]["Fecha y Hora Cita Destino"], dt.Rows[i]["CONFIRMACION"], dt.Rows[i]["OBSERVACIONES"], dt.Rows[i]["COLOR"]);
                            }
                        }
                    }
                }

            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }

        public static DataTable GetExcelDataTable(string filePath2)
        {
            DataTable dt = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(filePath2))
            {
                IXLWorksheet workSheet = workBook.Worksheet("CALENDARIO");
                bool firstRow = true;
                int contador = 0;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (contador > 2)
                    {
                        if (firstRow)
                        {
                            int columnas = 0;
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                                columnas = columnas + 1;
                            }
                            firstRow = false;
                        }
                        else
                        {
                            dt.Rows.Add();
                            int i = 1;
                            foreach (IXLCell cell in row.Cells())
                            {
                                if (i < 25)
                                {
                                    dt.Rows[dt.Rows.Count - 1][i - 1] = cell.Value.ToString();
                                }
                                i++;
                            }
                        }
                    }
                    contador = contador + 1;
                }
            }

            return dt;
        }
    }
}

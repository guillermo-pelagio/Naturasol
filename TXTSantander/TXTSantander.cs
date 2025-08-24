using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TXTSantander
{
    /***********************************************************************************************EN PAUSA ***********************************************************************************************/
    public partial class TXTSantander : Form
    {
        //VARIABLES GLOBALES
#pragma warning disable CS0414 // El campo 'TXTSantander.indexBanco' está asignado pero su valor nunca se usa
        int indexBanco = 0;
#pragma warning restore CS0414 // El campo 'TXTSantander.indexBanco' está asignado pero su valor nunca se usa
#pragma warning disable CS0414 // El campo 'TXTSantander.indexUsuario' está asignado pero su valor nunca se usa
        int indexUsuario = 0;
#pragma warning restore CS0414 // El campo 'TXTSantander.indexUsuario' está asignado pero su valor nunca se usa
        int indexCompania = 0;
#pragma warning disable CS0414 // El campo 'TXTSantander.indexTipoPago' está asignado pero su valor nunca se usa
        int indexTipoPago = 0;
#pragma warning restore CS0414 // El campo 'TXTSantander.indexTipoPago' está asignado pero su valor nunca se usa
        int camino = 0;
        string mensajeTexto = "";
        int porcentaje = 0;
        int errorSinBancoEncontrado = 0;

        public TXTSantander()
        {
            InitializeComponent();
            progressBar1.Visible = false;
        }

        //BOTON DE EJECUTAR
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = String.Empty;
            lblMensaje.BackColor = Color.Empty;
            progressBar1.Visible = true;

            //REALIZA LA TAREA EN SEGUNDO PLANO
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }

        }

        //PROCESO DE SEGUNDO PLANO
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            santander();
        }

        private void santander()
        {
            //INICIALIZACION
            string claveBanco = "";
            string fileName = "";
            string titulo = "";
            string ruta = "";
            string banco = "Santander";
            string abreviaturaBanco = "SANT";
            int compania = indexCompania;

            //REPORTA PROGRESO
            backgroundWorker1.ReportProgress(10);

            //BUSCAR PAGOS BANCOMER
            PagosSantanderBLL pagosSantander = new PagosSantanderBLL();
            List<PagosSantander> listaPagos = new List<PagosSantander>();
            listaPagos = pagosSantander.obtenerPagos(indexCompania);

            ruta = banco + "\\";
            titulo = abreviaturaBanco + " SCO " + DateTime.Now.ToString("yyyyMMddhhmmss");

            if (indexCompania == 1)
            {
                //fileName = @"\\POWERBISVR\Chrono-Tesoreria\Pruebas\TXT Mielmex\" + ruta + titulo + ".txt";
                fileName = @"\\POWERBISVR\Bancos\TXT Mielmex\" + ruta + titulo + ".txt";
            }
            else if (indexCompania == 2)
            {
                //fileName = @"\\POWERBISVR\Chrono-Tesoreria\Pruebas\TXT Naturasol\" + ruta + titulo + ".txt";
                fileName = @"\\POWERBISVR\Bancos\TXT Naturasol\" + ruta + titulo + ".txt";
            }

            //HAY PAGOS, SE RECORREN
            if (listaPagos.Count > 0)
            {
                try
                {
                    //CREA EL ARCHIVO
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    backgroundWorker1.ReportProgress(30);

                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        //SE CONECTA A SAP
                        backgroundWorker1.ReportProgress(50);
                        porcentaje = 45 / listaPagos.Count;

                        int sumamonto = 0;
                        int contador = 0;

                        for (int i = 0; i < listaPagos.Count; i++)
                        {
                            mensajeTexto = "Escribiendo registros " + (i + 1) + " de " + listaPagos.Count;
                            backgroundWorker1.ReportProgress((porcentaje * (i + 1)) + 50);
                            //SE BUSCA LA CLAVE DEL BANCO
                            claveBanco = pagosSantander.buscarClaveBanco(listaPagos[i].numeroBanco);

                            if (claveBanco != "0000" && claveBanco != "0")
                            {
                                //ACTUALIZA A TABLA INTERMEDIA
                                PagosBancomerBLL.insertPagoIntermedio(Convert.ToInt32(listaPagos[i].numeroDocumento), indexCompania == 1 ? "TSSL_Mielmex" : "TSSL_Naturasol");

                                sw.Write("EEHA8581001000000000000000000000000000                                                                                                                                                                                                                                                                                                                                            ");
                                sw.Write("EEHB000000001033162110000001603000                                                                                                                                                                                                                                                                                                                                                ");

#pragma warning disable CS0219 // La variable 'lineaDemo' está asignada pero su valor nunca se usa
                                string lineaDemo = "EEHA8581001000000000000000000000000000" +
#pragma warning restore CS0219 // La variable 'lineaDemo' está asignada pero su valor nunca se usa
                                "EEDA04" +
                                 "00000000000300000" +
                                 "20240711" +
                                 "03" +
                                 "20240711" +
                                 " XAXX010101000" +
                                 "JUAN ISMAEL PEREZ VARGAS                " +
                                 "0000000000000001" +
                                 "00000000000" +
                                 "002130806467776856" +
                                 "000000                                        9 00000044" +
                                 "021" +
                                 "001" +
                                 "JUAN ISMAEL PEREZ VARGAS                          " +
                                 "COMPRAS INGENIERO ULISES                                    " +
                                 "0000000000000000000000000";

                                contador = contador + 1;
                                string formatoMonto = "00000000000000000";
                                string monto = (Convert.ToInt32(listaPagos[i].importe) * 100).ToString(formatoMonto);
                                sumamonto = sumamonto + (Convert.ToInt32(listaPagos[i].importe) * 100);
                                string fecha = DateTime.Now.ToString("yyyyMMdd") + "03" + DateTime.Now.ToString("yyyyMMdd");
                                string RFC = listaPagos[i].RFC;
                                string titular = listaPagos[i].titular;
                                string consecutivo = contador.ToString().PadLeft(16, '0');
                                string cadenafija = "00000000000";
                                string cadenafija2 = "000000                                        9 00000044";
                                string cadenafija3 = "001";
                                string cadenafija4 = "0000000000000000000000000";

                                string linea = "EEDA04" + monto + fecha + RFC + titular + consecutivo + cadenafija + listaPagos[i].cuentaBeneficiario + cadenafija2 + listaPagos[i].cuentaBeneficiario.Substring(0, 3) + cadenafija3 + titular.ToString().PadRight(50) + listaPagos[i].motivoPago + cadenafija4;
                                sw.Write(linea.Replace('Ñ', 'N').Replace('´', ' ').Replace('Ú', 'U').Replace('Ó', 'O').Replace('Í', 'I').Replace('É', 'E').Replace('Á', 'A').Replace('Ü', 'U'));
                                sw.Write(Environment.NewLine);

#pragma warning disable CS0219 // La variable 'linea2' está asignada pero su valor nunca se usa
                                string linea2 = "EEDMvanessa.martinez@naturasol.com.mx                                                                   ";
#pragma warning restore CS0219 // La variable 'linea2' está asignada pero su valor nunca se usa
                                sw.Write(linea.Replace('Ñ', 'N').Replace('´', ' ').Replace('Ú', 'U').Replace('Ó', 'O').Replace('Í', 'I').Replace('É', 'E').Replace('Á', 'A').Replace('Ü', 'U'));
                                sw.Write(Environment.NewLine);
                            }
                            else
                            {
                                errorSinBancoEncontrado = 1;
                                mensajeTexto = "Banco no encontrado para el documento " + listaPagos[i].numeroDocumento;
                            }
                        }

                        sw.Write("EETB" + contador.ToString().PadLeft(7, '0') + sumamonto.ToString().PadLeft(16, '0') + "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                           ");
                        sw.Write(Environment.NewLine);
                        sw.Write("EETA" + contador.ToString().PadLeft(7, '0') + sumamonto.ToString().PadLeft(16, '0') + "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                                                                           ");
                        sw.Write(Environment.NewLine);
                    }

                    backgroundWorker1.ReportProgress(100);
                    camino = 1;
                }
                catch (Exception Ex)
                {
                    //ERROR EN LA TAREA
                    backgroundWorker1.ReportProgress(30);
                    camino = -1;
                    mensajeTexto = Ex.ToString();
                }
            }
            else
            {
                //SIN PAGOS
                backgroundWorker1.ReportProgress(100);
                camino = 0;
            }
        }

        //EVENTO DE VISUALIZACION DE AVANCE
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //PORCENTAJE DE LA BARRA
            progressBar1.Value = e.ProgressPercentage;

            //MENSAJES PERSONALIZADOS SEGUN EL AVANCE.
            if (e.ProgressPercentage == 10)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Buscando registros...";
            }
            if (e.ProgressPercentage == 30)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Generando archivo...";
            }
            if (e.ProgressPercentage == 50)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Conectandose a SAP..." + mensajeTexto;
            }
            if (e.ProgressPercentage > 50 && e.ProgressPercentage < 100)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + mensajeTexto;
            }
            if (errorSinBancoEncontrado == 1)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + mensajeTexto;
                lblMensaje.BackColor = Color.LightYellow;
            }
            if (e.ProgressPercentage == 99)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Desconectandose de SAP...";
            }
            if (e.ProgressPercentage == 100 && camino == 0)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Sin registros encontrados";
                lblMensaje.BackColor = Color.LightBlue;
            }
            if (e.ProgressPercentage == 100 && camino == -1)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + mensajeTexto;
                lblMensaje.BackColor = Color.LightPink;
            }
            if (e.ProgressPercentage == 100 && camino == 1)
            {
                lblMensaje.Text = lblMensaje.Text + Environment.NewLine + "Archivo generado";
            }
            if (e.ProgressPercentage == 100 && errorSinBancoEncontrado == 0)
            {
                lblMensaje.BackColor = Color.LightGreen;
            }
        }
    }
}


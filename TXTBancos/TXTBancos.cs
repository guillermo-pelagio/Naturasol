using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TXTBancos
{
    public partial class TXTBancos : Form
    {
        //VARIABLES GLOBALES
        int indexBanco = 0;
        int indexUsuario = 0;
        int indexCompania = 0;
        int indexTipoPago = 0;
        int camino = 0;
        string mensajeTexto = "";
        int porcentaje = 0;
        int errorSinBancoEncontrado = 0;

        public TXTBancos()
        {
            //INICIALIZACION
            InitializeComponent();
            cmbBanco.SelectedIndex = 0;
            cmbTipoPago.SelectedIndex = 0;
            cmbSociedades.SelectedIndex = 0;
            cmbUsuarios.SelectedIndex = 0;
            progressBar1.Visible = false;
        }

        //BOTON DE EJECUTAR
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = String.Empty;
            lblMensaje.BackColor = Color.Empty;
            progressBar1.Visible = true;

            //SIN BANCO SELECCIONADO
            if (cmbBanco.SelectedIndex == 0)
            {
                MessageBox.Show("Debes de seleccionar un Banco", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //SIN TIPO DE PAGO SELECCIONADO
            if (cmbTipoPago.SelectedIndex == 0)
            {
                MessageBox.Show("Debes de seleccionar un tipo de pago", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //SIN SOCIEDAD SELECCIONADA
            else if (cmbSociedades.SelectedIndex == 0)
            {
                MessageBox.Show("Debes de seleccionar una sociedad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //SIN USUARIO SELECCIONADO
            else if (cmbUsuarios.SelectedIndex == 0)
            {
                MessageBox.Show("Debes de seleccionar un usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                indexBanco = cmbBanco.SelectedIndex;
                indexTipoPago = cmbTipoPago.SelectedIndex;
                indexUsuario = cmbUsuarios.SelectedIndex;
                indexCompania = cmbSociedades.SelectedIndex;

                //REALIZA LA TAREA EN SEGUNDO PLANO
                if (backgroundWorker1.IsBusy != true)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        //PROCESO DE SEGUNDO PLANO
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (indexBanco == 1)
            {
                bancomer();

            }
            if (indexBanco == 2)
            {
                banamex();
            }
        }

        private void bancomer()
        {
            //INICIALIZACION
            string claveBanco = "";
            string fileName = "";
            string titulo = "";
            string ruta = "";
            string banco = "Bancomer";
            string abreviaturaBanco = "BBVA";
            int compania = indexCompania;

            //REPORTA PROGRESO
            backgroundWorker1.ReportProgress(10);

            //BUSCAR PAGOS BANCOMER
            PagosBancomerBLL pagosBancomer = new PagosBancomerBLL();
            List<CapaEntidades.PagosBancomer> listaPagos = new List<CapaEntidades.PagosBancomer>();
            listaPagos = pagosBancomer.obtenerPagos(indexTipoPago, indexCompania, indexUsuario);

            if (indexTipoPago == 1)
            {
                ruta = banco + "\\";
            }
            else if (indexTipoPago == 2)
            {
                ruta = banco + " Interbancario\\";
            }

            if (indexUsuario == 1)
            {
                titulo = abreviaturaBanco + " CXP " + DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            else if (indexUsuario == 2)
            {
                titulo = abreviaturaBanco + " API " + DateTime.Now.ToString("yyyyMMddhhmmss");
            }

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

                        for (int i = 0; i < listaPagos.Count; i++)
                        {
                            mensajeTexto = "Escribiendo registros " + (i + 1) + " de " + listaPagos.Count;
                            backgroundWorker1.ReportProgress((porcentaje * (i + 1)) + 50);
                            //SE BUSCA LA CLAVE DEL BANCO
                            claveBanco = pagosBancomer.buscarClaveBanco(listaPagos[i].numeroBanco);

                            if (claveBanco != "0000" && claveBanco != "0")
                            {
                                //ACTUALIZA A TABLA INTERMEDIA
                                PagosBancomerBLL.insertPagoIntermedio(Convert.ToInt32(listaPagos[i].docEntry), indexCompania == 1 ? "TSSL_Mielmex" : "TSSL_Naturasol");

                                //SE ESRIBE EN EL ARCHIVO
                                if (indexTipoPago == 1)
                                {
                                    string linea = "";
                                    if (listaPagos[i].divisa == "MXP")
                                    {
                                        linea = listaPagos[i].cuentaBeneficiario2 + listaPagos[i].cuentaOrdenante + listaPagos[i].divisa + listaPagos[i].importe + listaPagos[i].motivoPago;
                                    }
                                    else
                                    {
                                        string ordenante = "000000000194449592";
                                        linea = listaPagos[i].cuentaBeneficiario2 + ordenante + listaPagos[i].divisa + listaPagos[i].importeFC + listaPagos[i].motivoPago;
                                    }
                                    sw.Write(linea.Replace('Ñ', 'N').Replace('´', ' ').Replace('Ú', 'U').Replace('Ó', 'O').Replace('Í', 'I').Replace('É', 'E').Replace('Á', 'A').Replace('Ü', 'U'));
                                }
                                else if (indexTipoPago == 2)
                                {
                                    string linea = "";
                                    if (listaPagos[i].divisa == "MXP")
                                    {
                                        linea = listaPagos[i].cuentaBeneficiario + listaPagos[i].cuentaOrdenante + listaPagos[i].divisa + listaPagos[i].importe + listaPagos[i].titular + listaPagos[i].tipoCuenta + claveBanco + listaPagos[i].motivoPago + listaPagos[i].numeroDocumento + listaPagos[i].disponibilidad;
                                    }
                                    else
                                    {
                                        string ordenante = "000000000194449592";
                                        linea = listaPagos[i].cuentaBeneficiario + ordenante + listaPagos[i].divisa + listaPagos[i].importeFC + listaPagos[i].titular + listaPagos[i].tipoCuenta + claveBanco + listaPagos[i].motivoPago + listaPagos[i].numeroDocumento + listaPagos[i].disponibilidad;
                                    }
                                    sw.Write(linea.Replace('Ñ', 'N').Replace('´', ' ').Replace('Ú', 'U').Replace('Ó', 'O').Replace('Í', 'I').Replace('É', 'E').Replace('Á', 'A').Replace('Ü', 'U'));
                                }

                                if (i < listaPagos.Count - 1)
                                {
                                    sw.Write(Environment.NewLine);
                                }
                            }
                            else
                            {
                                errorSinBancoEncontrado = 1;
                                mensajeTexto = "Banco no encontrado para el documento " + listaPagos[i].numeroDocumento;
                            }
                        }
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

        private void banamex()
        {
            //INICIALIZACION
            string fileName = "";
            string titulo = "";
            string ruta = "";
            string banco = "Banamex";
            string abreviaturaBanco = "BNMX";
            int compania = indexCompania;
            string fecha = "";

            //REPORTA PROGRESO
            backgroundWorker1.ReportProgress(10);

            //BUSCAR PAGOS BANCOMER
            PagosBanamexBLL pagosBanamex = new PagosBanamexBLL();
            List<CapaEntidades.PagosBanamex> listaPagos = new List<CapaEntidades.PagosBanamex>();
            listaPagos = pagosBanamex.obtenerPagos(indexCompania, indexUsuario);

            ruta = banco + "\\";

            if (indexBanco == 1)
            {
                fecha = DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            else
            {
                fecha = DateTime.Now.ToString("MMdd");
            }

            if (indexUsuario == 1)
            {
                titulo = abreviaturaBanco + "CXP" + fecha;
            }
            else if (indexUsuario == 2)
            {
                titulo = abreviaturaBanco + "API" + fecha;
            }

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

                        for (int i = 0; i < listaPagos.Count; i++)
                        {
                            mensajeTexto = "Escribiendo registros " + (i + 1) + " de " + listaPagos.Count;
                            backgroundWorker1.ReportProgress((porcentaje * (i + 1)) + 50);

                            //ACTUALIZA A TABLA INTERMEDIA
                            PagosBanamexBLL.insertPagoIntermedio(Convert.ToInt32(listaPagos[i].numeroDocumento), indexCompania == 1 ? "TSSL_Mielmex" : "TSSL_Naturasol");
                            if (listaPagos[i].sucursalCuentaDestino.Length < 5)
                            {
                                //SE ESRIBE EN EL ARCHIVO
                                string linea = listaPagos[i].transaccion + listaPagos[i].tipoCuentaOrigen + listaPagos[i].sucursalOrigen + listaPagos[i].cuentaOrigen + listaPagos[i].tipoCuentaDestino + listaPagos[i].sucursalCuentaDestino + listaPagos[i].cuentaDestino + listaPagos[i].importe + listaPagos[i].monedaTransaccion + listaPagos[i].descripcion + listaPagos[i].concepto + listaPagos[i].referencia + listaPagos[i].moneda + listaPagos[i].fechaAplicacion + listaPagos[i].horaAplicacion;
                                sw.Write(linea.Replace('Ñ', 'N').Replace('´', ' ').Replace('Ú', 'U').Replace('Ó', 'O').Replace('Í', 'I').Replace('É', 'E').Replace('Á', 'A').Replace('Ü', 'U'));

                                if (i < listaPagos.Count - 1)
                                {
                                    sw.Write(Environment.NewLine);
                                }
                            }
                        }
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

        private void cmbBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBanco.SelectedIndex == 2)
            {
                cmbTipoPago.SelectedIndex = 1;
                cmbTipoPago.Enabled = false;
            }
            else
            {
                cmbTipoPago.Enabled = true;
            }
        }

        private void TXTBancos_Load(object sender, EventArgs e)
        {

        }
    }
}
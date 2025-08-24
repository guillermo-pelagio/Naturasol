using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioEntradaIntercompania
{
    class Program
    {
        //CLASE PARA CREAR ENTRADA DE COMPRA BASADO EN UN PEDIDO DE VENTA (INTERCOMPANIA) -TAREA DE CADA MINUTO-
        static void Main(string[] args)
        {
            while (true)
            {
                bool bandera = true;
                if (DateTime.Now.Minute % 2 == 0 && bandera == true && DateTime.Now.Second > 15 && DateTime.Now.Second < 35)
                {
                    //BUSQUEDA DE ENTREGA INTERCOMPANIA
                    List<EntregaMercancia> entregas = new List<EntregaMercancia>();
                    entregas = buscarEntregaIntercompania();
                    if (entregas.Count > 0)
                    {
                        crearEntradaMercancia(entregas);
                    }
                    else
                    {
                        bandera = false;
                    }
                }
                else
                {
                    bandera = true;
                }
            }
        }

        //BUSCAR LOS PEDIDOS INTERCOMPANIA 
        private static List<EntregaMercancia> buscarEntregaIntercompania()
        {
            EntregasMercanciaBLL entregasMercanciaBLL = new EntregasMercanciaBLL();
            string[] basesDatos = { "TSSL_MIELMEX", "TSSL_DISTRIBUIDORA", "TSSL_NATURASOL", "TSSL_NOVAL" };
            List<EntregaMercancia> ordenes = new List<EntregaMercancia>();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<EntregaMercancia> orden = new List<EntregaMercancia>();
                orden = entregasMercanciaBLL.obtenerEntregasIntercompania(basesDatos[i]);



                for (int j = 0; j < orden.Count; j++)
                {
                    ordenes.Add(orden[j]);
                }
            }

            return ordenes;
        }

        //CREAR LAS ORDENES DE COMPRA
        private static void crearEntradaMercancia(List<EntregaMercancia> entregas)
        {
            string[] basesDatos = { "TSSL_MIELMEX", "TSSL_DISTRIBUIDORA", "TSSL_NATURASOL", "TSSL_NOVAL" };
            EntregasMercanciaBLL entregasMercanciaBLL = new EntregasMercanciaBLL();
            EntradasBLL entradasBLL = new EntradasBLL();
            string mensaje = "";

            //SE BUSCA LA SOCIEDAD DESTINO
            for (int i = 0; i < entregas.Count; i++)
            {
                string baseDatosVenta = "";
                //EVI
                if ((entregas[i].CardCode.Equals("1122000069")) || (entregas[i].CardCode.Equals("1120400132")))
                {
                    baseDatosVenta = basesDatos[1];
                    DIAPIBLL.conectarDIAPI(basesDatos[1]);

                    if (entregas[i].Sociedad.Contains("NATURASOL"))
                    {
                        entregas[i].CardCodeEspejo = "2110100502";
                    }
                    if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {

                    }
                    if (entregas[i].Sociedad.Contains("MIELMEX"))
                    {
                        entregas[i].CardCodeEspejo = "2110100501";
                    }
                    if (entregas[i].Sociedad.Contains("NOVAL"))
                    {
                        entregas[i].CardCodeEspejo = "2110100704";
                    }

                    if ((entregas[i].CardCode.Equals("1120400132")))
                    {
                        if (entregas[i].Sociedad.Contains("NATURASOL"))
                        {
                            entregas[i].CardCodeEspejo = "2110600167";
                        }
                        if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {

                        }
                        if (entregas[i].Sociedad.Contains("MIELMEX"))
                        {
                            entregas[i].CardCodeEspejo = "2110600166";
                        }
                        if (entregas[i].Sociedad.Contains("NOVAL"))
                        {
                            entregas[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }
                else if ((entregas[i].CardCode.Equals("1120400133")) || (entregas[i].CardCode.Equals("1122000147")))
                {
                    //NOVAL
                    baseDatosVenta = basesDatos[3];
                    DIAPIBLL.conectarDIAPI(basesDatos[3]);

                    if (entregas[i].Sociedad.Contains("NATURASOL"))
                    {
                        entregas[i].CardCodeEspejo = "2110600167";
                    }
                    if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        entregas[i].CardCodeEspejo = "2110600168";
                    }
                    if (entregas[i].Sociedad.Contains("MIELMEX"))
                    {
                        entregas[i].CardCodeEspejo = "2110600166";
                    }
                    if (entregas[i].Sociedad.Contains("NOVAL"))
                    {

                    }

                    if ((entregas[i].CardCode.Equals("1122000147")))
                    {
                        if (entregas[i].Sociedad.Contains("NATURASOL"))
                        {
                            entregas[i].CardCodeEspejo = "2110100502";
                        }
                        if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {

                        }
                        if (entregas[i].Sociedad.Contains("MIELMEX"))
                        {
                            entregas[i].CardCodeEspejo = "2110100501";
                        }
                        if (entregas[i].Sociedad.Contains("NOVAL"))
                        {
                            entregas[i].CardCodeEspejo = "2110100704";
                        }
                    }
                }
                else if ((entregas[i].CardCode.Equals("1122000081")) || (entregas[i].CardCode.Equals("1120400130")))
                {
                    //MIEL MEX
                    baseDatosVenta = basesDatos[0];
                    DIAPIBLL.conectarDIAPI(basesDatos[0]);

                    if (entregas[i].Sociedad.Contains("NATURASOL"))
                    {
                        entregas[i].CardCodeEspejo = "2110100502";
                    }
                    if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        entregas[i].CardCodeEspejo = "2110100503";
                    }
                    if (entregas[i].Sociedad.Contains("MIELMEX"))
                    {

                    }
                    if (entregas[i].Sociedad.Contains("NOVAL"))
                    {
                        entregas[i].CardCodeEspejo = "2110100704";
                    }

                    if ((entregas[i].CardCode.Equals("1120400130")))
                    {
                        if (entregas[i].Sociedad.Contains("NATURASOL"))
                        {
                            entregas[i].CardCodeEspejo = "2110600167";
                        }
                        if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {
                            entregas[i].CardCodeEspejo = "2110600168";
                        }
                        if (entregas[i].Sociedad.Contains("MIELMEX"))
                        {

                        }
                        if (entregas[i].Sociedad.Contains("NOVAL"))
                        {
                            entregas[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }
                else if ((entregas[i].CardCode.Equals("1122000020")) || (entregas[i].CardCode.Equals("1120400131")))
                {
                    //NATURASOL
                    baseDatosVenta = basesDatos[2];
                    DIAPIBLL.conectarDIAPI(basesDatos[2]);

                    if (entregas[i].Sociedad.Contains("NATURASOL"))
                    {

                    }
                    if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        entregas[i].CardCodeEspejo = "2110100503";
                    }
                    if (entregas[i].Sociedad.Contains("MIELMEX"))
                    {
                        entregas[i].CardCodeEspejo = "2110100501";
                    }
                    if (entregas[i].Sociedad.Contains("NOVAL"))
                    {
                        entregas[i].CardCodeEspejo = "2110100704";
                    }

                    if ((entregas[i].CardCode.Equals("1120400131")))
                    {
                        if (entregas[i].Sociedad.Contains("NATURASOL"))
                        {

                        }
                        if (entregas[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {
                            entregas[i].CardCodeEspejo = "2110600168";
                        }
                        if (entregas[i].Sociedad.Contains("MIELMEX"))
                        {
                            entregas[i].CardCodeEspejo = "2110600166";
                        }
                        if (entregas[i].Sociedad.Contains("NOVAL"))
                        {
                            entregas[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }

                //if (entregas[i].U_REPLICAR != "4")
                {
                    //SE CREA LA ORDEN DE COMPRA
                    mensaje = entradasBLL.crearEntrada(entregas[i], entregasMercanciaBLL.obtenerEntradasDetalleIntercompania(entregas[i].Sociedad, entregas[i]), baseDatosVenta);
                    Console.WriteLine(entregas[i].Sociedad + " " + entregas[i].DocNum + " " + mensaje);

                    DIAPIBLL.desconectarDIAPI();

                    if (mensaje.Contains("OK"))
                    {
                        List<EntradaMaterial> entradaMaterial = new List<EntradaMaterial>();
                        entradaMaterial = entradasBLL.obtenerEntradaIntercompania(entregas[i], baseDatosVenta);

                        for (int x = 0; x < entradaMaterial.Count; x++)
                        {
                            //ACTUALIZAR OV EN PRIMER SOCIEDAD
                            DIAPIBLL.conectarDIAPI(entregas[i].Sociedad);
                            entregasMercanciaBLL.actualizarEntregaIntercompania(entregas[i].DocEntry, entradaMaterial[x].DocNum, "", baseDatosVenta);
                            DIAPIBLL.desconectarDIAPI();
                        }
                    }
                    else
                    {
                        //ACTUALIZAR OC EN PRIMER SOCIEDAD CON EL ERROR
                        DIAPIBLL.conectarDIAPI(entregas[i].Sociedad);
                        entregasMercanciaBLL.actualizarEntregaIntercompania(entregas[i].DocEntry, "", mensaje, baseDatosVenta);
                        DIAPIBLL.desconectarDIAPI();
                    }
                }
            }
        }
    }
}
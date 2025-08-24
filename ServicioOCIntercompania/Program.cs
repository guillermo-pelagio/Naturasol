using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;

namespace ServicioOCIntercompania
{
    public class Program
    {
        //CLASE PARA CREAR PEDIDO DE COMPRA BASADO EN UN PEDIDO DE VENTA (INTERCOMPANIA) -TAREA DE CADA MINUTO-
        static void Main(string[] args)
        {
            while (true)
            {
                bool bandera = true;
                if (DateTime.Now.Minute % 2 == 0 && bandera == true && DateTime.Now.Second < 15)
                {
                    //BUSQUEDA DE OV INTERCOMPANIA
                    List<OrdenVenta> ordenes = new List<OrdenVenta>();
                    ordenes = buscarOVIntercompania();
                    if (ordenes.Count > 0)
                    {
                        crearOrdenesCompra(ordenes);
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
        private static List<OrdenVenta> buscarOVIntercompania()
        {
            OrdenesVentaBLL ordenesVentaBLL = new OrdenesVentaBLL();
            string[] basesDatos = { "TSSL_MIELMEX", "TSSL_DISTRIBUIDORA", "TSSL_NATURASOL", "TSSL_NOVAL" };
            List<OrdenVenta> ordenes = new List<OrdenVenta>();

            for (int i = 0; i < basesDatos.Length; i++)
            {
                List<OrdenVenta> orden = new List<OrdenVenta>();
                orden = ordenesVentaBLL.obtenerOrdenesVentaIntercompania(basesDatos[i]);



                for (int j = 0; j < orden.Count; j++)
                {
                    ordenes.Add(orden[j]);
                }
            }

            return ordenes;
        }

        //CREAR LAS ORDENES DE COMPRA
        private static void crearOrdenesCompra(List<OrdenVenta> ordenes)
        {
            string[] basesDatos = { "TSSL_MIELMEX", "TSSL_DISTRIBUIDORA", "TSSL_NATURASOL", "TSSL_NOVAL" };
            OrdenesVentaBLL ordenesVentaBLL = new OrdenesVentaBLL();
            OrdenesCompraBLL ordenesCompraBLL = new OrdenesCompraBLL();
            string mensaje = "";

            //SE BUSCA LA SOCIEDAD DESTINO
            for (int i = 0; i < ordenes.Count; i++)
            {
                string baseDatosVenta = "";
                //EVI
                if ((ordenes[i].CardCode.Equals("1122000069")) || (ordenes[i].CardCode.Equals("1120400132")))
                {
                    baseDatosVenta = basesDatos[1];
                    DIAPIBLL.conectarDIAPI(basesDatos[1]);

                    if (ordenes[i].Sociedad.Contains("NATURASOL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100502";
                    }
                    if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {

                    }
                    if (ordenes[i].Sociedad.Contains("MIELMEX"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100501";
                    }
                    if (ordenes[i].Sociedad.Contains("NOVAL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100704";
                    }

                    if ((ordenes[i].CardCode.Equals("1120400132")))
                    {
                        if (ordenes[i].Sociedad.Contains("NATURASOL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600167";
                        }
                        if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {

                        }
                        if (ordenes[i].Sociedad.Contains("MIELMEX"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600166";
                        }
                        if (ordenes[i].Sociedad.Contains("NOVAL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }
                else if ((ordenes[i].CardCode.Equals("1120400133")) || (ordenes[i].CardCode.Equals("1122000147")))
                {
                    //NOVAL
                    baseDatosVenta = basesDatos[3];
                    DIAPIBLL.conectarDIAPI(basesDatos[3]);

                    if (ordenes[i].Sociedad.Contains("NATURASOL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110600167";
                    }
                    if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        ordenes[i].CardCodeEspejo = "2110600168";
                    }
                    if (ordenes[i].Sociedad.Contains("MIELMEX"))
                    {
                        ordenes[i].CardCodeEspejo = "2110600166";
                    }
                    if (ordenes[i].Sociedad.Contains("NOVAL"))
                    {

                    }

                    if ((ordenes[i].CardCode.Equals("1122000147")))
                    {
                        if (ordenes[i].Sociedad.Contains("NATURASOL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110100502";
                        }
                        if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {

                        }
                        if (ordenes[i].Sociedad.Contains("MIELMEX"))
                        {
                            ordenes[i].CardCodeEspejo = "2110100501";
                        }
                        if (ordenes[i].Sociedad.Contains("NOVAL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110100704";
                        }
                    }
                }
                else if ((ordenes[i].CardCode.Equals("1122000081")) || (ordenes[i].CardCode.Equals("1120400130")))
                {
                    //MIEL MEX
                    baseDatosVenta = basesDatos[0];
                    DIAPIBLL.conectarDIAPI(basesDatos[0]);

                    if (ordenes[i].Sociedad.Contains("NATURASOL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100502";
                    }
                    if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100503";
                    }
                    if (ordenes[i].Sociedad.Contains("MIELMEX"))
                    {

                    }
                    if (ordenes[i].Sociedad.Contains("NOVAL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100704";
                    }

                    if ((ordenes[i].CardCode.Equals("1120400130")))
                    {
                        if (ordenes[i].Sociedad.Contains("NATURASOL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600167";
                        }
                        if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600168";
                        }
                        if (ordenes[i].Sociedad.Contains("MIELMEX"))
                        {

                        }
                        if (ordenes[i].Sociedad.Contains("NOVAL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }
                else if ((ordenes[i].CardCode.Equals("1122000020")) || (ordenes[i].CardCode.Equals("1120400131")))
                {
                    //NATURASOL
                    baseDatosVenta = basesDatos[2];
                    DIAPIBLL.conectarDIAPI(basesDatos[2]);

                    if (ordenes[i].Sociedad.Contains("NATURASOL"))
                    {

                    }
                    if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100503";
                    }
                    if (ordenes[i].Sociedad.Contains("MIELMEX"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100501";
                    }
                    if (ordenes[i].Sociedad.Contains("NOVAL"))
                    {
                        ordenes[i].CardCodeEspejo = "2110100704";
                    }

                    if ((ordenes[i].CardCode.Equals("1120400131")))
                    {
                        if (ordenes[i].Sociedad.Contains("NATURASOL"))
                        {

                        }
                        if (ordenes[i].Sociedad.Contains("DISTRIBUIDORA"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600168";
                        }
                        if (ordenes[i].Sociedad.Contains("MIELMEX"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600166";
                        }
                        if (ordenes[i].Sociedad.Contains("NOVAL"))
                        {
                            ordenes[i].CardCodeEspejo = "2110600169";
                        }
                    }
                }
                /*
                //////////////////////////////////////////////////////////////////////////////////////////////////

                */
                //SE CREA LA ORDEN DE COMPRA
                mensaje = ordenesCompraBLL.crearOC(ordenes[i], ordenesVentaBLL.obtenerOrdenesCompraDetalleIntercompania(ordenes[i].Sociedad, ordenes[i]), baseDatosVenta);
                Console.WriteLine(ordenes[i].Sociedad + " " + ordenes[i].DocNum + " " + mensaje);


                DIAPIBLL.desconectarDIAPI();

                if (mensaje.Contains("OK"))
                {
                    List<OrdenCompra> ordenCompra = new List<OrdenCompra>();
                    ordenCompra = ordenesCompraBLL.obtenerOCIntercompania(ordenes[i], baseDatosVenta);

                    for (int x = 0; x < ordenCompra.Count; x++)
                    {
                        //ACTUALIZAR OV EN PRIMER SOCIEDAD
                        DIAPIBLL.conectarDIAPI(ordenes[i].Sociedad);
                        ordenesVentaBLL.actualizarOV(ordenes[i].DocEntry, ordenCompra[x].DocNum, "");
                        DIAPIBLL.desconectarDIAPI();
                    }
                }
                else
                {
                    //ACTUALIZAR OC EN PRIMER SOCIEDAD CON EL ERROR
                    DIAPIBLL.conectarDIAPI(ordenes[i].Sociedad);
                    ordenesVentaBLL.actualizarOV(ordenes[i].DocEntry, "", mensaje);
                    DIAPIBLL.desconectarDIAPI();
                }
            }
        }
    }
}
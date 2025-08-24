using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocios
{
    public class ArticulosBLL
    {
        ArticulosDAL articulosDAL = new ArticulosDAL();

        //S1-REQUERIDO-ACTUALIZACION
        public List<Articulos> obtenerArticulosAlmacen(string sociedad, string ItemCode, string WhsCode)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            listaArticulos = articulosDAL.obtenerArticulosAlmacen(sociedad, ItemCode, WhsCode);
            return listaArticulos;
        }

        //S1-REQUERIDO-METODO PARA OBTENER LOS ARTICULOS
        public List<Articulos> obtenerArticulos(string sociedad)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            listaArticulos = articulosDAL.obtenerArticulos(sociedad);
            return listaArticulos;
        }

        //S2-REQUERIDO-METODO PARA OBTENER LOS ARTICULOS CON ITEMS EN LA LISTA DE PRECIOS CON DESCRIPCIONES DIFERENTES
        public List<Articulos> obtenerArticulosDesactualizados(string sociedad)
        {
            List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
            listaArticulosDesactualizados = articulosDAL.obtenerArticulosDesactualizados(sociedad);
            return listaArticulosDesactualizados;
        }
        //S2-REQUERIDO
        public List<Articulos> obtenerArticulosDesactualizadosPadre(string sociedad)
        {
            List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
            listaArticulosDesactualizados = articulosDAL.obtenerArticulosDesactualizadosPadre(sociedad);
            return listaArticulosDesactualizados;
        }

        //METODO PARA OBTENER LOS ARTICULOS CON ITEMS EN LA LISTA DE PRECIOS CON PRECIOS DIFERENTES A LA DEL ARTICULO MAESTRO
        public List<Articulos> obtenerArticulosPreciosDesactualizados(string sociedad)
        {
            List<Articulos> listaArticulosDesactualizados = new List<Articulos>();
            listaArticulosDesactualizados = articulosDAL.obtenerArticulosPreciosDesactualizados(sociedad);
            return listaArticulosDesactualizados;
        }

        /*
        //METODO PARA OBTENER LOS ARTICULOS EQUIVALENTES DE LAS BOM
        public List<Articulos> obtenerArticulosEquivalentesBOM(string sociedad)
        {
            List<Articulos> listaArticulosEquivalentes = new List<Articulos>();
            listaArticulosEquivalentes = articulosDAL.obtenerArticulosEquivalentesBOM(sociedad);
            return listaArticulosEquivalentes;
        }
        */

        //S2-ACTUALIZACION-METODO PARA ACTUALIZAR LOS ITEMS DE UNA LISTA DE PRECIOS
        public void actualizarDescripcionListaPrecio(Articulos articulo)
        {
            SAPbobsCOM.ProductTrees oProductTrees;
            oProductTrees = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);
            oProductTrees.GetByKey(articulo.Father);

            for (int i = 0; i < oProductTrees.Items.Count; i++)
            {
                oProductTrees.Items.SetCurrentLine(i);

                if (oProductTrees.Items.ItemCode.Equals(articulo.ItemCode))
                {
                    oProductTrees.Items.ItemName = articulo.ItemName;
                    int respuesta = oProductTrees.Update();

                    if (respuesta == 0)
                    {
                        Console.WriteLine("Articulo actualizado");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar el articulo " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
            }
        }
        //S2-ACTUALIZACION
        public void actualizarDescripcionListaPrecioPadre(Articulos articulo)
        {
            SAPbobsCOM.ProductTrees oProductTrees;
            oProductTrees = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);
            oProductTrees.GetByKey(articulo.Father);            

            for (int i = 0; i < oProductTrees.Items.Count; i++)
            {
                oProductTrees.ProductDescription = articulo.ItemName;

                int respuesta = oProductTrees.Update();

                if (respuesta == 0)
                {
                    Console.WriteLine("Articulo actualizado");
                }
                else
                {
                    Console.WriteLine("Error al actualizar el articulo " + DIAPIDAL.company.GetLastErrorDescription());
                }
            }
        }

        //S1-ACTUALIZACION
        public void actualizarAlmacenesArticulo(string articulo, string almacen)
        {
            SAPbobsCOM.Items oItems;
            oItems = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
            oItems.GetByKey(articulo);

            oItems.WhsInfo.Add();
            oItems.WhsInfo.WarehouseCode = almacen;

            int respuesta = oItems.Update();

            if (respuesta == 0)
            {
                Console.WriteLine("Articulo actualizado " + articulo + " " + almacen);
            }
            else
            {
                Console.WriteLine("Error al actualizar el articulo " + DIAPIDAL.company.GetLastErrorDescription());
            }
        }

        //METODO PARA ACTUALIZAR LOS PRECIOS DE UNA LISTA DE PRECIOS
        public void actualizarPrecioListaPrecio(Articulos articulo)
        {
            SAPbobsCOM.ProductTrees oProductTrees;
            oProductTrees = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);
            oProductTrees.GetByKey(articulo.Father);
            double totalMXN = 0;

            for (int i = 0; i < oProductTrees.Items.Count; i++)
            {
                oProductTrees.Items.SetCurrentLine(i);

                if (oProductTrees.Items.ItemCode.Equals(articulo.ItemCode))
                {
                    oProductTrees.Items.Price = Convert.ToDouble(articulo.Price);
                    oProductTrees.Items.Currency = articulo.Currency;
                    int respuesta = oProductTrees.Update();

                    if (respuesta == 0)
                    {
                        Console.WriteLine("BOM actualizada");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar la BOM " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
                Console.WriteLine(" " + oProductTrees.Items.ParentItem + " " + oProductTrees.Items.ItemCode);

                if (oProductTrees.Items.Currency.Equals("MXP"))
                {
                    Console.WriteLine(" Moneda " + oProductTrees.Items.Currency);
                    totalMXN = totalMXN + (oProductTrees.Items.Quantity * oProductTrees.Items.Price);
                    Console.WriteLine(" " + totalMXN);
                }
                else
                {
                    if (!oProductTrees.Items.Currency.Equals(""))
                    {
                        SAPbobsCOM.SBObob oTSSL_bob;
                        Console.WriteLine(" Moneda " + oProductTrees.Items.Currency);
                        oTSSL_bob = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                        double tipoCambio = oTSSL_bob.GetCurrencyRate(oProductTrees.Items.Currency, DateTime.Now.Date).Fields.Item(0).Value;
                        totalMXN = totalMXN + (oProductTrees.Items.Quantity * oProductTrees.Items.Price * tipoCambio);
                        Console.WriteLine(" " + totalMXN);
                    }
                }

                if ((i + 1) == oProductTrees.Items.Count)
                {
                    SAPbobsCOM.Items oItems;
                    oItems = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                    oItems.GetByKey(articulo.Father);

                    if (oItems.PriceList.PriceListName.Equals("Lista de Materiales"))
                    {
                        totalMXN = Math.Round(totalMXN / Convert.ToDouble(articulo.Qauntity), 4);
                        oItems.PriceList.SetCurrentLine(0);
                        Console.WriteLine("Precio calculado " + totalMXN);
                        Console.WriteLine("Precio lista 0 " + oItems.PriceList.Price);


                        if (oItems.PriceList.Price != totalMXN)
                        {
                            oItems.PriceList.Price = totalMXN;
                            int respuesta = oItems.Update();

                            if (respuesta == 0)
                            {
                                Console.WriteLine("Articulo actualizado " + articulo + " " + totalMXN);
                            }
                            else
                            {
                                Console.WriteLine("Error al actualizar el articulo " + DIAPIDAL.company.GetLastErrorDescription());
                            }
                        }
                    }
                }
            }
        }



        //METODO PARA ACTUALIZAR LOS ITEMS DE UNA LISTA DE PRECIOS
        public void actualizarCantidadBOM(Articulos articulo)
        {
            SAPbobsCOM.ProductTrees oProductTrees;
            oProductTrees = DIAPIDAL.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);
            oProductTrees.GetByKey(articulo.Father);

            for (int i = 0; i < oProductTrees.Items.Count; i++)
            {
                oProductTrees.Items.SetCurrentLine(i);

                if (oProductTrees.Items.ItemCode.Equals(articulo.ItemCode))
                {
                    oProductTrees.Items.Quantity = 0;
                    int respuesta = oProductTrees.Update();

                    if (respuesta == 0)
                    {
                        Console.WriteLine("Articulo actualizado");
                    }
                    else
                    {
                        Console.WriteLine("Error al actualizar el articulo " + DIAPIDAL.company.GetLastErrorDescription());
                    }
                }
            }
        }
    }
}

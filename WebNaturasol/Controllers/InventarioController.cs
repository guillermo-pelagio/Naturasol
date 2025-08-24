using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class InventarioController : Controller
    {
        InventarioBLL inventarioBLL = new InventarioBLL();
        ReportesBLL reportesBLL = new ReportesBLL();

        // GET: Inventario
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult consultar_inventario()
        {

            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        public ActionResult solvencia_material()
        {

            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        [HttpGet]
        public JsonResult obtener_inventario()
        {
            List<Inventario> inventario = new List<Inventario>();

            inventario = inventarioBLL.obtenerInventarioActualWeb("TSSL_NATURASOL", Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen"]), Convert.ToString(Session["lote"]));

            var jsonResult = Json(new
            {
                data = inventario
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_inventario(Inventario inventario)
        {
            Session["almacen"] = inventario.Almacén;
            Session["lote"] = inventario.Lote;
            Session["articulo"] = inventario.Artículo;
            Session["descripcion"] = inventario.Descripción;

            return "1";
        }

        [HttpGet]
        public JsonResult obtener_kardex()
        {
            List<Kardex> kardex = new List<Kardex>();

            kardex = inventarioBLL.obtenerKardexWeb(Convert.ToString(Session["sociedad"]), Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen"]), Convert.ToString(Session["lote"]), Convert.ToString(Session["fecha1"]), Convert.ToString(Session["fecha2"]));

            var jsonResult = Json(new
            {
                data = kardex
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_kardex(Kardex kardex)
        {
            Session["sociedad"] = kardex.Sociedad;
            Session["almacen"] = kardex.Almacen;
            Session["lote"] = kardex.Lote;
            Session["articulo"] = kardex.Artículo;
            Session["descripcion"] = kardex.Descripción;
            Session["fecha1"] = kardex.FechaInicio;
            Session["fecha2"] = kardex.FechaFin;

            return "1";
        }

        [HttpGet]
        public JsonResult obtener_st_ts()
        {
            List<SolicitudTrasladoDetalle> kardex = new List<SolicitudTrasladoDetalle>();
            if (Convert.ToInt32(Session["puesto"]) == 32)
            {
                kardex = inventarioBLL.obteneSTTS(Convert.ToString((Session["ubicacion"])), Convert.ToString(Session["puesto"]), Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen1"]), Convert.ToString(Session["almacen2"]), Convert.ToString(Session["fecha1"]), Convert.ToString(Session["fecha2"]));
            }
            if (Convert.ToInt32(Session["puesto"]) == 33 || Convert.ToInt32(Session["puesto"]) == 2 || Convert.ToInt32(Session["puesto"]) == 1)
            {
                kardex = inventarioBLL.obteneSTTS(Convert.ToString((Session["ubicacion"])), Convert.ToString(Session["puesto"]), Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen1"]), Convert.ToString(Session["almacen2"]), Convert.ToString(Session["fecha1"]), Convert.ToString(Session["fecha2"]));
            }
            else
            {
                kardex = inventarioBLL.obteneSTTS(Convert.ToString((Session["ubicacion"])), Convert.ToString(Session["puesto"]), Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen1"]), Convert.ToString(Session["almacen2"]), Convert.ToString(Session["fecha1"]), Convert.ToString(Session["fecha2"]));
            }

            for (int i = 0; i < kardex.Count; i++)
            {
                kardex[i].botonArticulo += "<button type='button' id='btnDetalle' class='btn btn-link' style='padding: 0.5rem; border-radius: 5px;'>" + kardex[i].ItemCodeST + "</button>";
                kardex[i].botonTransferencias += "<button type='button' id='btnDetalleTRF' class='btn btn-link' style='padding: 0.5rem; border-radius: 5px;'>" + kardex[i].DocNumST + "</button>";
                kardex[i].botonSurtidor += "<button type='button' id='btnSurtidorS' class='btn btn-link' style='padding: 0.5rem; border-radius: 5px;'>" + kardex[i].nombreS + "</button>";

                int sociedad;
                if (kardex[i].Sociedad == "Mielmex")
                {
                    sociedad = 0;
                }
                else
                {
                    sociedad = 1;
                }

                if ((kardex[i].ToWhsCodeST == "2306") || (kardex[i].ToWhsCodeST == "1406") || (kardex[i].ToWhsCodeST == "1106") || (kardex[i].ToWhsCodeST == "1606") || (kardex[i].ToWhsCodeST == "1706") || (kardex[i].ToWhsCodeST == "1806")
                    || (kardex[i].ToWhsCodeST == "1906") || (kardex[i].ToWhsCodeST == "2006") || (kardex[i].ToWhsCodeST == "2106") || (kardex[i].ToWhsCodeST == "2206"))
                {
                    if (kardex[i].status == "C")
                    {
                        kardex[i].estatusSurtido = "C";
                    }
                    if (kardex[i].status == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }
                    if (kardex[i].status == "O" && kardex[i].estatusSurtido == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }

                    if ((Convert.ToInt32(Session["puesto"]) == 100) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "O" && kardex[i].FechaMC1 == "")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        }
                        else if (kardex[i].estatusSurtido == "D")
                        {
                            if (kardex[i].FechaMC2 != "")
                            {
                                kardex[i].acciones += "";
                            }
                            else
                            {
                                kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                            }
                        }
                        else
                        {
                            if (kardex[i].estatusSurtido != "P" && kardex[i].FechaSurtir == "" && kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T" && kardex[i].FechaTraspaso == "" && kardex[i].estatusSurtido != "D")
                            {
                                //kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S" && kardex[i].FechaSurtiendo == "" && kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            //if (kardex[i].estatusSurtido != "D" && kardex[i].FechaSurtido == "")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                        }
                    }
                    else if ((Convert.ToInt32(Session["puesto"]) == 31) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "O" && kardex[i].FechaMC1 == "")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        }
                        else if (kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "X")
                        {
                            if (kardex[i].FechaMC2 != "")
                            {
                                kardex[i].acciones += "";
                            }
                            else
                            {
                                kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                            }
                        }
                        else
                        {
                            kardex[i].acciones = "";
                            /*
                            if (kardex[i].estatusSurtido != "P")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T")
                            {
                                kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                            */
                        }
                    }
                    else if ((Convert.ToInt32(Session["puesto"]) == 32) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "O" && kardex[i].FechaMC1 == "")
                        {
                            kardex[i].acciones = "";
                            //kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        }
                        else
                        {
                            if (kardex[i].estatusSurtido != "P" && kardex[i].FechaSurtir == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido != "X")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T" && kardex[i].FechaTraspaso == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido == "P" && kardex[i].estatusSurtido != "X")
                            {
                                //kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S" && kardex[i].FechaSurtiendo == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido == "P" && kardex[i].estatusSurtido != "X")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if ((kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "S") && kardex[i].estatusSurtido != "X")
                            {
                                if (kardex[i].FechaMC2 != "")
                                {
                                    kardex[i].acciones += "";
                                }
                                else
                                {
                                    kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                                    //kardex[i].acciones += "<button title='Cerrar' type='button' id='btnCerrado' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                                }
                            }
                        }
                    }

                    else if (((Convert.ToInt32(Session["puesto"]) == 33) || Convert.ToInt32(Session["puesto"]) == 2 || Convert.ToInt32(Session["puesto"]) == 1) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "X")
                        {
                            if (kardex[i].FechaMC2 != "")
                            {
                                kardex[i].acciones += "";
                            }
                            else
                            {
                                kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar2' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                            }
                        }
                        else
                        {
                            kardex[i].acciones = "";
                            /*
                            if (kardex[i].estatusSurtido != "P")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T")
                            {
                                kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                            */
                        }
                    }
                    else
                    {
                        kardex[i].acciones = "";
                    }
                }
                else if ((kardex[i].FillerST == "2306") || (kardex[i].FillerST == "1406") || (kardex[i].FillerST == "1106") || (kardex[i].FillerST == "1606") || (kardex[i].FillerST == "1706") || (kardex[i].FillerST == "1806") || (kardex[i].FillerST == "1906")
                    || (kardex[i].FillerST == "2006") || (kardex[i].FillerST == "2106") || (kardex[i].FillerST == "2206"))
                {
                    if (kardex[i].status == "C")
                    {
                        kardex[i].estatusSurtido = "C";
                    }
                    if (kardex[i].status == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }
                    if (kardex[i].status == "O" && kardex[i].estatusSurtido == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }

                    if ((Convert.ToInt32(Session["puesto"]) == 100) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "O" && kardex[i].FechaMC1 == "")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        }
                        else if (kardex[i].estatusSurtido == "D")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                        }
                        else
                        {
                            if (kardex[i].estatusSurtido != "P" && kardex[i].FechaSurtir == "" && kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T" && kardex[i].FechaTraspaso == "" && kardex[i].estatusSurtido != "D")
                            {
                                //kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S" && kardex[i].FechaSurtiendo == "" && kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            //if (kardex[i].estatusSurtido != "D" && kardex[i].FechaSurtido == "")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                        }
                    }
                    else if ((Convert.ToInt32(Session["puesto"]) == 31) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        /*if (kardex[i].estatusSurtido == "O" && kardex[i].FechaMC1 == "")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        }
                        else if (kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "X")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                        }
                        else*/
                        {
                            kardex[i].acciones = "";
                            /*
                            if (kardex[i].estatusSurtido != "P")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T")
                            {
                                kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                            */
                        }
                    }
                    else if ((Convert.ToInt32(Session["puesto"]) == 32) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "O" && kardex[i].FechaSurtir == "")
                        {
                            kardex[i].acciones = "";
                            //kardex[i].acciones += "<button title='Validar' type='button' id='btnValidar' class='btn btn-dark' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                        }
                        else
                        {
                            if (kardex[i].estatusSurtido != "P" && kardex[i].FechaSurtir == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido != "X")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T" && kardex[i].FechaTraspaso == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido == "P" && kardex[i].estatusSurtido != "X")
                            {
                                //kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S" && kardex[i].FechaSurtiendo == "" && kardex[i].estatusSurtido != "D" && kardex[i].estatusSurtido == "P" && kardex[i].estatusSurtido != "X")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if ((kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "S") && kardex[i].estatusSurtido != "X")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                                //kardex[i].acciones += "<button title='Cerrar' type='button' id='btnCerrado' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                        }
                    }

                    else if (((Convert.ToInt32(Session["puesto"]) == 33) || Convert.ToInt32(Session["puesto"]) == 2 || Convert.ToInt32(Session["puesto"]) == 1) && kardex[i].status == "O")
                    {
                        kardex[i].acciones = "";
                        if (kardex[i].estatusSurtido == "D" || kardex[i].estatusSurtido == "X")
                        {
                            kardex[i].acciones += "<button title='Validar' type='button' id='btnFinalizar2' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-circle-check'></i></button>";
                        }
                        else
                        {
                            kardex[i].acciones = "";
                            /*
                            if (kardex[i].estatusSurtido != "P")
                            {
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                                kardex[i].acciones += "<button title='Por surtir' type='button' id='btnSurtir' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "T")
                            {
                                kardex[i].acciones += "<button title='De traspaso'  type='button' id='btnTraspaso' class='btn btn-secondary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-truck'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "S")
                            {
                                kardex[i].acciones += "<button title='Surtiendo' type='button' id='btnSurtiendo' class='btn btn-outline-primary' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-spinner'></i></button>";
                            }
                            if (kardex[i].estatusSurtido != "D")
                            {
                                kardex[i].acciones += "<button title='Surtido' type='button' id='btnSurtido' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                            }
                            */
                        }
                    }
                    else
                    {
                        kardex[i].acciones = "";
                    }
                }
                else
                {
                    kardex[i].acciones = "";
                    if (kardex[i].status == "C")
                    {
                        kardex[i].estatusSurtido = "C";
                    }
                    if (kardex[i].status == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }
                    if (kardex[i].status == "O" && kardex[i].estatusSurtido == "")
                    {
                        kardex[i].estatusSurtido = "O";
                    }
                }
            }

            var jsonResult = Json(new
            {
                data = kardex
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string detalle_inventario(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<Inventario> inventarioActual = new List<Inventario>();
            inventarioActual = inventarioBLL.obtenerInventarioActualWeb(solicitudTrasladoDetalle.Sociedad, solicitudTrasladoDetalle.ItemCodeST, "", "", "");

            string select = "";
            int contador = 0;

            select = select + "<div class='form-group row'>" +
                                        "<div class='col-sm-6'>" +
                                            "<label col-form-label' style='font-weight: bold'>Artículo:</label>" +
                                            "<label col-form-label'>" + " " + solicitudTrasladoDetalle.ItemCodeST + " " + solicitudTrasladoDetalle.DscriptionST + "</label>" +
                                        "</div>" +
                                        "<div class='col-sm-6'>" +
                                            "<label col-form-label' style='font-weight: bold'>OF:</label>" +
                                            "<label col-form-label'>" + " " + solicitudTrasladoDetalle.OF + "+</label>" +
                                        "</div>" +
                                    "</div>";

            select = select + "<div class='form-group row'>" +
                                        "<div class='col-sm-6'>" +
                                            "<label col-form-label' style='font-weight: bold'>Planta:</label>" +
                                            "<label col-form-label'>" + " " + solicitudTrasladoDetalle.planta + "</label>" +
                                        "</div>" +
                                        "<div class='col-sm-6'>" +
                                            "<label col-form-label' style='font-weight: bold'>Linea:</label>" +
                                            "<label col-form-label'>" + " " + solicitudTrasladoDetalle.centroCosto + "</label>" +
                                        "</div>" +
                                    "</div>";


            select = select + "<div class='form-group row'>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Articulo</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label' style='font-weight: bold'>Cantidad</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Lote</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label' style='font-weight: bold'>Caducidad</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label' style='font-weight: bold'>Almacen</label>" +
                                        "</div>" +
                                    "</div>";

            for (int i = 0; i < inventarioActual.Count; i++)
            {
                //selectLotes = selectLotes + "<option value=" + ordenFabricacionDetalleLotes[j].DistNumber + ">" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ") </option>";


                select = select + "<div class='form-group row'>" +
                                    "<div class='col-sm-3'>";
                //"<input type='text' class='form-control' id='componenteOFEdit" + contador + "' readonly title='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "' value='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "'>" +                                        
                if (Convert.ToDateTime(inventarioActual[i].Caducidad) < DateTime.Now)
                {
                    select = select + "<label col-form-label' style='color: red; font-weight : bold'>" + inventarioActual[i].Artículo + "</label>";
                }
                else
                {
                    select = select + "<label col-form-label'>" + inventarioActual[i].Artículo + "</label>";
                }
                select = select + "</div>" +
                "<div class='col-sm-2'>";
                if (Convert.ToDateTime(inventarioActual[i].Caducidad) < DateTime.Now)
                {
                    select = select + "<label col-form-label' style='color: red; font-weight : bold'>" + inventarioActual[i].Stock + "</label>";
                }
                else
                {
                    select = select + "<label col-form-label'>" + inventarioActual[i].Stock + "</label>";
                }
                select = select + "</div>" +
                "<div class='col-sm-3'>";

                if (Convert.ToDateTime(inventarioActual[i].Caducidad) < DateTime.Now)
                {
                    select = select + "<label col-form-label' style='color: red; font-weight : bold'>" + inventarioActual[i].Lote + "</label>";
                }
                else
                {
                    select = select + "<label col-form-label'>" + inventarioActual[i].Lote + "</label>";
                }
                select = select + "</div>" +
                                    "<div class='col-sm-2'>";
                if (Convert.ToDateTime(inventarioActual[i].Caducidad) < DateTime.Now)
                {
                    select = select + "<label col-form-label' style='color: red; font-weight : bold'>" + inventarioActual[i].Caducidad + "</label>";
                }
                else
                {
                    select = select + "<label col-form-label'>" + inventarioActual[i].Caducidad + "</label>";
                }
                select = select + "</div>" +
                "<div class='col-sm-2'>";
                if (Convert.ToDateTime(inventarioActual[i].Caducidad) < DateTime.Now)
                {
                    select = select + "<label col-form-label' style='color: red; font-weight : bold'>" + inventarioActual[i].Almacén + "</label>";
                }
                else
                {
                    select = select + "<label col-form-label'>" + inventarioActual[i].Almacén + "</label>";
                }

                select = select + "</div>" +
                "</div>";
                contador = contador + 1;
            }

            return select;
        }

        [HttpPost]
        public string obtener_transferencias(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<TransferenciaStockDetalle> inventarioActual = new List<TransferenciaStockDetalle>();
            inventarioActual = inventarioBLL.obtenerTS(solicitudTrasladoDetalle.DocNumST, solicitudTrasladoDetalle.ItemCodeST);

            string select = "";
            int contador = 0;


            select = select + "<div class='form-group row'>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Folio TRF</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Cantidad</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Lote</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label' style='font-weight: bold'>Caducidad</label>" +
                                        "</div>" +
                                    "</div>";

            for (int i = 0; i < inventarioActual.Count; i++)
            {
                select = select + "<div class='form-group row'>";

                select = select + "<div class='col-sm-3'<label col-form-label'>" + inventarioActual[i].DocNum + "</label></div>";
                select = select + "<div class='col-sm-3'<label col-form-label'>" + inventarioActual[i].Quantity + "</label></div>";
                select = select + "<div class='col-sm-3'<label col-form-label'>" + inventarioActual[i].DistNumber + "</label></div>";
                select = select + "<div class='col-sm-3'<label col-form-label'>" + inventarioActual[i].ExpDate + "</label></div>";

                select = select + "</div>";
                contador = contador + 1;
            }

            return select;
        }

        [HttpPost]
        public string obtener_confirmaciones(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.obtenerConfirmaciones(solicitudTrasladoDetalle);

            string select = "";
            int contador = 0;

            select = "<div class='form-group row'>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Articulo</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Cantidad</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Lote</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Acciones</label>" +
                                        "</div>" +
                                    "</div>";

            for (int i = 0; i < solicitudTraslados.Count; i++)
            {
                //selectLotes = selectLotes + "<option value=" + ordenFabricacionDetalleLotes[j].DistNumber + ">" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ") </option>";


                select = select + "<div class='form-group row'>" +
                                    "<div class='col-sm-3'>" +
                                        //"<input type='text' class='form-control' id='componenteOFEdit" + contador + "' readonly title='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "' value='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "'>" +
                                        "<input type = 'text' class='form-control' readonly id='articuloConfirmar" + contador + "' value='" + solicitudTraslados[i].ItemCodeST + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-3'>" +
                                        "<input type = 'number' class='form-control' id='cantidadConfirmar" + contador + "' value='" + solicitudTraslados[i].QuantityST + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-3'>" +
                                        "<input type = 'text' class='form-control' readonly id='loteConfirmar" + contador + "' value='" + solicitudTraslados[i].DistNumber + "'>" +
                                    "</div>";
                /*
                "<div class='col-sm-3'>" +

                    "<input type='text' class='form-control' id='loteEdit" + contador + "' title='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")' readonly value='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")'>" +
                "</div>";
                */
                //if (((Convert.ToInt32(Session["puesto"]) < 3) || (Convert.ToInt32(Session["puesto"]) == 100)) && ordenFabricacionDetalleLotes[j].Familia == "ST")

                select = select + "<div class='col-sm-3'>" +
                                    "<button type='button' id='btnGuardarConfirmar" + contador + "' onclick='ConfirmarConsumoLinea(" + contador + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>" +
                                    "<button type='button' id='btnRechazarConfirmar" + contador + "' onclick='RechazarConsumoLinea(" + contador + ")' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>" +
                                    "<button type='button' id='btnRegresarConfirmar" + contador + "' onclick='RegresarConsumoLinea(" + contador + ")' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>" +
                                "</div>" +
                            "</div>";

                contador = contador + 1;
            }

            return select;
        }

        [HttpPost]
        public string obtener_confirmaciones_2(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.obtenerConfirmaciones2(solicitudTrasladoDetalle);

            string select = "";
            int contador = 0;

            select = "<div class='form-group row'>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Articulo</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Cantidad</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Lote</label>" +
                                        "</div>" +
                                        "<div class='col-sm-3'>" +
                                            "<label col-form-label'>Acciones</label>" +
                                        "</div>" +
                                    "</div>";

            for (int i = 0; i < solicitudTraslados.Count; i++)
            {
                //selectLotes = selectLotes + "<option value=" + ordenFabricacionDetalleLotes[j].DistNumber + ">" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ") </option>";


                select = select + "<div class='form-group row'>" +
                                    "<div class='col-sm-3'>" +
                                        //"<input type='text' class='form-control' id='componenteOFEdit" + contador + "' readonly title='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "' value='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "'>" +
                                        "<input type = 'text' class='form-control' readonly id='articuloConfirmar" + contador + "' value='" + solicitudTraslados[i].ItemCodeST + "'>" +
                                        "<input style='display:none' type = 'text' class='form-control' readonly id='idSurtidoParcial" + contador + "' value='" + solicitudTraslados[i].idSurtido + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-3'>" +
                                        "<input type = 'number' class='form-control' id='cantidadConfirmar" + contador + "' value='" + solicitudTraslados[i].QuantityST + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-3'>" +
                                        "<input type = 'text' class='form-control' readonly id='loteConfirmar" + contador + "' value='" + solicitudTraslados[i].DistNumber + "'>" +
                                    "</div>";
                /*
                "<div class='col-sm-3'>" +

                    "<input type='text' class='form-control' id='loteEdit" + contador + "' title='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")' readonly value='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")'>" +
                "</div>";
                */
                //if (((Convert.ToInt32(Session["puesto"]) < 3) || (Convert.ToInt32(Session["puesto"]) == 100)) && ordenFabricacionDetalleLotes[j].Familia == "ST")

                select = select + "<div class='col-sm-3'>" +
                                    "<button type='button' id='btnGuardarConfirmar" + contador + "' onclick='ConfirmarConsumoLinea(" + contador + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>" +
                                    "<button type='button' id='btnRechazarConfirmar" + contador + "' onclick='RechazarConsumoLinea(" + contador + ")' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>" +
                                    "<button type='button' id='btnRegresarConfirmar" + contador + "' onclick='RegresarConsumoLinea(" + contador + ")' class='btn btn-warning' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>" +
                                "</div>" +
                            "</div>";

                contador = contador + 1;
            }

            return select;
        }

        [HttpPost]
        public JsonResult actualizar_estatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            solicitudTraslado.usuarioMovimiento = Convert.ToString((Session["usuarioId"]));
            int idConsumoROVE = inventarioBLL.actualizarEstatus(solicitudTraslado);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult regresar_estatus(SolicitudTrasladoDetalle solicitudTraslado)
        {
            solicitudTraslado.usuarioMovimiento = Convert.ToString((Session["usuarioId"]));
            int idConsumoROVE = inventarioBLL.regresarEstatus(solicitudTraslado);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizar_estatus2(string DocEntry)
        {
            int idConsumoROVE = inventarioBLL.actualizarEstatus2(DocEntry, Convert.ToString((Session["usuarioId"])));

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizar_estatus3(string DocEntry)
        {
            int idConsumoROVE = inventarioBLL.actualizarEstatus3(DocEntry, Convert.ToString((Session["usuarioId"])));

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizar_estatus4(string DocEntry)
        {
            int idConsumoROVE = inventarioBLL.actualizarEstatus4(DocEntry, Convert.ToString((Session["usuarioId"])));

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizar_estatus_surtido_parcial(SolicitudTrasladoDetalle solicitudTraslado)
        {
            solicitudTraslado.usuarioMovimiento = Convert.ToString((Session["usuarioId"]));
            int idConsumoROVE = inventarioBLL.actualizarEstatusSurtidoParcial(solicitudTraslado);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardar_parcial(string DocEntry)
        {
            int idConsumoROVE = inventarioBLL.guardarSurtidoParcial(DocEntry, Convert.ToString((Session["usuarioId"])));

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string obtener_surtidores()
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.obtenerSurtidores(Convert.ToString((Session["ubicacion"])), "TSSL_NATURASOL");

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < solicitudTraslados.Count; i++)
            {
                select = select + "<option value=" + solicitudTraslados[i].idSurtidor + ">" + solicitudTraslados[i].nombreSurtidor + " </option>";
            }

            return select;
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string obtener_lotes(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.obtenerLoteST(solicitudTrasladoDetalle);

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < solicitudTraslados.Count; i++)
            {
                select = select + "<option value=" + solicitudTraslados[i].DistNumber + ">" + solicitudTraslados[i].DistNumber + " (" + solicitudTraslados[i].OnHandQty + " - " + solicitudTraslados[i].UOM + ") </option>";
            }

            return select;
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string confirmar_existencia_lote(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.confirmarExistenciaLote(solicitudTrasladoDetalle);

            if (inventarioBLL.buscarMovimientoSAP(solicitudTrasladoDetalle.idSurtido) == null)
            {
                if ((solicitudTraslados != null))
                {
                    if (solicitudTraslados.Count > 0)
                    {
                        if (solicitudTrasladoDetalle.estatusSurtido == "W")
                        {
                            string solicitudTraslados2 = inventarioBLL.crearTransferencia(solicitudTrasladoDetalle);
                            return solicitudTraslados2;
                        }
                        else
                        {
                            return "2";
                        }
                    }
                    else
                    {
                        return "-1";
                    }
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "0";
            }
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string obtener_confirmados(SolicitudTrasladoDetalle solicitudTrasladoDetalle)
        {
            List<SolicitudTrasladoDetalle> solicitudTraslados = new List<SolicitudTrasladoDetalle>();
            solicitudTraslados = inventarioBLL.obtenerParcialConfirmado(solicitudTrasladoDetalle);

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < solicitudTraslados.Count; i++)
            {
                select = select + "<option value=" + solicitudTraslados[i].DistNumber + ">" + solicitudTraslados[i].DistNumber + " (" + solicitudTraslados[i].OnHandQty + " - " + solicitudTraslados[i].UOM + ") </option>";
            }

            return select;
        }


        [HttpPost]
        public string obtener_st_ts(SolicitudTrasladoDetalle st)
        {
            Session["articulo"] = st.ItemCodeST;
            Session["descripcion"] = st.DscriptionST;
            Session["almacen1"] = st.FillerST;
            Session["almacen2"] = st.ToWhsCodeST;
            Session["fecha1"] = st.FechaInicio;
            Session["fecha2"] = st.FechaFin;

            return "1";
        }

        public ActionResult consultar_kardex()
        {

            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        //[Authorize]
        public ActionResult st_ts()
        {

            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        //[Authorize]
        public ActionResult reportes_inventario()
        {
            if ((Session["usuarioId"]) != null)
            {

                return View();

            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        [HttpGet]
        public JsonResult obtener_reportes()
        {
            List<Reportes> reporte = new List<Reportes>();
            List<Reportes> reportesInventario = new List<Reportes>();
            reporte = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reporte.Count; i++)
            {
                if (reporte[i].areaReporte == "A")
                {
                    reportesInventario.Add(reporte[i]);
                }
            }
            return Json(new
            {
                data = reportesInventario
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult obtener_excel(string idReporte)
        {
            /*
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Users\Desarrollo2\Downloads\Cotizaciones.xlsx");
            string fileName = "Cotizaciones.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            */
            DataTable table = new DataTable();
            ReportesBLL reportesBLL = new ReportesBLL();

            List<Reportes> reporte = new List<Reportes>();
            reporte = reportesBLL.obtenerQuery(idReporte);
            table = reportesBLL.obtenerResultadosQuery(reporte[0].queryReporte);


            using (XLWorkbook workbook = new XLWorkbook())
            {
                table.TableName = reporte[0].nombreReporte;
                workbook.Worksheets.Add(table);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reporte[0].nombreReporte + " " + DateTime.Now + ".xlsx");
                }
            }
        }
    }
}

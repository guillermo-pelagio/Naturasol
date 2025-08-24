using CapaEntidades;
using CapaNegocios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CalidadController : Controller
    {
        InventarioBLL inventarioBLL = new InventarioBLL();
        ReportesBLL reportesBLL = new ReportesBLL();

        // GET: Inventario
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult gestion_lotes()
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
        public ActionResult entradas_lote()
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

        //ACCION DE OBTENER LAS LIBERACIONES ROVE
        [HttpGet]
        public JsonResult obtener_lotes()
        {
            EntradasBLL entradasBLL = new EntradasBLL();
            List<EntradaMaterial> listaEntradas = new List<EntradaMaterial>();
            listaEntradas = entradasBLL.obtenerLotesDetalle(Convert.ToString(Session["articulo"]), Convert.ToString(Session["descripcion"]), Convert.ToString(Session["almacen"]), Convert.ToString(Session["lote"]));

            for (int i = 0; i < listaEntradas.Count; i++)
            {
                if ((Convert.ToInt32(Session["puesto"]) == 100))
                {
                    listaEntradas[i].acciones = "<button type='button' id='btn-resumen-lote' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa fa-book' aria-hidden='true'></i></button><button type='button' id='btnAceptar' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 60))
                {
                    listaEntradas[i].acciones = "<button type='button' id='btn-resumen-lote' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa fa-book' aria-hidden='true'></i></button><button type='button' id='btnAceptar' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 61))
                {
                    listaEntradas[i].acciones = "<button type='button' id='btn-resumen-lote' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa fa-book' aria-hidden='true'></i></button><button type='button' id='btnAceptar2' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else
                {
                    listaEntradas[i].acciones = "";
                }
            }

            string a = Newtonsoft.Json.JsonConvert.SerializeObject(listaEntradas, Formatting.Indented);
            Console.WriteLine(a); ;


            var jsonResult = Json(new
            {
                data = listaEntradas
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult obtener_entradas()
        {
            EntradasBLL entradasBLL = new EntradasBLL();
            List<EntradaMaterial> listaEntradas = new List<EntradaMaterial>();
            listaEntradas = entradasBLL.obtenerEntradasLote();

            for (int i = 0; i < listaEntradas.Count; i++)
            {
                if ((Convert.ToInt32(Session["puesto"]) == 100))
                {
                    if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "1")
                    {
                        listaEntradas[i].acciones = "<button type='button' id='btnOK' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check-double' aria-hidden='true'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    else if (listaEntradas[i].analisisMicro != "1" && listaEntradas[i].Status == "2")
                    {
                        listaEntradas[i].acciones = "<button type='button' id='btnOK' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check-double' aria-hidden='true'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    else if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "2")
                        {
                        listaEntradas[i].acciones += "<button type='button' id='btnAceptar' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 60))
                {
                    if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "1")
                    {
                        
                    }
                    else if (listaEntradas[i].analisisMicro != "1" && listaEntradas[i].Status == "2")
                    {
                        listaEntradas[i].acciones = "<button type='button' id='btnOK' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check-double' aria-hidden='true'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    else if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "2")
                    {
                        listaEntradas[i].acciones += "<button type='button' id='btnAceptar' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 65))
                {
                    if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "1")
                    {
                        listaEntradas[i].acciones = "<button type='button' id='btnOK' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check-double' aria-hidden='true'></i></button>";
                        listaEntradas[i].acciones += "<button type='button' id='btnRechazar' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    else if (listaEntradas[i].analisisMicro != "1" && listaEntradas[i].Status == "2")
                    {
                        
                    }
                    else if (listaEntradas[i].analisisMicro == "1" && listaEntradas[i].Status == "2")
                    {
                        
                    }
                }
                else
                {
                    listaEntradas[i].acciones = "";
                }
            }

            string a = Newtonsoft.Json.JsonConvert.SerializeObject(listaEntradas, Formatting.Indented);
            Console.WriteLine(a); ;


            var jsonResult = Json(new
            {
                data = listaEntradas
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public string obtener_lotes(Inventario inventario)
        {
            Session["almacen"] = inventario.Almacén;
            Session["lote"] = inventario.Lote;
            Session["articulo"] = inventario.Artículo;
            Session["descripcion"] = inventario.Descripción;

            return "1";
        }


        [HttpPost]
        public JsonResult actualizar_estatus(EntradaMaterial entradaMaterial)
        {
            EntradasBLL entradasBLL = new EntradasBLL();
            int idConsumoROVE = entradasBLL.actualizarEstatus(entradaMaterial);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizar_lote(EntradaMaterial detalleLote)
        {
            EntradasBLL entradasBLL = new EntradasBLL();
            int idCotizacion = entradasBLL.actualizar_lote(detalleLote);

            return Json(new
            {
                data = idCotizacion
            }, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        public ActionResult reportes_calidad()
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
            List<Reportes> reportes = new List<Reportes>();
            List<Reportes> reportesPlaneacion = new List<Reportes>();
            reportes = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reportes.Count; i++)
            {
                if (reportes[i].areaReporte == "D")
                {
                    reportesPlaneacion.Add(reportes[i]);
                }
            }
            return Json(new
            {
                data = reportesPlaneacion
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

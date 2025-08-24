using CapaEntidades;
using CapaNegocios;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ProduccionController : Controller
    {
        OrdenFabricacionBLL ordenFabricacionBLL = new OrdenFabricacionBLL();

        // GET: Produccion
        public ActionResult Index()
        {
            return View();
        }

        /////////////////////////////////////////////////////////////////OF
        //[Authorize]
        public ActionResult orden_fabricacion()
        {
            if (((Session["usuarioId"]) != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        //ACCION DE OBTENER LAS OF
        [HttpGet]
        public JsonResult obtener_OF()
        {
            List<OrdenFabricacion> ordenFabricacion = new List<OrdenFabricacion>();
            ordenFabricacion = ordenFabricacionBLL.obtenerOFAbiertasWeb("TSSL_NATURASOL");

            return Json(new
            {
                data = ordenFabricacion
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LOS CONSUMOS ROVE
        [HttpGet]
        public JsonResult obtener_consumos_rove()
        {
            List<ConsumosROVE> consumosROVE = new List<ConsumosROVE>();
            consumosROVE = ordenFabricacionBLL.obtenerConsumosROVE(Convert.ToString((Session["ubicacion"])), "TSSL_NATURASOL");

            for (int i = 0; i < consumosROVE.Count; i++)
            {
                int nuevoEstatus = Convert.ToInt32(consumosROVE[i].estatus) + 1;
                if ((Convert.ToInt32(Session["puesto"]) == 100) && Convert.ToInt32(consumosROVE[i].estatus) < 3 && Convert.ToInt32(consumosROVE[i].estatus) > -1)
                {
                    consumosROVE[i].acciones = "<button type='button' id='btn-validar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 1) && Convert.ToInt32(consumosROVE[i].estatus) == 0)
                {
                    consumosROVE[i].acciones = "<button type='button' id='btn-validar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 2) && Convert.ToInt32(consumosROVE[i].estatus) == 1)
                {
                    consumosROVE[i].acciones = "<button type='button' id='btn-validar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 2) && Convert.ToInt32(consumosROVE[i].estatus) == 4)
                {
                    nuevoEstatus = 2;
                    consumosROVE[i].acciones = "<button type='button' id='btn-validar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-consumo" + consumosROVE[i].idConsumoROVE + "' onclick='validarConsumo(" + consumosROVE[i].idConsumoROVE + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    consumosROVE[i].acciones += "<button type='button' class='btn btn-outline-secondary'  title='" + consumosROVE[i].Comentarios + "' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-question'></i></button>";
                }
                else
                {
                    if (Convert.ToInt32(consumosROVE[i].estatus) == 4)
                    {
                        consumosROVE[i].acciones = "<button type='button' class='btn btn-outline-secondary'  title='" + consumosROVE[i].Comentarios + "' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-question'></i></button>";
                    }
                    else
                    {
                        consumosROVE[i].acciones = "";
                    }
                }
            }

            return Json(new
            {
                data = consumosROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDADO
        [HttpPost]
        public JsonResult guardar_consumo_rove(ConsumosROVE consumoROVE)
        {
            consumoROVE.usuarioCreacion = Convert.ToString(Session["usuarioId"]);
            int idConsumoROVE = ordenFabricacionBLL.guardarConsumoROVE(consumoROVE);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult validar_consumo_rove(ConsumosROVE consumoROVE)
        {
            consumoROVE.usuarioSupervisor = Convert.ToString(Session["usuarioId"]);
            consumoROVE.usuarioAnalista = Convert.ToString(Session["usuarioId"]);
            consumoROVE.usuarioRechazo = Convert.ToString(Session["usuarioId"]);
            int idConsumoROVE = ordenFabricacionBLL.validarConsumoROVE(consumoROVE);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string confirmar_existencia_lote(ConsumosROVE consumoROVE)
        {
            List<ConsumosROVE> listaLotesROVE = ordenFabricacionBLL.confirmarExistenciaLote(consumoROVE);

            if ((listaLotesROVE != null))
            {
                if (listaLotesROVE.Count > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        public string confirmar_existencia_lote2(ConsumosROVE consumoROVE)
        {
            List<ConsumosROVE> listaLotesROVE = ordenFabricacionBLL.confirmarExistenciaLote2(consumoROVE);

            if ((listaLotesROVE != null))
            {
                if (listaLotesROVE.Count > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        //ACCION DE OBTENER LAS LIBERACIONES ROVE
        [HttpGet]
        public JsonResult obtener_liberaciones_rove()
        {
            List<LiberacionesROVE> liberacionesROVE = new List<LiberacionesROVE>();
            liberacionesROVE = ordenFabricacionBLL.obtenerLiberacionesROVE(Convert.ToString((Session["ubicacion"])), "TSSL_NATURASOL");

            for (int i = 0; i < liberacionesROVE.Count; i++)
            {

                int nuevoEstatus = Convert.ToInt32(liberacionesROVE[i].estatus) + 1;
                if ((Convert.ToInt32(Session["puesto"]) == 100) && Convert.ToInt32(liberacionesROVE[i].estatus) < 3 && Convert.ToInt32(liberacionesROVE[i].estatus) > -1)
                {
                    liberacionesROVE[i].acciones = "<button type='button' id='btn-validar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 1) && Convert.ToInt32(liberacionesROVE[i].estatus) == 0)
                {
                    liberacionesROVE[i].acciones = "<button type='button' id='btn-validar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 2) && Convert.ToInt32(liberacionesROVE[i].estatus) == 1)
                {
                    liberacionesROVE[i].acciones = "<button type='button' id='btn-validar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 2) && Convert.ToInt32(liberacionesROVE[i].estatus) == 4)
                {
                    nuevoEstatus = 2;
                    liberacionesROVE[i].acciones = "<button type='button' id='btn-validar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-liberacion" + liberacionesROVE[i].idLiberacionROVE + "' onclick='validarLiberacion(" + liberacionesROVE[i].idLiberacionROVE + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    liberacionesROVE[i].acciones += "<button type='button' class='btn btn-outline-secondary'  title='" + liberacionesROVE[i].Comentarios + "' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-question'></i></button>";
                }
                else
                {
                    if (Convert.ToInt32(liberacionesROVE[i].estatus) == 4)
                    {
                        liberacionesROVE[i].acciones = "<button type='button' class='btn btn-outline-danger'  title='" + liberacionesROVE[i].Comentarios + "' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                    else
                    {
                        liberacionesROVE[i].acciones = "";
                    }
                }
            }

            return Json(new
            {
                data = liberacionesROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDADO
        [HttpPost]
        public JsonResult guardar_liberaciones_rove(LiberacionesROVE liberacionROVE)
        {
            liberacionROVE.BatchNumber = DateTime.Now.ToShortDateString();
            liberacionROVE.usuarioCreacion = Convert.ToString(Session["usuarioId"]);
            int idLiberacionROVE = ordenFabricacionBLL.guardarLiberacionesROVE(liberacionROVE);

            return Json(new
            {
                data = idLiberacionROVE
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult guardar_liberaciones_rove2(LiberacionesROVE liberacionROVE)
        {
            //liberacionROVE.BatchNumber = DateTime.Now.ToShortDateString();
            liberacionROVE.usuarioCreacion = Convert.ToString(Session["usuarioId"]);
            int idLiberacionROVE = ordenFabricacionBLL.guardarLiberacionesROVE(liberacionROVE);

            return Json(new
            {
                data = idLiberacionROVE
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult validar_liberacion_rove(LiberacionesROVE liberacionROVE)
        {
            liberacionROVE.usuarioSupervisor = Convert.ToString(Session["usuarioId"]);
            liberacionROVE.usuarioAnalista = Convert.ToString(Session["usuarioId"]);
            liberacionROVE.usuarioRechazo = Convert.ToString(Session["usuarioId"]);

            int idConsumoROVE = ordenFabricacionBLL.validarLiberacionROVE(liberacionROVE);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LAS LIBERACIONES ROVE
        [HttpGet]
        public JsonResult obtener_paros_rove()
        {
            List<ParosROVE> parosROVE = new List<ParosROVE>();
            parosROVE = ordenFabricacionBLL.obtenerParosROVE(Convert.ToString((Session["ubicacion"])), "TSSL_NATURASOL");

            for (int i = 0; i < parosROVE.Count; i++)
            {
                int nuevoEstatus = Convert.ToInt32(parosROVE[i].estatus) + 1;
                if ((Convert.ToInt32(Session["puesto"]) == 100) && Convert.ToInt32(parosROVE[i].estatus) < 3 && Convert.ToInt32(parosROVE[i].estatus) > -1)
                {
                    if (nuevoEstatus > 1)
                    {
                        parosROVE[i].acciones = "";
                    }
                    else
                    {
                        parosROVE[i].acciones = "<button type='button' id='btn-validar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + ",-1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                    }
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 1) && Convert.ToInt32(parosROVE[i].estatus) == 0)
                {
                    parosROVE[i].acciones = "<button type='button' id='btn-validar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else if ((Convert.ToInt32(Session["puesto"]) == 2) && Convert.ToInt32(parosROVE[i].estatus) == 1)
                {
                    parosROVE[i].acciones = "<button type='button' id='btn-validar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + "," + nuevoEstatus + ")' class='btn btn-info' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-check'></i></button><button type='button' id='btn-rechazar-paro" + parosROVE[i].idParo + "' onclick='validarParo(" + parosROVE[i].idParo + ", -1 )' class='btn btn-danger' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-xmark'></i></button>";
                }
                else
                {
                    parosROVE[i].acciones = "";
                }

            }

            return Json(new
            {
                data = parosROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE GUARDADO
        [HttpPost]
        public JsonResult guardar_paros_rove(ParosROVE parosROVE)
        {
            parosROVE.usuarioCreacion = Convert.ToString(Session["usuarioId"]);
            int idParo = ordenFabricacionBLL.guardarParosROVE(parosROVE);

            return Json(new
            {
                data = idParo
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult validar_paro_rove(ParosROVE paroROVE)
        {
            paroROVE.usuarioSupervisor = Convert.ToString(Session["usuarioId"]);
            paroROVE.usuarioAnalista = Convert.ToString(Session["usuarioId"]);
            paroROVE.usuarioRechazo = Convert.ToString(Session["usuarioId"]);

            int idConsumoROVE = ordenFabricacionBLL.validarParoROVE(paroROVE);

            return Json(new
            {
                data = idConsumoROVE
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LOS CONSUMOS
        [HttpGet]
        public JsonResult obtener_consumos()
        {
            List<OrdenFabricacion> consumosROVE = new List<OrdenFabricacion>();
            consumosROVE = ordenFabricacionBLL.obtenerConsumosOFAbiertasWeb("TSSL_NATURASOL");

            return Json(new
            {
                data = consumosROVE
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult obtener_liberaciones()
        {
            List<OrdenFabricacion> liberacionesOF = new List<OrdenFabricacion>();
            liberacionesOF = ordenFabricacionBLL.obtenerLiberacionesOFAbiertasWeb("TSSL_NATURASOL");

            return Json(new
            {
                data = liberacionesOF
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string obtener_codigoParo(string areaParo)
        {
            List<ParosROVE> parosRove = new List<ParosROVE>();
            parosRove = ordenFabricacionBLL.obtenercodigoParo(areaParo);

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < parosRove.Count; i++)
            {
                select = select + "<option value=" + parosRove[i].codigoParo + ">" + parosRove[i].descripcionParo + " </option>";
            }

            return select;
        }

        //ACCION DE OBTENER LAS OF
        [HttpPost]
        public string obtener_of()
        {
            List<OrdenFabricacion> ordenFabricacion = new List<OrdenFabricacion>();
            ordenFabricacion = ordenFabricacionBLL.obtenerOFWeb(Convert.ToString((Session["ubicacion"])), "TSSL_NATURASOL");

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < ordenFabricacion.Count; i++)
            {
                select = select + "<option value=" + ordenFabricacion[i].DocNum + ">" + ordenFabricacion[i].DocNum + " </option>";
            }

            return select;
        }

        //ACCION DE OBTENER LOS COMPONENTES
        [HttpPost]
        public string obtener_componentes(OrdenFabricacion OF)
        {
            OF.Sociedad = "TSSL_NATURASOL";
            List<OrdenFabricacionDetalle> ordenFabricaciondetalle = new List<OrdenFabricacionDetalle>();
            ordenFabricaciondetalle = ordenFabricacionBLL.obtenerComponenteOFWeb(OF);

            string select = "";

            int contador = 0;


            select = "<div class='form-group row'>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>Articulo</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>U.M.</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>Consumo</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>Lote</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>Merma</label>" +
                                        "</div>" +
                                        "<div class='col-sm-2'>" +
                                            "<label col-form-label'>Guardar</label>" +
                                        "</div>" +
                                    "</div>";

            for (int i = 0; i < ordenFabricaciondetalle.Count; i++)
            {
                OF.Code = ordenFabricaciondetalle[i].ItemCode;

                List<OrdenFabricacionDetalle> ordenFabricacionDetalleLotes = new List<OrdenFabricacionDetalle>();
                ordenFabricacionDetalleLotes = ordenFabricacionBLL.obtenerLoteComponenteOFWeb(OF);
                string selectLotes = "";
                selectLotes = "<select class='custom-select' id='loteEdit" + contador + "'>";

                for (int j = 0; j < ordenFabricacionDetalleLotes.Count; j++)
                {
                    selectLotes = selectLotes + "<option value=" + ordenFabricacionDetalleLotes[j].DistNumber + ">" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + Math.Round(Convert.ToDecimal(ordenFabricacionDetalleLotes[j].OnHandQty), 4) + " - " + ordenFabricacionDetalleLotes[j].UOM + ") </option>";
                }
                selectLotes = selectLotes + "</select>";

                select = select + "<div class='form-group row'>" +
                                    "<div class='col-sm-2'>" +
                                        //"<input type='text' class='form-control' id='componenteOFEdit" + contador + "' readonly title='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "' value='" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "'>" +
                                        "<textarea rows='4'class='form-control' id='componenteOFEdit" + contador + "' readonly >" + ordenFabricaciondetalle[i].ItemCode + " - " + ordenFabricaciondetalle[i].ItemName + "</textarea>" +
                                    "</div>" +
                                    "<div class='col-sm-2'>" +
                                        "<input type = 'text' class='form-control' readonly id='uomEdits" + contador + "' value='" + ordenFabricaciondetalle[i].UOM + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-2'>" +
                                        "<input type = 'number' class='form-control' id='cantidadEdit" + contador + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-2'>" +
                                    selectLotes +
                                    //"<input type = 'text' class='form-control' id='loteEdit" + contador + "'>" +
                                    "</div>" +
                                    "<div class='col-sm-2'>" +
                                        "<input type = 'number' class='form-control' id='mermaEdit" + contador + "'>" +
                                    "</div>";
                /*
                "<div class='col-sm-3'>" +

                    "<input type='text' class='form-control' id='loteEdit" + contador + "' title='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")' readonly value='" + ordenFabricacionDetalleLotes[j].DistNumber + " (" + ordenFabricacionDetalleLotes[j].OnHandQty + " - " + ordenFabricacionDetalleLotes[j].UOM + ")'>" +
                "</div>";
                */
                //if (((Convert.ToInt32(Session["puesto"]) < 3) || (Convert.ToInt32(Session["puesto"]) == 100)) && ordenFabricacionDetalleLotes[j].Familia == "ST")
                if (((Convert.ToInt32(Session["puesto"]) < 3) || (Convert.ToInt32(Session["puesto"]) == 100)))
                {
                    select = select + "<div class='col-sm-2'>" +
                                        "<button type='button' id='btnGuardarL" + contador + "' onclick='guardarConsumoLinea(" + contador + ")' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-floppy-disk'></i></button>" +
                                    "</div>" +
                                "</div>";
                }
                else if (((Convert.ToInt32(Session["puesto"]) < 2) || (Convert.ToInt32(Session["puesto"]) == 100)))
                {
                    select = select + "<div class='col-sm-2'>" +
                                        "<button type='button' id='btnGuardarL" + contador + "' onclick='guardarConsumoLinea(" + contador + ")' class='btn btn-success' style='padding: 0.5rem; border-radius: 5px;'><i class='fa-solid fa-floppy-disk'></i></button>" +
                                    "</div>" +
                                "</div>";
                }
                else
                {
                    select = select + "<div class='col-sm-2'>" +
                                    "</div>" +
                                "</div>";
                }

                contador = contador + 1;
            }


            return select;
        }

        //ACCION DE OBTENER LOS COMPONENTES
        [HttpPost]
        public string obtener_maquinas(OrdenFabricacion OF)
        {
            OF.Sociedad = "TSSL_NATURASOL";
            List<OrdenFabricacion> ordenFabricacion = new List<OrdenFabricacion>();
            ordenFabricacion = ordenFabricacionBLL.obtenerMaquinasOFWeb(OF);

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < ordenFabricacion.Count; i++)
            {
                select = select + "<option value=" + (i + 1) + ">" + ordenFabricacion[i].U_LINEA + "</option>";
            }

            return select;
        }

        //ACCION DE OBTENER LOS COMPONENTES
        [HttpPost]
        public string obtener_capacidades(OrdenFabricacion OF)
        {
            List<OrdenFabricacionDetalle> ordenFabricaciondetalle = new List<OrdenFabricacionDetalle>();
            ordenFabricaciondetalle = ordenFabricacionBLL.obtenerCapacidades(OF.Linea, OF.DocNum);

            return ordenFabricaciondetalle[0].ItemName;
        }

        //ACCION DE OBTENER EL ARTICULO A PRODUCIR
        [HttpPost]
        public string obtener_articulo(OrdenFabricacion OF)
        {
            OF.Sociedad = "TSSL_NATURASOL";
            List<OrdenFabricacionDetalle> ordenFabricaciondetalle = new List<OrdenFabricacionDetalle>();
            ordenFabricaciondetalle = ordenFabricacionBLL.obtenerArticuloOFWeb(OF);

            return ordenFabricaciondetalle[0].ItemName;
        }


        [HttpPost]
        public string obtener_lote(OrdenFabricacion OF)
        {
            OF.Sociedad = "TSSL_NATURASOL"; ;
            List<OrdenFabricacionDetalle> ordenFabricacion = new List<OrdenFabricacionDetalle>();
            ordenFabricacion = ordenFabricacionBLL.obtenerLoteComponenteOFWeb(OF);

            string select = "<option value='0'>Selecciona una opción</option>";

            for (int i = 0; i < ordenFabricacion.Count; i++)
            {
                select = select + "<option value=" + ordenFabricacion[i].DistNumber + ">" + ordenFabricacion[i].DistNumber + " (" + ordenFabricacion[i].OnHandQty + " - " + ordenFabricacion[i].UOM + ") </option>";
            }

            return select;
        }


        [HttpPost]
        public string obtener_um(OrdenFabricacion OF)
        {
            OF.Sociedad = "TSSL_NATURASOL"; ;
            List<OrdenFabricacionDetalle> ordenFabricacion = new List<OrdenFabricacionDetalle>();
            ordenFabricacion = ordenFabricacionBLL.obtenerUMComponenteOFWeb(OF);

            string UOM = ordenFabricacion[0].UOM;

            return UOM;
        }

        //ACCION DE OBTENER LA FECHA DEL DIA DE HOY
        [HttpPost]
        public string obtener_fecha()
        {
            JulianCalendar julianCalendar = new JulianCalendar();

            return Convert.ToString(julianCalendar.GetDayOfYear(DateTime.Now)) + Convert.ToString(DateTime.Now.Year).Substring(2, 2);

        }

        //ACCION DE OBTENER LOS MOVIMIENTOS
        public ActionResult contabilizacion_stocks()
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

        /////////////////////////////////////////////////////////////////ROVE
        public ActionResult rove()
        {
            if (((Session["usuarioId"]) != null) && ((Convert.ToInt32(Session["puesto"]) == 100) || (Convert.ToInt32(Session["puesto"]) < 10)))
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }

        //ACCION DE OBTENER LOS MOVIMIENTOS DE LA OF
        [HttpGet]
        public JsonResult contabilizacion_stock_of()
        {
            List<OrdenFabricacion> ordenFabricacion = new List<OrdenFabricacion>();
            ordenFabricacion = ordenFabricacionBLL.contabilizacion_stocks("TSSL_NATURASOL", Convert.ToString(Session["OFConsultar"]), Convert.ToString(Session["OFTipoContabilizacion"]));

            return Json(new
            {
                data = ordenFabricacion
            }, JsonRequestBehavior.AllowGet);
        }

        //ACCION DE OBTENER LOS MOVIMIENTOS DE LA OF
        [HttpPost]
        public string contabilizacion_stocks_mp(OrdenFabricacion ordenFabricacion)
        {
            Session["OFConsultar"] = ordenFabricacion.DocNum;
            Session["OFTipoContabilizacion"] = "1";
            return "1";
        }

        //ACCION DE OBTENER LOS MOVIMIENTOS DE LA OF
        [HttpPost]
        public string contabilizacion_stocks_pt(OrdenFabricacion ordenFabricacion)
        {
            Session["OFConsultar"] = ordenFabricacion.DocNum;
            Session["OFTipoContabilizacion"] = "2";
            return "1";
        }

        [HttpGet]
        public JsonResult obtener_numero_paros()
        {
            List<int> numero = ordenFabricacionBLL.obtenerParos("TSSL_NATURASOL");
            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_numero_liberaciones()
        {
            List<int> numero = ordenFabricacionBLL.obtenerLiberaciones("TSSL_NATURASOL");
            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_numero_consumos()
        {
            List<int> numero = ordenFabricacionBLL.obtenerConsumos("TSSL_NATURASOL");
            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_kg_producidos()
        {
            List<string> numero = ordenFabricacionBLL.obtenerKgs("TSSL_NATURASOL");
            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_estatus_of()
        {
            List<string> OF = ordenFabricacionBLL.obtenerEstatusOF("TSSL_NATURASOL");
            return Json(new
            {
                data = OF
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_kg_06()
        {
            List<string> numero = ordenFabricacionBLL.obtenerKgs06("TSSL_NATURASOL");
            return Json(new
            {
                data = numero
            }, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        public ActionResult reportes()
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
        public ActionResult reportes_produccion()
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
        public ActionResult reportes_inventario_bi()
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
        public ActionResult reportes_variaciones_bi()
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
            ReportesBLL reportesBLL = new ReportesBLL();

            List<Reportes> reportesProduccion = new List<Reportes>();
            reporte = reportesBLL.obtenerReportes("TSSL_NATURASOL");


            for (int i = 0; i < reporte.Count; i++)
            {
                if (reporte[i].areaReporte == "P")
                {
                    reportesProduccion.Add(reporte[i]);
                }
            }

            return Json(new
            {
                data = reportesProduccion
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtener_formatos()
        {
            List<Formatos> reporte = new List<Formatos>();
            ReportesBLL reportesBLL = new ReportesBLL();

            List<Formatos> formatosProduccion = new List<Formatos>();
            formatosProduccion = reportesBLL.obtenerFormatos();

            return Json(new
            {
                data = formatosProduccion
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
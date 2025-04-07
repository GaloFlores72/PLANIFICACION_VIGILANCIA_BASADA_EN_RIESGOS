using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatosRBS;
using CapaModeloRBS;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class PlanificacionController : Controller
    {
        private static tbUsuario SesionUsuario;

        // GET: SeguridadOperacional
        public ActionResult ListasVerificacion()
        {
            List<tbListaDeVerificacion> listaVerificacion = new List<tbListaDeVerificacion>();
            listaVerificacion = CD_ListaDeVerificacion.Instancia.ObtenerListas();
            return View(listaVerificacion);
        }


        public ActionResult Crear()
        {
            
            SesionUsuario = (tbUsuario)Session["Usuario"];
            List<tbRespuestaLV> olistaRespuesta = new List<tbRespuestaLV>();


            olistaRespuesta = CD_RespuestaLV.Instancia.ObtenerRespuestaCabeceraTodos();
            ViewBag.ListaTipoServicio = SelectTipoServicio();
            ViewBag.ListaSelectOrganizacion = ToSelectListOrganizaciones();
            ViewBag.ListaUsuarios = ToSelectListaInspectores();

            return View(olistaRespuesta);
        }


        [HttpPost]
        public JsonResult GuardarEncabezadoRespuesta(tbRespuestaLV objeto)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                objeto.UsuarioCrea = SesionUsuario.CodigoUsuario;
                if (objeto.RespuestaID == 0)
                {
                    objeto.oListaDeVerificacion = CD_ListaDeVerificacion.Instancia.ObtenerListaVerificacionPorOidXml(objeto.ListaID);
                    if (objeto.oListaDeVerificacion != null)
                    {
                        objeto.NombreLista = objeto.oListaDeVerificacion.Nombre;
                        objeto.DescripcionLista = objeto.oListaDeVerificacion.Descripcion;
                        respuesta = CD_RespuestaLV.Instancia.GrabarRespuestaCabcera(objeto);
                    }
                    else
                    {
                        respuesta = false;
                        mensaje = "No puede grabar ya que no tiene datos de la Lista de Verificación";
                    }
                }
                else
                {
                    respuesta = true; // CD_Cliente.Instancia.ModificarCliente(objeto);
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }



            return Json(new { resultado = respuesta, mensajeError = mensaje }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// hay que quita
        /// </summary>
        /// <param name="listaID"></param>
        /// <returns></returns>
        public ActionResult ListaRespuestas(int listaID)
        {
            List<tbRespuestaLV> listaRespuestas = new List<tbRespuestaLV>();
            tbListaDeVerificacion olistaVerificacion = new tbListaDeVerificacion();
            olistaVerificacion = CD_ListaDeVerificacion.Instancia.ObtenerListaVerificacionPorOid(listaID);
            ViewBag.idLista = olistaVerificacion.ListaID;
            ViewBag.ListaVerificacion = olistaVerificacion.Nombre + " " + olistaVerificacion.Descripcion;
            return View(listaRespuestas);
        }

        public ActionResult Formulario(int idResp, int idListaV)
        {                       
            tbRespuesta oRespuesta = new tbRespuesta();
            int filaInsp = 0;
            string nombreInpector = string.Empty;

            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            try
            {
               
                oRespuesta = CD_Respuesta.Instancia.ObtenerRespuestaPorId(idResp);
                var listaArea = CD_Area.Instancia.ObtenerAreaPorPorOrganizacionIDXml(oRespuesta.OrganizacionID);
                foreach (var item in oRespuesta.oInpectores)
                {
                    nombreInpector = item.Nombres + " " + item.Apellidos;
                    if (filaInsp >= 1)
                    {
                        oRespuesta.NombreInpectores = oRespuesta.NombreInpectores + " - " + nombreInpector.ToUpper();
                    }
                    else
                    {
                        oRespuesta.NombreInpectores = nombreInpector.ToUpper();
                    }                    
                    filaInsp++;
                    nombreInpector = string.Empty;
                }



                ViewBag.ListaSelectOrganizacion = ToSelectListOrganizaciones();
                ViewBag.ListaUsuarios = ToSelectListaInspectores();
                ViewBag.ListaAreas = ToSelectListaArea(oRespuesta.OrganizacionID);

            }
            catch (Exception ex)
            {
                oRespuesta = null;
                throw;
            }


            return View(oRespuesta);
        }

        [HttpGet]
        public JsonResult CambiaOrientacionEstado(int detalleRespuestaId, int respuestaOrientacionId, int estadoId, string comentario)
        {
            bool resupesta = false;
            string estadoMensaje = string.Empty;
            string nombreEstado = string.Empty;
            tbOrientacionEstado oEstadoDeImplementacion = new tbOrientacionEstado();
            oEstadoDeImplementacion = CD_OrientacionEstado.Instancia.ObtieneOrientacionEstado(estadoId);
            //Actualiza tabla 
            resupesta = CD_RespuestaOrientacion.Instancia.ActulizarRespuestaOrientacion(respuestaOrientacionId, estadoId, oEstadoDeImplementacion.Color, comentario);
            if (resupesta)
            {
                //recupera RespuestaOrientacion
                var oRespuestaOrientacion = CD_RespuestaOrientacion.Instancia.ObtenerRespuestaOrientacion(detalleRespuestaId);
                if (oRespuestaOrientacion.Count == 1)
                {
                    nombreEstado = retornaEstadoCumplimientoRequisito(oEstadoDeImplementacion);
                    //Actualiza DetalleRespuesta
                    resupesta = CD_DetalleRespuestaLV.Instancia.ActualizaDetalleRespuestaEstado(detalleRespuestaId, nombreEstado, oEstadoDeImplementacion.Color);
                }
                else
                {
                    int idEstadoImpletscion = 0;
                    foreach (var item in oRespuestaOrientacion)
                    {                       
                        if (item.EstadoImplementacionID > idEstadoImpletscion)
                        {
                            idEstadoImpletscion = item.EstadoImplementacionID;
                        }
                    }
                    nombreEstado = retornaEstadoCumplimientoRequisito(oEstadoDeImplementacion);
                    //Actualiza DetalleRespuesta
                    resupesta = CD_DetalleRespuestaLV.Instancia.ActualizaDetalleRespuestaEstado(detalleRespuestaId, nombreEstado, oEstadoDeImplementacion.Color);

                }
                
                
                
                
            }
            return Json(new { resultado = resupesta, menssaje = estadoMensaje }, JsonRequestBehavior.AllowGet);
        }

        private string retornaEstadoCumplimientoRequisito(tbOrientacionEstado oEstadoDeImplementacion)
        {
            string estadoCumplimiento = string.Empty;
            if(oEstadoDeImplementacion.OrientacionEstadoID > 0)
            {
                if (oEstadoDeImplementacion.oEstadoDeImplementacion.Descripcion.Contains("Implemen"))
                {
                    estadoCumplimiento = "Satisfactorio";
                }
                else if (oEstadoDeImplementacion.oEstadoDeImplementacion.Descripcion.Contains("No aplic"))
                {
                    estadoCumplimiento = "No Aplicable";
                }
                else if (oEstadoDeImplementacion.oEstadoDeImplementacion.Descripcion.Contains("No imple"))
                {
                    estadoCumplimiento = "No Satisfactorio";
                }
                else
                {
                    estadoCumplimiento = "";
                }
            }
            else
            {
                estadoCumplimiento = "";
            }
            
            return estadoCumplimiento;
        }
        public SelectList ToSelectListOrganizaciones()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var listValores = CD_Organizacion.Instancia.ObtenerOrganizaciones();
            foreach (var item in listValores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Nombre.Trim(),
                    Value = item.OrganizacionID.ToString()
                });
            }
            var seleccion = new SelectListItem()
            {
                Value = "0",
                Text = "---SELECCIONAR..."
            };
            seleccion.Selected = true;
            list.Insert(0, seleccion);

            return new SelectList(list, "Value", "Text");
        }

        public SelectList SelectTipoServicio()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            var listValores = CD_TipoProveedorServicio.Instancia.ObtenerTipoProveedorServicio();
            foreach (var item in listValores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.DescripcionTipoProveedor.Trim(),
                    Value = item.IdTipoProveedor.ToString()
                });
            }
            var seleccion = new SelectListItem()
            {
                Value = "0",
                Text = "---SELECCIONAR..."
            };
            seleccion.Selected = true;
            list.Insert(0, seleccion);

            return new SelectList(list, "Value", "Text");
        }

        [HttpGet]
        public JsonResult ObtieneTipoProveedorServicioPorOid(int id = 0)
        {
            tbTipoProveedorServicio otipoprovser = new tbTipoProveedorServicio();
            otipoprovser = CD_TipoProveedorServicio.Instancia.ObtenerTipoProveedorServicioPorId(id);

            return Json(otipoprovser, JsonRequestBehavior.AllowGet);
        }

        public SelectList ToSelectListUsuarios()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var listValores = CD_Usuario.Instancia.ObtenerUsuarios();
            foreach (var item in listValores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Nombres.Trim(),
                    Value = item.IdUsuario.ToString()
                });
            }
            var seleccion = new SelectListItem()
            {
                Value = "0",
                Text = "---SELECCIONAR..."
            };
            seleccion.Selected = true;
            list.Insert(0, seleccion);

            return new SelectList(list, "Value", "Text");
        }

        public SelectList ToSelectListaInspectores()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var listValores = CD_Usuario.Instancia.ObtenerUsuariosInspectores();
            foreach (var item in listValores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Nombres.Trim(),
                    Value = item.IdUsuario.ToString()
                });
            }
            var seleccion = new SelectListItem()
            {
                Value = "0",
                Text = "---SELECCIONAR..."
            };
            seleccion.Selected = true;
            list.Insert(0, seleccion);

            return new SelectList(list, "Value", "Text");
        }

        public SelectList ToSelectListaArea(int OrganizacionId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var listValores = CD_Area.Instancia.ObtenerAreaPorPorOrganizacionIDXml(OrganizacionId);
            foreach (var item in listValores)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Descripcion.Trim(),
                    Value = item.AreaID.ToString()
                });
            }
            var seleccion = new SelectListItem()
            {
                Value = "0",
                Text = "---SELECCIONAR..."
            };
            seleccion.Selected = true;
            list.Insert(0, seleccion);

            return new SelectList(list, "Value", "Text");
        }

        [HttpGet]
        public JsonResult ObtieneOrganizacionPorOid(int id = 0)
        {
            tbOrganizacion organizacion = new tbOrganizacion();
            organizacion = CD_Organizacion.Instancia.ObtenerOrganizacion(id);

            return Json(organizacion, JsonRequestBehavior.AllowGet);
        }

        #region "Constataciones"
        [HttpGet]
        public JsonResult ObtenerConstantacionPorOrientacionIdBorrar(int id = 0)
        {
            List<tbConstatacion> oListConstataciones = new List<tbConstatacion>();
            try
            {
                oListConstataciones = CD_Constatacion.Instancia.ObtenerConstantacionPorOrientacionId(id);
            }
            catch 
            {
                oListConstataciones = null;
            }
            

            return Json(new { Data = oListConstataciones }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerConstantacionPorOrientacionId(int id = 0)
        {
            tbOrientacion oOrientacion = new tbOrientacion();            
            try
            {
                oOrientacion = CD_Orientacion.Instancia.ObtieneOrientacionConstatacionesPororientacionId(id);
            }
            catch
            {
                oOrientacion = null;
            }


            return Json(oOrientacion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerOrientacionPorId(int id = 0)
        {
            tbConstatacion oConstatacion = new tbConstatacion();
            try
            {
                oConstatacion = CD_Constatacion.Instancia.ObtenerContatacionPorId(id);
            }
            catch
            {
                oConstatacion = null;
            }


            return Json(oConstatacion, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarConstacion(tbConstatacion objeto)
        {
            bool respuesta = false;
            /*Nota de EstadoConstatacion
             * REGISTRADO
             * VALIDADO
             * ENVIADO
             * ANULADO
             * */
            if (Session["Usuario"] != null)
            {
                SesionUsuario = (tbUsuario)Session["Usuario"];
                if (objeto.ConstatacionID <= 0)
                {
                    objeto.UsuarioCreaID = SesionUsuario.IdUsuario;
                    objeto.EstadoConstatacion = "REGISTRADO";
                    CD_Constatacion.Instancia.RegistraConstacion(objeto);
                    respuesta = true;
                }
                else
                {
                    objeto.UsuarioModificaID = SesionUsuario.IdUsuario;
                    respuesta = CD_Constatacion.Instancia.ModificaConstacion(objeto);
                }
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarConstacion(int id)
        {
            bool respuesta = false;
            if (Session["Usuario"] != null)
            {                
                respuesta = CD_Constatacion.Instancia.EliminarConstacion(id);
               
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatosRBS;
using CapaModeloRBS;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class SeguridadOperacionalController : Controller
    {
        // GET: SeguridadOperacional
        public ActionResult ListasVerificacion()
        {
            List<tbListaDeVerificacion> listaVerificacion = new List<tbListaDeVerificacion>();
            listaVerificacion = CD_ListaDeVerificacion.Instancia.ObtenerListas();
            return View(listaVerificacion);
        }


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
            tbRespuestaLV oRespuesta = new tbRespuestaLV();
            try
            {
                if (idResp < 1)
                {
                    oRespuesta.RespuestaID = idResp;
                    oRespuesta.ListaID = idListaV;
                    oRespuesta.FechaInicio = DateTime.Now;
                    oRespuesta.FechaFin = DateTime.Now;
                    oRespuesta.oListaDeVerificacion = CD_ListaDeVerificacion.Instancia.ObtenerListaVerificacionSubTitulosPorOid(idListaV);
                    oRespuesta.oOrientacion = CD_Orientacion.Instancia.ObtenerOrientacionesPorPreguntasPorIdPregunta(idListaV);
                    ViewBag.ListaSelectOrganizacion = ToSelectListOrganizaciones();
                    ViewBag.ListaUsuarios = ToSelectListUsuarios();
                }

            }
            catch (Exception ex)
            {
                oRespuesta = null;
                throw;
            }


            return View(oRespuesta);
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

        [HttpGet]
        public JsonResult ObtieneOrganizacionPorOid(int id = 0)
        {
            tbOrganizacion organizacion = new tbOrganizacion();
            organizacion = CD_Organizacion.Instancia.ObtenerOrganizacion(id);

            return Json(organizacion, JsonRequestBehavior.AllowGet);
        }
    }
}
using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class OrientacionesController : Controller
    {
        // GET: Orientaciones
        public ActionResult CrearOrientacion()
        {
            return View();
        }
        public ActionResult CrearOrientacion2()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerOrientacion()
        {
            List<tbOrientacion> orientacion = CD_Orientacion.Instancia.ObtenerOrientacion();
            return Json(new { data = orientacion }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerOrientacionesPorIdPregunta(int PreguntaID)
        {
            List<tbOrientacion> preguntas = CD_Orientacion.Instancia.ObtenerOrientacionesPorIdPregunta(PreguntaID);
            return Json(new { data = preguntas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarOrientacion(tbOrientacion objeto)
        {
            int resultado;

            if (objeto.OrientacionID == 0)
            {
                // Registro nuevo
                resultado = CD_Orientacion.Instancia.RegistrarOrientacion(objeto);
                // resultado = 1: éxito | 2: duplicado | 0: error
            }
            else
            {
                // Modificación
                bool actualizado = CD_Orientacion.Instancia.ModificarOrientacion(objeto);
                resultado = actualizado ? 1 : 0;
            }

            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EliminarOrientacion(int id = 0)
        {
            bool respuesta = CD_Orientacion.Instancia.EliminarOrientacion(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerOrientacionPorId(int idOrientacion)
        {
            var orientacion = CD_Orientacion.Instancia.ObtenerOrientacionPorId(idOrientacion);
            return Json(new { data = orientacion }, JsonRequestBehavior.AllowGet);
        }
    }
}

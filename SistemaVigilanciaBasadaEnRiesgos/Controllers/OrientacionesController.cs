using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class OrientacionesController : Controller
    {
        // GET: Orientaciones
        public ActionResult CrearOrientacion()
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
            bool respuesta = true;

            if (objeto.OrientacionID == 0)
            {
                respuesta = CD_Orientacion.Instancia.RegistrarOrientacion(objeto);
            }
            else
            {
                respuesta = CD_Orientacion.Instancia.ModificarOrientacion(objeto);
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarOrientacion(int id = 0)
        {
            bool respuesta = CD_Orientacion.Instancia.EliminarOrientacion(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}
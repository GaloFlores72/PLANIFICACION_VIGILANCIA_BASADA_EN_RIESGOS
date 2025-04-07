using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class PreguntaController : Controller
    {
        // GET: Pregunta
        public ActionResult CrearPregunta()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerPreguntas()
        {
            List<tbPregunta> opregunta = CD_Pregunta.Instancia.ObtenerPreguntas();

            return Json(new { data = opregunta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPreguntasPorSubtitulo(int subtituloID)
        {
            List<tbPregunta> preguntas = CD_Pregunta.Instancia.ObtenerPreguntasPorSubtitulo(subtituloID);

            return Json(new { data = preguntas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPregunta(tbPregunta objeto)
        {
            bool respuesta = true;

            if (objeto.PreguntaID == 0)
            {
                respuesta = CD_Pregunta.Instancia.RegistrarPregunta(objeto);
            }
            else
            {
                respuesta = CD_Pregunta.Instancia.ModificarPregunta(objeto);
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarPregunta(int id = 0)
        {
            bool respuesta = CD_Pregunta.Instancia.EliminarPregunta(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}
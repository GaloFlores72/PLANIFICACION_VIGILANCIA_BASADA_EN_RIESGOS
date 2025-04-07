using CapaModeloRBS;
using CapaDatosRBS;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class SubtituloController : Controller
    {
        public ActionResult CrearSubtitulo()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult ObtenerSubtituloTodos()
        {
            List<tbSubtitulo> listaSub = CD_Subtitulos.Instancia.ObtenerSubtitulo();
            return Json(new { data = listaSub }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult ObtenerSubtitulosPorListaId(int ListaID)
        {
            List<tbSubtitulo> subtitulos = CD_Subtitulos.Instancia.ObtenerSubtitulosPorListaId(ListaID);
            return Json(new { data = subtitulos }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarSubtitulo(tbSubtitulo objeto)
        {
            bool respuesta = false;

                if (objeto.SubtituloID == 0) 
                {
                    respuesta = CD_Subtitulos.Instancia.RegistrarSubtitulo(objeto);
                }
                else
                {
                    respuesta = CD_Subtitulos.Instancia.ModificarSubtitulo(objeto);
                }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarSubtitulo(int id = 0)
        {
            bool respuesta = CD_Subtitulos.Instancia.EliminarSubtitulo(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}

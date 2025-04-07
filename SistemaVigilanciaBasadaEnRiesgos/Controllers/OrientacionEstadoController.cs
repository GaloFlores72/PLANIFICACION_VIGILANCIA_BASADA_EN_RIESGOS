using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class OrientacionEstadoController : Controller
    {
        // GET: OrientacionEstado
        public ActionResult CrearOrientacionEstado()
        {
            return View();
        }
        public JsonResult ObtenerOrientacionEstadosTodos()
        {
            List<tbOrientacionEstado> orientacionEstado = CD_OrientacionEstado.Instancia.ObtenerOrientacionEstadosTodos();

            return Json(new { data = orientacionEstado }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerOrientacionEstadoPorOrientacionID(int OrientacionID)
        {
            List<tbOrientacionEstado> orientacionEstado = CD_OrientacionEstado.Instancia.ObtenerOrientacionEstadoPorOrientacionID(OrientacionID);

            return Json(new { data = orientacionEstado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarOrientacionEstado(tbOrientacionEstado objeto)
        {

            bool respuesta = true;

            if (objeto.OrientacionEstadoID == 0)
            {
                respuesta = CD_OrientacionEstado.Instancia.RegistrarOrientacionEstado(objeto);
            }
            else
            {
                respuesta = CD_OrientacionEstado.Instancia.ModificarOrientacionEstado(objeto);
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


    }
}
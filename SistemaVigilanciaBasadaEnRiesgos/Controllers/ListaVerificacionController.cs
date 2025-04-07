using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class ListaVerificacionController : Controller
    {
        // GET: ListaVerificacion
        public ActionResult CrearListaVerificacion()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerListaVerificacionTodos()
        {
            List<tbListaDeVerificacion> olista = CD_ListaDeVerificacion.Instancia.ObtenerListas();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarListaVerificacion(tbListaDeVerificacion objeto)
        {
            bool respuesta = false;
            string usuarioAutenticado = Session["CodigoUsuario"] != null ? Session["CodigoUsuario"].ToString() : "Sistema";

            if (objeto.ListaID == 0)
            {
                objeto.UsuarioCrea = usuarioAutenticado;
                objeto.FechaCreacion = DateTime.Now;
                respuesta = CD_ListaDeVerificacion.Instancia.RegistrarListaVerificacion(objeto);
            }
            else
            {
                objeto.UsuarioModifica = usuarioAutenticado;
                objeto.FechaModifica = DateTime.Now;
                respuesta = CD_ListaDeVerificacion.Instancia.ModificarListaVerificacion(objeto);
            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult EliminarListaVerificacion(int id = 0)
        {
            bool respuesta = CD_ListaDeVerificacion.Instancia.EliminarListaVerificacion(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerListaVerificacionPorProveedor(int IdTipoProveedor)
        {
            List<tbListaDeVerificacion> olista = CD_ListaDeVerificacion.Instancia.ObtenerListas()
                .Where(l => l.IdTipoProveedorServicio == IdTipoProveedor)
                .ToList();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

    }
}
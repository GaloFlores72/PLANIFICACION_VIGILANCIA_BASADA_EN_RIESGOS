using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class TipoProveedorServicioController : Controller
    {
        // GET: TipoProveedorServicio
        public ActionResult CrearTipoProveedor()
        {
            return View();
        }

        // Obtener lista de tipos de proveedores
        [HttpGet]
        public JsonResult ObtenerTipoProveedorServicio()
        {
            List<tbTipoProveedorServicio> lista = CD_TipoProveedorServicio.Instancia.ObtenerTipoProveedorServicio();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // Guardar o actualizar un tipo de proveedor
        [HttpPost]
        public JsonResult Guardar(tbTipoProveedorServicio objeto)
        {
            bool respuesta = false;
            string usuarioAutenticado = Session["CodigoUsuario"] != null ? Session["CodigoUsuario"].ToString() : "Sistema";

            if (objeto.IdTipoProveedor == 0)
            {
                objeto.UsuarioCrea = usuarioAutenticado;
                objeto.FechaCrea = DateTime.Now;
                respuesta = CD_TipoProveedorServicio.Instancia.RegistrarTipoProveedorServicio(objeto);
            }
            else
            {
                objeto.UsuarioModifica = usuarioAutenticado;
                objeto.FechaModifica = DateTime.Now;
                respuesta = CD_TipoProveedorServicio.Instancia.ModificarTipoProveedorServicio(objeto);
            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        // Eliminar un tipo de proveedor
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            bool respuesta = false;
            string usuarioAutenticado = Session["CodigoUsuario"] != null ? Session["CodigoUsuario"].ToString() : "Sistema";

            respuesta = CD_TipoProveedorServicio.Instancia.EliminarTipoProveedorServicio(id, usuarioAutenticado);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}
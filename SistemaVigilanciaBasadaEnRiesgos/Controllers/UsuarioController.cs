using CapaDatosRBS;
using CapaModeloRBS;
using SistemaVigilanciaBasadaEnRiesgos.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Crear()
        {
            return View();
        }

        public JsonResult Obtener()
        {
            List<tbUsuario> oListaUsuario = CD_Usuario.Instancia.ObtenerUsuarios();
            return Json(new { data = oListaUsuario }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(tbUsuario objeto)
        {
            bool respuesta = false;

            if (objeto.IdUsuario == 0)
            {
                objeto.Clave = Encriptar.GetSHA256(objeto.Clave);

                respuesta = CD_Usuario.Instancia.RegistrarUsuario(objeto);
            }
            else
            {
                respuesta = CD_Usuario.Instancia.ModificarUsuario(objeto);
            }


            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Eliminar(int id = 0)
        {
            bool respuesta = CD_Usuario.Instancia.EliminarUsuario(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}
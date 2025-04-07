using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult CrearRol()
        {            
            return View();
        }

       
        [HttpGet]
        public JsonResult ObtenerRol()
        {
            List<tbRol> olista = CD_Rol.Instancia.ObtenerRol();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(tbRol objeto)
        {
            bool respuesta = false;

            if (objeto.IdRol == 0)
            {

                respuesta = CD_Rol.Instancia.RegistrarRol(objeto);
            }
            else
            {
                respuesta = CD_Rol.Instancia.ModificarRol(objeto);
            }


            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Eliminar(int id = 0)
        {
            bool respuesta = CD_Rol.Instancia.EliminarRol(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}
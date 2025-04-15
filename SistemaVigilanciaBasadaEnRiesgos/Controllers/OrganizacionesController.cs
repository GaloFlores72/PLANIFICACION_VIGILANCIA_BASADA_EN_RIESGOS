using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class OrganizacionesController : Controller
    {
        // GET: Organizaciones
        public ActionResult CrearOrganizacion()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerOrganizaciones()
        {
            var organizaciones = CD_Organizacion.Instancia.ObtenerOrganizaciones();

            var resultado = organizaciones?.Select(o => new
            {
                o.OrganizacionID,
                o.Nombre,
                o.Direccion,
                o.Correo,
                o.Telefono
            }).ToList();

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerOrganizacionPorId(int idOrganizacion)
        {
            var organizacion = CD_Organizacion.Instancia.ObtenerOrganizacion(idOrganizacion);
            return Json(new { data = organizacion }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarOrganizacion(int id)
        {
            int resultado = CD_Organizacion.Instancia.EliminarOrganizacion(id);
            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarOrganizacion(tbOrganizacion objeto)
        {
            int resultado = 0;

            if (objeto.OrganizacionID == 0)
            {
                resultado = CD_Organizacion.Instancia.RegistrarOrganizacion(objeto);
            }
            else
            {
                resultado = CD_Organizacion.Instancia.ModificarOrganizacion(objeto);
            }

            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }
    }
}

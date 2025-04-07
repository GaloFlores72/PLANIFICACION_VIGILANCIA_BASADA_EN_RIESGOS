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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Codigo, string clave)
        {

            tbUsuario ousuario = CD_Usuario.Instancia.ObtenerUsuarios().Where(u => u.CodigoUsuario.ToUpper() == Codigo.ToUpper() && u.Clave == Encriptar.GetSHA256(clave)).FirstOrDefault();

            if (ousuario == null)
            {
                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
            }

            tbUsuario rptUsuario = CD_Usuario.Instancia.ObtenerDetalleUsuario(ousuario.IdUsuario);
            Session["MenuMaster"] = rptUsuario;
            Session["Usuario"] = ousuario;
            Session["name"] = ousuario.Nombres + " " +ousuario.Apellidos ;
            Session["correo"] = ousuario.Correo;

            return RedirectToAction("Index", "Home");
        }
    }
}
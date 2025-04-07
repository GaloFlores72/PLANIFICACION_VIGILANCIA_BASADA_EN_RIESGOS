using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Superset()
        {
            return Redirect("http://172.20.18.139:8080/login/");
          
        }
    }
}
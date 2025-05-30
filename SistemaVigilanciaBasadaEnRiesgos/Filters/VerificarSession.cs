﻿using CapaModeloRBS;
using SistemaVigilanciaBasadaEnRiesgos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Filters
{
    public class VerificarSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            tbUsuario oUsuario = (tbUsuario)HttpContext.Current.Session["Usuario"];

            if (oUsuario == null)
            {

                if (filterContext.Controller is LoginController == false)
                {
                    filterContext.HttpContext.Response.Redirect("~/Login/Index");
                }
            }
            else
            {

                if (filterContext.Controller is LoginController == true)
                {
                    filterContext.HttpContext.Response.Redirect("~/Home/Index");
                }
            }


            base.OnActionExecuting(filterContext);
        }
    }
}
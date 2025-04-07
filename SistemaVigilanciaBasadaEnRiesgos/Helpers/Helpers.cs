using CapaDatosRBS;
using CapaModeloRBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SistemaVigilanciaBasadaEnRiesgos.Helpers
{
    public static class Helpers
    {
        public static MvcHtmlString ActionLinkAllow(this HtmlHelper helper)
        {

            StringBuilder sb = new StringBuilder();
            string urlAction = string.Empty;
            if (HttpContext.Current.Session["Usuario"] != null)
            {

                tbUsuario oUsuario = (tbUsuario)HttpContext.Current.Session["Usuario"];

                tbUsuario rptUsuario = CD_Usuario.Instancia.ObtenerDetalleUsuario(oUsuario.IdUsuario);


                foreach (tbMenu item in rptUsuario.oListaMenu)
                {
                    //sb.AppendLine("<li class='nav-item dropdown'>");
                    sb.AppendLine("<li class='nav-item dropdown'>");
                    sb.AppendLine("<a class='nav-link dropdown-toggle' href='#' data-toggle='dropdown'><i class='" + item.Icono + "'></i> " + item.NombreMenu + "</a>");

                    sb.AppendLine("<div class='dropdown-menu drop-menu'>");
                    foreach (tbSubMenu subitem in item.oSubMenu)
                    {
                        //fas fa-caret-right
                        if (subitem.Activo == true)
                        {
                             urlAction = "Url.Action("+ @subitem.Vista + ","+ @subitem.Controlador +")";
                                //"Url.Action(" + @subitem.Vista + "," + @subitem.Controlador + ")";
                            sb.AppendLine("<a class='dropdown-item' name='" + item.NombreMenu + "' href=" + @urlAction + "><i class='" + subitem.Icono + "'></i> " + subitem.NombreSubMenu + "</a>");
                        }
                        urlAction = string.Empty;

                    }
                    sb.AppendLine("</div>");

                    sb.AppendLine("</li>");
                }


            }
            //sb.AppendLine("<a class='dropdown-item' name='" + item.NombreMenu + "' href='/" + subitem.Controlador + "/" + subitem.Vista + "'><i class='" + subitem.Icono + "'></i> " + subitem.NombreSubMenu + "</a>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}
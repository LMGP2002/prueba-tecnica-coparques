using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SistemaGestionReservas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ruta para la gestión de salas
            routes.MapRoute(
                name: "Salas",
                url: "Salas/{action}/{id}",
                defaults: new { controller = "Salas", action = "Index", id = UrlParameter.Optional }
            );

            // Ruta para la gestión de reservas
            routes.MapRoute(
                name: "Reservas",
                url: "Reservas/{action}/{id}",
                defaults: new { controller = "Reservas", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

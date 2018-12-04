using System.Web.Mvc;
using System.Web.Routing;

namespace WeChatCms
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                 "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new string[] { "Tc.Cruise.FrontEnd.Web.Areas.Home.Controllers" }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShakuroMarketplaceNetMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "IndexRoute", url: "", defaults: new { controller = "Home", action = "Index" });
            routes.MapRoute(name: "СatalogRoute", url: "catalog/", defaults: new { controller = "Catalog", action = "Catalog" });
            routes.MapRoute(name: "CategoryRoute", url: "catalog/{category}/", defaults: new { controller = "Catalog", action = "Category" });
            routes.MapRoute(name: "SubcategoryRoute", url: "catalog/{category}/{subcategory}/", defaults: new { controller = "Catalog", action = "Subcategory" });
            routes.MapRoute(name: "GoodRoute", url: "catalog/{category}/{subcategory}/{goodName}/", defaults: new { controller = "Catalog", action = "Good" });
            routes.MapRoute(name: "ReviewsRoute", url: "catalog/{category}/{subcategory}/{goodName}/reviews", defaults: new { controller = "Catalog", action = "Reviews" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

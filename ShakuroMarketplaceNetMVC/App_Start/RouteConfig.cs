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
            routes.MapRoute(name: "FastGoodSearchRoute", url: "FastGoodSearch/{searchInputData}", defaults: new { controller = "Home", action = "FastGoodSearch" });
            routes.MapRoute(name: "GoodSearchRoute", url: "good-search-result/{searchInputData}", defaults: new { controller = "Home", action = "GoodSearch", searchInputData = UrlParameter.Optional });

            routes.MapRoute(name: "СatalogRoute", url: "catalog/", defaults: new { controller = "Catalog", action = "Catalog" });
            routes.MapRoute(name: "AddReviewRoute", url: "catalog/AddReview", defaults: new { controller = "Catalog", action = "AddReview" });
            routes.MapRoute(name: "AddMessageRoute", url: "catalog/AddMessage", defaults: new { controller = "Catalog", action = "AddMessage" });
            routes.MapRoute(name: "ReplyMessageRoute", url: "catalog/ReplyMessage", defaults: new { controller = "Catalog", action = "ReplyMessage" });
            

            routes.MapRoute(name: "CategoryRoute", url: "catalog/{categoryUrl}/", defaults: new { controller = "Catalog", action = "Category" });
            routes.MapRoute(name: "SubcategoryRoute", url: "catalog/{categoryUrl}/{subcategoryUrl}/", defaults: new { controller = "Catalog", action = "Subcategory" });
            routes.MapRoute(name: "GoodRoute", url: "catalog/{categoryUrl}/{subcategoryUrl}/{goodUrl}/", defaults: new { controller = "Catalog", action = "Good" });
            routes.MapRoute(name: "ReviewsRoute", url: "catalog/{categoryUrl}/{subcategoryUrl}/{goodUrl}/reviews", defaults: new { controller = "Catalog", action = "Reviews" });
            routes.MapRoute(name: "OverviewRoute", url: "catalog/{categoryUrl}/{subcategoryUrl}/{goodUrl}/overview", defaults: new { controller = "Catalog", action = "Overview" });
            routes.MapRoute(name: "DiscussionsRoute", url: "catalog/{categoryUrl}/{subcategoryUrl}/{goodUrl}/discussions", defaults: new { controller = "Catalog", action = "Discussions" });
            routes.MapRoute(name: "ShowMoreReviewsRoute", url: "catalog/ShowMoreReviews/{goodId}/{showedReviewsNumber}/{addedReviewsNumber}", defaults: new { controller = "Catalog", action = "ShowMoreReviews" });
            routes.MapRoute(name: "ShowMoreDiscussionsRoute", url: "catalog/ShowMoreDiscussions/{goodId}/{showedDiscussionsNumber}/{addedDiscussionsNumber}", defaults: new { controller = "Catalog", action = "ShowMoreDiscussions" });

            routes.MapRoute(name: "UserCartRoute", url: "cart/", defaults: new { controller = "Cart", action = "Index" });
            routes.MapRoute(name: "ConfirmOrderRoute", url: "cart/confirm-order", defaults: new { controller = "Cart", action = "ConfirmOrder" });
            routes.MapRoute(name: "OrderInformationRoute", url: "cart/order-acceptance", defaults: new { controller = "Cart", action = "OrderAcceptance" });

            routes.MapRoute(name: "PaymentMethodsRoute", url: "cart/payment-methods", defaults: new { controller = "Cart", action = "PaymentMethods" });
            routes.MapRoute(name: "AddGoodToCartRoute", url: "cart/AddGoodToCart/{goodId}", defaults: new { controller = "Cart", action = "AddGoodToCart" });
            routes.MapRoute(name: "RemoveGoodFromdCartRoute", url: "cart/RemoveGoodFromdCart/{goodId}", defaults: new { controller = "Cart", action = "RemoveGoodFromdCart" });
            routes.MapRoute(name: "DeleteGoodFromdCartRoute", url: "cart/DeleteGoodFromdCart/{goodId}", defaults: new { controller = "Cart", action = "DeleteGoodFromdCart" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

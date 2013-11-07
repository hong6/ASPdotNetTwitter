using System.Web.Mvc;
using System.Web.Routing;

namespace TwitterAPIProxy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default2", 
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "UserTweets", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "APIProxySearch",
                url: "{controller}/{action}",
                defaults: new { controller = "TwitterApiProxy", action = "Search" }
                );
            routes.MapRoute(
                name: "APIProxyUserTimeline",
                url: "{controller}/{action}/",
                defaults: new { controller = "TwitterApiProxy", action = "UserTimeline"}
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
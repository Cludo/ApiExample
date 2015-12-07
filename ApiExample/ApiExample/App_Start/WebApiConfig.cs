using System.Web.Http;

namespace ApiExample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();
            var routes = config.Routes;
            routes.MapHttpRoute(
                "publicsettings",
                "api/Cludo/{customerId}/{websiteId}/websites/publicsettings",
                new {controller = "Cludo", action = "publicsettings"}
                );

            routes.MapHttpRoute(
                "Autocomplete",
                "api/Cludo/{customerId}/{websiteId}/{action}/{text}",
                new {controller = "Cludo", action = "Autocomplete", text = RouteParameter.Optional}
                );


            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );
        }
    }
}
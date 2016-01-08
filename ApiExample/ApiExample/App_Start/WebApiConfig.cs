using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

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
            //This we need in case of CORS
            config.Routes.MapHttpRoute("Web API Options",
                "api/Cludo/{*catchAll}", new { action = "Options", controller = "Cludo" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) }
                );

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

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }
    }
}
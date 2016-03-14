# ApiExample
Example how to use Cludo Search API.



# Api controller
Add [API controller](ApiExample/ApiExample/Controllers/CludoController.cs) to your project.
Note that the controller is using attribute routings, so make sure that you have  **config.MapHttpAttributeRoutes();** enabled in your WebApiConfig. 
If you can't use MapHttpAttributeRoutes then you can use routing table. [See Example](ApiExample/ApiExample/App_Start/WebApiConfig.cs)

```
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
```
#Web Config
The Cludo controller is using **CustomerKey** which is read from config file. This key is your secure password to make requests to our api. Make sure that you have this key added to your config file and that you **don't** expose it publicly.

```
 <appSettings>
    <add key="Cludo.CustomerKey" value="YOUR_KEY_GOES_HERE"/>
  </appSettings>
```

#Switch Search Url
In our [search example](ApiExample/ApiExample/index.html), the configuration script is switched to an internal url. This can be done by changing the property **searchApiUrl** to **"/api/Cludo"** so all search requests will go though your own controller.
Then set **intranetSearch** to true

Have any questions, don't hesitate to [Contact us](https://www.cludo.com/contact)

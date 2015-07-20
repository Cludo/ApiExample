# ApiExample
Example how to use Cludo Search API.



# Api controller
Add [API controller](ApiExample/ApiExample/Controllers/CludoController.cs) to your project.
Note that the controller is using attribute routings, so make sure that you have  **config.MapHttpAttributeRoutes();** enabled in your WebApiConfig. If you are using an older version of WebAPI, then please make sure that you have added custom routing to follow same routing patterns. 

#Switch Search Url
In our [search example](ApiExample/ApiExample/index.html), the configuration script is switched to an internal url. This can be done by changing the property **searchApiUrl** to **"/api/Cludo"** so all search requests will go though your own controller.

Have any questions, don't hesitate to [Contact us](https://www.cludo.com/contact)

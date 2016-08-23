# ApiExample
Example how to use Cludo Search API.



# Api controller
Add [API controller](ApiExample/ApiExample/Controllers/CludoController.cs) to your project.
Note that the controller is using attribute routings, so make sure that you have  **config.MapHttpAttributeRoutes();** enabled in your WebApiConfig. 
If you can't use MapHttpAttributeRoutes then you can use routing table. [See Example](ApiExample/ApiExample/App_Start/WebApiConfig.cs)

```
routes.MapHttpRoute(
                "publicsettings",
                "api/Cludo/{customerId}/{engineId}/websites/publicsettings",
                new {controller = "Cludo", action = "publicsettings"}
                );

routes.MapHttpRoute(
                "Autocomplete",
                "api/Cludo/{customerId}/{engineId}/{action}/{text}",
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

#Track searches
If you are using Cludo's API, and don't use our JavaScript, then you need to track the search results yourself. Cludo's tracking is done by injecting a small empty gif image with the properties that should be tracked.

An example of an image is 
```
<img src="https://api.cludo.com/__utm.gif?sz=2560x1440&amp;ua=Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36&amp;refurl=https://www.cludo.com/en/&amp;refpt=Cloud-based search solutions | Cludo&amp;sw=integration&amp;brl=en-US&amp;pn=1&amp;hn=www.cludo.com&amp;rc=5&amp;enid=59&amp;fquery=&amp;ban=0&amp;rt=33&amp;ql=&amp;a=1464598884018" style="display: none;">
```

The base URL is https://api.cludo.com__utml.gif. Then you need to append the following parameters:
* sz (optional): Screensize – e.g. 2560x1440.
* ua: User Agent e.g. Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36
* refurl (optional): The URL the user was at when doing the initial search. E.g. https://www.cludo.com/en/
* refpt (optional): The title of the page the user was at when doing the intial search ”Cloud-based search solutions | Cludo”.
* sw: The search word the user used to perform a search. 
* brl (optional): Browser language of the user e.g. “en-US”.
* pn (optional): Page number of the search. E.g. “1”.
* hn: The host name of the page where the search is done at. E.g. “www.cludo.com”.
* rc: Result count – how many searches were returned to the user. E.g. “5”.
* enid: The engine id.
* fquery (optional): Fixed Query. If we catch spelling mistakes the fixed query will be set to the fixed spelling mistakes. If the search word is “carz”, the fixed query would be “cars”.
* rt (optional): Response time. How fast was the search done.
* ql (optional): If a Quicklink is used in the search, you can supply the id here so we can track it.
* a: Random ID used for cache busting.


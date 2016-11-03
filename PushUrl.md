# Push URL documentation
Example how to use Cludo Search API for pushing URL's to crawl immediately.



# Endpoint
The push url POST endpoint is located at:
```
https://api.cludo.com/api/v3/{customerId}/content/{crawlerId}/pushurls
```
Requirements:
* Body: List of URL's
* HTTP-Headers:
    * Content-Type: application/json
    * Authorization: Basic `{securityToken}`

# Example request

An actual request would look like the following:
```
POST /api/v3/1337/content/42/pushurls HTTP/1.1
Host: api.cludo.com
Content-Type: application/json
Cache-Control: no-cache
Authorization: Basic {securityToken}
```

Body:
```
[
	"http://www.domain.com/page",
	"http://www.domain.com/page2",
	...
]
```


# Security
In the above axample a `securityToken` is needed.
This token is calculated in the following way:

```
 securityToken = Base64("{customerId}:{customerKey}") 
 //ex. Base64("1337:982389hNbB8729") = MTMzNzo5ODIzODloTmJCODcyOQ==
```

You will be given a _customerKey_ when we setup your solution, otherwise contact support.

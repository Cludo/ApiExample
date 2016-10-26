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

A Complete request would look like the following:
```
POST https://api.cludo.com/api/v3/1337/content/42/pushurls
Content-Type:application/json
Authorization:Basic {securityToken}
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
 securityToken = Base64({customerId}:{customerKey}); 
 //ex. Base64("1337:982389hNbB8729") - where customerId=1337 and customerKey=982389hNbB8729
```

You will be given a _customerKey_ when we setup your solution, otherwise contact support.
# Yourls.Net
A simple strong-typed wrapper for YOURLS.org's API with async support.

## Installation
Install the packages from nuget:
* [Yourls.Net](https://www.nuget.org/packages/Yourls.Net/)
* [Yourls.Net.JsonNet](https://www.nuget.org/packages/Yourls.Net.JsonNet/)
* [Yourls.Net.AspNet](https://www.nuget.org/packages/Yourls.Net.AspNet/)

```bash
dotnet add package Yourls.Net
```

---
## API Reference

For the start, you must create an instance of `YourlsClient` class to communicate with the remote API.  
If you're using the `Yourls.Net.AspNet`, with correct configurations, the `YourlsClient` class should be injectable as a service.

### `YourlsClient` class
The client service to communicate with the remote API.
This class takes the remote API's url, an instance of `IAuthenticationHandler`, an object of *`HttpClient`* and an `IJsonDeserializer` as it's constructor's parameters.

* `IAuthenticationHandler` is described in [Authentication](#Authentication) section.
* `YourlsClient` disposes the *`HttpClient`* upon it's own disposal.
* `IJsonDeserializer.DeserializerToDictionary` method should return an *`IDictionary<string, object>`* for json objects.


#### `ShortenUrl`
ShortenUrl(ShortenUrlRequestModel model, CancellationToken ct)  
This method shortens a url.

* Url(required): The url which you want to shorten.
* Keyword(optional): The shortened url name, it should be a unique keyword or null.
* Title(optional): The title parameter for yourls.

#### `GetDbStats`
GetDbStats(CancellationToken ct)  
Returns general information about database. (i.e. total-links, total-clicks)

#### `GetStats`
GetStats(StatsFilterMode mode, int limit, CancellationToken ct)  
Returns statistical information about links.

* mode(required): It determines the filtering mode of the statistic, (Top, Bottom, Random, Last)
* limit(required): The number of rows returned.

#### `GetUrlStats`
GetUrlStats(string shortUrl, CancellationToken ct)  
Returns statistics about a link specified by it's short url.

* shortUrl(required): The short url. (i.e. hash name of the url)


---
## Authentication
YOURLS supports two types of authentication:
1) Signature
2) Username/Password

The authentication is provided with a dynamic approach so you can customize it if your gateway needs some extra information.

There's two classes for each type of these authentications in Yourls.Net:
1) `SignatureAuthentication` which takes the signature value as it's constructor parameters.
2) `UsernamePasswordAuthentication` which takes the username and the password as it's constructor parameters.


---
## Yourls.Net.AspNet
With this library you can configure the `YourlsClient` as a service in the *Asp.Net* pipeline.

To register `YourlsClient` in the *Asp.Net* pipeline call the `AddYourlsClient` in your `ConfigureServices` method.

```csharp
    services.AddYourlsClient(config => {
        config.ApiUrl = "XXX"; //(required) base url for your API, (e.g. http://test.com/yourls-api.php)
        
        //Choose between one of these authentication methods. (signature has the higher priority)
        config.Signature = "XXX";
        config.Username = "XXX";
        config.Password = "XXX";
        
        config.Timeout = TimeSpan.FromSeconds(3);

        config.JsonDeserializer = new YourlsJsonDeserializer(); //(optional) Sets the json deserializer used to deserialize json results.
        config.AuthenticationHandler = new XXX(); //(optional) Sets the authentication handler for requests.
    });
```
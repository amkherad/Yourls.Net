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

`Yourls.Net` is the base abstract package, to work with the API you have to install either `Yourls.Net.JsonNet` or 
`Yourls.Net.AspNet`, the later is also dependent on `Yourls.Net.JsonNet`.

---
## API Reference

For start, you must create an instance of `YourlsClient` class to communicate with the remote API.  
If you're using the `Yourls.Net.AspNet`, with correct configurations, the `YourlsClient` class should be injectable as a service, but without it you have to either use `Yourls.Net.JsonNet`'s `YourlsClientJsonNet` class, or create a class which is derived from `YourlsClient` to implement `DeserializeObject` and `DeserializeToDictionary` methods.

### `YourlsClient` class
This class takes the remote API's url, an instance of `IAuthenticationHandler` and an object of *`HttpClient`* as it's constructor's parameters.

* `IAuthenticationHandler` is described in [Authentication](#Authentication) section.
* `YourlsClient` disposes the *`HttpClient`* upon it's own disposal.
* `DeserializeToDictionary` method should return an *`IDictionary<string, object>`* as objects.


#### `ShortenUrl`(ShortenUrlRequestModel model, CancellationToken ct)
This method shortens a url.

* Url(required): The url which you want to shorten.
* Keyword: The shortened url name, it should be a unique keyword.
* Title: The title parameter for yourls.

#### `GetDbStats`(CancellationToken ct)
Returns general information about database. (i.e. total-links, total-clicks)

#### `GetStats`(StatsFilterMode mode, int limit, CancellationToken ct)
Returns statistical information about links.

* mode: It determines the filtering mode of the statistic, (Top, Bottom, Random, Last)
* limit: The number of rows returned.

#### `GetUrlStats`(string shortUrl, CancellationToken ct)
Returns statistics about a link specified by it's short url.

* shortUrl: The short url. (i.e. hash name of the url)


---
## Authentication
YOURLS supports two types of authentication:
1) Signature
2) Username/Password

The authentication is provided with a dynamic approach so you can customize it if your gateway need some extra information.

There's two classes for each type of these authentications in Yourls.Net:
1) `SignatureAuthentication` which takes the signature value as it's constructor parameters.
2) `UsernamePasswordAuthentication` which takes the username and the password as it's constructor parameters.
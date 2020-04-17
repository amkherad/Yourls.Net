using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Yourls.Net.Authentication;

namespace Yourls.Net
{
    public partial class YourlsClient : IDisposable
    {
        private const string ApiResponseFormat = "json";
        private const string ShortenUrlActionName = "shorturl";
        private const string ExpandActionName = "expand";
        private const string GetDbStatsActionName = "db-stats";
        private const string GetUrlStatsActionName = "url-stats";
        private const string GetStatsActionName = "stats";

        public Uri ApiUri { get; set; }

        public IAuthenticationHandler AuthenticationHandler { get; }

        public HttpClient HttpClient { get; }


        public HttpMethod HttpMethod { get; set; }
        
        
        public IJsonDeserializer JsonDeserializer { get; }

        

        public YourlsClient(
            Uri apiUri,
            IJsonDeserializer jsonDeserializer
        ) : this(apiUri, new NoAuthentication(), jsonDeserializer)
        {
        }

        public YourlsClient(
            string apiUri,
            IJsonDeserializer jsonDeserializer
        ) : this(apiUri, new NoAuthentication(), jsonDeserializer)
        {
        }

        public YourlsClient(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler,
            IJsonDeserializer jsonDeserializer
        ) : this(apiUri, authenticationHandler, new HttpClient(), jsonDeserializer)
        {
        }

        public YourlsClient(
            string apiUri,
            IAuthenticationHandler authenticationHandler,
            IJsonDeserializer jsonDeserializer
        ) : this(new Uri(apiUri), authenticationHandler, new HttpClient(), jsonDeserializer)
        {
        }

        public YourlsClient(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler,
            HttpClient httpClient,
            IJsonDeserializer jsonDeserializer
        )
        {
            if (apiUri is null) throw new ArgumentNullException(nameof(apiUri));
            if (authenticationHandler is null) throw new ArgumentNullException(nameof(authenticationHandler));
            if (httpClient is null) throw new ArgumentNullException(nameof(httpClient));
            if (jsonDeserializer is null) throw new ArgumentNullException(nameof(jsonDeserializer));

            ApiUri = apiUri;
            AuthenticationHandler = authenticationHandler;
            HttpClient = httpClient;
            JsonDeserializer = jsonDeserializer;

            HttpMethod = HttpMethod.Get;
        }


        protected virtual Uri GetApiUri(
            string action
        )
        {
            return this.ApiUri;
        }
        

        protected virtual async Task<HttpResponseMessage> CallApi(
            string action,
            Dictionary<string, object> values,
            CancellationToken cancellationToken
        )
        {
            var uri = GetApiUri(action);

            uri = Helpers.AppendQueryParametersToUri(uri, values);

            var request = new HttpRequestMessage(
                HttpMethod,
                uri
            );

            request = await AuthenticationHandler.HandleRequest(
                action,
                uri,
                HttpClient,
                request,
                cancellationToken
            );

            var response = await HttpClient.SendAsync(
                request,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException($"Remote API call was unsuccessful, StatusCode: {response.StatusCode}");
            }

            return response;
        }

        
        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
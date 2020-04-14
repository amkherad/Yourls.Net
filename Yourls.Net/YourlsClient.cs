using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Yourls.Net.Authentication;

namespace Yourls.Net
{
    public abstract partial class YourlsClient : IDisposable
    {
        private const string ApiResponseFormat = "json";
        private const string ShortenUrlActionName = "shorturl";
        //private const string ShortenUrlActionName = "shorturl";

        public Uri ApiUri { get; set; }

        public IAuthenticationHandler AuthenticationHandler { get; }

        public HttpClient HttpClient { get; }


        public HttpMethod HttpMethod { get; set; }


        public YourlsClient(
            Uri apiUri
        ) : this(apiUri, new NoAuthentication())
        {
        }

        public YourlsClient(
            string apiUri
        ) : this(apiUri, new NoAuthentication())
        {
        }

        public YourlsClient(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler
        ) : this(apiUri, authenticationHandler, new HttpClient())
        {
        }

        public YourlsClient(
            string apiUri,
            IAuthenticationHandler authenticationHandler
        ) : this(new Uri(apiUri), authenticationHandler, new HttpClient())
        {
        }

        public YourlsClient(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler,
            HttpClient httpClient
        )
        {
            if (apiUri is null) throw new ArgumentNullException(nameof(apiUri));
            if (authenticationHandler is null) throw new ArgumentNullException(nameof(authenticationHandler));
            if (httpClient is null) throw new ArgumentNullException(nameof(httpClient));

            ApiUri = apiUri;
            AuthenticationHandler = authenticationHandler;
            HttpClient = httpClient;

            HttpMethod = HttpMethod.Get;
        }


        protected virtual Uri GetApiUri(
            string action
        )
        {
            return this.ApiUri;
        }


        protected abstract TResult DeserializerJson<TResult>(
            string json
        );
        

        protected virtual async Task<HttpResponseMessage> CallApi(
            string action,
            Dictionary<string, string> values,
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
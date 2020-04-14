using System;
using System.Net.Http;
using Newtonsoft.Json;
using Yourls.Net.Authentication;

namespace Yourls.Net.JsonNet
{
    public class YourlsClientJsonNet : YourlsClient
    {
        public YourlsClientJsonNet(
            Uri apiUri
        ) : base(apiUri)
        {
        }

        public YourlsClientJsonNet(
            string apiUri
        ) : base(apiUri)
        {
        }

        public YourlsClientJsonNet(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler
        ) : base(apiUri, authenticationHandler)
        {
        }

        public YourlsClientJsonNet(
            string apiUri,
            IAuthenticationHandler authenticationHandler
        ) : base(apiUri, authenticationHandler)
        {
        }

        public YourlsClientJsonNet(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler,
            HttpClient httpClient
        ) : base(apiUri, authenticationHandler, httpClient)
        {
        }

        protected override TResult DeserializerJson<TResult>(
            string json
        )
        {
            return JsonConvert.DeserializeObject<TResult>(json);
        }
    }
}
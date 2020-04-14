using System;
using System.Net.Http;
using Yourls.Net.Authentication;
using Yourls.Net.JsonNet;

namespace Yourls.Net.AspNet
{
    public class YourlsClientAspNet : YourlsClientJsonNet
    {
        public YourlsClientAspNet(
            Uri apiUri
        ) : base(apiUri)
        {
        }

        public YourlsClientAspNet(
            string apiUri
        ) : base(apiUri)
        {
        }

        public YourlsClientAspNet(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler
        ) : base(apiUri, authenticationHandler)
        {
        }

        public YourlsClientAspNet(
            string apiUri,
            IAuthenticationHandler authenticationHandler
        ) : base(apiUri, authenticationHandler)
        {
        }

        public YourlsClientAspNet(
            Uri apiUri,
            IAuthenticationHandler authenticationHandler,
            HttpClient httpClient
        ) : base(apiUri, authenticationHandler, httpClient)
        {
        }
    }
}
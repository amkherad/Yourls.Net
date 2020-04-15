using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        protected override TResult DeserializeObject<TResult>(
            string json
        )
        {
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        protected override IDictionary<string, object> DeserializeToDictionary(
            string json
        )
        {
            var rootObject = JObject.Parse(json);
            
            
        }
    }
}
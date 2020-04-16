﻿using System;
using System.Collections.Generic;
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

        private object ReadToken(
            JToken token
        )
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                {
                    var obj = (JObject)token;
                    
                    var result = new Dictionary<string, object>();
                    
                    foreach (var property in obj.Properties())
                    {
                        result.Add(property.Name, ReadToken(property.Value));
                    }

                    return result;
                }
                case JTokenType.Array:
                {
                    var arr = (JArray) token;
                    var array = new List<object>();

                    foreach (var item in arr.Values())
                    {
                        array.Add(ReadToken(item));
                    }

                    return array.ToArray();
                }
                default:
                    var val = (JValue) token;

                    return val.Value;
            }
        }

        protected override IDictionary<string, object> DeserializeToDictionary(
            string json
        )
        {
            var rootObject = ReadToken(JToken.Parse(json));

            if (rootObject is IDictionary<string, object> dictionary)
            {
                return dictionary;
            }

            throw new YourlsException("Json should contain an object as first token.");
        }
    }
}
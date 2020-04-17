using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yourls.Net.JsonNet
{
    public class YourlsJsonDeserializer : IJsonDeserializer
    {
        public YourlsJsonDeserializer()
        {
        }

        public TResult DeserializeObject<TResult>(
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

        public IDictionary<string, object> DeserializeToDictionary(
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
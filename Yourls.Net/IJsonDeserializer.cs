using System.Collections.Generic;

namespace Yourls.Net
{
    public interface IJsonDeserializer
    {
        TResult DeserializeObject<TResult>(
            string json
        );
        
        IDictionary<string, object> DeserializeToDictionary(
            string json
        );
    }
}
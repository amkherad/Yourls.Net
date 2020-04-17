using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net
{
    public partial class YourlsClient
    {
        private class GetUrlStatsResponse
        {
            public int StatusCode { get; set; }
            
            public string Message { get; set; }

            public UrlStatsResponseModel Link { get; set; }
        }
        
        public async Task<UrlStatsResponseModel> GetUrlStats(
            string shortUrl,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new InvalidOperationException("shortUrl was null or whitespace.");
            }


            var values = new Dictionary<string, object>();

            
            values.Add("shorturl", shortUrl);
            values.Add("format", ApiResponseFormat);

            
            values.Add("action", GetUrlStatsActionName);
            var response = await CallApi(
                GetUrlStatsActionName,
                values,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException();
            }

            var responseText = await response.Content.ReadAsStringAsync();

            var resultModel = JsonDeserializer.DeserializeObject<GetUrlStatsResponse>(responseText);

            if (resultModel is null || resultModel.Message is null || resultModel.Link is null)
            {
                throw new YourlsException($"The result of {nameof(JsonDeserializer.DeserializeObject)} was null or it didn't contain Message or Link values.");
            }
            
            return resultModel.Link;
        }
    }
}
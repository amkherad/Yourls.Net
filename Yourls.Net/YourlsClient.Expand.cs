using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net
{
    public partial class YourlsClient
    {
        private class ExpandResponse
        {
            public string Keyword { get; set; }
            
            public string ShortUrl { get; set; }
            
            public string LongUrl { get; set; }
            
            public string Title { get; set; }
            
            public string Message { get; set; }
            
            public int StatusCode { get; set; }
        }
        
        public async Task<ExpandResponseModel> Expand(
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

            
            values.Add("action", ExpandActionName);
            var response = await CallApi(
                ExpandActionName,
                values,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException();
            }

            var responseText = await response.Content.ReadAsStringAsync();

            var resultModel = DeserializeObject<ExpandResponse>(responseText);

            if (resultModel is null || resultModel.Keyword is null)
            {
                throw new YourlsException($"The result of {nameof(DeserializeObject)} was null or it didn't contain a Keyword value.");
            }
            
            return new ExpandResponseModel
            {
                Keyword = resultModel.Keyword,
                Title = resultModel.Title,
                LongUrl = resultModel.LongUrl,
                ShortUrl = resultModel.ShortUrl
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net
{
    public partial class YourlsClient
    {
        private class ShortenUrlResponse
        {
            public string Status { get; set; }
            
            public string Code { get; set; }
            
            public UrlInfo Url { get; set; }
            
            public string Message { get; set; }
            
            public string Title { get; set; }
            
            public string ShortUrl { get; set; }
            
            public int StatusCode { get; set; }
        }
        
        public async Task<ShortenUrlResponseModel> ShortenUrl(
            ShortenUrlRequestModel model,
            CancellationToken cancellationToken
        )
        {
            if (model is null) throw new ArgumentNullException(nameof(model));


            if (string.IsNullOrWhiteSpace(model.Url))
            {
                throw new InvalidOperationException("Url was null or whitespace.");
            }


            var values = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                values.Add("keyword", model.Keyword);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                values.Add("title", model.Title);
            }
 

            values.Add("url", model.Url);
            values.Add("format", ApiResponseFormat);

            
            values.Add("action", ShortenUrlActionName);

            
            var response = await CallApi(
                ShortenUrlActionName,
                values,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException();
            }

            var responseText = await response.Content.ReadAsStringAsync();
            
            var resultModel = JsonDeserializer.DeserializeObject<ShortenUrlResponse>(responseText);

            var status = resultModel.Status?.ToLower();
            var code = resultModel.Code?.ToLower();

            if (code == "error:keyword")
            {
                throw new YourlsException("Keyword was already taken.");
            }
            
            if (status != "success" && code != "error:url")
            {
                throw new YourlsException($"Unknown error occured, original code was: {resultModel.Code}");
            }

            if (resultModel is null || resultModel.Url is null)
            {
                throw new YourlsException($"The result of {nameof(JsonDeserializer.DeserializeObject)} was null or it didn't contain a Url value.");
            }

            var result = resultModel.Url;

            if (resultModel.Status?.ToLower() != "success")
            {
                result.Existed = true;
            }
            
            return new ShortenUrlResponseModel
            {
                Url = result,
                
                Code = resultModel.Code,
                Message = resultModel.Message,
                Status = resultModel.Status,
                Title = resultModel.Title,
                ShortUrl = resultModel.ShortUrl,
                StatusCode = resultModel.StatusCode
            };
        }

    }
}
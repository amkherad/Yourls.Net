﻿using System;
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
            
            public ShortenUrlResponseModel Url { get; set; }
            
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


            var values = new Dictionary<string, string>();

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

            var resultModel = DeserializeObject<ShortenUrlResponse>(responseText);

            if (resultModel is null || resultModel.Url is null)
            {
                throw new YourlsException("The result of DeserializerJson was null or it didn't contain a Url value.");
            }

            var result = resultModel.Url;

            if (resultModel.Status?.ToLower() != "success")
            {
                result.Existed = true;
            }
            
            return result;
        }

    }
}
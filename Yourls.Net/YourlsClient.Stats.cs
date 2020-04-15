using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net
{
    public partial class YourlsClient
    {
        private const string MessagePropertyName = "message";
        private const string ShortUrlPropertyName = "shorturl";
        private const string UrlPropertyName = "url";
        private const string TitlePropertyName = "title";
        private const string TimeStampPropertyName = "timestamp";
        private const string IpPropertyName = "ip";
        private const string ClicksPropertyName = "clicks";

        class GetStatsResponse
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }


            public DbStatsResponse Stats { get; set; }
        }


        private UrlStatsResponseModel ReadUrlStatFromDictionary(
            IDictionary<string, object> dictionary
        )
        {
            var result = new UrlStatsResponseModel();

            if (dictionary.TryGetValue(ShortUrlPropertyName, out var shortUrlObj) && shortUrlObj is string shortUrl)
            {
                result.ShortUrl = shortUrl;
            }

            if (dictionary.TryGetValue(UrlPropertyName, out var urlObj) && urlObj is string url)
            {
                result.Url = url;
            }

            if (dictionary.TryGetValue(TitlePropertyName, out var titleObj) && titleObj is string title)
            {
                result.Title = title;
            }

            if (dictionary.TryGetValue(TimeStampPropertyName, out var timeStampObj) && timeStampObj is string timeStampStr &&
                DateTime.TryParse(timeStampStr, out var timeStamp))
            {
                result.TimeStamp = timeStamp;
            }

            if (dictionary.TryGetValue(TimeStampPropertyName, out var ipObj) && ipObj is string ip)
            {
                result.Ip = ip;
            }

            if (dictionary.TryGetValue(ClicksPropertyName, out var clicksObj) && clicksObj is string clicksStr &&
                int.TryParse(clicksStr, out var clicks))
            {
                result.Clicks = clicks;
            }
            
            return result;
        }

        private UrlStatsResponseModel[] ReadUrlStatsFromDictionary(
            IDictionary<string, object> dictionary
        )
        {
            var entries = new List<UrlStatsResponseModel>();
            
            foreach (var row in dictionary)
            {
                if (!(row.Value is IDictionary<string, object> rowDict))
                {
                    throw new YourlsException("Invalid json format.");
                }

                var entry = ReadUrlStatFromDictionary(rowDict);
                
                entries.Add(entry);
            }
            
            return entries.ToArray();
        }


        public async Task<StatsResponseModel> GetStats(
            StatsFilterMode mode,
            int limit,
            CancellationToken cancellationToken
        )
        {
            if (limit <= 0)
            {
                throw new InvalidOperationException("limit was zero or less than zero.");
            }


            var values = new Dictionary<string, object>();

            string filter;

            switch (mode)
            {
                case StatsFilterMode.Top:
                    filter = "top";
                    break;
                case StatsFilterMode.Bottom:
                    filter = "bottom";
                    break;
                case StatsFilterMode.Random:
                    filter = "rand";
                    break;
                case StatsFilterMode.Last:
                    filter = "last";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            values.Add("filter", filter);
            values.Add("limit", limit);

            values.Add("format", ApiResponseFormat);


            values.Add("action", GetStatsActionName);
            var response = await CallApi(
                GetStatsActionName,
                values,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException();
            }

            var responseText = await response.Content.ReadAsStringAsync();

            var resultModel = DeserializeToDictionary(responseText);

            if (resultModel is null || !resultModel.TryGetValue(MessagePropertyName, out var message) ||
                !(message is string strMsg) || strMsg?.ToLower() != "success")
            {
                throw new YourlsException(
                    $"The result of {nameof(DeserializeObject)} was null or it didn't contain a Message value.");
            }

            
            var dbStats = ReadDbStatsFromDictionary(resultModel);

            var links = ReadUrlStatsFromDictionary(resultModel);

            
            return new StatsResponseModel
            {
                Stats = dbStats,
                
                Links = links
            };
        }
    }
}
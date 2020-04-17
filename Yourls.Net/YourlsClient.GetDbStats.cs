using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net
{
    public partial class YourlsClient
    {
        private const string DbStatsPropertyName = "db-stats";
        private const string DbStatsTotalLinksPropertyName = "total_links";
        private const string DbStatsTotalClicksPropertyName = "total_clicks";


        private class DbStatsResponse
        {
            public string total_links { get; set; }
            
            public string total_clicks { get; set; }
        }

        private DbStatsResponseModel ReadDbStatsFromDictionary(
            string fieldName,
            IDictionary<string, object> dictionary
        )
        {
            if (dictionary is null || !dictionary.TryGetValue(fieldName, out var dbStats) ||
                !(dbStats is IDictionary<string, object> dbStatsDict))
            {
                throw new YourlsException(
                    $"The result of {nameof(JsonDeserializer.DeserializeToDictionary)} was null or it didn't contain a valid value.");
            }

            if (!dbStatsDict.TryGetValue(DbStatsTotalLinksPropertyName, out var totalLinksObj) ||
                !dbStatsDict.TryGetValue(DbStatsTotalClicksPropertyName, out var totalClicksObj) ||
                !(totalLinksObj is string) || !(totalClicksObj is string))
            {
                throw new YourlsException(
                    $"The result of {nameof(JsonDeserializer.DeserializeToDictionary)} was null or it didn't contain a valid value.");
            }

            var totalLinks = int.Parse(totalLinksObj as string);
            var totalClicks = int.Parse(totalClicksObj as string);

            return new DbStatsResponseModel
            {
                TotalLinks = totalLinks,
                TotalClicks = totalClicks
            };
        }
        
        public async Task<DbStatsResponseModel> GetDbStats(
            CancellationToken cancellationToken
        )
        {
            var values = new Dictionary<string, object>();


            values.Add("format", ApiResponseFormat);


            values.Add("action", GetDbStatsActionName);
            var response = await CallApi(
                GetDbStatsActionName,
                values,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new YourlsException();
            }

            var responseText = await response.Content.ReadAsStringAsync();

            var resultDict = JsonDeserializer.DeserializeToDictionary(responseText);

            return ReadDbStatsFromDictionary(DbStatsPropertyName, resultDict);
        }
    }
}
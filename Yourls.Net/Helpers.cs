using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Yourls.Net
{
    internal static class Helpers
    {
        public static bool QueryContainsParameter(
            string query,
            string parameterName
        )
        {
            var regex = new Regex($"^({parameterName}=).*$");

            return regex.IsMatch(query);
        }

        public static string AppendParameterToQuery(
            string sourceQuery,
            string parameterName,
            string parameterValue
        )
        {
            parameterValue = WebUtility.UrlEncode(parameterValue);

            if (sourceQuery is null || string.IsNullOrWhiteSpace(sourceQuery))
            {
                return $"{parameterName}={parameterValue}";
            }

            return $"{sourceQuery}&{parameterName}={parameterValue}";
        }

        public static Uri UpdateQuery(
            Uri uri,
            string query
        )
        {
            var (username, password) = Helpers.ExtractUserNameAndPasswordFromQueryUserInfo(uri.UserInfo);

            var newUri = new UriBuilder
            {
                Host = uri.Host,
                Path = uri.AbsolutePath,
                Scheme = uri.Scheme,
                Port = uri.Port,
                Fragment = uri.Fragment,
                UserName = username,
                Password = password,

                Query = query,
            };

            return newUri.Uri;
        }

        public static Uri AppendQueryParametersToUri(
            Uri uri,
            IDictionary<string, object> values
        )
        {
            var queryKeyValues = string.Join(
                "&",
                values.Select(kv => $"{kv.Key}={WebUtility.UrlEncode(kv.Value.ToString())}")
            );

            var originalQuery = uri.Query;
            if (string.IsNullOrWhiteSpace(originalQuery))
            {
                originalQuery = originalQuery.Trim();

                if (originalQuery.EndsWith("&"))
                {
                    queryKeyValues = $"{originalQuery}{queryKeyValues}";
                }
                else
                {
                    queryKeyValues = $"{originalQuery}&{queryKeyValues}";
                }

                uri = UpdateQuery(uri, queryKeyValues);
            }

            return uri;
        }

        public static (string, string) ExtractUserNameAndPasswordFromQueryUserInfo(
            string userInfo
        )
        {
            if (userInfo is null)
            {
                return (null, null);
            }

            if (userInfo.Contains(":"))
            {
                var parts = userInfo.Split(':');

                return (parts[0], parts[1]);
            }

            return (userInfo, null);
        }
    }
}
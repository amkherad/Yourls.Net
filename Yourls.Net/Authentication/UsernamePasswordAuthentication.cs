using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net.Authentication
{
    public class UsernamePasswordAuthentication : IAuthenticationHandler
    {
        private const string UsernameParameterName = "username";
        private const string PasswordParameterName = "password";
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        
        public UsernamePasswordAuthentication(string username, string password)
        {
            Username = username;
            Password = password;
        }
        
        public Task<HttpRequestMessage> HandleRequest(
            string action,
            Uri uri,
            HttpClient client,
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            var query = request.RequestUri.Query;
            var modified = false;

            if (!Helpers.QueryContainsParameter(query, UsernameParameterName))
            {
                modified = true;
                query = Helpers.AppendParameterToQuery(query, UsernameParameterName, Username);
            }

            if (!Helpers.QueryContainsParameter(query, PasswordParameterName))
            {
                modified = true;
                query = Helpers.AppendParameterToQuery(query, PasswordParameterName, Password);
            }
            
            if (modified)
            {
                uri = Helpers.UpdateQuery(uri, query);
                
                request.RequestUri = uri;
            }

            return Task.FromResult(request);
        }
    }
}
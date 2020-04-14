using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net.Authentication
{
    public class SignatureAuthentication : IAuthenticationHandler
    {
        private const string SignatureParameterName = "signature";
        
        public string Signature { get; set; }
        
        
        public SignatureAuthentication(string signature)
        {
            Signature = signature;
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

            if (!Helpers.QueryContainsParameter(query, SignatureParameterName))
            {
                query = Helpers.AppendParameterToQuery(query, SignatureParameterName, Signature);

                uri = Helpers.UpdateQuery(uri, query);
                
                request.RequestUri = uri;
            }

            return Task.FromResult(request);
        }
    }
}
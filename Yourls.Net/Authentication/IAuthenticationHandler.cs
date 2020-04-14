using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Yourls.Net.Authentication
{
    public interface IAuthenticationHandler
    {
        Task<HttpRequestMessage> HandleRequest(
            string action,
            Uri uri,
            HttpClient client,
            HttpRequestMessage request,
            CancellationToken cancellationToken
        );
    }
}
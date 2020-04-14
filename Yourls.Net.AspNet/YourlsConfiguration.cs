using System;
using System.Net.Http;

namespace Yourls.Net.AspNet
{
    public class YourlsConfiguration
    {
        public string ApiUrl { get; set; }
        
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        
        public string Signature { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public TimeSpan? Timeout { get; set; }
    }
}
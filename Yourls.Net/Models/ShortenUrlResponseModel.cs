using System;

namespace Yourls.Net
{
    public class ShortenUrlResponseModel
    {
        public string Keyword { get; set; }
        
        public string Url { get; set; }
        
        public string Title { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public string Ip { get; set; }
        
        
        public bool Existed { get; set; }
    }
}
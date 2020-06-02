using System;

namespace Yourls.Net
{
    public class UrlInfo
    {
        public string Keyword { get; set; }
        
        public string Url { get; set; }
        
        public string Title { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public string Ip { get; set; }
        
        
        public bool Existed { get; set; }
    }

    public class ShortenUrlResponseModel
    {
        public string Status { get; set; }
            
        public string Code { get; set; }
            
        public UrlInfo Url { get; set; }
            
        public string Message { get; set; }
            
        public string Title { get; set; }
            
        public string ShortUrl { get; set; }
            
        public int StatusCode { get; set; }
    }
}
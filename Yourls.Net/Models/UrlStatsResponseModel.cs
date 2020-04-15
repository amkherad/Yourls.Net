using System;

namespace Yourls.Net
{
    public class UrlStatsResponseModel
    {
        public string ShortUrl { get; set; }
        
        public string Url { get; set; }
        
        public string Title { get; set; }
        
        public DateTime TimeStamp { get; set; }
        
        public string Ip { get; set; }
        
        public int Clicks { get; set; }
    }
}
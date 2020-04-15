namespace Yourls.Net
{
    public class StatsResponseModel
    {
        public UrlStatsResponseModel[] Links { get; set; }
        
        public DbStatsResponseModel Stats { get; set; }
    }
}
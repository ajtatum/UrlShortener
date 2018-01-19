using System;

namespace UrlShortener.Models
{
    public class UrlShortener
    {
        public string Id { get; set; }
        public string LongUrl { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace BitlyAssistant.Shared.Models
{
    public class ShortenedUrlResponse
    {
        public DateTime created_at { get; set; }
        public string id { get; set; }
        public string link { get; set; }
        public List<string> custom_bitlinks { get; set; }
        public string long_url { get; set; }
        public bool archived { get; set; }
        public List<string> tags { get; set; }
        public List<string> deeplinks { get; set; }
        public References reference { get; set; }
    }

    public class References
    {
        public string group { get; set; }
    }
}

using System;

namespace BitlyAssistant.Shared.Models
{
    public class ShortLinkModel
    {
        public int short_link_id { get; set; } = -1;
        public string short_link { get; set; } = "";
        public string long_link { get; set; } = "";
        public string description { get; set; } = "";
        public int request_id { get; set; } = -1;
        public int response_id { get; set; } = -1;
        public DateTime date_added { get; set; } = DateTime.Now;
    }
}

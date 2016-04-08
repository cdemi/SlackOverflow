using Newtonsoft.Json;
using System.Collections.Generic;

namespace SlackOverflow.Models
{
    public class SlackResponse
    {
        public string response_type { get; set; }
        public string text { get; set; }
        public List<Attachment> attachments { get; set; }
    }

    public class Attachment
    {
        public string fallback { get; set; }
        public string color { get; set; }
        public string pretext { get; set; }
        public string author_name { get; set; }
        public string author_link { get; set; }
        public string author_icon { get; set; }
        public string title { get; set; }
        public string title_link { get; set; }
        public string text { get; set; }
        public Field[] fields { get; set; }
        public string image_url { get; set; }
        public string thumb_url { get; set; }
    }

    public class Field
    {
        public string title { get; set; }
        public string value { get; set; }
        [JsonProperty(PropertyName = "short")]
        public bool _short { get; set; }
    }

}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Slack
{
    /// <summary>
    /// Attachment model for Slack messages.
    /// </summary>
    public class Attachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; } = "#36a64f";
        public string Pretext { get; set; }

        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [JsonPropertyName("author_link")]
        public string AuthorLink { get; set; }

        [JsonPropertyName("author_icon")]
        public string AuthorIcon { get; set; }
        public string Title { get; set; }

        [JsonPropertyName("title_link")]
        public string TitleLink { get; set; }

        public string Text { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
        public string ImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Footer { get; set; }

        [JsonPropertyName("footer_icon")]
        public string FooterIcon { get; set; }
        public int Ts { get; set; }
    }

}
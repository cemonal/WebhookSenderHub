using System.Text.Json.Serialization;

namespace WebhookSenderHub.Discord
{
    public class Embed
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("color")]
        public int? Color { get; set; }
    }
}
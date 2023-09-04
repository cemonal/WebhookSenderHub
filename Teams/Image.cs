using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams
{
    /// <summary>
    /// Represents an image used in Microsoft Teams webhook messages.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets the URI of the image.
        /// </summary>
        [JsonPropertyName("image")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the title of the image.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
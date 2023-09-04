using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams
{
    /// <summary>
    /// Represents a key-value pair (fact) used in Microsoft Teams webhook messages.
    /// </summary>
    public class Fact
    {
        /// <summary>
        /// Gets or sets the name of the fact (key).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the fact (value).
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}

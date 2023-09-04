using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.HttpPost
{
    /// <summary>
    /// Represents an HTTP header for use with Microsoft Teams HTTP POST actions.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Gets or sets the name of the header.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the header.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}

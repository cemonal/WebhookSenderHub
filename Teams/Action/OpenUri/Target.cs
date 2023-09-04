
using WebhookSenderHub.JsonConverters;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.OpenUri
{
    /// <summary>
    /// Represents a target for the OpenUri action in Microsoft Teams message cards.
    /// </summary>
    public class Target
    {
        /// <summary>
        /// Gets or sets the operating system for the target of the OpenUri action.
        /// </summary>
        [JsonPropertyName("os")]
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public TargetOs OS { get; set; } = TargetOs.Default;

        /// <summary>
        /// Gets or sets the URI for the target of the OpenUri action.
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}

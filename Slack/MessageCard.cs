using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Slack
{
    /// <summary>
    /// Represents a Slack message card to be sent via webhook.
    /// </summary>
    public class MessageCard : IMessageCard
    {
        /// <summary>
        /// Gets or sets the channel where the message card will be sent.
        /// </summary>
        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the username of the message sender.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = "HealthCheck Bot";

        /// <summary>
        /// Gets or sets the emoji icon for the message sender.
        /// </summary>
        [JsonPropertyName("icon_emoji")]
        public string IconEmoji { get; set; } = ":robot_face:";

        /// <summary>
        /// The attachments of the message.
        /// </summary>
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}

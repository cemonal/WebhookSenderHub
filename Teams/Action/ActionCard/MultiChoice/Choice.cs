using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.ActionCard.MultiChoice
{
    /// <summary>
    /// Represents a single choice within a multi-choice input for an action card in Microsoft Teams.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Gets or sets the display text for the choice.
        /// </summary>
        [JsonPropertyName("display")]
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the choice.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}

using WebhookSenderHub.Teams.Action;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams
{
    /// <summary>
    /// Represents a base action for Microsoft Teams message cards.
    /// </summary>
    public class BaseAction : IAction
    {
        /// <summary>
        /// Gets or sets the action type of the Microsoft Teams message card.
        /// </summary>
        [JsonPropertyName("@type")]
        public ActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the action on the Microsoft Teams message card.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

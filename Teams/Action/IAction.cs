
using WebhookSenderHub.JsonConverters;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action
{
    /// <summary>
    /// Represents an action that can be performed from a Microsoft Teams webhook message.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        ActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        string Name { get; set; }
    }
}

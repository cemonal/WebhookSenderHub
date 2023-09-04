using WebhookSenderHub.JsonConverters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action
{
    /// <summary>
    /// Represents the types of actions available for Microsoft Teams message cards.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ActionType
    {
        /// <summary>
        /// An ActionCard action, which can be used to show a set of inputs and actions within a card.
        /// </summary>
        [EnumMember(Value = "ActionCard")]
        ActionCard,

        /// <summary>
        /// An HttpPOST action, which sends an HTTP POST request to a specified URL with a JSON payload.
        /// </summary>
        [EnumMember(Value = "HttpPOST")]
        HttpPost,

        /// <summary>
        /// An OpenUri action, which opens a specified URL in a separate browser or app.
        /// </summary>
        [EnumMember(Value = "OpenUri")]
        OpenUri
    }
}

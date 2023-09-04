using WebhookSenderHub.JsonConverters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.ActionCard.MultiChoice
{
    /// <summary>
    /// Represents the display style of a multi-choice input in an action card for Microsoft Teams.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Style
    {
        /// <summary>
        /// The multi-choice input is displayed with a normal (collapsed) style.
        /// </summary>
        [EnumMember(Value = "normal")]
        Normal,

        /// <summary>
        /// The multi-choice input is displayed with an expanded style.
        /// </summary>
        [EnumMember(Value = "expanded")]
        Expanded
    }
}
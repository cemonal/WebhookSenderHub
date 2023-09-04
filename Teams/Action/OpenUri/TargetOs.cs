using WebhookSenderHub.JsonConverters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.OpenUri
{
    /// <summary>
    /// Represents the target operating systems for the OpenUri action in Microsoft Teams message cards.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TargetOs
    {
        /// <summary>
        /// The default target operating system.
        /// </summary>
        [EnumMember (Value = "default")]
        Default,

        /// <summary>
        /// The Windows operating system.
        /// </summary>
        [EnumMember(Value = "windows")]
        Windows,

        /// <summary>
        /// The iOS operating system.
        /// </summary>
        [EnumMember(Value = "iOS")]
        iOS,

        /// <summary>
        /// The Android operating system.
        /// </summary>
        [EnumMember(Value = "android")]
        Android
    }
}

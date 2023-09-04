using System.Runtime.Serialization;

namespace WebhookSenderHub.Teams.Action.HttpPost
{
    /// <summary>
    /// Represents the content type for the body of an HTTP POST action in Microsoft Teams.
    /// </summary>
    public enum BodyContentType
    {
        /// <summary>
        /// Specifies the content type as application/json.
        /// </summary>
        [EnumMember(Value = "application/json")]
        Json,
        /// <summary>
        /// Specifies the content type as application/x-www-form-urlencoded.
        /// </summary>
        [EnumMember(Value = "application/x-www-form-urlencoded")]
        FormEncoded
    }
}
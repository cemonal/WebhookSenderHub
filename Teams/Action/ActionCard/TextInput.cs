using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action.ActionCard
{
    public class TextInput : Input
    {
        /// <summary>
        /// Indicates whether the text input should accept multiple lines of text
        /// </summary>
        [JsonPropertyName("isMultiline")]
        public bool Multiline { get; set; }

        /// <summary>
        /// Indicates the maximum number of characters that can be entered
        /// </summary>
        [JsonPropertyName("maxLength")]
        public int MaxLength { get; set; }
    }
}

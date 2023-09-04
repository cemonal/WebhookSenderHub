
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Discord
{
    /// <summary>
    /// Represents a Discord message card to be sent via webhook.
    /// </summary>
    public class MessageCard
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = "Spidey Bot";

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// True if this is a TTS message
        /// </summary>
        [JsonPropertyName("tts")]
        public bool TTS { get; set; }
        
        [JsonPropertyName("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();

        /// <summary>
        /// Name of thread to create (requires the webhook channel to be a forum channel)
        /// </summary>
        [JsonPropertyName("thread_name")]
        public string ThreadName { get; set; }
    }
}
using WebhookSenderHub.JsonConverters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams
{
    /// <summary>
    /// Provides extension methods for MessageCard objects to support serialization to JSON.
    /// </summary>
    internal static class MessageCardExtensions
    {
        /// <summary>
        /// Serializes the MessageCard object to a JSON string using custom JsonSerializerOptions.
        /// </summary>
        /// <param name="messageCard">The MessageCard object to be serialized.</param>
        /// <returns>A JSON string representing the serialized MessageCard object.</returns>
        public static string ToJson(this MessageCard messageCard)
        {
            var option = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase), new ActionConverter() },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(messageCard, option);
        }
    }
}
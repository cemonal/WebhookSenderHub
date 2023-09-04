using WebhookSenderHub.Teams.Action;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.JsonConverters
{
    /// <summary>
    /// A custom JSON converter for IAction objects, handling serialization of different IAction implementations.
    /// </summary>
    internal class ActionConverter : JsonConverter<IAction>
    {
        /// <summary>
        /// Deserializes JSON data to an IAction object. Currently not implemented.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when the method is called.</exception>
        public override IAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Serializes an IAction object to JSON data.
        /// </summary>
        /// <param name="writer">The JSON writer to which the IAction object is written.</param>
        /// <param name="value">The IAction object to be serialized.</param>
        /// <param name="options">The JSON serialization options.</param>
        /// <exception cref="ArgumentException">Thrown when the provided IAction object is not a recognized type.</exception>
        public override void Write(Utf8JsonWriter writer, IAction value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case ActionCardAction action:
                    JsonSerializer.Serialize(writer, action, options);
                    break;
                case HttpPostAction action:
                    JsonSerializer.Serialize(writer, action, options);
                    break;
                case OpenUriAction action:
                    JsonSerializer.Serialize(writer, action, options);
                    break;
                default:
                    throw new ArgumentException("It is not a recognized type.", nameof(value));
            }
        }
    }
}
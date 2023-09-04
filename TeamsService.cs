using WebhookSenderHub.JsonConverters;
using WebhookSenderHub.Teams;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace WebhookSenderHub
{
    /// <summary>
    /// A webhook service implementation for sending messages to Microsoft Teams.
    /// </summary>
    public sealed class TeamsService : WebhookService<MessageCard>, IMessagingService<MessageCard>
    {
        private readonly string _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsService"/> class using a URL and channel.
        /// </summary>
        /// <param name="url">The URL of the webhook endpoint.</param>
        /// <param name="channel">The name of the Teams channel to send messages to.</param>
        public TeamsService(string url, string channel = null) : base(url)
        {
            _channel = channel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsService"/> class using a URI and channel.
        /// </summary>
        /// <param name="uri">The URI of the webhook endpoint.</param>
        /// <param name="channel">The name of the Teams channel to send messages to.</param>
        public TeamsService(Uri uri, string channel = null) : base(uri)
        {
            _channel = channel;
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, string themeColor, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Text = text, Title = title, Summary = summary, ThemeColor = themeColor }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Text = text, Title = title, Summary = summary }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Text = text, Title = title }, cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<bool> SendAsync(MessageCard message, CancellationToken cancellationToken = default)
        {
            var option = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase), new ActionConverter() },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            using (var content = new StringContent(JsonSerializer.Serialize(message, option), Encoding.UTF8, "application/json"))
                return await SendAsync(content, cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<bool> SendAsync(string message, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Text = message, Channel = _channel }, cancellationToken);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using WebhookSenderHub.Slack;

namespace WebhookSenderHub
{
    /// <summary>
    /// A webhook service implementation for sending messages to Slack.
    /// </summary>
    public sealed class SlackService : WebhookService<MessageCard>, IMessagingService<MessageCard>
    {
        private readonly string _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackService"/> class using a URL and channel.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="url"/> is null or whitespace.</exception>
        public SlackService(string url, string channel = null) : base(url)
        {
            _channel = channel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackService"/> class using a URI and channel.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null or whitespace.</exception>
        public SlackService(Uri uri, string channel = null) : base(uri)
        {
            _channel = channel;
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, string themeColor, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Attachments = new System.Collections.Generic.List<Attachment> { new Attachment { Color = themeColor, Title = title, Text = text, Fallback = summary } } }, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Attachments = new System.Collections.Generic.List<Attachment> { new Attachment { Title = title, Text = text, Fallback = summary } } }, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Attachments = new System.Collections.Generic.List<Attachment> { new Attachment { Title = title, Text = text } } }, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async override Task<bool> SendAsync(string message, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard { Channel = _channel, Attachments = new System.Collections.Generic.List<Attachment> { new Attachment { Text = message } } }, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override async Task<bool> SendAsync(MessageCard message, CancellationToken cancellationToken = default)
        {
            return await base.SendAsync(message, cancellationToken).ConfigureAwait(false);
        }
    }
}
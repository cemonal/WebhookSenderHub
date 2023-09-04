using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebhookSenderHub.Discord;

namespace WebhookSenderHub
{
    /// <summary>
    /// A webhook service implementation for sending messages to Discord.
    /// </summary>
    public sealed class DiscordService : WebhookService<MessageCard>, IMessagingService<MessageCard>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordService"/> class using a URL.
        /// </summary>
        public DiscordService(string url) : base(url) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordService"/> class using a URI.
        /// </summary>
        public DiscordService(Uri uri) : base(uri) { }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, string themeColor, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard
            {
                Content = text,
                Embeds = new List<Embed> { new Embed { Title = title, Description = summary, Color = HexToInt(themeColor) } }
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, string summary, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard
            {
                Content = text,
                Embeds = new List<Embed> { new Embed { Title = title, Description = summary } }
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> SendAsync(string text, string title, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard
            {
                Content = text,
                Embeds = new List<Embed> { new Embed { Title = title } }
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<bool> SendAsync(MessageCard message, CancellationToken cancellationToken = default)
        {
            return await base.SendAsync(message, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override async Task<bool> SendAsync(string message, CancellationToken cancellationToken = default)
        {
            return await SendAsync(new MessageCard
            {
                Content = message
            }, cancellationToken);
        }

        private int? HexToInt(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return null;

            if (hex.StartsWith("#"))
                hex = hex.TrimStart(new[] { '#' });

            int result = Convert.ToInt32(hex, 16);
            return result;
        }
    }
}

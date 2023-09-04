using WebhookSenderHub.JsonConverters;
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
    /// Base class for Webhook Services. Inherit from this class to implement webhook functionality.
    /// </summary>
    public abstract class WebhookService<T> : IWebhookService<T> where T : class
    {
        private readonly Uri _uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookService"/> class with the specified URL.
        /// </summary>
        /// <param name="url">The URL of the webhook.</param>
        /// <exception cref="UriFormatException">Thrown when the <paramref name="url"/> is not a well-formed Uri.</exception>
        public WebhookService(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new UriFormatException();

            _uri = new Uri(url);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookService"/> class with the specified Uri.
        /// </summary>
        /// <param name="uri">The Uri of the webhook.</param>
        public WebhookService(Uri uri)
        {
            _uri = uri;
        }

        /// <inheritdoc/>
        public virtual async Task<bool> SendAsync(T message, CancellationToken cancellationToken = default)
        {
            // Serialize message using the default options.
            var option = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase) },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            using (var content = new StringContent(JsonSerializer.Serialize(message, option), Encoding.UTF8, "application/json"))
                return await SendAsync(content, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<bool> SendAsync(StringContent content, CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient { BaseAddress = _uri })
            {
                // Send the request and return whether it was successful.
                var response = await client.PostAsync(client.BaseAddress, content, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
        }

        /// <inheritdoc/>
        public virtual Task<bool> SendAsync(string message, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            return SendAsync(content, cancellationToken);
        }
    }
}
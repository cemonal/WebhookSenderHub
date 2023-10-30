using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebhookSenderHub
{
    /// <summary>
    /// Interface for Webhook Services. Implement this interface to create webhook functionality.
    /// </summary>
    /// <typeparam name="T">The type of message to be sent via the webhook.</typeparam>
    public interface IWebhookService<T> where T : IMessageCard
    {
        /// <summary>
        /// Sends the specified message to the webhook asynchronously.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean value indicating whether the operation succeeded or not.</returns>
        Task<bool> SendAsync(T message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the specified content to the webhook asynchronously.
        /// </summary>
        /// <param name="content">The content to be sent.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean value indicating whether the operation succeeded or not.</returns>
        Task<bool> SendAsync(StringContent content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a message to the webhook endpoint as an asynchronous operation.
        /// </summary>
        /// <param name="message">The message to send to the webhook endpoint as a string.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a boolean value indicating whether the operation succeeded or not.</returns>
        Task<bool> SendAsync(string message, CancellationToken cancellationToken = default);
    }
}

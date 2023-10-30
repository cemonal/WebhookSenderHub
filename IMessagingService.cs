using System.Threading;
using System.Threading.Tasks;

namespace WebhookSenderHub
{
    public interface IMessagingService<T> : IWebhookService<T> where T : IMessageCard
    {
        /// <summary>
        /// Sends a message with the specified text, title, summary, and theme color.
        /// </summary>
        Task<bool> SendAsync(string text, string title, string summary, string themeColor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a message with the specified text, title, and summary.
        /// </summary>
        Task<bool> SendAsync(string text, string title, string summary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a message with the specified text and title.
        /// </summary>
        Task<bool> SendAsync(string text, string title, CancellationToken cancellationToken = default);
    }
}

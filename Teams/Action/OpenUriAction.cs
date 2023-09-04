using WebhookSenderHub.Teams.Action.OpenUri;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action
{
    /// <summary>
    /// Represents an OpenUri action for Microsoft Teams message cards.
    /// The action opens a specified URL in a separate browser or app when clicked.
    /// </summary>
    public class OpenUriAction : BaseAction, IEmbeddableAction
    {
        /// <summary>
        /// Gets or sets the collection of Target objects, each defining the URL to open for a specific operating system.
        /// </summary>
        [JsonPropertyName("targets")]
        public IEnumerable<Target> Targets { get; set; }
    }
}
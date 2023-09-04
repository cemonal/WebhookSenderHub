using WebhookSenderHub.Teams.Action.ActionCard;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams.Action
{
    /// <summary>
    /// Represents an ActionCard action for Microsoft Teams message cards.
    /// The action card displays a set of inputs and actions, allowing users to interact with the card.
    /// </summary>
    public class ActionCardAction : BaseAction
    {
        /// <summary>
        /// Gets or sets the collection of Input objects, which represent the input fields displayed on the action card.
        /// </summary>
        [JsonPropertyName("inputs")]
        public IEnumerable<Input> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the collection of IEmbeddableAction objects, which represent the actions available on the action card.
        /// </summary>
        [JsonPropertyName("actions")]
        public IEnumerable<IEmbeddableAction> Actions { get; set; }
    }
}

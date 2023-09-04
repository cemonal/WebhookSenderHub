
using WebhookSenderHub.Teams.Action;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebhookSenderHub.Teams
{
    /// <summary>
    /// Represents a Section object used within a MessageCard to organize and display information.
    /// </summary>
    public class Section
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("startGroup")]
        public bool? IsStartGroup { get; set; }

        [JsonPropertyName("activityTitle")]
        public string ActivityTitle { get; set; }

        [JsonPropertyName("activitySubtitle")]
        public string ActivitySubtitle { get; set; }

        [JsonPropertyName("activityText")]
        public string ActivityText { get; set; }

        [JsonPropertyName("activityImage")]
        public string ActivityImage { get; set; }

        [JsonPropertyName("heroImage")]
        public Image HeroImage { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("facts")]
        public IEnumerable<Fact> Facts { get; set; }

        [JsonPropertyName("images")]
        public IEnumerable<Image> Images { get; set; }

        [JsonPropertyName("potentialAction")]
        public IEnumerable<IAction> Actions { get; set; }

        [JsonPropertyName("markdown")]
        public bool? MarkDown { get; set; }
    }
}

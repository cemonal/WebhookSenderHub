namespace WebhookSenderHub.Slack
{
    /// <summary>
    /// Field model for attachments in Slack messages.
    /// </summary>
    public class Field
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Short { get; set; } = false;
    }
}

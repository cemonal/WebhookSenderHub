# WebhookSenderHub

WebhookSenderHub is a .NET library that allows you to send webhook messages to various real-time messaging platforms such as Teams, Slack, and Discord using webhooks.

## Compatibility

This library is compatible with .NET Standard 2.0.

## Installation

You can install the WebhookSenderHub NuGet package using the following command in the NuGet Package Manager Console:

```shell
Install-Package WebhookSenderHub
```

## Usage

### Microsoft Teams

To send messages to Microsoft Teams using the `TeamsService`, follow these steps:

1. Install the `WebhookSenderHub` NuGet package.
2. Create an instance of the `TeamsService` class with the webhook URL and optional channel name.
3. Use the various `SendAsync` methods to send messages to Teams.

Here's an example of sending a message to Teams:

```csharp
using WebhookSenderHub;

IMessagingService messagingService = new TeamsService("your_teams_webhook_url", "optional_channel_name");

bool success = await messagingService.SendAsync("Hello from WebhookSenderHub!", "Webhook Message", "This is a sample message.", "#0078D4");

if (success)
{
    Console.WriteLine("Message sent successfully to Teams!");
}
else
{
    Console.WriteLine("Message sending failed to Teams.");
}
```

Replace `"your_teams_webhook_url"` with your actual Teams webhook URL and `"optional_channel_name"` with the desired channel name, if applicable.

Remember to customize the message content, title, summary, and theme color as needed for your use case.

For more information on Teams message formatting, refer to the [Microsoft Teams documentation](https://docs.microsoft.com/en-us/microsoftteams/platform/webhooks-and-connectors/how-to/connectors-using).


### Slack

To send messages to Slack using the `SlackService`, follow these steps:
1. Install the `WebhookSenderHub` NuGet package.
2. Create an instance of the `SlackService` class with the webhook URL and optional channel name.
3. Use the various `SendAsync` methods to send messages to Teams.

Here's an example of sending a message to Teams:

```csharp
using WebhookSenderHub;

IMessagingService messagingService = new SlackService("your_slack_webhook_url", "optional_channel_name");

bool success = await messagingService.SendAsync("Hello from WebhookSenderHub!", "Webhook Message", "This is a sample message.");

if (success)
{
    Console.WriteLine("Message sent successfully to Slack!");
}
else
{
    Console.WriteLine("Message sending failed to Slack.");
}
```
### Discord

To send messages to Discord using the `DiscordService`, follow these steps:
1. Install the `WebhookSenderHub` NuGet package.
2. Create an instance of the `DiscordService` class with the webhook URL and optional channel name.
3. Use the various `SendAsync` methods to send messages to Teams.

Here's an example of sending a message to Teams:

```csharp
using WebhookSenderHub;

IMessagingService messagingService = new DiscordService("your_discord_webhook_url");

bool success = await messagingService.SendAsync("Hello from WebhookSenderHub!", "Webhook Message", "This is a sample message.", "#FF5733");

if (success)
{
    Console.WriteLine("Message sent successfully to Discord!");
}
else
{
    Console.WriteLine("Message sending failed to Discord.");
}
```

## Contributing

Contributions are welcome! Feel free to submit issues, pull requests, or feedback in the GitHub repository.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ranker.Interfaces;
using Ranker.Responders;
using Ranker.Services;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Results;
using Remora.Results;

var cancellationSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    cancellationSource.Cancel();
};
IDataService _dataService = new DataService();

var botToken = _dataService.GetBotTokenAsync(); 

var services = new ServiceCollection()
    .AddDiscordGateway(_ => botToken.Result)
    .AddResponder<PingPongResponder>()
    .AddResponder<MessageCounter>()
    .AddSingleton<IDataService, DataService>()
    .Configure<DiscordGatewayClientOptions>(o => o.Intents |= GatewayIntents.MessageContents)
    .BuildServiceProvider();

var gatewayClient = services.GetRequiredService<DiscordGatewayClient>();
var log = services.GetRequiredService<ILogger<Program>>();

var runResult = await gatewayClient.RunAsync(cancellationSource.Token);

if (!runResult.IsSuccess)
{
    switch (runResult.Error)
    {
        case ExceptionError exe:
            {
                Console.WriteLine($"Exception during gateway connection: {exe.Message}");
                break;
            }

        case GatewayWebSocketError:
        case GatewayDiscordError:
            {
                log.LogError("Gateway error: {Message}", runResult.Error.Message);
                break;
            }
        default:
            {
                log.LogError("Unknown error: {Message}", runResult.Error.Message);
                break;
            }
    }
}

Console.WriteLine("bye bye");



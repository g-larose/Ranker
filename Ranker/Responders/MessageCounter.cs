using Ranker.Interfaces;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Responders
{
    public class MessageCounter : IResponder<IMessageCreate>
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IDataService _dataService;
        Dictionary<string, int> userMessages = new Dictionary<string, int>();
        public MessageCounter(IDiscordRestChannelAPI channelApi, IDataService dataService)
        {
            _channelApi = channelApi;
            _dataService = dataService;
        }

        public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
        {
           
            var author = gatewayEvent.Author;
            var userScore = 0;

            //if the message is from the bot , ignore it.
            if (gatewayEvent.Author.IsBot.IsDefined(out var isBot)) return Result.FromSuccess();

            author = gatewayEvent.Author;
            if (gatewayEvent.Content != null || gatewayEvent.Content != "")
            {
                if (!userMessages.ContainsKey(gatewayEvent.Author.Username))
                {
                    userMessages.Add(author.Username, 1);
                    userScore++;
                }
                else
                {
                    var score = userMessages.TryGetValue(author.Username, out userScore);
                    var user = userMessages[author.Username];
                    userMessages[author.Username] = userScore++;
                }
                return (Result)await _channelApi.CreateMessageAsync(gatewayEvent.ChannelID, $"Message Counted: score is {userScore}");
            }
                   
            return Result.FromSuccess();
        }
    }
}

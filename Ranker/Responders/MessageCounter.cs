using Ranker.Interfaces;
using Ranker.Models;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ranker.Responders
{
    public class MessageCounter : IResponder<IMessageCreate>
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IDataService _dataService;
        private string membersXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "member.xml");
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
                if (!File.Exists(membersXml))
                {
                    var newUser = new Member()
                    {
                        MemberId = author.ID.ToString(),
                        Username = author.Username,
                        Xp = 1,
                        MessageCount = 1
                    };
                    await _dataService.CreateNewXmlDocumentAsync(newUser);
                    userScore = 1;
                    return (Result)await _channelApi.CreateMessageAsync(gatewayEvent.ChannelID, $"Message Counted: score is {userScore}");
                }
                else
                {
                    var doc = XDocument.Load(membersXml);
                    var member = doc.Descendants("member")
                    .Where(x => (string)x.Attribute("username")! == author.Username)
                    .Select(x => new
                    {
                        id = x.Attribute("id")!,
                        name = x.Attribute("username")!,
                        messageCount = x.Attribute("message_count"),
                        xp = x.Attribute("xp")
                    }).FirstOrDefault();
                    userScore = int.Parse(member!.messageCount!.Value);
                    userScore++;
                    var newUser = new Member()
                    {
                        MemberId = member.id.ToString(),
                        Username = member.name.Value,
                        MessageCount = int.Parse(member.messageCount!.Value),
                        Xp = int.Parse(member!.xp!.Value)
                    };
                    var userXp = newUser.Xp += 1;
                    await _dataService.UpdateMemberMessageCountAsync(newUser);
                    return (Result)await _channelApi.CreateMessageAsync(gatewayEvent.ChannelID, $"Message Counted: message_count: {userScore} : User XP: {userXp}");
                       
                }
                
            }
                   
            return Result.FromSuccess();
        }
    }
}

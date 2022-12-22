using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Responders
{
    public class PingPongResponder : IResponder<IMessageCreate>
    {
        private readonly IDiscordRestChannelAPI _channelApi;

        public PingPongResponder(IDiscordRestChannelAPI channelApi)
        {
            _channelApi = channelApi;
        }

        public async Task<Result> RespondAsync(IMessageCreate gatewayEvent,
                                                CancellationToken ct = default)
        {
            if (gatewayEvent.Content != "!ping")
                return Result.FromSuccess();

            var embed = new Embed(Description: "Pong!", Colour: Color.PeachPuff);
            return (Result)await _channelApi.CreateMessageAsync(gatewayEvent.ChannelID, embeds: new[] { embed }, ct: ct);
        }
    }
}

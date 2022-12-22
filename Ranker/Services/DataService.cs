using Ranker.Data;
using Ranker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ranker.Services
{
    public class DataService : IDataService
    {
        public async Task<string> GetBotTokenAsync()
        {
            var tokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "config.json");
            var json = await File.ReadAllTextAsync(tokenPath);
            var token = JsonSerializer.Deserialize<ConfigJson>(json);
            return token!.Token;
        }

        public async Task<int> GetMemberMessageCountAsync(ulong id)
        {
            throw new NotImplementedException();
        }
    }
}

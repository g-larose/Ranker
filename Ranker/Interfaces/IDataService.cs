using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Interfaces
{
    public interface IDataService
    {
        Task<int> GetMemberMessageCountAsync(ulong id);
        Task<string> GetBotTokenAsync();
    }
}

using Ranker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Interfaces
{
    public interface IDataService
    {
        int GetMemberMessageCount(ulong id);
        Task<string> GetBotTokenAsync();
        Task<bool> CreateNewXmlDocumentAsync(Member member);
        Task<bool> UpdateMemberMessageCountAsync(Member member);
    }
}

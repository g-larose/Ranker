using Ranker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Interfaces
{
    public interface IMember
    {
        Task<Member> GetAll();
        Task<Member> Get(ulong id);

    }
}

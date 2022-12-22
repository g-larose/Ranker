using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Models
{
    public class Member
    {
        public ulong MemberId { get; set; }
        public string? Username  { get; set; }
        public int MessageCount { get; set; }
    }
}

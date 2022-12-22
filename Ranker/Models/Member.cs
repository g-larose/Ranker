using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranker.Models
{
    public class Member
    {
        public string? MemberId { get; set; }
        public string? Username  { get; set; }
        public int Xp { get; set; }
        public int MessageCount { get; set; }
    }
}

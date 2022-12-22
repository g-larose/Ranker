using Ranker.Data;
using Ranker.Interfaces;
using Ranker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ranker.Services
{
    public class DataService : IDataService
    {
        string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "member.xml");
        public async Task<bool> CreateNewXmlDocumentAsync(Member member)
        {
            if (File.Exists(xmlPath)) return false;
            var doc = new XDocument(
                            new XDeclaration("1.0", "utf-8", "yes"),
                            new XElement("members",
                                new XElement("member",
                                    new XAttribute("id", member.MemberId),
                                    new XAttribute("username", member.Username!),
                                    new XAttribute("message_count", member.MessageCount),
                                    new XAttribute("xp", member.Xp))));
            using (var fs = new FileStream(xmlPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, default);
            }
            
            return true;
        }

        public async Task<string> GetBotTokenAsync()
        {
            var tokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "config.json");
            var json = await File.ReadAllTextAsync(tokenPath);
            var token = JsonSerializer.Deserialize<ConfigJson>(json);
            return token!.Token!;
        }

        public int GetMemberMessageCount(ulong id)
        {
            
            if (!File.Exists(xmlPath)) return 0;
            
            var members = XDocument.Load(xmlPath);
            var messageCount = members.Descendants("member")
                                        .Where(x => x.Attribute("id")?.Value == id.ToString())
                                        .Select(x => x.Attribute("messages")?.Value)
                                        .FirstOrDefault();
            return int.Parse(messageCount!);
            
        }

        public async Task<bool> UpdateMemberMessageCountAsync(Member member)
        {
            if (!File.Exists(xmlPath)) return false;
            
            var doc = XDocument.Load(xmlPath);
            var ele = doc.Descendants("member")
                                .Where(x => x.Attribute("username")?.Value == member.Username)
                                .FirstOrDefault();
            var count = int.Parse(ele!.Attribute("message_count")!.Value);
            count += 1;
            ele!.Attribute("message_count")?.SetValue(count);
            ele!.Attribute("xp")?.SetValue(count);
            using (var fs = new FileStream(xmlPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, default);
            }

            return true;
        }
    }
}

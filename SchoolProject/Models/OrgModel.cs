using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class OrgModel
    {
        [BsonId]
        public string OrgName
        {
            get;
            set;
        }
        public string WebsiteLink
        {
            get;
            set;
        }
        public SocialModel socialModel;
        public string LogoLink
        {
            get;
            set;
        }
    }
}
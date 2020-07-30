using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class SocialModel
    {
        public string TwitterLink
        {
            get;
            set;
        }
        public string FacebookLink
        {
            get;
            set;
        }
        public string InstagramLink
        {
            get;
            set;
        }
        public string YoutubeLinks
        {
            get;
            set;
        }
        public string TwitchTeam
        {
            get;
            set;
        }
    }
}
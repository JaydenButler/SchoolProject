using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class WarningModel
    {
        public string warnReason
        {
            get;
            set;
        }
        [BsonId]
        public string dateTime
        {
            get;
            set;
        }
        public ModeratorModel moderator
        {
            get;
            set;
        }
    }
}
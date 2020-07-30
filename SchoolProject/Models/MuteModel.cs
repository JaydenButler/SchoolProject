using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class MuteModel
    {
        [BsonId]
        public DateTime muteFinished
        {
            get;
            set;
        }
        public DateTime timeMuted
        {
            get;
            set;
        }
        public bool isMuted
        {
            get;
            set;
        }
        public string duration
        {
            get;
            set;
        }
        public string reason
        {
            get;
            set;
        }
        public TargetModel target
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
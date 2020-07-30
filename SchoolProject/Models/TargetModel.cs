using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class TargetModel
    {
        public ulong id
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public int Discriminator
        {
            get;
            set;
        }
    }
}
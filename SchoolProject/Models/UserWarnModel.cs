using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class UserWarnModel
    {
        [BsonId]
        public string userId
        {
            get;
            set;
        }
        public List<WarningModel> warnings
        {
            get;
            set;
        }
    }
}
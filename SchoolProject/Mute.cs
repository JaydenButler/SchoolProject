using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolProject
{
    public class Mute
    {

        private static Mute instance;

        public static Mute Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Mute();
                }

                return instance;
            }
        }
        public Mute()
        {

        }
        public async Task CheckMutesAsync()
        {
            DateTime todayDateTime = System.DateTime.Now;
            await Task.Delay(5000);

            var recs = MongoCRUD.Instance.LoadRecords<MuteModel>("Mutes");

            foreach (var rec in recs)
            {
                if (todayDateTime >= rec.muteFinished)
                {
                    var client = DiscordBot.Instance._client;
                    var guild = client.GetGuild(rec.guildId);
                    SocketGuildUser user = (SocketGuildUser)client.GetUser(rec.target.id);
                    var role = guild.Roles.FirstOrDefault(r => r.Name == "Muted");


                    await user.RemoveRoleAsync(role);

                    Console.WriteLine("Person has been unmuted");
                }
            }
            Task.Run(CheckMutesAsync);
        }
    }
    //change socket user to its own objects or it dies
    public class MuteModel
    {
        [BsonId]
        public DateTime muteFinished { get; set; }
        public DateTime timeMuted { get; set; }
        public ulong guildId { get; set; }
        public bool isMuted { get; set; }
        public string duration { get; set; }
        public Target target { get; set; }
        public Moderator moderator { get; set; }
    }
    public class Target
    {
        public ulong id { get; set; }
        public string Username { get; set; }
        public int Discriminator { get; set; }
    }
    public class Moderator
    {
        public ulong id { get; set; }
        public string Username { get; set; }
        public int Discriminator { get; set; }
    }
}


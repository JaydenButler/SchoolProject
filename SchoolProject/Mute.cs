using System;
using System.Threading.Tasks;
using Discord.WebSocket;

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

        public async Task CheckMutesAsync()
        {
            DateTime todayDateTime = System.DateTime.Now;
            await Task.Delay(10000);

            var recs = MongoCRUD.Instance.LoadRecords<MuteModel>();

            foreach (var rec in recs)
            {
                if (todayDateTime >= rec.muteFinished)
                {
                    Console.WriteLine("User needs to be unmuted");
                }
            }
            Task.Run(CheckMutesAsync);
        }
    }
    public class MuteModel
    {
        public DateTime timeMuted { get; set; }
        public string duration { get; set; }
        public DateTime muteFinished { get; set; }
        public SocketUser target { get; set; }
        public SocketUser moderator { get; set; }
    }
}

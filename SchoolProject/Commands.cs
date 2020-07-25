using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject
{
    // This is the Commands class. We need to inherit the Context class through our Modules so we can use Context.
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // Registering a new command. Ping can be whatever, but this is what will follow the prefix we assign in the Program file. In this case, it's an !, so !ping will cause this to run.
        [Command("Ping")]
        // Every command needs to be followed with a public async Task or Task<T> function.
        public async Task PingAsync()
        {
            // Creates an EmbedBuilder, something we can use to create an Embed.            
            EmbedBuilder builder = new EmbedBuilder();

            // Gives the Embed a title, description, and side color.
            builder.WithTitle("Ping!").WithDescription("This is a really nice ping.. apparently.").WithColor(Discord.Color.Red);

            // Replies in the channel the command was used, with an empty string, non-text to speech, and using the Embed we made earlier.
            await ReplyAsync("", false, builder.Build());
        }
        [Command("set")]
        public async Task AddOrgAsync(string collection, string orgName, string twitterLink, string facebookLink,
            string instagramLink, string youtubeLink, string twitchTeam, string websiteLink, [Remainder] string logoLink)
        {
            try
            {
                SocialModel socialModel = new SocialModel
                {
                    TwitterLink = twitterLink,
                    FacebookLink = facebookLink,
                    InstagramLink = instagramLink,
                    YoutubeLinks = youtubeLink,
                    TwitchTeam = twitchTeam
                };
                OrgModel orgModel = new OrgModel { OrgName = orgName, socialModel = socialModel, WebsiteLink = websiteLink, LogoLink = logoLink };
                MongoCRUD.Instance.InsertRecord(collection, orgModel);
                await ReplyAsync("I worked");
            }
            catch
            {
                await ReplyAsync("An errror as occured. Please make sure that you have not entered an Organisation already in the Databse.");
            }
        }
        [Command("get")]
        public async Task ReadOrgAsync(string org)
        {
            var rec = MongoCRUD.Instance.LoadRecordById<OrgModel>(org, "OrgDatabase", "OrgName");
            await ReplyAsync($"__**{rec.OrgName}**__\nTwitter Link: {rec.socialModel.TwitterLink}\nFacebook Link: {rec.socialModel.FacebookLink}\n" +
                $"Instagram Link: {rec.socialModel.InstagramLink}\nTwitch Team: {rec.socialModel.TwitchTeam}\nWebsite Link: {rec.WebsiteLink}\nLogo Link {rec.LogoLink}");
        }
        [Command("orgs")]
        public async Task OrgsAsync()
        {
            var recs = MongoCRUD.Instance.LoadRecords<OrgModel>("OrgDatabase");
            StringBuilder sb = new StringBuilder();
            foreach (var rec in recs)
            {
                sb.Append($"{rec.OrgName}\n");
            }
            await ReplyAsync($"The following is the list of orgs we cater for:\n{sb.ToString()}\n*Please remember while using commands that names are case-sensitive. " +
                $"Please use the exact name seen above.*");
        }
        [Command("mute")]
        public async Task MuteAsync(SocketGuildUser targetFake, string time)
        {
            Moderator moderator = new Moderator { Username = Context.User.Username, Discriminator = Context.User.DiscriminatorValue, id = Context.User.Id};
            Target target = new Target { Username = targetFake.Username, Discriminator = targetFake.DiscriminatorValue, id = targetFake.Id };
            MuteModel muteModel = new MuteModel { timeMuted = System.DateTime.Now, isMuted = true ,moderator = moderator, target = target };
            #region Adding time shit
            if (time.Contains("d"))
            {
                var realTime = time.Substring(0, time.Length - 1);
                muteModel.duration = time;
                
                System.TimeSpan duration = new System.TimeSpan(int.Parse(realTime), 0, 0, 0);
                DateTime endMute = System.DateTime.Now.Add(duration);
                muteModel.muteFinished = endMute;
            }
            else if (time.Contains("h"))
            {
                var realTime = time.Substring(0, time.Length - 1);
                muteModel.duration = time;

                System.TimeSpan duration = new System.TimeSpan(0, int.Parse(realTime), 0, 0);
                DateTime endMute = System.DateTime.Now.Add(duration);
                muteModel.muteFinished = endMute;
            }
            else if (time.Contains("m"))
            {
                var realTime = time.Substring(0, time.Length - 1);
                muteModel.duration = time;

                System.TimeSpan duration = new System.TimeSpan(0, 0, int.Parse(realTime), 0);
                DateTime endMute = System.DateTime.Now.Add(duration);
                muteModel.muteFinished = endMute;
            }
            #endregion
            MongoCRUD.Instance.InsertRecord("Mutes", muteModel);

            await ReplyAsync("User has been muted.");
        }
        [Command("test")]
        public async Task testasync()
        {
            DateTime todayDateTime = System.DateTime.Now;
            var recs = MongoCRUD.Instance.LoadRecords<MuteModel>("Mutes");

            foreach (var rec in recs)
            {
                if (todayDateTime >= rec.muteFinished)
                {
                    await ReplyAsync("User needs to be unmuted");
                }
            }
        }
    }

}


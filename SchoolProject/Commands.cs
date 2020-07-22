using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
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
            SocialModel socialModel = new SocialModel { TwitterLink = twitterLink, FacebookLink =  facebookLink, InstagramLink = instagramLink,
            YoutubeLinks = youtubeLink, TwitchTeam = twitchTeam};
            OrgModel orgModel = new OrgModel { OrgName = orgName, socialModel = socialModel, WebsiteLink = websiteLink, LogoLink = logoLink};
            MongoCRUD.Instance.InsertRecord(collection, orgModel);
            await ReplyAsync("I worked");
        }
        [Command("get")]
        public async Task ReadOrgAsync(string org)
        {
            var rec = MongoCRUD.Instance.LoadRecordById<OrgModel>(org);
            await ReplyAsync($"Org Name: __**{rec.OrgName}**__\nTwitter Link: {rec.socialModel.TwitterLink}\nFacebook Link: <{rec.socialModel.FacebookLink}>\n" +
                $"Instagram Link: <{rec.socialModel.InstagramLink}>\nTwitch Team: <{rec.socialModel.TwitchTeam}>\nWebsite Link: <{rec.WebsiteLink}>\n Logo Link <{rec.LogoLink}>");
        }
    }
}

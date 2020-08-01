using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SchoolProject
{
    // This is the Commands class. We need to inherit the Context class through our Modules so we can use Context.
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // Registering a new command. Ping can be whatever, but this is what will follow the prefix we assign in the Program file. In this case, it's an !, so !ping will cause this to run.
        // [Command ("Ping")]
        // // Every command needs to be followed with a public async Task or Task<T> function.
        // public async Task PingAsync ()
        // {
        //     // Creates an EmbedBuilder, something we can use to create an Embed.            
        //     EmbedBuilder builder = new EmbedBuilder ();

        //     // Gives the Embed a title, description, and side color.
        //     builder.WithTitle ("Ping!").WithDescription ("This is a really nice ping.. apparently.").WithColor (Discord.Color.Red);

        //     // Replies in the channel the command was used, with an empty string, non-text to speech, and using the Embed we made earlier.
        //     await ReplyAsync ("", false, builder.Build ());
        // }

        [Command ("set")]
        public async Task AddOrgAsync (string orgName, string twitterLink, string facebookLink,
            string instagramLink, string youtubeLink, string twitchTeam, string websiteLink, [Remainder] string logoLink)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
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
                    OrgModel orgModel = new OrgModel
                    {
                        OrgName = orgName, socialModel = socialModel, WebsiteLink = websiteLink, LogoLink = logoLink
                    };
                    MongoCRUD.Instance.InsertRecord ("OrgDatabase", orgModel);
                    await ReplyAsync ("Org added.");
                }
                catch
                {
                    await ReplyAsync ("An errror as occured. Please make sure that you have not entered an Organisation already in the Database.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("set")]
        public async Task AddOr2gAsync (string orgName, string orgName2, string twitterLink, string facebookLink,
            string instagramLink, string youtubeLink, string twitchTeam, string websiteLink, string logoLink)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
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
                    OrgModel orgModel = new OrgModel
                    {
                        OrgName = orgName + " " + orgName2, socialModel = socialModel, WebsiteLink = websiteLink, LogoLink = logoLink
                    };
                    MongoCRUD.Instance.InsertRecord ("OrgDatabase", orgModel);
                    await ReplyAsync ("Org added.");
                }
                catch
                {
                    await ReplyAsync ("An errror as occured. Please make sure that you have not entered an Organisation already in the Database.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("get")]
        public async Task ReadOrgAsync (string org)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (Context.IsPrivate == true || Context.Channel.Name == "bot-commands" || staffRole != null)
            {
                var rec = MongoCRUD.Instance.LoadRecordById<OrgModel> (org, "OrgDatabase", "OrgName");
                EmbedBuilder builder = new EmbedBuilder ();
                builder.WithTitle ($"**{rec.OrgName}**").WithDescription ($"Twitter Link: {rec.socialModel.TwitterLink}\nFacebook Link: {rec.socialModel.FacebookLink}\n" +
                    $"Instagram Link: {rec.socialModel.InstagramLink}\nTwitch Team: {rec.socialModel.TwitchTeam}\nWebsite Link: {rec.WebsiteLink}\nLogo Link: {rec.LogoLink}").WithColor (Discord.Color.Red);

                if (rec.LogoLink != "N/A")
                {
                    builder.WithThumbnailUrl (rec.LogoLink);
                }

                await ReplyAsync ("", false, builder.Build ());
            }
        }
        [Command ("get")]
        public async Task ReadOrgAsync (string org1, string org2)
        {
            string org = org1 + " " + org2;
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (Context.IsPrivate == true || Context.Channel.Name == "bot-commands" || staffRole != null)
            {
                var rec = MongoCRUD.Instance.LoadRecordById<OrgModel> (org, "OrgDatabase", "OrgName");
                EmbedBuilder builder = new EmbedBuilder ();
                builder.WithTitle ($"**{rec.OrgName}**").WithDescription ($"Twitter Link: {rec.socialModel.TwitterLink}\nFacebook Link: {rec.socialModel.FacebookLink}\n" +
                    $"Instagram Link: {rec.socialModel.InstagramLink}\nTwitch Team: {rec.socialModel.TwitchTeam}\nWebsite Link: {rec.WebsiteLink}\nLogo Link: {rec.LogoLink}").WithColor (Discord.Color.Red);

                if (rec.LogoLink != "N/A")
                {
                    builder.WithThumbnailUrl (rec.LogoLink);
                }

                await ReplyAsync ("", false, builder.Build ());
            }
        }

        [Command ("rm")]
        public async Task UnmuteAsync (string orgName)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                try
                {
                    MongoCRUD.Instance.Delete<OrgModel> ("OrgDatabase", orgName);

                    await ReplyAsync ($"{orgName} has been removed from the Orgs Database.");
                }
                catch
                {
                    await ReplyAsync ("An error as occured. Please ensure you have entered the correct information and you aren't trying to remove something " +
                        "that doesn't exist.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }

        }

        [Command ("orgs")]
        public async Task OrgsAsync ()
        {
            var recs = MongoCRUD.Instance.LoadRecords<OrgModel> ("OrgDatabase");
            StringBuilder sb = new StringBuilder ();
            foreach (var rec in recs)
            {
                sb.Append ($"{rec.OrgName}\n");
            }

            EmbedBuilder builder = new EmbedBuilder ();
            builder.WithTitle ($"**Org command list**").WithDescription ($"{sb.ToString()}").WithColor (Discord.Color.Red)
                .WithFooter ("Please remember while using commands that names are case-sensitive.");
            await ReplyAsync ("", false, builder.Build ());
        }
        #region Mute related
        [Command ("mute")]
        public async Task MuteAsync (SocketGuildUser targetFake, string time, [Remainder] string reason)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {

                if (DiscordBot.Instance.currentServer != null)
                {
                ModeratorModel moderator = new ModeratorModel
                {
                Username = Context.User.Username,
                Discriminator = Context.User.DiscriminatorValue,
                id = Context.User.Id
                    };
                    TargetModel target = new TargetModel
                    {
                        Username = targetFake.Username,
                        Discriminator = targetFake.DiscriminatorValue,
                        id = targetFake.Id
                    };
                    MuteModel muteModel = new MuteModel
                    {
                        timeMuted = System.DateTime.Now,
                        isMuted = true,
                        reason = reason,
                        moderator = moderator,
                        target = target
                    };
                    #region Adding time shit
                    if (time.Contains ("d"))
                    {
                        var realTime = time.Substring (0, time.Length - 1);
                        muteModel.duration = time;

                        System.TimeSpan duration = new System.TimeSpan (int.Parse (realTime), 0, 0, 0);
                        DateTime endMute = System.DateTime.Now.Add (duration);
                        muteModel.muteFinished = endMute;
                    }
                    else if (time.Contains ("h"))
                    {
                        var realTime = time.Substring (0, time.Length - 1);
                        muteModel.duration = time;

                        System.TimeSpan duration = new System.TimeSpan (0, int.Parse (realTime), 0, 0);
                        DateTime endMute = System.DateTime.Now.Add (duration);
                        muteModel.muteFinished = endMute;
                    }
                    else if (time.Contains ("m"))
                    {
                        var realTime = time.Substring (0, time.Length - 1);
                        muteModel.duration = time;

                        System.TimeSpan duration = new System.TimeSpan (0, 0, int.Parse (realTime), 0);
                        DateTime endMute = System.DateTime.Now.Add (duration);
                        muteModel.muteFinished = endMute;
                    }
                    #endregion
                    MongoCRUD.Instance.InsertRecord ("Mutes", muteModel);

                    var muteRole = Context.Guild.Roles.FirstOrDefault (r => r.Name == "Muted");

                    await targetFake.AddRoleAsync (muteRole);

                    EmbedBuilder builder = new EmbedBuilder ();
                    builder.WithTitle ($"**You have been muted in the OEA**").WithDescription ($"Duration: {time}\nReason: {reason}").WithColor (Discord.Color.Red)
                        .WithFooter ("If you think this was an error, please contact a moderator.");
                    await targetFake.SendMessageAsync ("", false, builder.Build ());

                    await ReplyAsync ($"<@{targetFake.Id}> was muted by <@{Context.User.Id}>.\n" +
                        $"Duration: {time}\n" +
                        $"Reason: {reason}");
                }
                else
                {
                    await ReplyAsync ("Please run the setup command before using this command.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("unmute")]
        public async Task UnmuteAsync (SocketGuildUser target)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                var rec = MongoCRUD.Instance.LoadRecordById<MuteModel> (target.Id.ToString (), "Mutes", "target.id");
                rec.isMuted = false;
                MongoCRUD.Instance.InsertRecord ("OldMutes", rec);

                MongoCRUD.Instance.DeleteMute<MuteModel> (rec.muteFinished);

                var muteRole = Context.Guild.Roles.FirstOrDefault (r => r.Name == "Muted");

                await target.RemoveRoleAsync (muteRole);

                await ReplyAsync ($"<@{target.Id}> has been unmuted.");

            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("mutes")]
        public async Task MutesAsync ()
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                var recs = MongoCRUD.Instance.LoadRecords<MuteModel> ("Mutes");
                StringBuilder sb = new StringBuilder ();
                foreach (var rec in recs)
                {
                    TimeSpan timeLeft = rec.muteFinished.Subtract (DateTime.UtcNow);
                    string timeLeftTrimmed = string.Format ($"{timeLeft.Days}:{timeLeft.Hours}:{timeLeft.Minutes}:{timeLeft.Seconds}");
                    sb.Append ($"<@{rec.target.id}> - {rec.reason} - {timeLeftTrimmed}\n");
                }

                EmbedBuilder builder = new EmbedBuilder ();
                builder.WithTitle ("Active Mutes:").WithDescription (sb.ToString ()).WithColor (Discord.Color.DarkerGrey);

                await ReplyAsync ("", false, builder.Build ());
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }
        #endregion

        #region Warn related
        [Command ("warn")]
        public async Task WarnAsync (SocketGuildUser target, [Remainder] string reason)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                var recs = MongoCRUD.Instance.LoadAllRecordsById<UserWarnModel> (target.Id.ToString (), "Warnings", "_id");

                if (recs.Count != 0)
                {
                    ModeratorModel moderator = new ModeratorModel
                    {
                    Username = Context.User.Username,
                    Discriminator = Context.User.DiscriminatorValue,
                    id = Context.User.Id
                    };
                    foreach (var rec in recs)
                    {
                        rec.warnings.Add (
                            new WarningModel
                            {
                                warnReason = reason,
                                    dateTime = DateTime.Now.ToString ("F"),
                                    moderator = moderator
                            });
                        MongoCRUD.Instance.UpdateWarning<UserWarnModel> ("Warnings", rec.userId, rec);
                    }
                }
                else
                {
                    ModeratorModel moderator = new ModeratorModel
                    {
                        Username = Context.User.Username,
                        Discriminator = Context.User.DiscriminatorValue,
                        id = Context.User.Id
                    };
                    List<WarningModel> warningModel = new List<WarningModel>
                    {
                        new WarningModel
                        {
                        warnReason = reason,
                        dateTime = DateTime.Now.ToString ("F"),
                        moderator = moderator
                        }
                    };
                    UserWarnModel userWarnModel = new UserWarnModel
                    {
                        userId = target.Id.ToString (),
                        warnings = warningModel
                    };
                    MongoCRUD.Instance.InsertRecord ("Warnings", userWarnModel);
                }
                EmbedBuilder builder1 = new EmbedBuilder ();
                builder1.WithTitle ($"**You have been warned in the OEA**").WithDescription ($"Reason: {reason}").WithColor (Discord.Color.Red)
                    .WithFooter ("If you think this was an error, please contact a moderator.");

                await target.SendMessageAsync ("", false, builder1.Build ());

                EmbedBuilder builder = new EmbedBuilder ();
                builder.WithTitle ($"**{target.Username}#{target.Discriminator} has been warned.**").WithDescription ($"Reason: {reason}").WithColor (Discord.Color.Red);
                await ReplyAsync ("", false, builder.Build ());
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("warnings")]
        public async Task WarningsAsync (SocketGuildUser target)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                int amount = 0;
                var recs = MongoCRUD.Instance.LoadAllRecordsById<UserWarnModel> (target.Id.ToString (), "Warnings", "_id");

                StringBuilder sb = new StringBuilder ();
                if (recs.Count != 0)
                {
                    foreach (var rec in recs)
                    {
                        if (rec.warnings.Count () != 0)
                        {
                            for (int i = 0; i < rec.warnings.Count (); i++)
                            {
                                sb.Append ($"**Warning #{i + 1}**: {rec.warnings[i].warnReason} - {rec.warnings[i].dateTime}\n\n");
                            }
                            amount = rec.warnings.Count ();
                            EmbedBuilder builder = new EmbedBuilder ();
                            builder.WithTitle ($"**Warnings for {target.Username}#{target.Discriminator}**").WithColor (Discord.Color.Red)
                                .WithDescription (sb.ToString ()).WithThumbnailUrl (target.GetAvatarUrl ()).WithFooter ($"Total: {amount}");

                            await ReplyAsync ("", false, builder.Build ());
                        }
                        else
                        {
                            await ReplyAsync ("This user has no warnings.");
                        }

                    }

                }
                else
                {
                    await ReplyAsync ("This user has no warnings.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("warning")]
        public async Task WarningAsync (SocketGuildUser target, int warning)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                warning -= 1;
                var recs = MongoCRUD.Instance.LoadAllRecordsById<UserWarnModel> (target.Id.ToString (), "Warnings", "_id");

                if (recs.Count != 0)
                {
                    foreach (var rec in recs)
                    {
                        if (rec.warnings[warning] != null)
                        {
                            EmbedBuilder builder = new EmbedBuilder ();
                            builder.WithTitle ($"**Warning #{warning + 1} for {target.Username}#{target.Discriminator}**").WithColor (Discord.Color.Red)
                                .WithDescription ($"Reason: {rec.warnings[warning].warnReason}\n\nTime given: {rec.warnings[warning].dateTime}\n\n" +
                                    $"Moderator: {rec.warnings[warning].moderator.Username}#{rec.warnings[warning].moderator.Discriminator}").WithThumbnailUrl (target.GetAvatarUrl ());

                            await ReplyAsync ("", false, builder.Build ());
                        }
                        else
                        {
                            await ReplyAsync ("User's warning doesn't exist.");
                        }
                    }
                }
                else
                {
                    await ReplyAsync ("This user has no warnings.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("rm warning")]
        public async Task RmWarningAsync (SocketGuildUser target, int warning)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                warning -= 1;
                var recs = MongoCRUD.Instance.LoadAllRecordsById<UserWarnModel> (target.Id.ToString (), "Warnings", "_id");

                if (recs.Count != 0)
                {
                    foreach (var rec in recs)
                    {
                        rec.warnings.Remove (rec.warnings[warning]);
                        MongoCRUD.Instance.UpdateWarning<UserWarnModel> ("Warnings", rec.userId, rec);
                    }
                    await ReplyAsync ("User's warning has been removed.");
                }
                else
                {
                    await ReplyAsync ("This user has no warnings.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }
        #endregion

        [Command ("clear")]
        public async Task ClearAsync (int amount)
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                var messages = await Context.Channel.GetMessagesAsync (amount + 1).FlattenAsync ();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync (messages);
                var msg = await ReplyAsync ($"{amount} messages cleared.");

                await Task.Delay (2000);

                await msg.DeleteAsync ();
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("softban")]
        public async Task SoftbanAsync (SocketGuildUser user, [Remainder] string reason)
        {
            var user1 = Context.User as SocketGuildUser;
            var staffRole = user1.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                if (DiscordBot.Instance.currentServer != null)
                {

                    EmbedBuilder builder = new EmbedBuilder ();
                    builder.WithTitle ($"**{user.Username}#{user.Discriminator} has been soft banned.**").WithColor (Discord.Color.Red);

                    EmbedBuilder builder1 = new EmbedBuilder ();
                    builder1.WithTitle ($"**You have been kicked from the OEA**").WithDescription ($"Reason: {reason}").WithColor (Discord.Color.Red)
                        .WithFooter ("If you think this was an error, please contact a moderator.");

                    await user.SendMessageAsync ("", false, builder1.Build ());
                    await user.BanAsync ();
                    await DiscordBot.Instance.currentServer.RemoveBanAsync (user);
                    await ReplyAsync ("", false, builder.Build ());

                }
                else
                {
                    await ReplyAsync ("Please run the setup command before using this command.");
                }
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }

        [Command ("setup")]
        public async Task SetupAsync ()
        {
            var user = Context.User as SocketGuildUser;
            var staffRole = user.Roles.FirstOrDefault (x => x.Name == "Staff");
            if (staffRole != null)
            {
                DiscordBot.Instance.currentServer = Context.Guild;

                _ = Mute.Instance.CheckMutesAsync ();

                await ReplyAsync ("Setup complete.");
            }
            else
            {
                await ReplyAsync ("You do not have permission to use this command.");
            }
        }
    }
}
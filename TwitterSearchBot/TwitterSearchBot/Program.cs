using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchBot.Contracts;
using TwitterSearchBot.Impl;
using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Sheets.v4;
using System.Threading;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using TwitterSearchBot.Types;

namespace TwitterSearchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => Tweets());
            Console.ReadLine();           
        }

        static async Task Tweets()
        {

            var key = ConfigurationManager.AppSettings["ConsumerKey"];
            var secret = ConfigurationManager.AppSettings["ConsumerSecret"];

            var sheetId = "12NAfjwWQtjPhasxUmzBvI2VBPDTTjRSW5PlerYiwlYY";
            var writer = new GoogleSheetWriter(sheetId);

            var twitter = new TwitterFacade(key, secret);
            var keywords = new String[] { "#BBNaija" }; //    "#TrySearch" 

            var cts = await twitter.CreateTweetSearchInstance();

            while (true)
            {
                await GetTweets(cts, keywords, writer);
                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
        static async Task GetTweets(ITweetSearch tweetSearch, string[] keywords, IProfileSheet profileSheet)
        {
            var tweets = await tweetSearch.SearchByTagsAsync(keywords);

            if (tweets == null || tweets.Any() == false)
            {
                Console.WriteLine("...");
            }
            else
            {
                // Write profiles to sheet
                foreach(var tweet in tweets)
                {
                    profileSheet.WriteProfile(new FollowData { NumOfFollowers = tweet.user.followers_count, TwitterUsername = tweet.user.screen_name });
                }

                // Show profiles  in console
                foreach(var tweet in tweets)
                {
                    Console.WriteLine($"Name:       @{tweet.user.screen_name}");
                    Console.WriteLine($"Followers:  {tweet.user.followers_count}");
                    Console.WriteLine($"{tweet.text}");
                    Console.WriteLine("------");
                }

               
            }

        }
    }
}

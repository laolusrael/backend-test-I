using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchBot.Contracts;

namespace TwitterSearchBot.Impl
{
    public class TwitterFacade
    {
        private TwitterContext _twitterContext;
        private readonly string _twitterConsumerKey;
        private readonly string _twitterConsumerSecret;

        public TwitterFacade(string twitterConsumerKey, string twitterConsumerSecret)
        {
            _twitterConsumerKey = twitterConsumerKey;
            _twitterConsumerSecret = twitterConsumerSecret;
        }

        private async Task _InitContext()
        {

            if (_twitterContext != null)
                return;


            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = _twitterConsumerKey,
                    ConsumerSecret = _twitterConsumerSecret
                }
            };

            await auth.AuthorizeAsync();
            
            _twitterContext = new TwitterContext(auth);

        }
        public async Task<ITweetSearch> CreateTweetSearchInstance()
        {
            await _InitContext();
            return new TweetSearch(_twitterContext);

        }

        public async Task<ITweetStream> CreateTweetStreamInstance()
        {
            await _InitContext();
            return new TweetStream(_twitterContext);
        }
    }
}
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchBot.Contracts;
using TwitterSearchBot.Types;

namespace TwitterSearchBot.Impl
{
    public class TweetSearch:ITweetSearch
    {
        private readonly TwitterContext _twitterContext;
        public TweetSearch(TwitterContext twitterCtx)
        {
            _twitterContext = twitterCtx;
        }

        private static ulong _lastId;
        public async Task<List<Types.Status>> SearchByTagsAsync(string[] tags)
        {

            var searchQuery = from search in _twitterContext.Search
                                where search.Type == SearchType.Search && search.Query == string.Join(",", tags)
                                select search;
            if(_lastId != 0)
            {
                searchQuery = searchQuery.Where(r => r.SinceID == _lastId);
            }

            var searchResult = await searchQuery.SingleOrDefaultAsync();

            _lastId = searchResult.Statuses.OrderBy( s => s.CreatedAt).LastOrDefault().StatusID;

            var statuses =  searchResult  
                                    .Statuses
                                    .Select(s => new Types.Status {
                                                     created_at  = s.CreatedAt,
                                                     id          = s.ID,
                                                     id_str      = s.TweetIDs,
                                                     text        = s.Text,
                                                     truncated   = s.Truncated,
                                                     user = new Profile
                                                     {
                                                         description     = s.User.Description,
                                                         followers_count = s.User.FollowersCount,
                                                         friends_count   = s.User.FriendsCount,
                                                         id              = s.User.UserID,
                                                         id_str          = s.User.UserIdList,
                                                         @protected      = s.User.Protected,
                                                         name            = s.User.Name,
                                                         screen_name     = s.User.ScreenNameResponse,
                                                         location        = s.User.Location,
                                                         url             = s.User.Url,
                                                         email           = s.User.Email
                                                     }
                                            });

            
            return statuses.ToList();
        }
    }
}

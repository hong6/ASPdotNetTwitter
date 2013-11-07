using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;

namespace TwitterApiClient
{
    /// <summary>
    /// Provides LINQ access to Twitter API via Linq2Twitter.
    /// </summary>
    /// <remarks>
    /// Dependant on web.config appSettings params twitterConsumerKey and twitterConsumerSecret.
    //  Twitter API client oAuth settings: https://dev.twitter.com/app
    /// </remarks>
    public class LinqClient
    {
        /// <summary>
        /// Gets a Twitter search result.
        /// </summary>
        /// <param name="query">Search query.
        /// API docs: https://dev.twitter.com/docs/api/1.1/get/search/tweets
        /// </param>
        /// <returns>Tweets found by search, or null if no results.</returns>
        public List<Status> GetTweetsBySearch(string query)
        {
            var auth = new WebAuthorizer
            {
                Credentials = new SessionStateCredentials()
            };

            var twitterCtx = new TwitterContext(auth);

            var searchResults =
                from search in twitterCtx.Search
                where search.Type == SearchType.Search && search.Query == query
                select search;

            var searched = searchResults.SingleOrDefault();

            if (searched != null)
            {
                return searched.Statuses;
            }

            return null;
        }
    }
}
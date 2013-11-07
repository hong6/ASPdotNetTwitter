using System;
using System.Net;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Diagnostics;

namespace TwitterApiProxy.Controllers
{
    /// <summary>
    /// Provides a proxy for Twitter API 1.1 to facilitate client-side implementation (with no oAuth to emulate API 1.0).
    /// Website: http://www.timacheson.com/blog/2013/jul/twitter_api_proxy
    /// </summary>
    public class TwitterApiProxyController : Controller
    {
        [OutputCache(Duration = 120, VaryByParam = "screen_name;count;excludeReplies;includeRTs", VaryByHeader = "Referrer")]
        public ActionResult UserTimeline()
        {
            if (!IsAuthorisedReferrer())
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new ErrorContainer("Unauthorised."), JsonRequestBehavior.AllowGet);
            }

            string tweets = GetUserTimelineJson();

            return tweets == null ? null : Content(tweets, "application/json", Encoding.UTF8);                 
           
        }

        [OutputCache(Duration = 120, VaryByParam = "q;max_id;count", VaryByHeader = "Referrer")]
        public ActionResult Search()
        {
            if (!IsAuthorisedReferrer())
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new ErrorContainer("Unauthorised."), JsonRequestBehavior.AllowGet);
            }

            string tweets = GetSearchJson();

            return tweets == null ? null : Content(tweets, "application/json", Encoding.UTF8);
        }

        #region Security

        private bool IsAuthorisedReferrer()
        {
            // TODO: Consider increasing protection, to prevent abuse of proxy API from harming the web app, e.g. by more stringent referrer restrictions, token-based authentication, cap cache/outputcache, etc.
            return IsRequestFromCurrentWebsite();
        }

        private bool IsRequestFromCurrentWebsite()
        {
            if (Request.UrlReferrer != null && Request.Url != null && Request.UrlReferrer.Host == Request.Url.Host)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Business logic

        private string GetUserTimelineJson()
        {
            string tweets = null;

            try
            {
                var twitterApi = new TwitterApiClient.Client();
                string bearerToken;

                if (HttpContext.Cache["TwitterAPIBearerToken2"] != null)
                {
                    bearerToken = HttpContext.Cache["TwitterAPIBearerToken2"] as string;
                }
                else
                {
                    bearerToken = twitterApi.GetBearerToken();
                    if (bearerToken != null)
                    {
                        CacheBearerToken2(bearerToken);
                    }
                }

                if (Request.Url != null && !string.IsNullOrWhiteSpace(Request.Url.Query))
                {                  
                    tweets = twitterApi.GetUserTimelineJson(bearerToken, Request.Url.Query);                    
                }
            }
            catch
            {
            }

            return tweets;
        }

        private string GetSearchJson()
        {
            string tweets = null;

            try
            {
                var twitterApi = new TwitterApiClient.Client();
                string bearerToken;

                if (HttpContext.Cache["TwitterAPIBearerToken"] != null)
                {
                    bearerToken = HttpContext.Cache["TwitterAPIBearerToken"] as string;
                }
                else
                {
                    bearerToken = twitterApi.GetBearerToken();
                    if (bearerToken != null)
                    {
                        CacheBearerToken(bearerToken);
                    }
                }

                if (Request.Url != null && !string.IsNullOrWhiteSpace(Request.Url.Query))
                {
                    tweets = twitterApi.GetSearchJson(bearerToken, Request.Url.Query);
                }
            }
            catch
            {
            }

            return tweets;
        }

        #endregion

        #region Helper methods

        private void CacheBearerToken(string bearerToken)
        {
            // Bearer token currently never expires, but Twitter reccomends checking every 15 mins in case that policy changes.
            HttpContext.Cache.Add("TwitterAPIBearerToken", bearerToken, null, DateTime.Now.AddMinutes(14), Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
        }

        private void CacheBearerToken2(string bearerToken)
        {
            // Bearer token currently never expires, but Twitter reccomends checking every 15 mins in case that policy changes.
            HttpContext.Cache.Add("TwitterAPIBearerToken2", bearerToken, null, DateTime.Now.AddMinutes(14), Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
        }

        #endregion

        #region Entities

        public class ErrorContainer
        {
            public ErrorContainer(string message)
            {
                Error = new Error { Message = message };
            }

            public Error Error { get; set; }
        }

        public class Error
        {
            public string Message { get; set; }
        }

        #endregion
    }
}

//http://www.timacheson.com/Blog/2013/jul/twitter_api_proxy
//var query = "timacheson";

//var twitterApiClient = new TwitterApiClient.Client();

//var bearerToken = twitterApiClient.GetBearerToken();

//var tweets = twitterApiClient.GetUserTimelineJson(bearerToken, query);

//Debug.WriteLine(tweets);
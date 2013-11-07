using System.Web.Mvc;

namespace TwitterAPIProxy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Tweets from Twitter API via a proxy API:";

            return View();
        }

        public ActionResult UserTweets(string id)
        {
            if (id == null || id == "")
            {
                id = "clubmonaco";
            }

            ViewBag.Message = "Tweets from " + id;
            ViewBag.screen_name = id;

            return View();
        }
    }
}
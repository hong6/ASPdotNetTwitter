ASP.Net Twitter 1.0
===================

* A client for Twitter API 1.1.
* Demo includes a proxy API making it easy to use Twitter API in JavaScript code (like Twitter API 1.0 before oAuth was required).

AUTHOR: Tim Acheson
Web: www.timacheson.com


Quick start guide:
------------------

Instead of calling the Twitter API directly, your JavaScript code can call an API proxy like this one.

The proxy behaves exactly like the real Twitter API, and just passes requests on to the real Twitter API and hands back the response. Except that the proxy API handles the oAuth authentication, so your JavaScript code can use the API and ignore oAuth.

Ultimately you only really need these two code files, e.g. if you're using a different version of .NET, or porting the code to a different platform:

1) A simple Twitter API client class which connects to the Twitter API:
ASPdotNetTwitter.zip\TwitterAPIClient\Client.cs

2) A simple web page which acts as a mirror of the Twitter API:
ASPdotNetTwitter.zip\TwitterApiProxy\Controllers\TwitterApiProxyController.cs

The Twitter API client handles oAuth authentication behind the scenes. Apart from that, it just calls the Twitter API and returns the raw response.

The web page acting as a proxy for the Twitter API is just outputting pure raw data from the Twitter API by Response.Write. It could be an ASP.NET MVC controller like my example, but a WebForms aspx page could obviously do the same job.

Thus, an API request to the proxy API is passed on to the Twitter API as efficiently as possible.



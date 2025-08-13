using Microsoft.Extensions.Logging;
using Snickler.RSSCore.Models;
using Snickler.RSSCore.Providers;
using System.ServiceModel.Syndication;
using System.Xml;


namespace UbytkacBackend.ServerCoreDBSettings {

    internal static class RSSLoad {

        /// <summary>
        /// Load RSS Feed
        /// </summary>
        /// <param name="rssUrl"></param>
        /// <returns></returns>
        public static IEnumerable<SyndicationItem> GetRssFeed(string rssUrl) {
            var reader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(reader);
            var posts = feed.Items;
            return posts;
        }
    }

    /// <summary>
    /// RSS Provider
    /// </summary>
    public class SomeRSSProvider : IRSSProvider {

        public Task<IList<RSSItem>> RetrieveSyndicationItems() {
            IList<RSSItem> syndicationList = new List<RSSItem>();
            var synd1 = new RSSItem() {
                Title = DataOperations.RemoveDiacritism("IT Řešení v nejmodenější podobě jaké jinde Nenajdete"),
                PermaLink = new Uri(ServerConfigSettings.ServerPublicUrl),
                LinkUri = new Uri(ServerConfigSettings.ServerPublicUrl),
                LastUpdated = DateTime.Now,
                PublishDate = DateTime.Now,
                CommentsUri = new Uri(ServerConfigSettings.ServerPublicUrl),
                Content = "Novinky",
                FeaturedImage = new Uri(ServerConfigSettings.ServerPublicUrl + "/logo")
            };
            syndicationList.Add(synd1);
            return Task.FromResult(syndicationList);
        }
    }

    /// <summary>
    /// Server Restart Api for Remote Control
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    /// [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("rss", Name = "rss")]
    [Route("rss.xml", Name = "rssxml")]
    public class ServerRssFeedApi : ControllerBase {
        private readonly ILogger logger;

        public ServerRssFeedApi(ILogger<ServerRssFeedApi> _logger) => logger = _logger;

        [HttpGet("/rss.xml")]
        public ActionResult IndexRss() {
            try {
                if (ServerConfigSettings.WebRSSFeedsEnabled) {
                    var feed = new SyndicationFeed("Nazev", "Popisek", new Uri(ServerConfigSettings.ServerPublicUrl), "RSSUrl", DateTime.Now) {
                        Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Libor Svoboda")
                    };
                    var items = new List<SyndicationItem>();
                    var postings = ServerModulesExtensions.GetItemRssList();
                    foreach (var item in postings) {
                        var postUrl = Url.Action("Produkty", "Vyvoj", new { id = item.UrlSlug }, HttpContext.Request.Scheme);
                        var title = item.Title;
                        var description = item.Description;
                        items.Add(new SyndicationItem(title, description, new Uri(postUrl), item.UrlSlug, item.CreatedDate));
                    }
                    feed.Items = items;
                    var settings = new XmlWriterSettings {
                        Encoding = Encoding.UTF8,
                        NewLineHandling = NewLineHandling.Entitize,
                        NewLineOnAttributes = true,
                        Indent = true
                    };
                    using (var stream = new MemoryStream()) {
                        using (var xmlWriter = XmlWriter.Create(stream, settings)) {
                            var rssFormatter = new Rss20FeedFormatter(feed, false);
                            rssFormatter.WriteTo(xmlWriter);
                            xmlWriter.Flush();
                        }
                        return File(stream.ToArray(), MimeTypes.GetMimeType("rss.xml"), "rss.xml");
                    }
                }
                else { return BadRequest(); }
            } catch (Exception ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }
            return BadRequest();
        }
    }
}
using SimpleMvcSitemap;

namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// robots.txt routing
    /// </summary>
    /// <seealso cref="Controller"/>
    [Route("robots", Name = "robot")]
    [Route("robots.txt", Name = "robottxt")]
    public class RobotsController : ControllerBase {

        [HttpGet("/robots.txt")]
        public ActionResult Index() {
            try {
                string data = "";
                if (ServerConfigSettings.WebRobotTxtFileEnabled) {
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    })) { data = new hotelsContext().WebPageSettingLists.Where(a => a.Key == "WebRobotTxtFile").First().Value; }
                }
                return new ContentResult { Content = data, ContentType = MimeTypes.GetMimeType("robots.txt") };
            } catch (Exception ex) {
                return new ContentResult { Content = DataOperations.GetUserApiErrMessage(ex), ContentType = MimeTypes.GetMimeType("robots.txt") };
            }
        }
    }

    /// <summary>
    /// Sitemap.xml Routing
    /// </summary>
    /// <seealso cref="Controller"/>
    [Route("sitemap", Name = "sitemap")]
    [Route("sitemap.xml", Name = "sitemapxml")]
    [Route("sitemap_index.xml", Name = "sitemapindex")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SitemapController : ControllerBase {
        private readonly ISitemapProvider sitemapProvider;
        private SitemapDataBuilder dataBuilder;

        public SitemapController(ISitemapProvider sitemapProvider) {
            this.sitemapProvider = sitemapProvider;
            dataBuilder = new SitemapDataBuilder();
        }

        [HttpGet("/sitemap.xml")]
        public ActionResult IndexSiteMap() {
            //Sitemap Types
            return sitemapProvider.CreateSitemapIndex(new SitemapIndexModel(new List<SitemapIndexNode>
            {
                !ServerConfigSettings.WebSitemapFileEnabled ? null : new SitemapIndexNode(Url.Action("WebPages")),
                //new SitemapIndexNode(Url.Action("Images")),
                //new SitemapIndexNode(Url.Action("Videos")),
                //new SitemapIndexNode(Url.Action("News")),
                //new SitemapIndexNode(Url.Action("Translation")),
                //new SitemapIndexNode(Url.Action("StyleSheet")),
                //new SitemapIndexNode(Url.Action("Huge")),
            }));
        }

        [Route("webpages")]
        public ActionResult WebPages() {
            List<HotelList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().HotelLists.Where(a => a.Advertised).ToList(); }
            List<SitemapNode> webpagesUrls = new();

            string requestUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            data.ForEach(webMenu => { webpagesUrls.Add(new SitemapNode(requestUrl + webMenu.Id + "-" + webMenu.Name.Replace(" ", string.Empty)) { LastModificationDate = DateTime.UtcNow.ToLocalTime(),Url = ServerConfigSettings.ServerPublicUrl, ChangeFrequency = ChangeFrequency.Weekly, Priority = 0.8M }); });
            sitemapProvider.CreateSitemap(new SitemapModel(webpagesUrls));
            return sitemapProvider.CreateSitemap(new SitemapModel(webpagesUrls));
        }

        [Route("images")]
        public ActionResult Images() {
            return sitemapProvider.CreateSitemap(new SitemapModel(new List<SitemapNode>
            {
                dataBuilder.CreateSitemapNodeWithImageRequiredProperties(),
                dataBuilder.CreateSitemapNodeWithImageAllProperties()
            }));
        }

        [Route("videos")]
        public ActionResult Videos() {
            return sitemapProvider.CreateSitemap(new SitemapModel(new List<SitemapNode>
            {
                dataBuilder.CreateSitemapNodeWithVideoRequiredProperties(),
                dataBuilder.CreateSitemapNodeWithVideoAllProperties()
            }));
        }

        [Route("news")]
        public ActionResult News() {
            return sitemapProvider.CreateSitemap(new SitemapModel(new List<SitemapNode>
            {
                dataBuilder.CreateSitemapNodeWithNewsRequiredProperties(),
                dataBuilder.CreateSitemapNodeWithNewsAllProperties()
            }));
        }

        [Route("translation")]
        public ActionResult Translation() {
            return sitemapProvider.CreateSitemap(dataBuilder.CreateSitemapWithTranslations());
        }

        [Route("stylesheet")]
        public ActionResult StyleSheet() {
            return sitemapProvider.CreateSitemap(dataBuilder.CreateSitemapWithSingleStyleSheet());
        }

        [Route("huge")]
        public ActionResult Huge() {
            return sitemapProvider.CreateSitemap(dataBuilder.CreateHugeSitemap());
        }

        //[Route("sitemapcategories")]
        //public ActionResult Categories()
        //{
        //    return _sitemapProvider.CreateSitemap(_builder.BuildSitemapModel());
        //}

        //[Route("sitemapbrands")]
        //public ActionResult Brands()
        //{
        //    return _sitemapProvider.CreateSitemap(_builder.BuildSitemapModel());
        //}

        //public ActionResult Products(int? currentPage)
        //{
        //    IQueryable<Product> dataSource = _products.Where(item => item.Status == ProductStatus.Active);
        //    ProductSitemapIndexConfiguration configuration = new ProductSitemapIndexConfiguration(Url, currentPage);

        //    return _sitemapProvider.CreateSitemap(dataSource, configuration);
        //}

        //public ActionResult StaticPages(int? id)
        //{
        //    IQueryable<string> urls = new List<string> { "/1", "/1", "/1", "/1", "/1" }.AsQueryable();
        //    return _sitemapProvider.CreateSitemap(urls, new SitemapIndexConfiguration(id, Url));
        //}
    }
}
using UbytkacBackend.ServerCoreStructure;

namespace UbytkacBackend.DBModel {

    public class WebUrlRequest {
        public string Url { get; set; }
    }

    public class RssPost {
        public string Title { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class StaticFilesRequest {
        public string Website { get; set; }
        public string Path { get; set; }
        public bool Recursive { get; set; } = false;
    }



    public class ServerModulesExtensions {

        internal static IEnumerable<RssPost> GetItemRssList() {
            var posts = new List<RssPost>();
            try {
                List<HotelList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                })) { data = new hotelsContext().HotelLists.OrderBy(a => a.Name).ToList(); }

                data.ForEach(item => {
                    posts.Add(new RssPost() {
                        Title = item.Name,
                        UrlSlug = item.Name,
                        Description = item.DescriptionCz ?? "",
                        CreatedDate = item.Timestamp
                    });
                });
            } catch (Exception ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }
            return posts;
        }
    }
}
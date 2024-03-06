namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi")]
    public class InfoPagesApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/PrivacyPolicy/{language}")]
        public async Task<string> GetPrivacyPolicy(string language = null) {
            List<PrivacyPolicyList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().PrivacyPolicyLists.OrderBy(a => a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/Terms/{language}")]
        public async Task<string> GetTermsInfo(string language = null) {
            List<TermsList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().TermsLists.OrderBy(a => a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/UbytkacInfo/{language}")]
        public async Task<string> GetUbytkacInfo(string language = null) {
            List<UbytkacInfoList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().UbytkacInfoLists.OrderBy(a=> a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        [HttpGet("/WebApi/RegistrationInfo/{language}")]
        public async Task<string> GetRegistrationInfo(string language = null) {
            List<RegistrationInfoList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().RegistrationInfoLists.OrderBy(a => a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/OftenQuestion/{language}")]
        public async Task<string> GetOftenQuestion(string language = null) {
            List<OftenQuestionList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().OftenQuestionLists.OrderBy(a => a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/HolidayTips/{language}")]
        public async Task<string> GetHolidayTips(string language = null) {
            List<HolidayTipsList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HolidayTipsLists.OrderBy(a => a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionEn != null ? item.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
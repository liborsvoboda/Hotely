namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi")]
    public class InfoPagesApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/UbytkacInfo/{language}")]
        public async Task<string> GetUbytkacInfo(string language = null) {
            List<UbytkacInfoList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().UbytkacInfoLists.OrderBy(a=> a.Sequence).ToList();
            }

            result.ForEach(item => {
                item.DescriptionCz = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                item.DescriptionEn = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
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
                item.DescriptionEn = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
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
                item.DescriptionEn = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
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
                item.DescriptionEn = item.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
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
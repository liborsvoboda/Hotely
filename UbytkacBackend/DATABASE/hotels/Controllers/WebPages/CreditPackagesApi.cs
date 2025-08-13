using Stripe;

namespace UbytkacBackend.Controllers {


    [ApiController]
    [Route("WebApi/WebPages")]
    public class CreditPackagesApi : ControllerBase {

        [HttpGet("/WebApi/Credits/GetCreditPackages")]
        [Consumes("application/json")]
        public async Task<string> GetCreditPackages() {
            List<CreditPackageList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().CreditPackageLists.Where(a => a.Active)
                        .OrderBy(a=>a.Sequence).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }



        /// <summary>
        /// PRICE MUST BE *100  Calculated in CENT
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/WebApi/Credits/PaymentProcess")]
        [Consumes("application/json")]
        public string Create(PaymentIntentCreateRequest request) {

            StripeConfiguration.ApiKey = ServerConfigSettings.StripeRunInSharpMode
                ? ServerConfigSettings.StripeSharpSecretApiKey : ServerConfigSettings.StripeTestSecretApiKey;

            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions {
                Amount = (long)request.Price,
                Currency = "czk", 
            });
            return JsonSerializer.Serialize(new { clientSecret = paymentIntent.ClientSecret }, 
                new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        //private int CalculateOrderAmount(Item[] items) {
        //    return 140000;
        //}


        public class Item {
            [Newtonsoft.Json.JsonProperty("id")]
            public string Id { get; set; }
        }
        public class PaymentIntentCreateRequest {
            [Newtonsoft.Json.JsonProperty("items")]
            public Item[] Items { get; set; }
            public double Price { get; set; }
        }
    }
}

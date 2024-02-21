namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessCreditNoteList")]
    public class BusinessCreditNoteListApi : ControllerBase {

        [HttpGet("/BusinessCreditNoteList")]
        public async Task<string> GetBusinessCreditNoteList() {
            List<BusinessCreditNoteList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().BusinessCreditNoteLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/BusinessCreditNoteList/Filter/{filter}")]
        public async Task<string> GetBusinessCreditNoteListByFilter(string filter) {
            List<BusinessCreditNoteList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().BusinessCreditNoteLists.FromSqlRaw("SELECT * FROM BusinessCreditNoteList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/BusinessCreditNoteList/{id}")]
        public async Task<string> GetBusinessCreditNoteListKey(int id) {
            BusinessCreditNoteList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().BusinessCreditNoteLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessCreditNoteList")]
        [Consumes("application/json")]
        public async Task<string> InsertBusinessCreditNoteList([FromBody] BusinessCreditNoteList record) {
            try {
                //Increase and update Last Document Number
                SystemDocumentAdviceList documentAdvice = new SystemDocumentAdviceList(); string lastDocumentNumber = string.Empty;
                documentAdvice = new hotelsContext().SystemDocumentAdviceLists.Where(a => a.DocumentType == "creditNote" && (a.StartDate == null || a.StartDate <= DateTime.UtcNow.Date) && (a.EndDate == null || a.EndDate >= DateTime.UtcNow.Date)).FirstOrDefault();
                if (documentAdvice != null) {
                    documentAdvice.Number = (int.Parse(documentAdvice.Number) + 1).ToString("D" + documentAdvice.Number.Length.ToString());
                    lastDocumentNumber = documentAdvice.Prefix + documentAdvice.Number;
                    var documentData = new hotelsContext().SystemDocumentAdviceLists.Update(documentAdvice);
                    await documentData.Context.SaveChangesAsync();
                    record.DocumentNumber = lastDocumentNumber;
                }

                //Load Credit Note Status
                BusinessPaymentStatusList creditNoteStatus = new hotelsContext().BusinessPaymentStatusLists.Where(a => a.CreditNote).FirstOrDefault();

                //Update Invoice Identificator And Credit Note Status
                BusinessOutgoingInvoiceList parentInvoice = new BusinessOutgoingInvoiceList();
                parentInvoice = new hotelsContext().BusinessOutgoingInvoiceLists.Where(a => a.DocumentNumber == record.InvoiceNumber).First();
                if (creditNoteStatus != null) { parentInvoice.PaymentStatusId = creditNoteStatus.Id; } //Automatic Credit Note status

                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().BusinessCreditNoteLists.Add(record);
                int result = await data.Context.SaveChangesAsync();

                //Insert CreditNoteId to Invoice
                parentInvoice.CreditNoteId = record.Id;
                var invoiceData = new hotelsContext().BusinessOutgoingInvoiceLists.Update(parentInvoice);
                await invoiceData.Context.SaveChangesAsync();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = lastDocumentNumber, RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/BusinessCreditNoteList")]
        [Consumes("application/json")]
        public async Task<string> UpdateBusinessCreditNoteList([FromBody] BusinessCreditNoteList record) {
            try {
                var data = new hotelsContext().BusinessCreditNoteLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = record.DocumentNumber, RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessCreditNoteList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteBusinessCreditNoteList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                string docNumber = null;
                docNumber = new hotelsContext().BusinessCreditNoteLists.First(a => a.Id == int.Parse(id)).DocumentNumber;
                BusinessCreditNoteList record = new() { Id = int.Parse(id), DocumentNumber = docNumber };

                var data = new hotelsContext().BusinessCreditNoteLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}
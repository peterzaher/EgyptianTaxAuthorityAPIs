namespace Domain.ResponseModels;

public interface IDocumentStatusModel
{
	string DateTimeIssued { get; set; }
	string DateTimeReceived { get; set; }
	string InternalId { get; set; }
	IList<IInvoiceLineItemCodesModel> InvoiceLineItemCodes { get; set; }
	string IssuerId { get; set; }
	string IssuerName { get; set; }
	string LongId { get; set; }
	decimal NetAmount { get; set; }
	string ReceiverId { get; set; }
	string receiverName { get; set; }
	string Status { get; set; }
	string SubmissionUUID { get; set; }
	decimal Total { get; set; }
	decimal TotalDiscount { get; set; }
	decimal TotalSales { get; set; }
	string TypeName { get; set; }
	string TypeVersionName { get; set; }
	string Uuid { get; set; }
	IValidationResultModel ValidationResults { get; set; }
}
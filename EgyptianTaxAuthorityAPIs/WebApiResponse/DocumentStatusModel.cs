using System.Collections.Generic;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse;

public class DocumentStatusModel : IDocumentStatusModel
{
	public string Uuid { get; set; }
	public string SubmissionUUID { get; set; }
	public string LongId { get; set; }
	public string InternalId { get; set; }
	public string TypeName { get; set; }
	public string TypeVersionName { get; set; }
	public string IssuerId { get; set; }
	public string IssuerName { get; set; }
	public string ReceiverId { get; set; }
	public string receiverName { get; set; }
	public string DateTimeIssued { get; set; }
	public string DateTimeReceived { get; set; }
	public decimal TotalSales { get; set; }
	public decimal TotalDiscount { get; set; }
	public decimal NetAmount { get; set; }
	public decimal Total { get; set; }
	public string Status { get; set; }
	public IList<IInvoiceLineItemCodesModel> InvoiceLineItemCodes { get; set; }
	public IValidationResultModel ValidationResults { get; set; }
}

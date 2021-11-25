using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse;

public class InvoiceLineItemCodesModel : IInvoiceLineItemCodesModel
{
	public int CodeTypeId { get; set; }
	public string CodeTypeNamePrimaryLang { get; set; }
	public string CodeTypeNameSecondaryLang { get; set; }
	public string ItemCode { get; set; }
	public string CodeNamePrimaryLang { get; set; }
	public string CodeNameSecondaryLang { get; set; }
}

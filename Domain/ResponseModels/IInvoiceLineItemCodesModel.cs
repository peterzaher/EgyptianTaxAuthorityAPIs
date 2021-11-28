namespace Domain.ResponseModels;

public interface IInvoiceLineItemCodesModel
{
	string CodeNamePrimaryLang { get; set; }
	string CodeNameSecondaryLang { get; set; }
	int CodeTypeId { get; set; }
	string CodeTypeNamePrimaryLang { get; set; }
	string CodeTypeNameSecondaryLang { get; set; }
	string ItemCode { get; set; }
}
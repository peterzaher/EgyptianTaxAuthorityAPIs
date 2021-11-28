using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EInvoicing")]

namespace Domain.DocumentModels;


public interface IDocumentModel
{
	string DateTimeIssued { get; set; }
	IDeliveryModel Delivery { get; set; }
	string DocumentType { get; }
	string DocumentTypeVersion { get; }
	decimal ExtraDiscountAmount { get; set; }
	string InternalID { get; set; }
	IList<IInvoiceLineModel> InvoiceLines { get; set; }
	IIssuerReceiverInfoModel Issuer { get; set; }
	decimal NetAmount { get; }
	IPaymentModel Payment { get; set; }
	string ProformaInvoiceNumber { get; set; }
	string PurchaseOrderDescription { get; set; }
	string PurchaseOrderReference { get; set; }
	IIssuerReceiverInfoModel Receiver { get; set; }
	string SalesOrderDescription { get; set; }
	string SalesOrderReference { get; set; }
	List<ISignatureModel> Signatures { get; set; }
	string TaxPayerActivityCode { get; }
	List<ITaxTotalsModel> TaxTotals { get; }
	decimal TotalAmount { get; }
	decimal TotalDiscountAmount { get; }
	decimal TotalItemsDiscountAmount { get; }
	decimal TotalSalesAmount { get; }
}
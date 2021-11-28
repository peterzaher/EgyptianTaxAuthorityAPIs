using Domain.Enum;

namespace Domain.DocumentModels;

public interface IInvoiceLineModel
{
	string Description { get; set; }
	IDiscountModel Discount { get; set; }
	string InternalCode { get; set; }
	string ItemCode { get; }
	decimal ItemsDiscount { get; set; }
	string ItemType { get; }
	decimal NetTotal { get; }
	decimal Quantity { get; set; }
	decimal SalesTotal { get; }
	IList<ITaxableItemModel> TaxableItems { get; set; }
	decimal Total { get; }
	decimal TotalTaxableFees { get; set; }
	UnitTypeCode UnitType { get; set; }
	IUnitValueModel UnitValue { get; set; }
	decimal ValueDifference { get; set; }
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class InvoiceLineModel
	{
		//external code part
		private const string _externalCode = "EG-204961254-";
		private const string _codeType = "EGS";
		private DiscountModel _discount;
		private IList<TaxableItemModel> _taxableItems;

		public InvoiceLineModel(string itemInternalCode, decimal quantity, decimal unitValue, UnitTypeCode unitType = UnitTypeCode.EA)
		{
			InternalCode = itemInternalCode.Trim();
			ItemCode = _externalCode + InternalCode;
			ItemType = _codeType;
			Quantity = quantity;
			UnitValue = new UnitValueModel(unitValue);
			UnitType = unitType;
			TaxableItems = new List<TaxableItemModel>();
		}
		public string InternalCode { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("itemType")]
		public string ItemType { get; }

		[JsonPropertyName("itemCode")]
		public string ItemCode { get; }

		[JsonPropertyName("unitType")]
		public UnitTypeCode UnitType { get; set; }// = "EA"; //Other values: "KGM"/"MTQ"

		[JsonPropertyName("quantity")]
		public decimal Quantity { get; set; }

		[JsonPropertyName("unitValue")]
		public UnitValueModel UnitValue { get; set; }

		// item price * quantity

		[JsonPropertyName("salesTotal")]
		public decimal SalesTotal => UnitValue.AmountEGP * Quantity;

		[JsonPropertyName("total")]
		public decimal Total => SalesTotal + GetTaxTotal() - (Discount?.Amount ?? 0M);

		[JsonPropertyName("valueDifference")]
		public decimal ValueDifference { get; set; }

		[JsonPropertyName("totalTaxableFees")]
		public decimal TotalTaxableFees { get; set; }

		[JsonPropertyName("discount")]
		public DiscountModel Discount
		{
			set => _discount = value;
			get {
				//if (discount == null) discount = new DiscountModel(0);
				if (_discount != null)
				{
					_discount.Amount = SalesTotal * _discount.Rate / 100M;
				}
				return _discount;
			}
		}

		[JsonPropertyName("netTotal")]
		public decimal NetTotal => SalesTotal - (Discount?.Amount ?? 0M);

		[JsonPropertyName("itemsDiscount")]
		public decimal ItemsDiscount { get; set; } //non-taxable items discount

		[JsonPropertyName("taxableItems")]
		public IList<TaxableItemModel> TaxableItems
		{
			get {
				foreach (TaxableItemModel item in _taxableItems)
				{
					item.Amount = NetTotal * item.Rate / 100;
				}
				return _taxableItems;
			}
			set => _taxableItems = value;
		}

		private decimal GetTaxTotal()
		{
			if (TaxableItems.Count == 0)
			{
				return 0M;
			}
			decimal taxTotal = 0M;
			foreach (TaxableItemModel item in TaxableItems)
			{
				taxTotal += item.Amount;
			}
			return taxTotal;
		}
	}
}

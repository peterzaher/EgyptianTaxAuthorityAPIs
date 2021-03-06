using DataAccess;
using Domain.DocumentModels;
using EInvoicing.WebApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EInvoicing.DocumentComponent;

public class DocumentModel : IDocumentModel
{
	//activityCode: 3100
	private string _dateTimeIssued;

	public DocumentModel(IIssuerReceiverInfoModel issuer, IIssuerReceiverInfoModel receiver, IList<IInvoiceLineModel> invoices, string invoiceInternalId)
	{
		Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer), "Issuer cannot be null");
		Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver), "Receiver cannot be null");
		InvoiceLines = invoices ?? throw new ArgumentNullException(nameof(invoices), "Invoices cannot be null");
		InternalID = string.IsNullOrEmpty(invoiceInternalId) ? throw new ArgumentNullException(nameof(invoiceInternalId), "Internal invoice Id cannot be null") : invoiceInternalId;
		//InternalID = invoiceInternalId;
	}

	[JsonPropertyName("documentType")]
	public string DocumentType { get; } = "i";

	[JsonPropertyName("documentTypeVersion")]
	public string DocumentTypeVersion { get; } = "1.0";

	[JsonPropertyName("dateTimeIssued")]
	public string DateTimeIssued
	{
		set {
			if (!DateTime.TryParse(value, out DateTime dt))
			{
				throw new Exception("Invalid date format");
			}
			_dateTimeIssued = dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
		}
		get {
			if (string.IsNullOrEmpty(_dateTimeIssued)) _dateTimeIssued = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
			return _dateTimeIssued;
		}
	}

	[JsonPropertyName("taxpayerActivityCode")]
	public string TaxPayerActivityCode { get; } = "3100";

	[JsonPropertyName("internalID")]
	public string InternalID { get; set; }

	[JsonPropertyName("purchaseOrderReference")]
	public string PurchaseOrderReference { get; set; } = "";

	[JsonPropertyName("purchaseOrderDescription")]
	public string PurchaseOrderDescription { get; set; } = "";

	[JsonPropertyName("salesOrderReference")]
	public string SalesOrderReference { get; set; } = "";

	[JsonPropertyName("salesOrderDescription")]
	public string SalesOrderDescription { get; set; } = "";

	[JsonPropertyName("proformaInvoiceNumber")]
	public string ProformaInvoiceNumber { get; set; } = "";

	[JsonPropertyName("payment")]
	public IPaymentModel Payment { get; set; }

	[JsonPropertyName("delivery")]
	public IDeliveryModel Delivery { get; set; }

	[JsonPropertyName("issuer")]
	public IIssuerReceiverInfoModel Issuer { get; set; }

	[JsonPropertyName("receiver")]
	public IIssuerReceiverInfoModel Receiver { get; set; }

	[JsonPropertyName("invoiceLines")]
	public IList<IInvoiceLineModel> InvoiceLines { get; set; }

	//sum of all salesAmount in all invoice lines
	[JsonPropertyName("totalSalesAmount")]
	public decimal TotalSalesAmount => GetTotalSalesAmount();

	//discount applied at items level
	[JsonPropertyName("totalItemsDiscountAmount")]
	public decimal TotalItemsDiscountAmount => GetTotalItemsDiscountAmount();

	[JsonPropertyName("extraDiscountAmount")]
	public decimal ExtraDiscountAmount { get; set; } //discount applied at document level

	[JsonPropertyName("totalDiscountAmount")]
	public decimal TotalDiscountAmount => GetTotalDiscount();

	//netAmount = (total sales - total discount) at document level
	[JsonPropertyName("netAmount")]
	public decimal NetAmount { get => TotalSalesAmount - TotalDiscountAmount; }

	//public taxTotals needs implement
	[JsonPropertyName("taxTotals")]
	public List<ITaxTotalsModel> TaxTotals => GetTaxTotals();

	//totalAmount = (netAmount + taxTotals) at document level
	[JsonPropertyName("totalAmount")]
	public decimal TotalAmount => NetAmount + GetTaxtotalAmount();

	[JsonPropertyName("signatures")]
	public List<ISignatureModel> Signatures { get; set; } = new();

	private decimal GetTotalSalesAmount()
	{
		if (InvoiceLines == null)
		{
			throw new Exception("No invoice lines added to the document");
		}
		decimal totalSales = 0M;
		foreach (var invoiceLine in InvoiceLines)
		{
			totalSales += invoiceLine.SalesTotal;
		}
		return totalSales;
	}

	private decimal GetTotalItemsDiscountAmount()
	{
		decimal totalItemsDiscount = 0M;
		foreach (var invoiceLine in InvoiceLines)
		{
			totalItemsDiscount += invoiceLine.ItemsDiscount;
		}
		return totalItemsDiscount;
	}

	private decimal GetTotalDiscount()
	{
		decimal total = 0M;
		foreach (InvoiceLineModel invoice in InvoiceLines)
		{
			total += invoice.Discount?.Amount ?? 0M;
		}
		return total + TotalItemsDiscountAmount + ExtraDiscountAmount;
	}

	private decimal GetTaxtotalAmount()
	{
		decimal total = 0M;
		foreach (InvoiceLineModel invoiceLine in InvoiceLines)
		{
			foreach (TaxableItemModel taxItem in invoiceLine.TaxableItems)
			{
				total += taxItem?.Amount ?? 0M;
			}
		}
		return total;
	}

	private List<ITaxTotalsModel> GetTaxTotals()
	{
		List<ITaxableItemModel> TaxInvoiceAll = new();
		List<ITaxTotalsModel> taxTotals = new();

		foreach (var invoice in InvoiceLines)
		{
			foreach (var tax in invoice.TaxableItems)
			{
				TaxInvoiceAll.Add(tax);
			}
		}

		var taxQuery = from item in TaxInvoiceAll
					   group item by item.TaxType;

		foreach (var taxGroup in taxQuery)
		{
			TaxTotalsModel taxItem = new() { TaxType = taxGroup.Key };
			foreach (var tax in taxGroup)
			{
				taxItem.Amount += tax.Amount;
			}
			taxTotals.Add(taxItem);
		}
		return taxTotals;
	}
}
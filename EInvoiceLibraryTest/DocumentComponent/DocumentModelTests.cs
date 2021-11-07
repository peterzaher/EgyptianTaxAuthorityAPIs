using System.Collections.Generic;
using EInvoicing.DocumentComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EInvoiceUnitTests.DocumentComponent;

[TestClass]
public class DocumentModelTests
{
	[TestMethod]
	public void TotalSalesAmount_ReturnsSumOfInvoiceLinesSalesTotal()
	{
		List<DocumentModel> document = CreateDocument();

		decimal expected = 1_936M;

		Assert.AreEqual(expected, document[0].TotalSalesAmount);

	}

	[TestMethod]
	public void TotalDiscountAmount_ReturnsSumOfInvoiceLinesDiscount()
	{
		List<DocumentModel> document = CreateDocument();

		decimal expected = 367.84M;

		Assert.AreEqual(expected, document[0].TotalDiscountAmount);
	}

	[TestMethod]
	public void NetAmount_ReturnsTotalSalesMinusTotalDiscount()
	{
		List<DocumentModel> document = CreateDocument();

		decimal expected = 1_568.16M;

		Assert.AreEqual(expected, document[0].NetAmount);
	}

	[TestMethod]
	public void TotalItemsDiscountAmount_ReturnsTotalAmountOfItemsDiscount()
	{
		List<DocumentModel> document = CreateDocument();

		decimal expected = 0M;

		Assert.AreEqual(expected, document[0].TotalItemsDiscountAmount);
	}

	[TestMethod]
	public void TotalAmount_ReturnsNetAmountPlusTotalTaxAmount()
	{
		List<DocumentModel> document = CreateDocument();

		decimal expected = 1_787.7024M;

		Assert.AreEqual(expected, document[0].TotalAmount);
	}

	private static List<DocumentModel> CreateDocument()
	{
		AddressModel issuerAddress = new("EG", "Qalyubiyya", "El Ubour", "13009", "1-15", "0");
		IssuerReceiverInfoModel issuer = new("المنزل للمفروشات", "204961254", "B", issuerAddress);

		AddressModel receiverAddress = new("EG", "Ismailia", "Ismailia", "Meena-Governet Employee", "9") { Floor = "1" };
		IssuerReceiverInfoModel receiver = new("Islam Zidan Store", "315268204", "B", receiverAddress);

		InvoiceLineModel invoiceLine1 = new("02110412A", 6, 161)
		{
			Description = "Pillow Fiber 40X100CM",
			UnitType = UnitTypeCode.EA
		};

		InvoiceLineModel invoiceLine2 = new("02110413A", 5, 194)
		{
			Description = "Pillow Fiber 40X120CM",
			UnitType = UnitTypeCode.EA
		};

		DiscountModel discount1 = new(19);

		TaxableItemModel taxableItem1 = new(TaxTypeCode.T1);

		invoiceLine1.Discount = discount1;
		invoiceLine2.Discount = discount1;
		invoiceLine1.TaxableItems.Add(taxableItem1);
		invoiceLine2.TaxableItems.Add(taxableItem1);

		List<InvoiceLineModel> invoiceLines = new() { invoiceLine1, invoiceLine2 };
		DocumentModel documentModel = new(issuer, receiver, invoiceLines, "62108396")
		{
			ExtraDiscountAmount = 0,
			DateTimeIssued = "2021-09-30T13:05"
		};

		List<DocumentModel> documentsList = new() { documentModel };
		//var documents = new { documents = documentsList };
		return documentsList;
	}

}

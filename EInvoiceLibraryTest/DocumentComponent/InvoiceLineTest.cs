using Microsoft.VisualStudio.TestTools.UnitTesting;
using EInvoicing.DocumentComponent;
using Domain.Enum;

namespace EInvoiceUnitTests.DocumentComponent;

[TestClass]
public class InvoiceLineModelTests
{
	[TestProperty("InvoiceLine", "SalesTotal")]
	[TestMethod]
	public void SalesTotal_ReturnsQuantityByUnitPrice()
	{
		InvoiceLineModel invoiceLine = new("A", 2, 4);

		decimal expected = 8;

		Assert.AreEqual(expected, invoiceLine.SalesTotal, "Wrong value in SalesTotal");
	}

	[TestMethod]
	public void Total_Discounted_ReturnsQuanityByPriceMinusDiscountPlustTax()
	{
		InvoiceLineModel invoiceLine = new("A", 2, 3);
		invoiceLine.Discount = new DiscountModel(25);
		TaxableItemModel taxableItemModel = new(TaxTypeCode.T1, TaxSubTypeCode.V009, 14);

		invoiceLine.TaxableItems.Add(taxableItemModel);
		decimal expected = 5.13M;

		Assert.AreEqual(expected, invoiceLine.Total);
	}

	[TestMethod]
	public void Total_NotDiscounted_ReturnsQuantityByPricePlusTax()
	{
		InvoiceLineModel invoiceLine = new("A", 2, 3);
		TaxableItemModel taxableItemModel = new(TaxTypeCode.T1, TaxSubTypeCode.V009, 14);

		invoiceLine.TaxableItems.Add(taxableItemModel);
		decimal expected = 6.84M;

		Assert.AreEqual(expected, invoiceLine.Total);
	}

	[TestMethod]
	public void NetTotal()
	{
		InvoiceLineModel invoiceLine = new("A", 2, 3);
		invoiceLine.Discount = new DiscountModel(25);
		TaxableItemModel taxableItemModelStub = new(TaxTypeCode.T1, TaxSubTypeCode.V009, 14);

		invoiceLine.TaxableItems.Add(taxableItemModelStub);
		decimal expected = 4.5M;

		Assert.AreEqual(expected, invoiceLine.NetTotal);
	}
}

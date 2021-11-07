using EInvoicing.DocumentComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EInvoiceUnitTests.DocumentComponent;

[TestClass()]
public class AddressModelTests
{
	[TestMethod()]
	public void Country_NullValue_ThrowsArgumentNullException()
	{
		static void address() { AddressModel a = new(null, "ABC", "ABC", "ABC", "1"); };

		Assert.ThrowsException<ArgumentNullException>(address, "Argument Null Exception not thrown");
	}

	[TestMethod()]
	public void Governate_NullValue_ThrowsArgumentNullException()
	{
		static void address() { AddressModel a = new("EG", null, "ABC", "ABC", "1"); };

		Assert.ThrowsException<ArgumentNullException>(address, "Argument Null Exception not thrown");
	}

	[TestMethod()]
	public void RegionCity_NullValue_ThrowsArgumentNullException()
	{
		static void address() { AddressModel a = new("EG", "Cairo", null, "ABC", "1"); };

		Assert.ThrowsException<ArgumentNullException>(address, "Argument Null Exception not thrown");
	}

	[TestMethod()]
	public void Street_NullValue_ThrowsArgumentNullException()
	{
		static void address() { AddressModel address = new("EG", "Cairo", "Obour", null, "1"); };

		Assert.ThrowsException<ArgumentNullException>(address, "Argument Null Exception not thrown");
	}

	[TestMethod]
	public void Constructor_NonNullValues_ShouldNotThrow()
	{
		try
		{
			AddressModel address = new("EG", "Cairo", "Obour", "ABC", "1");
		}
		catch (ArgumentNullException)
		{
			Assert.Fail("Contructor nonnull arguments values should't throw exception");
		}
	}
}
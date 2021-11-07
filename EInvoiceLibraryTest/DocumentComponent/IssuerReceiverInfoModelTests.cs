using EInvoicing.DocumentComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceUnitTests.DocumentComponent;

[TestClass()]
public class IssuerReceiverInfoModelTests
{
	[TestMethod()]
	public void Name_WhenNull_ShouldThrowNullException()
	{
		AddressModel address = CreateAddress();

		void CreateIssuer() { IssuerReceiverInfoModel receiver = new(null, "1234", "B", address); }

		Assert.ThrowsException<ArgumentNullException>(() => CreateIssuer());
	}

	[TestMethod()]
	public void Name_WhenEmpty_ShouldThrowNullException()
	{
		AddressModel address = CreateAddress();

		void CreateIssuer() { IssuerReceiverInfoModel receiver = new("", "1234", "B", address); }

		Assert.ThrowsException<ArgumentNullException>(() => CreateIssuer());
	}

	[TestMethod()]
	public void Id_WhenNotNumber_ShouldThrowArgumentException()
	{
		AddressModel address = CreateAddress();

		void CreateIssuer() { IssuerReceiverInfoModel receiver = new("ABC", "1234A", "B", address); }

		Assert.ThrowsException<ArgumentException>(() => CreateIssuer());
	}

	[TestMethod()]
	public void Id_WhenTypeIsPerson_ShouldThrowArgumentExceptionIfLessThan14Digit()
	{
		AddressModel address = CreateAddress();

		void CreateIssuer() { IssuerReceiverInfoModel receiver = new("ABC", "1234", "P", address); }

		Assert.ThrowsException<ArgumentException>(() => CreateIssuer());
	}

	[TestMethod()]
	public void Id_WhenTypeIsPerson_ShouldThrowArgumentExceptionIfMoreThan14Digit()
	{
		AddressModel address = CreateAddress();

		void CreateIssuer() { IssuerReceiverInfoModel receiver = new("ABC", "123456789012345", "P", address); }

		Assert.ThrowsException<ArgumentException>(() => CreateIssuer());
	}

	[TestMethod()]
	public void Id_WhenTypeIsPerson_ShouldPassWhen14Digit()
	{
		AddressModel address = CreateAddress();
		try
		{
			IssuerReceiverInfoModel receiver = new("ABC", "12345678901234", "P", address);
		}
		catch (Exception)
		{
			Assert.Fail();
		}
	}

	[TestMethod()]
	[DataRow("A")]
	[DataRow("a")]
	[DataRow("D")]
	[DataRow("d")]
	[DataRow("u")]
	[DataRow("U")]
	[DataRow("Z")]
	[DataRow("z")]
	[DataRow("k")]
	public void Type_WhenNotAllowedChar_ShouldThrowArgumentException(string value)
	{
		//Type argument should accept only one character of string, case insensitive
		//Allowed chars for type param: (B for businsess, P for Person, F for Foreigner)

		AddressModel address = CreateAddress();

		void CreateReceiver(string type) { IssuerReceiverInfoModel r = new("ABC", "12345678901234", type, address); }

		Assert.ThrowsException<ArgumentException>(() => CreateReceiver(value));
	}

	[TestMethod()]
	[DataRow("AB")]
	[DataRow("ab")]
	[DataRow("A1")]
	[DataRow("a1")]
	[DataRow("1a")]
	[DataRow("2B")]
	public void Type_WhenMoreThanOneChar_ShouldThrowArgumentException(string value)
	{
		AddressModel address = CreateAddress();
		void CreateReceiver(string str) { IssuerReceiverInfoModel r = new("ABC", "123", str, address); }

		Assert.ThrowsException<ArgumentException>(() => CreateReceiver(value));
	}

	[TestMethod()]
	[DataRow("B")]
	[DataRow("b")]
	[DataRow("P")]
	[DataRow("p")]
	[DataRow("F")]
	[DataRow("f")]
	public void Type_WhenAllowedChar_ShouldPass(string value)
	{
		AddressModel address = CreateAddress();

		try
		{
			IssuerReceiverInfoModel receiver1 = new("ABC", "12345678901234", value, address);
		}
		catch (Exception)
		{
			Assert.Fail();
		}
	}

	private static AddressModel CreateAddress()
	{
		return new AddressModel("EG", "Cairo", "Obour", "ABC", "123", "1");
	}
}

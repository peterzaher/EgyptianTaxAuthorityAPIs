using EInvoicing.Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EInvoiceUnitTests;

[TestClass()]
public class DocumentSerializationTests
{
	[TestMethod()]
	public void GetDocumentCanonicalString_ShouldReturnCanonicalString()
	{
		string jsonStr = "{\"str1\":\"abc\",\"str2\":\"def\",\"array\":[1,{\"key1\":\"A\"," +
			"\"key2\":\"B\"},2,3],\"nestedInObject\":{\"number1\":1,\"number2\":2,\"bool\":true}}";


		string expected = "\"str1\"\"abc\"\"str2\"\"def\"\"array\"\"array\"\"1\"\"key1\"\"A\"" +
			"\"key2\"\"B\"\"2\"\"3\"\"nestedInObject\"\"number1\"\"1\"\"number2\"\"2\"\"bool\"\"True\"";

		string actual = DocumentSerialization.GetDocumentCanonicalString(jsonStr);

		Assert.AreEqual(expected, actual);
	}
	
}

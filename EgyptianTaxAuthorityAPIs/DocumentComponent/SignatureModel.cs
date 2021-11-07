using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class SignatureModel
	{
		[JsonPropertyName("signatureType")]
		public string SignatureType { get; set; } = "I";

		[JsonPropertyName("value")]
		public string Value { get; set; }

		public SignatureModel() { }
		public SignatureModel(string value)
		{
			Value = value;
		}
	}
}

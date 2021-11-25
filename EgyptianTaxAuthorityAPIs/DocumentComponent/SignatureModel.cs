using System.Text.Json.Serialization;
using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class SignatureModel : ISignatureModel
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

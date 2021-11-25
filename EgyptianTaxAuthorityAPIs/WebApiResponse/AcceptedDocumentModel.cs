using System.Text.Json.Serialization;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse
{
	public class AcceptedDocumentModel : IAcceptedDocumentModel
	{
		[JsonPropertyName("uuid")]
		public string UUID { get; set; }

		[JsonPropertyName("longId")]
		public string LongId { get; set; }

		[JsonPropertyName("internalId")]
		public string InternalId { get; set; }
	}
}

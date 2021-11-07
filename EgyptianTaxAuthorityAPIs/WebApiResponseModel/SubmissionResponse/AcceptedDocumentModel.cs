using System.Text.Json.Serialization;

namespace EInvoicing.WebApiResponseModel.SubmissionResponse
{
	public class AcceptedDocumentModel
	{
		[JsonPropertyName("uuid")]
		public string UUID { get; set; }

		[JsonPropertyName("longId")]
		public string LongId { get; set; }

		[JsonPropertyName("internalId")]
		public string InternalId { get; set; }
	}
}

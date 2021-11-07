using System.Text.Json.Serialization;

namespace EInvoicing.WebApiResponseModel.SubmissionResponse
{
	public class RejectedDocumentModel
	{
		[JsonPropertyName("internalId")]
		public string InternalId { get; set; }

		[JsonPropertyName("error")]
		public ErrorModel Error { get; set; }
	}
}

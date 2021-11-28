using System.Text.Json.Serialization;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse
{
	public class RejectedDocumentModel : IRejectedDocumentModel
	{
		[JsonPropertyName("internalId")]
		public string InternalId { get; set; }

		[JsonPropertyName("error")]
		public IErrorModel Error { get; set; }
	}
}

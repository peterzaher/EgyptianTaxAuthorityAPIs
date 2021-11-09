using System.Text.Json.Serialization;

namespace EInvoicing.WebApiResponse
{
	public class MetadataModel
	{
		[JsonPropertyName("totalPages")]
		public int TotalPages { get; set; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; set; }
	}
}

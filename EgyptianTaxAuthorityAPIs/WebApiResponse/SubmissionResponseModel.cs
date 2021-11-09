using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EInvoicing.WebApiResponse;

public class SubmissionResponseModel
{
	[JsonPropertyName("submissionId")]
	public string SubmissionId { get; set; }

	[JsonPropertyName("acceptedDocuments")]
	public List<AcceptedDocumentModel> AcceptedDocuments { get; set; }

	[JsonPropertyName("rejectedDocuments")]
	public List<RejectedDocumentModel> RejectedDocuments { get; set; }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse;

public class SubmissionResponseModel : ISubmissionResponseModel
{
	[JsonPropertyName("submissionId")]
	public string SubmissionId { get; set; }

	[JsonPropertyName("acceptedDocuments")]
	public List<IAcceptedDocumentModel> AcceptedDocuments { get; set; }

	[JsonPropertyName("rejectedDocuments")]
	public List<IRejectedDocumentModel> RejectedDocuments { get; set; }
}

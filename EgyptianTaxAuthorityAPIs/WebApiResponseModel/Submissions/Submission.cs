using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace EInvoicing.WebApiResponseModel.Submissions;

public class Submission : ISubmissionReceiver
{
	public string Submissionid { get; set; }
	public int DocumentCount { get; set; }
	public DateTimeOffset DateTimeReceived { get; set; }
	public string OverallStatus { get; set; }
	public IList<DocumentSummaryModel> DocumentSummary { get; set; }
	public MetadataModel Metadata { get; set; }

	public async Task<Submission> GetSubmissionAsync(HttpClient client, string uuid, int pageNumber = 0, int pageSize = 0)
	{
		string path = $"/api/v1.0/documentsubmissions/{uuid}?pageNo={pageNumber}&pageSize={pageSize}";

		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		Submission submissionStatus = await client.GetFromJsonAsync<Submission>(path, options);
		return submissionStatus;

	}
}

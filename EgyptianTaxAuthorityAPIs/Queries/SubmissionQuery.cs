using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using EInvoicing.Queries;
using EInvoicing.WebApiResponse;

namespace EInvoicing.Queries;

public class SubmissionQuery
{
	public string Submissionid { get; set; }
	public int DocumentCount { get; set; }
	public DateTimeOffset DateTimeReceived { get; set; }
	public string OverallStatus { get; set; }
	public IList<DocumentSummaryModel> DocumentSummary { get; set; }
	public MetadataModel Metadata { get; set; }

	internal static async Task<SubmissionQuery> GetSubmissionAsync(HttpClient client, string uuid, int pageNumber = 0, int pageSize = 0)
	{
		string path = $"/api/v1.0/documentsubmissions/{uuid}?pageNo={pageNumber}&pageSize={pageSize}";

		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		SubmissionQuery submissionStatus = await client.GetFromJsonAsync<SubmissionQuery>(path, options);
		return submissionStatus;

	}
}

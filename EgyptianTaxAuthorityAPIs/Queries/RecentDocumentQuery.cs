using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EInvoicing.WebApiResponse;

namespace EInvoicing.Queries;

public class RecentDocumentQuery
{
	[JsonPropertyName("result")]
	public List<DocumentSummaryModel> DocumentsSummary { get; set; } = new();

	[JsonPropertyName("metadata")]
	public MetadataModel Metadata { get; set; } = new();

	internal static async Task<RecentDocumentQuery> GetRecentDocumentsAsync(int pageNumber, int pageSize, HttpClient client)
	{
		string path = $"api/v1.0/documents/recent?pageNo={pageNumber}&pageSize={pageSize}";
		JsonSerializerOptions jsonOptions = new()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
		};

		try
		{
			RecentDocumentQuery submittedDocuments = await client.GetFromJsonAsync<RecentDocumentQuery>(path, jsonOptions);
			return submittedDocuments;
		}
		catch (HttpRequestException e)
		{
			throw new HttpRequestException("Error getting data", e, e.StatusCode);
		}
		catch (Exception e)
		{
			throw new Exception("General error", e);
		}
	}
}

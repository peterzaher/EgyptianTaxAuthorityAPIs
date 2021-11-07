using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EInvoicing.WebApiResponseModel.RecentDocuments;

public class RecentDocumentModel : IRecentDocumentReceiver
{
	[JsonPropertyName("result")]
	public List<DocumentSummaryModel> DocumentsSummary { get; set; } = new();

	[JsonPropertyName("metadata")]
	public MetadataModel Metadata { get; set; } = new();

	public async Task<RecentDocumentModel> GetRecentDocumentsAsync(int pageNumber, int pageSize, HttpClient client)
	{
		string path = $"api/v1.0/documents/recent?pageNo={pageNumber}&pageSize={pageSize}";
		JsonSerializerOptions jsonOptions = new()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
		};

		try
		{
			RecentDocumentModel submittedDocuments = await client.GetFromJsonAsync<RecentDocumentModel>(path, jsonOptions);
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

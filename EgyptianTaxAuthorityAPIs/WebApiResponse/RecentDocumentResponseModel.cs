using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace EInvoicing.WebApiResponse;

internal class RecentDocumentResponseModel
{
	public async Task<DocumentStatusModel> GetDocumentAsync(HttpClient httpClient, string documentUuid)
	{
		string path = $"/api/v1.0/documents/{documentUuid}/raw";

		DocumentStatusModel documentStatus = await httpClient.GetFromJsonAsync<DocumentStatusModel>(path);
		return documentStatus;
	}
}

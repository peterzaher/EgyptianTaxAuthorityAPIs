using System.Net.Http;
using System.Threading.Tasks;

namespace EInvoicing.WebApiResponseModel.Documents;

internal interface IDocumentReceiver
{
	public Task<DocumentStatusModel> GetDocumentAsync(HttpClient httpClient, string documentUuid);
}

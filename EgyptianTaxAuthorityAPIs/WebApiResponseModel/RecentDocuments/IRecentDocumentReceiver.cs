using System.Net.Http;
using System.Threading.Tasks;

namespace EInvoicing.WebApiResponseModel.RecentDocuments;

internal interface IRecentDocumentReceiver
{
	internal Task<RecentDocumentModel> GetRecentDocumentsAsync(int pageNumber, int pageSize, HttpClient client);
}

using System.Net.Http;
using System.Threading.Tasks;

namespace EInvoicing.WebApiResponseModel.Submissions;

internal interface ISubmissionReceiver
{
	internal Task<Submission> GetSubmissionAsync(HttpClient client, string uuid, int pageNumber = 0, int pageSize = 0);
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EInvoicing.DocumentComponent;
using EInvoicing.WebApiResponse;
using EInvoicing.Processing;
using EInvoicing.Queries;
using DataAccess;
using System.Data.SqlClient;

namespace EInvoicing;

public class WebCallController
{
	private static HttpClient Client { get; } = new();
	public string SqlConnectionStr { get; set; }

	public WebCallController(string sqlDBConnectionString = "data source=dbsrv1;initial catalog=manufacturing;user id=sa;password=''")
	{
		SqlClientFactory sqlClientFactory = SqlClientFactory.Instance;
		SqlConnectionStr = sqlClientFactory.CreateConnection().ConnectionString = sqlDBConnectionString;
	}

	public async Task<SubmissionResponseModel> SubmitDocumentsAsync(IList<DocumentModel> documents)
	{
		await Token.GetAccessTokenAsync(Client, SqlConnectionStr);
		string jsonDocs = await DocumentProcessing.PrepareDocumentsToSend(documents);
		return await DocumentModel.SubmitDocumentsAsync(jsonDocs, Client);
	}

	public async Task<RecentDocumentQuery> GetRecentDocumentsAsync(int pageNumber = 1, int pageSize = 10)
	{
		await Token.GetAccessTokenAsync(Client, SqlConnectionStr);
		return await RecentDocumentQuery.GetRecentDocumentsAsync(pageNumber, pageSize, Client);
	}

	public async Task<SubmissionQuery> GetSubmissionStatusAysnc(string uuid, int pageNumber, int pageSize)
	{
		await Token.GetAccessTokenAsync(Client, SqlConnectionStr);
		return await SubmissionQuery.GetSubmissionAsync(Client, uuid, pageNumber, pageSize);
	}

	public async Task<DocumentStatusModel> GetDocumentStatusAsync(string documentUuid)
	{
		await Token.GetAccessTokenAsync(Client, SqlConnectionStr);
		Document document = new();
		return await document.GetDocumentAsync(Client, documentUuid);
	}
}

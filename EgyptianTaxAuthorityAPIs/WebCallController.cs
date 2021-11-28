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
using System.Text.Json;

namespace EInvoicing;

public class WebCallController
{
	private static HttpClient Client { get; } = new();
	public static string SqlConnectionStr { get; set; }

	public WebCallController(string sqlDBConnectionString = "data source=dbsrv1;initial catalog=manufacturing;user id=sa;password=''")
	{
		SqlClientFactory sqlClientFactory = SqlClientFactory.Instance;
		SqlConnectionStr = sqlClientFactory.CreateConnection().ConnectionString = sqlDBConnectionString;
	}

	public static async Task<SubmissionResponseModel> SubmitDocumentsAsync(DateTimeOffset upTo)
	{
		IList<string> documentList = await StagedDocuments.GetStagedDocumentsAsync(upTo, SqlConnectionStr);
		string documents = await DocumentProcessing.PrepareDocumentsToSend(documentList, SqlConnectionStr);

		Encoding encoding = new UTF8Encoding(false, true);
		StringContent content = new(documents, encoding, @"application/json");

		HttpResponseMessage response = await Client.PostAsync(@"/api/v1.0/documentsubmissions", content);

		if ((int)response.StatusCode == 400)
		{
			throw new Exception("Error bad structure or maximum size exceeded");
		}

		if ((int)response.StatusCode == 403)
		{
			throw new Exception("Incorrect submitter");
		}

		if ((int)response.StatusCode == 422)
		{
			throw new Exception("Duplicate submimssion, try again later");
		}

		SubmissionResponseModel submitResponse = await response.Content.ReadFromJsonAsync<SubmissionResponseModel>();

		string submissionId = submitResponse.SubmissionId;
		string jsonSubmissionStr = DocumentSerialization.SerializeToJson(submitResponse);
		await Submission.InsertSubmissionAsync(submissionId, jsonSubmissionStr, SqlConnectionStr);

		return submitResponse;
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
		RecentDocumentResponseModel document = new();
		return await document.GetDocumentAsync(Client, documentUuid);
	}

	internal static async Task<IEnumerable<string>> GetStagedDocumentAsync(DateTimeOffset upTo)
	{
		return await StagedDocuments.GetStagedDocumentsAsync(upTo, SqlConnectionStr);
	}

	public async Task Save(DocumentModel document)
	{
		string internalId = document.InternalID;
		string jsonDoc = DocumentSerialization.SerializeToJson(document);
		await StagedDocuments.InsertDocumentAsync(internalId, jsonDoc, SqlConnectionStr);
	}
}

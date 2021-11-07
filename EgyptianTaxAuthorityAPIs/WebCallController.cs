using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EInvoicing.DocumentComponent;
using EInvoicing.WebApiResponseModel;
using EInvoicing.WebApiResponseModel.RecentDocuments;
using EInvoicing.WebApiResponseModel.SubmissionResponse;
using EInvoicing.WebApiResponseModel.Submissions;
using EInvoicing.WebApiResponseModel.Documents;


namespace EInvoicing;

public static class WebCallController
{
	internal static readonly HttpClient client = new();
	private static readonly Uri identityUrl = new(@"https://id.eta.gov.eg/connect/token");
	private static readonly Uri baseUri = new(@"https://api.invoicing.eta.gov.eg");
	private static string sqlConnectionStr;
	private static DateTime tokenStartTime;

	public static void Initialize(string sqlDBConnectionString = "data source=dbsrv1;initial catalog=manufacturing;user id=sa;password=''")
	{
		sqlConnectionStr = sqlDBConnectionString;
		client.BaseAddress = baseUri;

		//temp code for test and debug
		//string temp = "";
		//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", temp);
	}

	private static bool IsTokenValid()
	{
		DateTime tokenElapsedTime = tokenStartTime.AddMinutes(58);
		int tokenExpired = DateTime.Compare(DateTime.Now, tokenElapsedTime);

		if (client.DefaultRequestHeaders.Authorization == null || tokenExpired > 0)
		{
			return false;
		}
		return true;
	}

	private static async Task GetAccessTokenAsync()
	{
		(string token, DateTime startTime) = await DataAccess.Credential.GetTokenFromLocalDB(sqlConnectionStr);

		if (string.IsNullOrEmpty(token))
		{
			token = await GetTokenFromWebApi();
			SetClientHeaders(token);
			tokenStartTime = DateTime.Now;
			return;
		}

		SetClientHeaders(token);
		tokenStartTime = startTime;
	}

	private static void SetClientHeaders(string token)
	{
		client.DefaultRequestHeaders.Clear();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	private static async Task<string> GetTokenFromWebApi()
	{
		(string userId, string password) = await DataAccess.Credential.GetETACredentialAsync(sqlConnectionStr);

		string encodedStr = BuildAuthorizationCode(userId, password);

		client.DefaultRequestHeaders.Clear();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedStr);

		Dictionary<string, string> requestContent = new()
		{
			{ "grant_type", "client_credentials" }
		};

		FormUrlEncodedContent content = new(requestContent);
		HttpResponseMessage response = await client.PostAsync(identityUrl, content);

		if (!response.IsSuccessStatusCode)
		{
			AuthenticationErrorModel errorResponse = await response.Content.ReadFromJsonAsync<AuthenticationErrorModel>();
			string authenticationError = $"{errorResponse.Error} \n {errorResponse.ErrorDesicription} \n {errorResponse.ErrorURI?.AbsolutePath}";
			throw new Exception(authenticationError);
		}

		AuthenticationResponseModel jsonResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponseModel>();

#if DEBUG
		if (System.IO.Directory.Exists("c:\\Doc\\DebugOutput"))
		{
			System.IO.File.WriteAllLines("c:\\Doc\\DebugOutput\\Token.txt", new string[] { client.DefaultRequestHeaders.Authorization.Parameter });
		}
#endif
		await DataAccess.Credential.PersistToken(sqlConnectionStr, jsonResponse.AccessToken);

		return jsonResponse.AccessToken;
	}

	private static string BuildAuthorizationCode(string userId, string secret)
	{
		string userIdSecret = userId + ":" + secret;
		byte[] textBytes = Encoding.UTF8.GetBytes(userIdSecret);
		return Convert.ToBase64String(textBytes);
	}

	public static async Task<SubmissionResponseModel> SubmitDocumentsAsync(IList<DocumentModel> documents)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		string jsonDocs = await DocumentProcessing.PrepareDocumentsToSend(documents);
		return await DocumentModel.SubmitDocumentsAsync(jsonDocs, client);
	}

	public static async Task<RecentDocumentModel> GetRecentDocumentsAsync(int pageNumber = 1, int pageSize = 10)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		IRecentDocumentReceiver recentDocuments = new RecentDocumentModel();
		return await recentDocuments.GetRecentDocumentsAsync(pageNumber, pageSize, client);
	}

	public static async Task<Submission> GetSubmissionStatusAysnc(string uuid, int pageNumber, int pageSize)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		ISubmissionReceiver submission = new Submission();
		return await submission.GetSubmissionAsync(client, uuid, pageNumber, pageSize);
	}

	public static async Task<DocumentStatusModel> GetDocumentStatusAsync(string documentUuid)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		IDocumentReceiver document = new Document();
		return await document.GetDocumentAsync(client, documentUuid);
	}
}

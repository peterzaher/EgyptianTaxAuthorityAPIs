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

namespace EInvoicing;

public static class WebCallController
{
	private static readonly HttpClient client = new();
	private static readonly Uri identityUrl = new(@"https://id.eta.gov.eg/connect/token");
	private static readonly Uri baseUri = new(@"https://api.invoicing.eta.gov.eg");
	private static string sqlConnectionStr;
	private static DateTime tokenStartTime;

	internal static HttpClient Client => client;

	public static void Initialize(string sqlDBConnectionString = "data source=dbsrv1;initial catalog=manufacturing;user id=sa;password=''")
	{
		sqlConnectionStr = sqlDBConnectionString;
		Client.BaseAddress = baseUri;

		//temp code for test and debug
		//string temp = "";
		//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", temp);
	}

	private static bool IsTokenValid()
	{
		DateTime tokenElapsedTime = tokenStartTime.AddMinutes(58);
		int tokenExpired = DateTime.Compare(DateTime.Now, tokenElapsedTime);

		if (Client.DefaultRequestHeaders.Authorization == null || tokenExpired > 0)
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
			SetHttpHeaders(token);
			tokenStartTime = DateTime.Now;
			return;
		}

		SetHttpHeaders(token);
		tokenStartTime = startTime;
	}

	private static void SetHttpHeaders(string token)
	{
		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
		Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	private static async Task<string> GetTokenFromWebApi()
	{
		(string userId, string password) = await DataAccess.Credential.GetETACredentialAsync(sqlConnectionStr);

		string encodedStr = BuildAuthorizationCode(userId, password);

		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedStr);

		Dictionary<string, string> requestContent = new()
		{
			{ "grant_type", "client_credentials" }
		};

		FormUrlEncodedContent content = new(requestContent);
		HttpResponseMessage response = await Client.PostAsync(identityUrl, content);

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
			System.IO.File.WriteAllLines("c:\\Doc\\DebugOutput\\Token.txt", new string[] { Client.DefaultRequestHeaders.Authorization.Parameter });
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
		return await DocumentModel.SubmitDocumentsAsync(jsonDocs, Client);
	}

	public static async Task<RecentDocumentQuery> GetRecentDocumentsAsync(int pageNumber = 1, int pageSize = 10)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		return await RecentDocumentQuery.GetRecentDocumentsAsync(pageNumber, pageSize, Client);
	}

	public static async Task<SubmissionQuery> GetSubmissionStatusAysnc(string uuid, int pageNumber, int pageSize)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		return await SubmissionQuery.GetSubmissionAsync(Client, uuid, pageNumber, pageSize);
	}

	public static async Task<DocumentStatusModel> GetDocumentStatusAsync(string documentUuid)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync();
		Document document = new Document();
		return await document.GetDocumentAsync(Client, documentUuid);
	}
}

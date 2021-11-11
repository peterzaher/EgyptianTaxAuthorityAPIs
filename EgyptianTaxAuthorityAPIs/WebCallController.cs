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
	private string _userId;
	private string _password;
	private static DateTime _tokenStartTime;
	private static HttpClient Client { get; } = new();
	public string SqlConnectionStr { get; set; }

	public WebCallController(string sqlDBConnectionString = "data source=dbsrv1;initial catalog=manufacturing;user id=sa;password=''")
	{
		SqlClientFactory sqlClientFactory = SqlClientFactory.Instance;
		SqlConnectionStr = sqlClientFactory.CreateConnection().ConnectionString = sqlDBConnectionString;
	}

	private static bool IsTokenValid()
	{
		DateTime tokenElapsedTime = _tokenStartTime.AddMinutes(58);
		int tokenExpired = DateTime.Compare(DateTime.Now, tokenElapsedTime);

		if (Client.DefaultRequestHeaders.Authorization == null || tokenExpired > 0)
		{
			return false;
		}
		return true;
	}

	private async Task GetAccessTokenAsync(string sqlDbConnectionStr)
	{
		(string token, DateTime startTime) = await Credential.GetTokenFromLocalDB(sqlDbConnectionStr);

		if (string.IsNullOrEmpty(token))
		{
			token = await GetTokenFromWebApi(sqlDbConnectionStr);
			await SetHttpHeaders(token);
			_tokenStartTime = DateTime.Now;
			return;
		}

		await SetHttpHeaders(token);
		_tokenStartTime = startTime;
	}

	private async Task<string> GetTokenFromWebApi(string sqlConnectionStr)
	{
		if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_password))
		{
			(_userId, _password) = await Credential.GetETACredentialAsync(sqlConnectionStr);
		}

		string encodedStr = BuildAuthorizationCode(_userId, _password);

		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedStr);

		Dictionary<string, string> requestContent = new()
		{
			{ "grant_type", "client_credentials" }
		};

		FormUrlEncodedContent content = new(requestContent);

		Uri identityUrl = new(await WebApiParameter.GetParameterByKey(sqlConnectionStr, "IdentityUrl"));

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
		await Credential.PersistToken(SqlConnectionStr, jsonResponse.AccessToken);

		return jsonResponse.AccessToken;
	}

	private async Task SetHttpHeaders(string token)
	{
		Client.DefaultRequestHeaders.Clear();
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
		Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		Client.BaseAddress = new Uri(await WebApiParameter.GetParameterByKey(SqlConnectionStr, "BaseUrl"));
	}

	private static string BuildAuthorizationCode(string userId, string secret)
	{
		string userIdSecret = userId + ":" + secret;
		byte[] textBytes = Encoding.UTF8.GetBytes(userIdSecret);
		return Convert.ToBase64String(textBytes);
	}

	public async Task<SubmissionResponseModel> SubmitDocumentsAsync(IList<DocumentModel> documents)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync(SqlConnectionStr);
		string jsonDocs = await DocumentProcessing.PrepareDocumentsToSend(documents);
		return await DocumentModel.SubmitDocumentsAsync(jsonDocs, Client);
	}

	public async Task<RecentDocumentQuery> GetRecentDocumentsAsync(int pageNumber = 1, int pageSize = 10)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync(SqlConnectionStr);
		return await RecentDocumentQuery.GetRecentDocumentsAsync(pageNumber, pageSize, Client);
	}

	public async Task<SubmissionQuery> GetSubmissionStatusAysnc(string uuid, int pageNumber, int pageSize)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync(SqlConnectionStr);
		return await SubmissionQuery.GetSubmissionAsync(Client, uuid, pageNumber, pageSize);
	}

	public async Task<DocumentStatusModel> GetDocumentStatusAsync(string documentUuid)
	{
		if (!IsTokenValid()) await GetAccessTokenAsync(SqlConnectionStr);
		Document document = new Document();
		return await document.GetDocumentAsync(Client, documentUuid);
	}
}

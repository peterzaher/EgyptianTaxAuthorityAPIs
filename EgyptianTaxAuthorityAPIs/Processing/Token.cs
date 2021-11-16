using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using EInvoicing.WebApiResponse;

namespace EInvoicing.Processing;


internal static class Token
{
	private static string _userId, _password, _baseUrl, _identityUrl;
	private static DateTimeOffset _tokenStartTime;

	public static async Task GetAccessTokenAsync(HttpClient httpClient, string sqlDbConnectionStr)
	{
		if (IsTokenValid(httpClient)) return;

		(string token, _tokenStartTime) = await Credential.GetTokenFromLocalDbAsync(sqlDbConnectionStr);

		if (!string.IsNullOrEmpty(token))
		{
			await SetHttpDefaultHeadersAsync(httpClient, token, sqlDbConnectionStr);
			return;
		}

		if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_password))
		{
			(_userId, _password) = await Credential.GetCredentialFromDbAsync(sqlDbConnectionStr);
		}

		string authorizationCode = BuildAuthorizationCode(_userId, _password);

		if (string.IsNullOrEmpty(_identityUrl))
		{
			_identityUrl = await WebApiParameter.GetParameterByKey(sqlDbConnectionStr, "IdentityUrl");
		}

		token = await GetTokenFromWebApiAsync(authorizationCode, _identityUrl);
		_tokenStartTime = DateTime.UtcNow;

		await Credential.PersistTokenToDbAsync(token, sqlDbConnectionStr);
		await SetHttpDefaultHeadersAsync(httpClient, token, sqlDbConnectionStr);
	}

	private static bool IsTokenValid(HttpClient client)
	{
		DateTimeOffset validityPeriod = _tokenStartTime.AddMinutes(58);
		int tokenExpired = DateTimeOffset.Compare(DateTime.UtcNow, validityPeriod);

		if (client.DefaultRequestHeaders.Authorization == null || tokenExpired > 0)
		{
			return false;
		}
		return true;
	}

	private static async Task<string> GetTokenFromWebApiAsync(string authorizationCode, string identityUrl)
	{
		HttpClient httpClient = new();
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationCode);

		Dictionary<string, string> requestContent = new()
		{
			{ "grant_type", "client_credentials" }
		};

		FormUrlEncodedContent content = new(requestContent);
		HttpResponseMessage response = await httpClient.PostAsync(identityUrl, content);
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
			System.IO.File.WriteAllLines("c:\\Doc\\DebugOutput\\Token.txt", new string[] { httpClient.DefaultRequestHeaders.Authorization.Parameter });
		}
#endif
		httpClient.Dispose();
		return jsonResponse.AccessToken;
	}

	private static async Task SetHttpDefaultHeadersAsync(HttpClient httpClient, string token, string sqlDbConnectionStr)
	{
		httpClient.DefaultRequestHeaders.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		if (string.IsNullOrEmpty(_baseUrl))
		{
			_baseUrl = await WebApiParameter.GetParameterByKey(sqlDbConnectionStr, "BaseUrl");
		}
		httpClient.BaseAddress = new Uri(_baseUrl);
	}

	private static string BuildAuthorizationCode(string userId, string secret)
	{
		string userIdSecret = userId + ":" + secret;
		byte[] textBytes = Encoding.UTF8.GetBytes(userIdSecret);
		return Convert.ToBase64String(textBytes);
	}
}

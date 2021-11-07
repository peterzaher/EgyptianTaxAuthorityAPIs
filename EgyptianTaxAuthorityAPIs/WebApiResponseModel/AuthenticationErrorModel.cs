using System;
using System.Text.Json.Serialization;

namespace EInvoicing.WebApiResponseModel;

internal class AuthenticationErrorModel
{
	[JsonPropertyName("error")]
	public string Error { get; set; }

	[JsonPropertyName("error_description")]
	public string ErrorDesicription { get; set; }

	[JsonPropertyName("error_uri")]
	public Uri ErrorURI { get; set; }
}

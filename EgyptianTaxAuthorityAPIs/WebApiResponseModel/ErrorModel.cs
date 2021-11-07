using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace EInvoicing.WebApiResponseModel
{
	public class ErrorModel
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("target")]
		public string Target { get; set; }

		[JsonPropertyName("propertyPath")]
		public string PropertyPath { get; set; }

		[JsonPropertyName("details")]
		public List<ErrorModel> Details { get; set; }// = new();
	}
}

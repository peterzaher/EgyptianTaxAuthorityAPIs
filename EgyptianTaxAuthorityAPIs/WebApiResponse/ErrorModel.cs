using System.Text.Json.Serialization;
using System.Collections.Generic;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse
{
	public class ErrorModel : IErrorModel
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
		public List<IErrorModel> Details { get; set; }// = new();
	}
}

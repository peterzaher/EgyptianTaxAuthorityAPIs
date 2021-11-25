using System.Text.Json.Serialization;
using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class TaxTotalsModel : ITaxTotalsModel
	{
		public TaxTotalsModel() { }
		public TaxTotalsModel(string taxType, decimal amount)
		{
			TaxType = taxType;
			Amount = amount;
		}
		[JsonPropertyName("taxType")]
		public string TaxType { get; set; }

		[JsonPropertyName("amount")]
		public decimal Amount { get; set; }
	}
}

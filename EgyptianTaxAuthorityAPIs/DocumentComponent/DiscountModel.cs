using System.Text.Json.Serialization;
using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class DiscountModel : IDiscountModel
	{
		[JsonPropertyName("rate")]
		public decimal Rate { get; set; } = 0;

		[JsonPropertyName("amount")]
		public decimal Amount { get; internal set; }

		public DiscountModel(decimal rate)
		{
			Rate = rate;
		}
	}
}

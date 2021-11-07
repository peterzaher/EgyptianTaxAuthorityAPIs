using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class DiscountModel
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class TaxTotalsModel
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

using System.Text.Json.Serialization;
using Domain.DocumentModels;
using Domain.Enum;

namespace EInvoicing.DocumentComponent
{
	public class TaxableItemModel : ITaxableItemModel
	{
		public TaxableItemModel(TaxTypeCode taxType = TaxTypeCode.T1, TaxSubTypeCode subType = TaxSubTypeCode.V009, decimal rate = 14)
		{
			TaxType = taxType.ToString();
			SubType = subType.ToString();
			Rate = rate;
		}

		[JsonPropertyName("taxType")]
		public string TaxType { get; }

		[JsonPropertyName("subType")]
		//private SubTypes _subType;
		public string SubType { get; }

		[JsonPropertyName("rate")]
		public decimal Rate { get; }

		[JsonPropertyName("amount")]
		public decimal Amount { get; internal set; }
	}
}

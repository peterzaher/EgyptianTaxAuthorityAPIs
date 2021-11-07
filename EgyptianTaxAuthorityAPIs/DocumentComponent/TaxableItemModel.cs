using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class TaxableItemModel
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

		//public enum TaxTypes
		//{
		//	T1 = 1,
		//	T2,
		//	T3,
		//	T4,
		//	T5,
		//	T6,
		//	T7,
		//	T8,
		//	T9,
		//	T10,
		//	T11,
		//	T12,
		//}
		//public enum TaxSubTypes
		//{
		//	V001 = 1,
		//	V002,
		//	V003,
		//	V004,
		//	V005,
		//	V006,
		//	V007,
		//	V008,
		//	V009,
		//	V010,
		//	Tbl01,
		//	Tbl02,
		//}
	}
}

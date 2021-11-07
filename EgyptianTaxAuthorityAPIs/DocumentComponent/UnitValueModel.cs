using System.Text.Json.Serialization;

namespace EInvoicing.DocumentComponent
{
	public class UnitValueModel
	{
		[JsonPropertyName("")]
		private decimal _amountEGP;

		[JsonPropertyName("currencySold")]
		public string CurrencySold { get; set; } //currency code from iso 4217

		[JsonPropertyName("amountEGP")]
		public decimal AmountEGP //amount in EGP (Egyptian Pound)
		{
			set => _amountEGP = value;
			get
			{
				if (CurrencySold == "EGP") { return _amountEGP; }
				return AmountSold * CurrencyExchangeRate;

			}
		}

		[JsonPropertyName("amountSold")]
		public decimal AmountSold { get; set; } //amount of foreign currency - not EGP

		[JsonPropertyName("currencyExchangeRate")]
		public decimal CurrencyExchangeRate { get; set; } //mandatory if currencySold not EGP

		public UnitValueModel() { }
		public UnitValueModel(decimal amount)
		{
			AmountEGP = amount;
			CurrencySold = "EGP";
		}
		/*
		    "currencySold": "EGP",
            "amountEGP": 100000.00000,
            "amountSold": 0.00000,
            "currencyExchangeRate": 0.00000
		*/

	}
}

namespace Domain.DocumentModels;

public interface IUnitValueModel
{
	decimal AmountEGP { get; set; }
	decimal AmountSold { get; set; }
	decimal CurrencyExchangeRate { get; set; }
	string CurrencySold { get; set; }
}
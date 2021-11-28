namespace Domain.DocumentModels;

public interface ITaxTotalsModel
{
	decimal Amount { get; set; }
	string TaxType { get; set; }
}
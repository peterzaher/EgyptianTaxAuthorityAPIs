namespace Domain.DocumentModels;

public interface ITaxableItemModel
{
	decimal Amount { get; }
	decimal Rate { get; }
	string SubType { get; }
	string TaxType { get; }
}
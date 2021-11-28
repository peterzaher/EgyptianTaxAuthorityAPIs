namespace Domain.DocumentModels;

public interface IDiscountModel
{
	decimal Amount { get; }
	decimal Rate { get; set; }
}
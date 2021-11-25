namespace Domain.ResponseModels;

public interface IValidationStepsModel
{
	string Error { get; set; }
	string Name { get; set; }
	string Status { get; set; }
}
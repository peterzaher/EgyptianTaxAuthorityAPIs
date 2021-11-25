namespace Domain.ResponseModels;

public interface IValidationResultModel
{
	string Status { get; set; }
	IList<IValidationStepsModel> ValidationSteps { get; set; }
}
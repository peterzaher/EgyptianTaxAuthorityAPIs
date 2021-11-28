using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse;

public class ValidationStepsModel : IValidationStepsModel
{
	public string Name { get; set; }
	public string Status { get; set; }
	public string Error { get; set; }
}

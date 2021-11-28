using System.Collections.Generic;
using Domain.ResponseModels;

namespace EInvoicing.WebApiResponse;

public class ValidationResultModel : IValidationResultModel
{
	public string Status { get; set; }
	public IList<IValidationStepsModel> ValidationSteps { get; set; }
}

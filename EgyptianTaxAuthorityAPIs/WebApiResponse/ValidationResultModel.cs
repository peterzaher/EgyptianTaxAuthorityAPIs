using System.Collections.Generic;

namespace EInvoicing.WebApiResponse;

public class ValidationResultModel
{
	public string Status { get; set; }
	public IList<ValidationStepsModel> ValidationSteps { get; set; }
}

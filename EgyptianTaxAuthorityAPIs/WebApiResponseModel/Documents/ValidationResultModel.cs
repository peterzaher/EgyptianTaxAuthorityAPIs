using System.Collections.Generic;

namespace EInvoicing.WebApiResponseModel.Documents;

public class ValidationResultModel
{
	public string Status { get; set; }
	public IList<ValidationStepsModel> ValidationSteps { get; set; }
}

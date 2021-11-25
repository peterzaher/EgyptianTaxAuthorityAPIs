namespace Domain.ResponseModels;

public interface IErrorModel
{
	string Code { get; set; }
	List<IErrorModel> Details { get; set; }
	string Message { get; set; }
	string PropertyPath { get; set; }
	string Target { get; set; }
}
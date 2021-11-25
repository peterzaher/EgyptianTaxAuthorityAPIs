namespace Domain.ResponseModels;

public interface IRejectedDocumentModel
{
	IErrorModel Error { get; set; }
	string InternalId { get; set; }
}
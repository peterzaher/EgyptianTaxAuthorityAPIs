namespace Domain.ResponseModels;

public interface ISubmissionResponseModel
{
	List<IAcceptedDocumentModel> AcceptedDocuments { get; set; }
	List<IRejectedDocumentModel> RejectedDocuments { get; set; }
	string SubmissionId { get; set; }
}
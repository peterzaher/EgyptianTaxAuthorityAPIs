namespace Domain.ResponseModels;

public interface IAcceptedDocumentModel
{
	string InternalId { get; set; }
	string LongId { get; set; }
	string UUID { get; set; }
}
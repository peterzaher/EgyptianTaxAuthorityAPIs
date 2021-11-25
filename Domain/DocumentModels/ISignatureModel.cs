namespace Domain.DocumentModels;

public interface ISignatureModel
{
	string SignatureType { get; set; }
	string Value { get; set; }
}
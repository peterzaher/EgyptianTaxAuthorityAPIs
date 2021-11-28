namespace Domain.DocumentModels;

public interface IIssuerReceiverInfoModel
{
	IAddressModel Address { get; }
	string Id { get; }
	string Name { get; set; }
	string Type { get; }
}
namespace Domain.DocumentModels;

public interface IAddressModel
{
	string AdditionalInformation { get; set; }
	string BranchId { get; set; }
	string BuildingNumber { get; }
	string Country { get; }
	string Floor { get; set; }
	string Governate { get; }
	string Landmark { get; set; }
	string PostalCode { get; set; }
	string RegionCity { get; }
	string Room { get; set; }
	string Street { get; }
}
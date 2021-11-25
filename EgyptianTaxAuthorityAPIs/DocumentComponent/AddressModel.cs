using System;
using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class AddressModel : IAddressModel
	{
		public AddressModel(string country, string governate, string regionCity, string street, string buildingNumber, string branchId = "")
		{
			Country = !string.IsNullOrWhiteSpace(country) ? country : throw new ArgumentNullException(nameof(country), "Country cannot be null or empty");
			Governate = !string.IsNullOrWhiteSpace(governate) ? governate : throw new ArgumentNullException(nameof(governate), "Governate cannot be null or empty");
			RegionCity = !string.IsNullOrWhiteSpace(regionCity) ? regionCity : throw new ArgumentNullException(nameof(regionCity), "RegionCity cannot be null or empty");
			Street = !string.IsNullOrWhiteSpace(street) ? street : throw new ArgumentNullException(nameof(street), "Street cannot be null or empty");
			BuildingNumber = !string.IsNullOrWhiteSpace(buildingNumber) ? buildingNumber : throw new ArgumentNullException(nameof(buildingNumber), "BuldingNumber cannot be null or empty");
			BranchId = branchId;
		}
		public string BranchId { get; set; }
		public string Country { get; }
		public string Governate { get; }
		public string RegionCity { get; }
		public string Street { get; }
		public string BuildingNumber { get; } = "";
		public string PostalCode { get; set; } = "";
		public string Floor { get; set; } = "";
		public string Room { get; set; } = "";
		public string Landmark { get; set; } = "";
		public string AdditionalInformation { get; set; } = "";
	}
}

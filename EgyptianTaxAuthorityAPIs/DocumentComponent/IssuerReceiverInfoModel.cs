using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class IssuerReceiverInfoModel : IIssuerReceiverInfoModel
	{
		public IssuerReceiverInfoModel(string name, string id, string type, AddressModel address)
		{
			Name = name;
			Id = id;
			Type = type;
			Address = address;
			ValidateInputValues();
		}

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; }

		[JsonPropertyName("type")]
		public string Type { get; }

		[JsonPropertyName("address")]
		public IAddressModel Address { get; }

		private void ValidateInputValues()
		{
			if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentNullException(nameof(Name), "Name cannot be null or empty");
			if (string.IsNullOrWhiteSpace(Id)) throw new ArgumentNullException(nameof(Id), "id cannot be null or empty");
			if (string.IsNullOrWhiteSpace(Type)) throw new ArgumentNullException(nameof(Type), "type cannot be null or empty");
			if (Address is null) throw new ArgumentNullException(nameof(Address), "Address cannot be null or empty");

			string TypeErrorMsg = "Type must be a single characther of 'B' for bussiness, 'P' for person or 'F' for foreigner";
			string idErrorMsg = "Id must be a string of digits";
			string nationalIdErrorMsg = "National Id number must be 14 digits";

			Regex regex = new(@"^[BbPpFf]$");
			if (!regex.IsMatch(Type)) throw new ArgumentException(TypeErrorMsg);

			regex = new(@"^\d+$");
			if (!regex.IsMatch(Id)) throw new ArgumentException(idErrorMsg);


			if (Type == "P" || Type == "p")
			{
				if (Id.Length != 14) throw new ArgumentException(nationalIdErrorMsg);
			}
		}
	}
}

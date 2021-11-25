namespace Domain.DocumentModels;

public interface IDeliveryModel
{
	string Approach { get; set; }
	string CountryOfOrigin { get; set; }
	string DateValidity { get; set; }
	string ExportPort { get; set; }
	decimal GrossWeight { get; set; }
	decimal NetWeight { get; set; }
	string Packaging { get; set; }
	string Terms { get; set; }
}
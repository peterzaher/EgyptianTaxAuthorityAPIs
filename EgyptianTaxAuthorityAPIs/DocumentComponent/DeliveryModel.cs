using Domain.DocumentModels;

namespace EInvoicing.DocumentComponent
{
	public class DeliveryModel : IDeliveryModel
	{
		public DeliveryModel() { }
		public DeliveryModel(string countryOfOrigin, string dateValidity)
		{
			CountryOfOrigin = countryOfOrigin;
			DateValidity = dateValidity;
		}

		public string Approach { get; set; }
		public string Packaging { get; set; }
		public string DateValidity { get; set; }
		public string ExportPort { get; set; }
		public string CountryOfOrigin { get; set; }
		public decimal GrossWeight { get; set; }
		public decimal NetWeight { get; set; }
		public string Terms { get; set; }

	}
}

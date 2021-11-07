using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoicing.DocumentComponent
{
	public class DeliveryModel
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

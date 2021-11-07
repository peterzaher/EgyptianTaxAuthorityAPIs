using System.ComponentModel;

namespace EInvoicing.DocumentComponent
{
	public enum UnitTypeCode
	{
		[Description("Each")]
		EA = 1,
		[Description("Kilogram")]
		KGM,
		[Description("Gram")]
		GRM,
		[Description("Meter")]
		M,
		[Description("Centimeter")]
		CMT,
		[Description("Qubic Meter")]
		MTK,
		[Description("Square Meter")]
		MTQ,
		[Description("Square Centimeter")]
		CMQ,
		[Description("Barrel")]
		BBL
	}
}

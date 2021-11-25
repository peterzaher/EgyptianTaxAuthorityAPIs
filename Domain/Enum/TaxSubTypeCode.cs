using System.ComponentModel;

namespace Domain.Enum
{
	public enum TaxSubTypeCode
	{
		[Description("Export")]
		V001 = 1,
		[Description("Export to free areas and other areas")]
		V002,
		[Description("Exempted good or service")]
		V003,
		[Description("A non-taxable good or service")]
		V004,
		[Description("Exemptions for diplomats, consulates and embassies")]
		V005,
		[Description("Defence and National security Exemptions")]
		V006,
		[Description("Agreements exemptions")]
		V007,
		[Description("Special Exemptios and other reasons")]
		V008,
		[Description("General Item sales")]
		V009,
		[Description("Other Rates")]
		V010,
		[Description("Table tax (percentage)")]
		Tbl01,
		[Description("Table tax (Fixed Amount)")]
		Tbl02,
	}
}

using System.ComponentModel;


namespace EInvoicing.DocumentComponent
{
	public enum TaxTypeCode
	{
		[Description("Value added tax (VAT)")]
		T1 = 1,
		[Description("Table tax (percentage)")]
		T2,
		[Description("Table tax (Fixed Amount)")]
		T3,
		[Description("Withholding tax (WHT)")]
		T4,
		[Description("Stamping tax (percentage)")]
		T5,
		[Description("Stamping Tax (amount)")]
		T6,
		[Description("Entertainment tax")]
		T7,
		[Description("Resource development fee")]
		T8,
		[Description("Table tax (percentage)")]
		T9,
		[Description("Municipality Fees")]
		T10,
		[Description("Medical insurance fee")]
		T11,
		[Description("Other fees")]
		T12,
	}
}

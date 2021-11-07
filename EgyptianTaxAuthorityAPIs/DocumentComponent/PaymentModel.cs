using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoicing.DocumentComponent
{
	public class PaymentModel
	{
		public PaymentModel() { }
		public PaymentModel(string bankName, string bankAccountNo)
		{
			BankName = bankName;
			BankAccountNo = bankAccountNo;
		}
		public string BankName { get; set; }
		public string BankAddress { get; set; }
		public string BankAccountNo { get; set; }
		public string BankAccountIBAN { get; set; }
		public string SwiftCode { get; set; }
		public string Terms { get; set; }
	}
}

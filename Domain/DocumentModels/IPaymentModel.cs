namespace Domain.DocumentModels;

public interface IPaymentModel
{
	string BankAccountIBAN { get; set; }
	string BankAccountNo { get; set; }
	string BankAddress { get; set; }
	string BankName { get; set; }
	string SwiftCode { get; set; }
	string Terms { get; set; }
}
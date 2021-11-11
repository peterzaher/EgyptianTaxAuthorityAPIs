Imports EInvoicing.DocumentComponent
Imports EInvoicing
Imports System.Net.Http
Imports EInvoicing.Processing
Imports EInvoicing.Queries
Imports EInvoicing.WebApiResponse

Public Class Form1
	ReadOnly webCallController = New WebCallController()

	Private Async Sub SignDocumets(sender As Object, e As EventArgs) Handles SignDocumentsBtn.Click
		'Dim a As IRecentDocumentReceiver
		'creating first document with initial values
		Dim issuerAddress As New AddressModel("EG", "Qalobia", "Obour City", "1rst industrial zone", "0")
		Dim issuer = New IssuerReceiverInfoModel("Mohamed", "23", "B", issuerAddress)

		Dim receiverAddress As New AddressModel("EG", "Cairo", "Nasr City", "Makrab Ebid", 9)
		Dim receiver As New IssuerReceiverInfoModel("Ahmed", "124", "A", receiverAddress)

		'adding invoice line1 to document
		Dim invoiceLine1 As New InvoiceLineModel("1122", 2, 150, 0)
		Dim taxItem1 As New TaxableItemModel()
		invoiceLine1.TaxableItems.Add(taxItem1)
		invoiceLine1.Discount = New DiscountModel(5)

		'adding invoice line2 to document
		Dim invoiceLine2 As New InvoiceLineModel("3344", 1, 100, 0)
		invoiceLine2.Discount = New DiscountModel(10)
		Dim taxitem3 As New TaxableItemModel()
		invoiceLine2.TaxableItems.Add(taxitem3)

		Dim invoiceLines = New List(Of InvoiceLineModel) From {invoiceLine1, invoiceLine2}

		Dim document As New DocumentModel(issuer, receiver, invoiceLines, "1234") With {
			.DateTimeIssued = Date.UtcNow,
			.Payment = New PaymentModel("Ahly Bank", "1234567890")
		}

		'Dim docToText As String() = DocumentSerialization.SerializeDocuments(documents)
		'Debug.WriteLine(docToText(0))

		Debug.WriteLine("")
		Debug.WriteLine("")

		'Dim documents = New RootDocumentModel() {document}
		Dim documents = New List(Of DocumentModel) From {document}
		Dim result As String = Await DocumentProcessing.PrepareDocumentsToSend(documents)
		Debug.WriteLine(result)

		Debug.WriteLine("")
		Debug.WriteLine("")

		'Debug.WriteLine(DocumentSerialization.GetCertificate())
		'Debug.WriteLine(DocumentSerialization.EncryptDocument(1))
	End Sub

	Private Async Sub GetRecentDocuments(sender As Object, e As EventArgs) Handles GetDocuments.Click
		Dim sender1 As Button = DirectCast(sender, Button)
		sender1.Enabled = False

		Dim number As Integer = Int(NumericUpDownDocNumbers.Value)
		Debug.Print("Summary display of last " + number.ToString() + " documents submitted:")
		Debug.WriteLine("")

		Try
			Dim recentDocs As RecentDocumentQuery = Await webCallController.GetRecentDocumentsAsync(1, number)
			For Each doc As DocumentSummaryModel In recentDocs.DocumentsSummary
				Debug.WriteLine($"Invoice Id :  {doc.UUID}")
				Debug.WriteLine("------------------------------------------")
				Debug.Indent()
				Debug.WriteLine($"Issuer name:  {doc.IssuerName}")
				Debug.WriteLine($"Receiver name:  {doc.ReceiverName}")
				Debug.WriteLine($"Date issued:  {doc.DateTimeIssued}")
				Debug.WriteLine($"Internal Id:  {doc.InternalId}")
				Debug.Unindent()
				Debug.WriteLine("")
			Next
		Catch ex As Exception
			MessageBox.Show(ex.Message)
		Finally
			sender1.Enabled = True
		End Try

		'Debug.WriteLine("")
		'Debug.WriteLine($"{recentDocs.Metadata.TotalPages} {vbTab} {recentDocs.Metadata.TotalCount}")
	End Sub

	Private Async Sub ParseJson(sender As Object, e As EventArgs) Handles ParseJsonObjectBtn.Click

		'Debug.WriteLine(DateTime.UtcNow.ToString("u"))

		Dim issuerAddress As New AddressModel("EG", "Cairo", " Qasr EL-Nile", "Bldg. 210", "0") With
			{
			.PostalCode = "",
			.Floor = "",
			.Room = "",
			.Landmark = "",
			.AdditionalInformation = ""
			}
		Dim issuer As New IssuerReceiverInfoModel("Ahmed Amr", "538486562", "B", issuerAddress)

		Dim receiverAddress As New AddressModel("EG", "Egypt", "Giza", "Wahat Road", "Bldg.90") With
			{
			.PostalCode = "",
			.Floor = "",
			.Room = "",
			.Landmark = "",
			.AdditionalInformation = ""
			}

		Dim receiver As New IssuerReceiverInfoModel("Trust", "22001058095457", "P", receiverAddress)

		Dim payment As New PaymentModel With {
		.BankName = "",
		.BankAddress = "",
		.BankAccountNo = "",
		.BankAccountIBAN = "",
		.SwiftCode = "",
		.Terms = ""
		}

		Dim devliery As New DeliveryModel() With
			{
			.Approach = "",
			.Packaging = "",
			.DateValidity = "",
			.ExportPort = "",
			.CountryOfOrigin = "",
			.GrossWeight = 5,
			.NetWeight = 4,
			.Terms = ""
			}

		Dim invoiceLine1 As New InvoiceLineModel("AD0", 2D, 100_000.00000, UnitTypeCode.EA) With
		{
		.Description = "Audi A5",
		.UnitType = UnitTypeCode.EA,
		.Quantity = 2D,
		.ValueDifference = 0.00000,
		.TotalTaxableFees = 0.00000,
		.ItemsDiscount = 0.00000
		}

		'Dim discount1 As New DiscountModel(0D)
		'invoiceLine1.Discount = discount1
		Dim taxableItem1 As New TaxableItemModel(TaxTypeCode.T1)
		'invoiceLine1.TaxableItems = New List(Of TaxableItemModel) From {taxableItem1}
		invoiceLine1.TaxableItems.Add(taxableItem1)

		Dim invoiceLine2 As New InvoiceLineModel("BA0", 3.0, 50_000.00000, UnitTypeCode.EA) With
	{
	.Description = "BMW 720i",
	.UnitType = UnitTypeCode.EA,
	.Quantity = 3.0,
	.ValueDifference = 0.00000,
	.TotalTaxableFees = 0.00000,
	.ItemsDiscount = 0.00000
	}
		'Dim discount2 As New DiscountModel(0.00)
		'invoiceLine2.Discount = discount2
		Dim taxableItem2 As New TaxableItemModel(TaxTypeCode.T1)
		'invoiceLine2.TaxableItems = New List(Of TaxableItemModel) From {taxableItem2}
		invoiceLine2.TaxableItems.Add(taxableItem2)

		Dim invoiceLine3 As New InvoiceLineModel("Md8", 5.0, 200.0) With
	{
	.Description = "labtop",
	.UnitType = UnitTypeCode.EA,
	.Quantity = 5.0,
	.ValueDifference = 0.00000,
	.TotalTaxableFees = 0.00000,
	.ItemsDiscount = 0.00000
	}
		Dim unitValue As New UnitValueModel() With
			{
			.CurrencySold = "USD",
			.AmountSold = 10.0,
			.CurrencyExchangeRate = 20.0
			}
		Dim discount3 As New DiscountModel(10D)
		invoiceLine3.Discount = discount3

		Dim taxableItem3 As New TaxableItemModel(TaxTypeCode.T1, TaxSubTypeCode.V009, 14.0)
		Dim taxableItem4 As New TaxableItemModel(TaxTypeCode.T2, TaxSubTypeCode.Tbl01, 1.0)

		'invoiceLine3.TaxableItems = New List(Of TaxableItemModel) From {taxableItem3, taxableItem4}
		invoiceLine3.TaxableItems.Add(taxableItem3)
		invoiceLine3.TaxableItems.Add(taxableItem4)
		invoiceLine3.UnitValue = unitValue


		Dim invoiceLine4 As New InvoiceLineModel("YD5", 7.0, 100.0) With
		{
		.Description = "pepsi",
		.UnitType = UnitTypeCode.EA,
		.Quantity = 7.0,
		.ValueDifference = 0.00000,
		.TotalTaxableFees = 0.00000,
		.ItemsDiscount = 0.00000
		}

		'Dim discount4 As New DiscountModel(0.00)
		Dim taxableItem5 As New TaxableItemModel(TaxTypeCode.T1)
		'invoiceLine4.Discount = discount4
		'invoiceLine1.TaxableItems = New List(Of TaxableItemModel) From {taxableItem5}
		invoiceLine1.TaxableItems.Add(taxableItem5)

		Dim invoiceLines = New InvoiceLineModel() {invoiceLine1, invoiceLine2, invoiceLine3}
		Dim document As New DocumentModel(issuer, receiver, invoiceLines, "234")
		document.DateTimeIssued = Date.Now
		'Dim document As New RootDocumentModel("234")
		'document.Issuer = issuer
		'document.Receiver = receiver
		'document.InvoiceLines = invoiceLines
		'document.Payment = payment
		'document.ExtraDiscountAmount = 0.00000D

		Dim documents = New List(Of DocumentModel) From {document}
		Dim result As String = Await DocumentProcessing.PrepareDocumentsToSend(documents)
		Debug.Write(result)

	End Sub

	Private Async Sub SendDocBtn_Click(sender As Object, e As EventArgs) Handles SendDocBtn.Click
		Dim btn As Button = DirectCast(sender, Button)
		btn.Enabled = False

		Dim issuerAddress As New AddressModel("EG", "Qalyubiyya", "El Ubour", "13009", "1-15", "0")

		'Name = "المنزل للمفروشات هابيتات"
		'Habitat registration number: 204961254
		'Mostafa National Id: 26806240102792
		Dim issuer As New IssuerReceiverInfoModel("شركه المنزل للمفروشات هابيتات", "204961254", "B", issuerAddress)

		Dim receiverAddress As New AddressModel("EG", "Alexandria", "Montaza", "جمال عبد الناصر", "73") With
			{
			.Floor = "1"
			}

		Dim receiver As New IssuerReceiverInfoModel("محمد أحمد محمد محمد", "286614464", "B", receiverAddress)

		Dim invoiceLine1 As New InvoiceLineModel("02011622A", 1, 2029) With
		{
		.Description = "LAMINATION MATTRESS NEW GOLD 120x190",
		.UnitType = UnitTypeCode.EA
		}

		'Dim invoiceLine2 As New InvoiceLineModel("02110413A", 5, 194) With
		'{
		'.Description = "Pillow Fiber 40X120CM",
		'.UnitType = UnitTypeCode.EA
		'}

		Dim discount1 As New DiscountModel(19)
		Dim taxableItem1 As New TaxableItemModel(TaxTypeCode.T1)

		invoiceLine1.Discount = discount1
		invoiceLine1.TaxableItems.Add(taxableItem1)
		'invoiceLine1.TaxableItems = New List(Of TaxableItemModel) From {taxableItem1}

		'invoiceLine2.Discount = discount1
		'invoiceLine2.TaxableItems.Add(taxableItem1)
		'invoiceLine2.TaxableItems = New List(Of TaxableItemModel) From {taxableItem1}

		Dim invoiceLines = New InvoiceLineModel() {invoiceLine1} ', invoiceLine2}
		Dim document As New DocumentModel(issuer, receiver, invoiceLines, "62109644")

		document.ExtraDiscountAmount = 0
		document.DateTimeIssued = "2021-10-26T13:05"

		Dim documents = New List(Of DocumentModel) From {document}

		'output result to console for debugging only
		'documents = DocumentProcessing.SignDocuments(documents)
		'Dim a = DocumentProcessing.SerializeToJson(documents)
		'Debug.WriteLine(a)

		Dim response As SubmissionResponseModel = Await webCallController.SubmitDocumentsAsync(documents)


		'Dim successReponse = TryCast(response, SubmitResponse)

		'If successReponse IsNot Nothing Then
		'	Debug.WriteLine("UUID: {0}  Internal code: {1}", successReponse.SubmissionUUID, successReponse.AcceptedDocuments(0))
		'Else
		'	Dim failResponse As ErrorResponse = CType(response, ErrorResponse)
		'	Debug.WriteLine("Code: {0}  Message: {1} Details {2}", failResponse.Code, failResponse.Message, failResponse.Details)
		'End If

		Debug.WriteLine("SubmissionId: " + response.SubmissionId)
		SubmissionUUIDTextBox.Text = response.SubmissionId

		If response.AcceptedDocuments.Count > 0 Then
			Debug.WriteLine("Number of documents accepted: " + response.AcceptedDocuments.Count.ToString())
			For i As Integer = 0 To response.AcceptedDocuments.Count - 1
				Debug.WriteLine(response.AcceptedDocuments(i).InternalId)
				Debug.WriteLine(response.AcceptedDocuments(i).UUID)
				Debug.WriteLine(response.AcceptedDocuments(i).LongId)
			Next
		End If

		If response.RejectedDocuments.Count > 0 Then
			Debug.WriteLine("Number of documents rejected: " + response.RejectedDocuments.Count.ToString())
			For i As Integer = 0 To response.RejectedDocuments.Count - 1
				Debug.WriteLine(response.RejectedDocuments(i).InternalId + vbTab)
				Debug.Write(response.RejectedDocuments(i).InternalId)
				Debug.Write(response.RejectedDocuments(i).Error.Code)
				Debug.Write(response.RejectedDocuments(i).Error.Message)
				Debug.Write(response.RejectedDocuments(i).Error.PropertyPath)

				For Each errorDetail As ErrorModel In response.RejectedDocuments(i).Error.Details
					Debug.WriteLine(vbTab)
					Debug.Write(errorDetail.Code + vbTab)
					Debug.Write(errorDetail.Message + vbTab)
					Debug.Write(errorDetail.Target + vbTab)
					Debug.Write(errorDetail.PropertyPath)
				Next
			Next
		End If

		btn.Enabled = True

	End Sub

	Private Async Sub GetSubmissionStatus_Click(sender As Object, e As EventArgs) Handles GetSubmissionStatus.Click
		Dim btn As Button = DirectCast(sender, Button)
		btn.Enabled = False
		Dim submissionUuid As String = SubmissionUUIDTextBox.Text
		Dim submissionStatus As SubmissionQuery
		Try
			submissionStatus = Await webCallController.GetSubmissionStatusAysnc(submissionUuid, 1, 1)

			Debug.WriteLine("SubmissionId: " + submissionStatus.Submissionid + vbTab)
			Debug.WriteLine("Document count: " + submissionStatus.DocumentCount.ToString())
			Debug.WriteLine("Date received: " + submissionStatus.DateTimeReceived.ToString() + vbTab)
			Debug.WriteLine("Overall status: " + submissionStatus.OverallStatus)
			Debug.WriteLine("")
			Debug.WriteLine("DOCUMENT SUMMARY:")
			Debug.WriteLine("---------------------------")
			For Each item As DocumentSummaryModel In submissionStatus.DocumentSummary
				Debug.WriteLine(vbTab + "Receiver Name: " + item.ReceiverName)
				Debug.WriteLine(vbTab + "Receiver Id: " + item.ReceiverId)
				Debug.WriteLine(vbTab + "Date Issued: " + item.DateTimeIssued.ToString())
				Debug.WriteLine(vbTab + "Status: " + item.Status)
				Debug.WriteLine(" ")
			Next
		Catch ex As Exception
			MsgBox(ex.Message)
		Finally
			btn.Enabled = True
		End Try

	End Sub

	Private Async Sub GetDocStatus_Click(sender As Object, e As EventArgs) Handles GetDocStatus.Click
		Dim btn As Button = DirectCast(sender, Button)
		btn.Enabled = False

		Dim docUuid As String = DocUuidTxtBox.Text
		Try
			Dim document As New DocumentStatusModel()
			document = Await webCallController.GetDocumentStatusAsync(docUuid)
			Debug.WriteLine("")
			Debug.Print("DETAILS OF DOCUMENT NUMBER: " + docUuid)
			Debug.WriteLine("-------------------------------------------")
			Debug.Indent()
			Debug.WriteLine("Issuer name: " + document.IssuerName)
			Debug.WriteLine("Issuer number: " + document.IssuerId)
			Debug.WriteLine("Receiver name: " + document.receiverName)
			Debug.WriteLine("Receiver Id: " + document.ReceiverId)
			Debug.WriteLine("Date issued: " + document.DateTimeIssued)
			Debug.WriteLine("Status: " + document.Status)
			Debug.IndentLevel = 0
		Catch ex As HttpRequestException
			Dim msg = "Description: " + ex.Message + Environment.NewLine
			Dim Code = "Error code: " + ex.StatusCode.ToString
			Dim errorString = "An error occured." + Environment.NewLine + msg + Code
			Debug.Fail(errorString)
			MsgBox(errorString)
		End Try

		btn.Enabled = True
	End Sub

	Private Async Sub GetEtaCredBtn_Click(sender As Object, e As EventArgs) Handles GetEtaCredBtn.Click
		Dim btn As Button = DirectCast(sender, Button)
		btn.Enabled = False

		Dim Credential As (userId As String, password As String) = Await DataAccess.Credential.GetETACredentialAsync(webCallController.SqlConnectionStr)
		Debug.WriteLine(Credential.userId)
		Debug.WriteLine(Credential.password)

		btn.Enabled = True
	End Sub
End Class


'	Dim invoiceLine2 As New InvoiceLine("BA0", 3.0, 50_000.00000) With
'{
'.Description = "BMW 720i",
'.ItemCode = "EG-538486562-A155",
'.UnitType = "EA",
'.Quantity = 3.0,
'.ValueDifference = 0.00000,
'.TotalTaxableFees = 0.00000,
'.ItemsDiscount = 0.00000
'}
'	Dim discount2 As New Discount(0.00)
'	Dim taxableItem2 As New TaxableItem(TaxableItem.Types.T1)
'	invoiceLine2.Discount = discount2
'	invoiceLine2.TaxableItems.Add(taxableItem2)

'	Dim invoiceLine3 As New InvoiceLine("Md8", 5.0, 200.0) With
'{
'.Description = "labtop",
'.ItemCode = "EG-538486562-120",
'.UnitType = "EA",
'.Quantity = 5.0,
'.ValueDifference = 0.00000,
'.TotalTaxableFees = 0.00000,
'.ItemsDiscount = 0.00000
'}
'	Dim unitValue As New UnitValue() With
'		{
'		.CurrencySold = "USD",
'		.AmountSold = 10.0,
'		.CurrencyExchangeRate = 20.0
'		}
'	Dim discount3 As New Discount(0.00)
'	Dim taxableItem3 As New TaxableItem(TaxableItem.Types.T1, TaxableItem.SubTypes.V009, 14.0)
'	Dim taxableItem4 As New TaxableItem(TaxableItem.Types.T2, TaxableItem.SubTypes.Tbl01, 1.0)

'invoiceLine3.TaxableItems.Add(taxableItem3)
'invoiceLine3.TaxableItems.Add(taxableItem4)
'invoiceLine3.UnitValue = unitValue
'invoiceLine3.Discount = discount3


'Dim invoiceLine4 As New InvoiceLine("YD5", 7.0, 100.0) With
'{
'.Description = "pepsi",
'.ItemCode = "EG-538486562-10000201",
'.UnitType = "EA",
'.Quantity = 7.0,
'.ValueDifference = 0.00000,
'.TotalTaxableFees = 0.00000,
'.ItemsDiscount = 0.00000
'}

'Dim discount4 As New Discount(0.00)
'Dim taxableItem5 As New TaxableItem(TaxableItem.Types.T1)
'invoiceLine4.Discount = discount4
'invoiceLine1.TaxableItems.Add(taxableItem5)
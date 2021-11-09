using System;
using System.Text.Json.Serialization;

namespace EInvoicing.Queries;

public class DocumentSummaryModel
{
	public string PublicUrl { get; set; }

	[JsonPropertyName("uuid")]
	public string UUID { get; set; }

	[JsonPropertyName("submissionUUID")]
	public string SubmissionUUID { get; set; }

	[JsonPropertyName("longId")]
	public string LongId { get; set; }

	[JsonPropertyName("internalId")]
	public string InternalId { get; set; }

	[JsonPropertyName("typeName")]
	public string TypeName { get; set; }

	[JsonPropertyName("typeVersionName")]
	public string TypeVersionName { get; set; }

	[JsonPropertyName("issuerId")]
	public string IssuerId { get; set; }

	[JsonPropertyName("issuerName")]
	public string IssuerName { get; set; }

	[JsonPropertyName("receiverId")]
	public string ReceiverId { get; set; }

	[JsonPropertyName("receiverName")]
	public string ReceiverName { get; set; }

	public int ReceiverType { get; set; }

	[JsonPropertyName("dateTimeIssued")]
	public DateTimeOffset? DateTimeIssued { get; set; }

	[JsonPropertyName("dateTimeReceived")]
	public DateTimeOffset? DateTimeReceived { get; set; }

	[JsonPropertyName("totalSales")]
	public decimal TotalSales { get; set; }

	[JsonPropertyName("totalDiscount")]
	public decimal TotalDiscount { get; set; }

	[JsonPropertyName("netAmount")]
	public decimal NetAmount { get; set; }

	public string DocumentStatusReason { get; set; }

	[JsonPropertyName("total")]
	public decimal Total { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("cancelRequestDate")]
	public DateTimeOffset? CancelRequestDate { get; set; }

	[JsonPropertyName("rejectRequestDate")]
	public DateTimeOffset? RejectRequestDate { get; set; }

	[JsonPropertyName("cancelRequestDelayedDate")]
	public DateTimeOffset? CancelRequestDelayedDate { get; set; }

	[JsonPropertyName("rejectRequestDelayedDate")]
	public DateTimeOffset? RejectRequestDelayedDate { get; set; }

	[JsonPropertyName("declineCancelRequestDate")]
	public DateTimeOffset? DeclineCancelRequestDate { get; set; }

	[JsonPropertyName("declineRejectRequestDate")]
	public DateTimeOffset? DeclineRejectRequestDate { get; set; }

	public DateTimeOffset? CanbeCancelledUntil { get; set; }

	public DateTimeOffset? CanbeRejectedUntil { get; set; }
}

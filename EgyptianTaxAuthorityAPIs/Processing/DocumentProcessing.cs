using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EInvoicing.DocumentComponent;
using System.Text.Json;
using DataAccess;
using EInvoicing.WebApiResponse;
using System;
using Domain.DocumentModels;
using System.IO;

#if DEBUG
[assembly: InternalsVisibleTo("UIApplication")]
#endif

[assembly: InternalsVisibleTo("EInvoiceUnitTests")]


namespace EInvoicing.Processing;

internal static class DocumentProcessing
{
	internal static async Task SaveSubmissionToDbAsync(SubmissionResponseModel submissionResponse, string sqlDbConnectionStr)
	{
		string submissionId = submissionResponse.SubmissionId;
		string submissionDetail = DocumentSerialization.SerializeToJson(submissionResponse);
		await Submission.InsertSubmissionAsync(submissionId, submissionDetail, sqlDbConnectionStr);
	}

	internal static async Task<string> PrepareDocumentsToSend(IList<string> documentList, string sqlDbConnectionStr)
	{
		IList<JsonDocument> documents = new List<JsonDocument>();

		foreach (string document in documentList)
		{
			string canonicalStr = await Task.Run(() => DocumentSerialization.GetDocumentCanonicalString(document));
			byte[] documentUtf8Encoded = Encoding.UTF8.GetBytes(canonicalStr);
			string documentSignature = await DocumentSigning.ComputeSignture(documentUtf8Encoded, sqlDbConnectionStr);

			//string docWithSignature = document.Remove(document.Length - 2) + $"\"{documentSignature}\"]}}";
			string docWithSignature = document.Insert(document.Length - 2, $"\"{documentSignature}\"");
			JsonDocument jsonDocument = JsonDocument.Parse(docWithSignature);
			documents.Add(jsonDocument);
		}

		var rootDocument = new { documents = documents };
		return DocumentSerialization.SerializeToJson(rootDocument);
	}
}

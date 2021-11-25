using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EInvoicing.DocumentComponent;
using System.Text.Json;

#if DEBUG
[assembly: InternalsVisibleTo("UIApplication")]
#endif

[assembly: InternalsVisibleTo("EInvoiceUnitTests")]


namespace EInvoicing.Processing;

internal static class DocumentProcessing
{
	public static async Task<string> PrepareDocumentsToSend(IList<DocumentModel> documentList, string sqlDbConnectionStr)
	{
		foreach (DocumentModel doc in documentList)
		{
			string documentAsTxt = await Task.Run(() => DocumentSerialization.ConvertDocumentToText(doc));
			byte[] documentUtf8Encoded = Encoding.UTF8.GetBytes(documentAsTxt);
			//string signedDocument = await DocumentSigning.ComputeSignture(documentUtf8Encoded, sqlDbConnectionStr );
			
			//SignatureModel signature = new(signedDocument);
			//doc.Signatures.Add(signature);
		}
		var documents = new { documents = documentList };

		var json = DocumentSerialization.SerializeToJson(documents);
		var jDoc = JsonDocument.Parse(json);
		//string kkk = DocumentSerialization.GetDocumentCanonicalVersion(jDoc);
		

		return DocumentSerialization.SerializeToJson(documents);


	}
}

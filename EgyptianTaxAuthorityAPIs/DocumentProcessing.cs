using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using EInvoicing.DocumentComponent;
using System.Formats.Asn1;

#if DEBUG
[assembly: InternalsVisibleTo("UIApplication")]
#endif

[assembly: InternalsVisibleTo("EInvoiceUnitTests")]


namespace EInvoicing;

internal static class DocumentProcessing
{
	public static async Task<string> PrepareDocumentsToSend(IList<DocumentModel> documentList)
	{
		foreach (DocumentModel doc in documentList)
		{
			string documentAsTxt = await Task.Run(() => DocumentSerializtion.ConvertDocumentToText(doc));
			byte[] documentUtf8Encoded = Encoding.UTF8.GetBytes(documentAsTxt);
			string signedDocument = await DocumentSigning.ComputeSignture(documentUtf8Encoded);
			SignatureModel signature = new(signedDocument);
			doc.Signatures.Add(signature);
		}
		var documents = new { documents = documentList };
		return DocumentSerializtion.SerializeToJson(documents);
	}
}

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

//[SupportedOSPlatform("windows")]
internal static class DocumentProcessing
{
	//public static async Task<string> GetSignedJsonString(DocumentCollectionModel documentCollection)
	//{

	//}

	public static async Task<string> PrepareDocumentsToSend(IList<DocumentModel> documentList)
	{
		foreach (DocumentModel doc in documentList)
		{
			string documentAsTxt = await Task.Run(() => ConvertDocumentToText(doc));
			byte[] documentUtf8Encoded = Encoding.UTF8.GetBytes(documentAsTxt);
			string signedDocument = await ComputeSignture(documentUtf8Encoded);
			SignatureModel signature = new(signedDocument);
			doc.Signatures.Add(signature);
		}
		var documents = new { documents = documentList };
		return SerializeToJson(documents);
	}
	internal static string ConvertDocumentToText(object document)
	{
		string result = "";
		Type type = document.GetType();
		PropertyInfo[] propertiesInfo = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

		foreach (PropertyInfo propertyInfo in propertiesInfo)
		{
			string propertyName = propertyInfo.Name.ToUpper();
			if (propertyName == "SIGNATURES") continue;
			var propertyValue = propertyInfo.GetValue(document);
			if (propertyValue == null) continue;
			//if (propertyValue is decimal c && c == 0) continue;

			result += $"\"{propertyName}\"";

			Type propertyType = propertyInfo.PropertyType;

			if (propertyType == typeof(IList<InvoiceLineModel>))
			{
				IList<InvoiceLineModel> list = (IList<InvoiceLineModel>)propertyValue;
				foreach (InvoiceLineModel item in list)
				{
					result += $"\"{propertyName}\"{ConvertDocumentToText(item)}";
				}
				continue;
			}

			if (propertyType == typeof(IList<TaxableItemModel>))
			{
				List<TaxableItemModel> list = (List<TaxableItemModel>)propertyValue;
				foreach (TaxableItemModel item in list)
				{
					result += $"\"{propertyName}\"{ConvertDocumentToText(item)}";
				}
				continue;
			}

			if (propertyType == typeof(PaymentModel))
			{
				PaymentModel payment = (PaymentModel)propertyValue;
				result += ConvertDocumentToText(payment);
				continue;
			}

			if (propertyType == typeof(UnitValueModel))
			{
				UnitValueModel unitValue = (UnitValueModel)propertyValue;
				result += ConvertDocumentToText(unitValue);
				continue;
			}

			if (propertyType == typeof(IssuerReceiverInfoModel))
			{
				IssuerReceiverInfoModel issuerReceiverInfo = (IssuerReceiverInfoModel)propertyValue;
				result += ConvertDocumentToText(issuerReceiverInfo);
				continue;
			}

			if (propertyType == typeof(AddressModel))
			{
				AddressModel address = (AddressModel)propertyValue;
				result += ConvertDocumentToText(address);
				continue;
			}

			if (propertyType == typeof(DiscountModel))
			{
				DiscountModel discount = (DiscountModel)propertyValue;
				result += ConvertDocumentToText(discount);
				continue;
			}

			if (propertyType == typeof(List<TaxTotalsModel>))
			{
				List<TaxTotalsModel> taxTotals = (List<TaxTotalsModel>)propertyValue;
				foreach (TaxTotalsModel item in taxTotals)
				{
					result += $"\"{propertyName}\"{ConvertDocumentToText(item)}";
				}
				continue;
			}
			result += $"\"{propertyValue}\"";
		}

#if DEBUG
		if (System.IO.Directory.Exists("c:\\Doc\\DebugOutput"))
		{
			System.IO.File.WriteAllBytes(@"c:\doc\debugoutput\canonical.txt", Encoding.UTF8.GetBytes(result));
		}
#endif

		return result;
	}

	internal static async Task<string> ComputeSignture(byte[] documentAsBytes)
	{
		X509Certificate2 signerCertificate = GetSigningCertificate();

		Signature.GetSignature(signerCertificate); //testing serial number

		Oid messageDigestOid = new("1.2.840.113549.1.7.5");
		ContentInfo content = new(messageDigestOid, documentAsBytes);
		SignedCms signedCms = new(SubjectIdentifierType.IssuerAndSerialNumber, content, true);
		CmsSigner signer = new(SubjectIdentifierType.IssuerAndSerialNumber, signerCertificate);
		signer.IncludeOption = X509IncludeOption.EndCertOnly;
		signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.Now));

		//Signature.GetSignature(signerCertificate);

		Pkcs9AttributeObject signingCertV2Encoded = CreateSigningCerAttribute(signerCertificate);

		string test = signingCertV2Encoded.Format(true);

		signer.SignedAttributes.Add(signingCertV2Encoded);

		if (!signer.Certificate.HasPrivateKey)
		{
			throw new Exception("No private key found");
		}
		try
		{
			await Task.Run(() => signedCms.ComputeSignature(signer, false));
			await Task.Run(() => signedCms.CheckSignature(false));
		}
		catch (CryptographicException e)
		{
			throw new Exception("Error reading USB token", e);
		}
		byte[] encoded = signedCms.Encode();
		return Convert.ToBase64String(encoded);
	}

	internal static X509Certificate2 GetSigningCertificate()
	{
		X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
		try
		{
			store.Open(OpenFlags.OpenExistingOnly);
			X509Certificate2Collection certCollection = store.Certificates;
			X509Certificate2Collection certificates = certCollection.Find(X509FindType.FindBySubjectName, @"شركه المنزل للمفروشات هابيتات", true);
			if (certificates.Count == 0)
			{
				return null;
			}
			return certificates[0];
		}
		catch (Exception e)
		{
			throw new Exception("Error finding a valid certificate", e);
		}
	}

	internal static string SerializeToJson(object documentList)
	{
		JsonSerializerOptions options = new()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters = { new JsonStringEnumConverter() }
		};

		string result = JsonSerializer.Serialize(documentList, options);

#if DEBUG
		if (System.IO.Directory.Exists("c:\\Doc\\DebugOutput"))
		{
			System.IO.File.WriteAllBytes(@"c:\doc\debugoutput\json.txt", Encoding.UTF8.GetBytes(result));
		}
#endif

		return result;
	}

	internal static Pkcs9AttributeObject CreateSigningCerAttribute(X509Certificate2 certificate)
	{
		AsnWriter writer = new(AsnEncodingRules.DER);

		AsnWriter.Scope signingCertificate = writer.PushSequence();

		// ****************** ESSCertIDV2 Field ******************
		AsnWriter.Scope essCertIDv2 = writer.PushSequence();

		writer.PushSequence();

		writer.WriteOctetString(certificate.GetCertHash(HashAlgorithmName.SHA256));

		// ****************** IssuerSerial Field ******************
		writer.PushSequence();
		writer.PushSequence();
		writer.WriteEncodedValue(certificate.IssuerName.RawData);
		writer.PopSequence();
		writer.WriteInteger(int.Parse(certificate.SerialNumber, System.Globalization.NumberStyles.HexNumber));
		writer.PopSequence();

		writer.PopSequence();

		essCertIDv2.Dispose();

		signingCertificate.Dispose();

		Pkcs9AttributeObject signingAttribute = new(@"1.2.840.113549.1.9.16.2.47", writer.Encode());

#if DEBUG
		string textstring = signingAttribute.Format(true);
#endif

		return signingAttribute;
	}
}

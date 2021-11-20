using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataAccess;

namespace EInvoicing.Processing;

internal static class DocumentSigning
{
	private static string _certificateSubjectName;
	internal static async Task<string> ComputeSignture(byte[] documentAsBytes, string sqlDbConnectionStr)
	{
		if (string.IsNullOrEmpty(_certificateSubjectName))
		{
			_certificateSubjectName = await WebApiParameter.GetParameterByKey(sqlDbConnectionStr, "CertificateSubjectName");
		}

		X509Certificate2 signerCertificate = GetSigningCertificate(_certificateSubjectName);

		Oid messageDigestOid = new("1.2.840.113549.1.7.5");
		System.Security.Cryptography.Pkcs.ContentInfo content = new(messageDigestOid, documentAsBytes);
		SignedCms signedCms = new(SubjectIdentifierType.IssuerAndSerialNumber, content, true);

		CmsSigner signer = new(SubjectIdentifierType.IssuerAndSerialNumber, signerCertificate);
		signer.IncludeOption = X509IncludeOption.EndCertOnly;

		signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.UtcNow));
		Pkcs9AttributeObject signingCertV2Encoded = CreateSigningAttribute(signerCertificate);
		signer.SignedAttributes.Add(signingCertV2Encoded);

		if (!signer.Certificate.HasPrivateKey)
		{
			throw new Exception("No private key found");
		}

		try
		{
			await Task.Run(() => signedCms.ComputeSignature(signer, false));
		}
		catch (CryptographicException e)
		{
			throw new Exception("Error reading USB token", e);
		}
		byte[] encoded = signedCms.Encode();
		return Convert.ToBase64String(encoded);
	}

	private static X509Certificate2 GetSigningCertificate(string subjectName)
	{
		X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
		store.Open(OpenFlags.OpenExistingOnly);
		X509Certificate2Collection certCollection = store.Certificates;

		X509Certificate2Collection certificates = certCollection.Find(X509FindType.FindBySubjectName, subjectName, true);
		return certificates[0];
	}

	private static Pkcs9AttributeObject CreateSigningAttribute(X509Certificate2 certificate)
	{
		AsnWriter writer = new(AsnEncodingRules.DER);

		AsnWriter.Scope signingCertificate = writer.PushSequence(); //parent seq of 2 elm

		// ****************** ESSCertIDV2 Field ******************
		AsnWriter.Scope essCertIDv2 = writer.PushSequence(); //seq 1

		writer.PushSequence();
		writer.WriteOctetString(certificate.GetCertHash(HashAlgorithmName.SHA256));

		// ****************** IssuerSerial Field ******************
		writer.PushSequence(); //sub seq of 2 elm

		//Issuer feild
		writer.PushSequence();
		AsnWriter.Scope contextSpecific = writer.PushSequence(new Asn1Tag(TagClass.ContextSpecific, 4, true));
		writer.WriteEncodedValue(certificate.IssuerName.RawData);
		contextSpecific.Dispose();
		writer.PopSequence();

		//Serial field
		writer.WriteInteger(int.Parse(certificate.SerialNumber, System.Globalization.NumberStyles.HexNumber));
		writer.PopSequence(); //pop seq of 2 elm

		writer.PopSequence();

		essCertIDv2.Dispose();

		signingCertificate.Dispose(); //pop parent seq

		Pkcs9AttributeObject signingAttribute = new(@"1.2.840.113549.1.9.16.2.47", writer.Encode());

#if DEBUG
		string textstring = signingAttribute.Format(true);
#endif

		return signingAttribute;
	}
}

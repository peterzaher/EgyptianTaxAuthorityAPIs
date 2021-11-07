using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EInvoicing;

internal static class DocumentSigning
{
	internal static async Task<string> ComputeSignture(byte[] documentAsBytes)
	{
		X509Certificate2 signerCertificate = GetSigningCertificate();

		Oid messageDigestOid = new("1.2.840.113549.1.7.5");
		ContentInfo content = new(messageDigestOid, documentAsBytes);
		SignedCms signedCms = new(SubjectIdentifierType.IssuerAndSerialNumber, content, true);
		CmsSigner signer = new(SubjectIdentifierType.IssuerAndSerialNumber, signerCertificate);
		signer.IncludeOption = X509IncludeOption.EndCertOnly;
		signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.Now));

		//Signature.GetSignature(signerCertificate);

		Pkcs9AttributeObject signingCertV2Encoded = CreateSigningAttribute(signerCertificate);

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

	private static X509Certificate2 GetSigningCertificate()
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

	private static Pkcs9AttributeObject CreateSigningAttribute(X509Certificate2 certificate)
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

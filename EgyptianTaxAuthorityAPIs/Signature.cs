using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace EInvoicing
{
	internal static class Signature
	{

		internal static void GetSignature(X509Certificate2 cert)
		{
			//CmsSignedData dd = new(cert.);

			//SigningCertificateV2 signinCertV2 = SigningCertificateV2.GetInstance(cert);
			//var der = signinCertV2.GetDerEncoded();

			//DerObjectIdentifier sha256Der = new("2.16.840.1.101.3.4.2");
			//Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier algId = new(sha256Der);

			//DerInteger serialNumber = new(cert.GetSerialNumber());

			//IssuerSerial issuerSerial = new(

			//EssCertIDv2 essCerIDv2 = new(cer
			//SigningCertificateV2 signingCertificatev2;

			string ss = cert.SerialNumber;
			int serial2 = int.Parse(ss, System.Globalization.NumberStyles.HexNumber);
		}

		internal static void GetSigingCertificateV2(X509Certificate2 cert)
		{
			byte[] certHash = SHA256.HashData(cert.RawData);
			DerObjectIdentifier sha256Der = new("2.16.840.1.101.3.4.2");
			Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier algId = new(sha256Der);

			EssCertIDv2 essCert1 = new(algId, certHash);
			SigningCertificateV2 scv2 = new(new EssCertIDv2[] { essCert1 });
			Org.BouncyCastle.Asn1.Cms.Attribute certHAttribute = new(Org.BouncyCastle.Asn1.Pkcs.PkcsObjectIdentifiers.IdAASigningCertificateV2, new DerSet(scv2));
			Asn1EncodableVector v = new();
			v.Add(certHAttribute);
			//Org.BouncyCastle.Asn1.Cms.AttributeTable AT = new(v);
			

		}

	}
}

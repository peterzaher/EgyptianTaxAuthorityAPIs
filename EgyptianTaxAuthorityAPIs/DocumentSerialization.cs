using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EInvoicing.DocumentComponent;

namespace EInvoicing;

internal static class DocumentSerialization
{
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
}

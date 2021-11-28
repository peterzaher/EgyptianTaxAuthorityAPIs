using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess;

internal static class StagedDocuments
{
	internal static async Task<IList<string>> GetStagedDocumentsAsync(DateTimeOffset upTo, string sqlConnectionStr)
	{
		IList<string> documents = new List<string>();
		using SqlConnection sqlConnection = new(sqlConnectionStr);
		using SqlCommand cmd = sqlConnection.CreateCommand();

		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		cmd.CommandText = "eta.usp_GetStagedDocuments";
		cmd.Parameters.Add("@UpTo", System.Data.SqlDbType.DateTimeOffset).Value = upTo;

		await sqlConnection.OpenAsync();
		SqlDataReader sqlDataReader = await cmd.ExecuteReaderAsync();

		while (sqlDataReader.Read())
		{
			documents.Add(sqlDataReader.GetString(0));
		}
		return documents;
	}

	internal static async Task InsertDocumentAsync(string internalId, string jsonDocument, string sqlConnectionStr)
	{
		using SqlConnection sqlConnection = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_InsertDocument", sqlConnection);
		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		cmd.Parameters.Add("@InternalId", System.Data.SqlDbType.VarChar, 12).Value = internalId;
		cmd.Parameters.Add("@JsonString", System.Data.SqlDbType.NVarChar).Value = jsonDocument;

		await sqlConnection.OpenAsync();
		await cmd.ExecuteNonQueryAsync();
	}
}

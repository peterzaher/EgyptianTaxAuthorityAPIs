using System.Data.SqlTypes;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UIApplication")]
#endif

[assembly: InternalsVisibleTo("EInvoicing")]

namespace DataAccess;

internal static class Credential
{
	internal static async Task<(string, string)> GetCredentialFromDbAsync(string sqlConnectionStr)
	{
		(string userId, string password) credential = default;
		using (SqlConnection conn = new(sqlConnectionStr))
		{
			using SqlCommand cmd = new("eta.usp_GetCredential", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			await conn.OpenAsync();
			SqlDataReader reader = await cmd.ExecuteReaderAsync();

			while (await reader.ReadAsync())
			{
				credential = (reader.GetString("UserId"), reader.GetString("Secret1"));
			}
			await reader.CloseAsync();
		}
		return credential;
	}

	internal static async Task<(string, DateTimeOffset)> GetTokenFromLocalDbAsync(string sqlConnectionStr)
	{
		(string value, DateTimeOffset startTimeOffset) token = default;
		using SqlConnection conn = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_GetToken", conn);
		cmd.CommandType = CommandType.StoredProcedure;

		await conn.OpenAsync();
		SqlDataReader reader = await cmd.ExecuteReaderAsync();
		if (reader.HasRows)
		{
			await reader.ReadAsync();
			token = (reader.GetSqlString(0).IsNull ? "" : reader.GetSqlString(0).Value, reader.GetDateTimeOffset(1));
		}
		await conn.CloseAsync();
		return token;
	}

	internal static async Task PersistTokenToDbAsync(string token, string sqlConnectionStr)
	{
		using SqlConnection conn = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_SaveToken", conn);
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.Add("@token", SqlDbType.VarChar).Value = token;

		await conn.OpenAsync();
		await cmd.ExecuteNonQueryAsync();
		await conn.CloseAsync();
	}
}

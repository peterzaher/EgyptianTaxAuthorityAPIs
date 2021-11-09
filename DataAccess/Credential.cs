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
	internal static async Task<(string, string)> GetETACredentialAsync(string sqlConnectionStr)
	{
		(string userId, string password) credential = ("", "");
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
			reader.Close();
		}
		return credential;
	}

	internal static async Task<(string?, DateTime)> GetTokenFromLocalDB(string sqlConnectionStr)
	{
		(string? value, DateTime startTime) token = (default, default);
		using SqlConnection conn = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_GetToken", conn);
		cmd.CommandType = CommandType.StoredProcedure;

		await conn.OpenAsync();
		SqlDataReader reader = await cmd.ExecuteReaderAsync();
		if (reader.HasRows)
		{
			await reader.ReadAsync();
			token = (reader.GetString("Value"), reader.GetDateTime("LastModified"));
		}
		conn.Close();
		return token;
	}

	internal static async Task PersistToken(string sqlConnectionStr, string token)
	{
		using SqlConnection conn = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_SaveToken", conn);
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.Add("@token", SqlDbType.VarChar).Value = token;

		await conn.OpenAsync();
		await cmd.ExecuteNonQueryAsync();
		conn.Close();
	}
}

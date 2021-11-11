using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess;

internal static class WebApiParameter
{
	internal static async Task<string> GetParameterByKey(string sqlConnectionStr, string key)
	{
		string value = "";
		using SqlConnection conn = new(sqlConnectionStr);
		using SqlCommand cmd = conn.CreateCommand();
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.CommandText = "eta.usp_GetParameterByKey";
		cmd.Parameters.Add("@key", SqlDbType.VarChar, 20).Value = key;

		conn.OpenAsync().Wait();

		SqlDataReader reader = await cmd.ExecuteReaderAsync();
		if (reader.HasRows)
		{
			await reader.ReadAsync();
			value = reader.GetString("Value");
		}
		return value;
	}
}

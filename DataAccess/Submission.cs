using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ResponseModels;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace DataAccess;

internal static class Submission
{
	internal static async Task InsertSubmissionAsync(string submissionId, string submissionDetail, string sqlConnectionStr)
	{
		using SqlConnection sqlConnection = new(sqlConnectionStr);
		using SqlCommand cmd = new("eta.usp_InsertSubmission", sqlConnection);
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.Add("@SubmissionId", SqlDbType.Char, 26).Value = submissionId;
		cmd.Parameters.Add("@SubmissionDetail", SqlDbType.VarChar).Value = submissionDetail;

		await sqlConnection.OpenAsync();
		await cmd.ExecuteNonQueryAsync();
	}
}

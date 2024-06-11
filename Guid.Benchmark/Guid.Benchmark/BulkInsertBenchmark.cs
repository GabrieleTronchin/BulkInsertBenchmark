using BenchmarkDotNet.Attributes;
using Dapper;
using MassTransit;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Guid.Benchmark;

public class BulkInsertBenchmark
{
    private const string ConnectionString = "Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True";

    [Params(1)]
    public int NumberOfRow { get; set; }

    record Test(System.Guid id, string description);

    [Benchmark]
    public async Task DapperBulkInsert()
    {
        using var connection = new SqlConnection(ConnectionString);
        var list2Insert = new List<Test>();

        for (int i = 0; i < this.NumberOfRow; i++)
        {
            list2Insert.Add(new Test(System.Guid.NewGuid(), $"Test Desc {i}"));
        }

        await connection.ExecuteAsync("INSERT INTO DapperBulkInsert (id, Description) VALUES (@id, @description)", list2Insert);
    }

    [Benchmark]
    public async Task BulkCopyInsert()
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
        {
            bulkCopy.DestinationTableName = "DapperBulkCopyInsert";

            var table = new DataTable();
            table.Columns.Add("id", typeof(System.Guid));
            table.Columns.Add("Description", typeof(string));

            for (int i = 0; i < this.NumberOfRow; i++)
            {
                table.Rows.Add( System.Guid.NewGuid(), $"Test Desc {i}");
            }

            await bulkCopy.WriteToServerAsync(table);
        }

        transaction.Commit();
    }

    [Benchmark]
    public async Task BulkInsertTable() {

        var table = new DataTable();
        table.Columns.Add("id", typeof(System.Guid));
        table.Columns.Add("Description", typeof(string));

        for (int i = 0; i < this.NumberOfRow; i++)
        {
            table.Rows.Add(System.Guid.NewGuid(), $"Test Desc {i}");
        }

        using var connection = new SqlConnection(ConnectionString);

        await connection.ExecuteAsync("usp_SampleTableInsert", new { TVP = table.AsTableValuedParameter("SampleTableType") }, commandType: CommandType.StoredProcedure);

    }

}

using BenchmarkDotNet.Attributes;
using Dapper;
using MassTransit;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Guid.Benchmark;

public class Benchmark
{

    public async Task Execute()
    {
       
        await DapperCustomInsert();

        await DapperBulkInsert();

        await BulkCopyInsert();

    }

    [Benchmark]
    public async Task DapperCustomInsert()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");
        for (int e = 0; e < 1; e++)
        {

            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine(@"INSERT INTO DapperCustomInsert (id, Description)
                    VALUES ");

            for (int i = 0; i < 10; i++)
            {
                stringBuilder.Append($"('{NewId.NextSequentialGuid()}', 'Test Desc {e + i}')");

                if (i < 9) stringBuilder.Append(", ");
            }
            stringBuilder.Append(';');

            await connection.ExecuteAsync(stringBuilder.ToString());

            stringBuilder.Clear();
        }

    }

    record Test(System.Guid id, string description);

    [Benchmark]
    public async Task DapperBulkInsert()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");
        var list2Insert = new List<Test>();

        for (int i = 0; i < 10; i++)
        {
            list2Insert.Add(new Test(System.Guid.NewGuid(), $"Test Desc {i}"));
        }

        await connection.ExecuteAsync("INSERT INTO DapperBulkInsert (id, Description) VALUES (@id, @description)", list2Insert);
    }

    [Benchmark]
    public async Task BulkCopyInsert()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
        {
            bulkCopy.DestinationTableName = "DapperBulkCopyInsert";

            var table = new DataTable();
            table.Columns.Add("id", typeof(System.Guid));
            table.Columns.Add("Description", typeof(string));

            for (int i = 0; i < 10; i++)
            {
                table.Rows.Add( System.Guid.NewGuid(), $"Test Desc {i}");
            }

            await bulkCopy.WriteToServerAsync(table);
        }

        transaction.Commit();
    }


}

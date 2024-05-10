using BenchmarkDotNet.Attributes;
using Dapper;
using MassTransit;
using System.Data.SqlClient;
using System.Text;


namespace Guid.Benchmark;

public class Benchmark
{

    public async Task Execute()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");

        await SeedFastTable(connection);

        await SeedSlowTable(connection);

        await SeedIdentityTable(connection);

        var slow = CountSlow();

        Console.WriteLine($"Slow = {slow}");

        var fast = CountFast();

        Console.WriteLine($"Slow = {fast}");
    }

    private async Task SeedFastTable(SqlConnection connection)
    {
       

        for (int e = 0; e < 1000; e++)
        {

            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine(@"INSERT INTO GuidTableFast (id, Description)
                    VALUES ");

            for (int i = 0; i < 1000; i++)
            {
                stringBuilder.Append($"('{NewId.NextSequentialGuid()}', 'Test Desc {e + i}')");

                if (i < 999) stringBuilder.Append(", ");
            }
            stringBuilder.Append(';');

            await connection.ExecuteAsync(stringBuilder.ToString());

            stringBuilder.Clear();
        }

    }

    private async Task SeedSlowTable(SqlConnection connection)
    {


        for (int e = 0; e < 1000; e++)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine(@"INSERT INTO GuidTableSlow (id, Description)
                    VALUES ");

            for (int i = 0; i < 1000; i++)
            {
                stringBuilder.Append($"('{System.Guid.NewGuid()}', 'Test Desc {e+i}')");

                if (i < 999) stringBuilder.Append(", ");
            }
            stringBuilder.Append(';');

            await connection.ExecuteAsync(stringBuilder.ToString());
            stringBuilder.Clear();

        }
    }


    private async Task SeedIdentityTable(SqlConnection connection)
    {


        for (int e = 0; e < 1000; e++)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine(@"INSERT INTO [IdentityTable] (Description)
                    VALUES ");

            for (int i = 0; i < 1000; i++)
            {
                stringBuilder.Append($"('Test Desc {e + i}')");

                if (i < 999) stringBuilder.Append(", ");
            }
            stringBuilder.Append(';');

            await connection.ExecuteAsync(stringBuilder.ToString());
            stringBuilder.Clear();

        }
    }

    [Benchmark]
    public int CountSlow()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");
        var rows = connection.Query("SELECT * FROM GuidTableSlow").ToList();
        return rows.Count();
    }

    [Benchmark]
    public int CountFast()
    {
        using var connection = new SqlConnection("Server=127.0.0.1,1433;Database=TestDb;User Id=sa;Password=Password12345;TrustServerCertificate=True");

        var rows = connection.Query("SELECT * FROM GuidTableFast").ToList();
        return rows.Count();
    }
}

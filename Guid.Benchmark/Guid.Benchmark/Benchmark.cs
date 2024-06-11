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

        var bInsert = new BulkInsertBenchmark();

        await bInsert.DapperBulkInsert();

        await bInsert.BulkCopyInsert();

        await bInsert.BulkInsertTable();


    }


}

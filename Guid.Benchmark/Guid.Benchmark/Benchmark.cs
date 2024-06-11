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

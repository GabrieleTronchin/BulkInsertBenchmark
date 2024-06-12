# Dapper vs BulkCopy vs Table-Value Parameters
Bulk inserts are essential for efficiently inserting large volumes of data into a database. In C#, there are several methods to achieve this, each with its own advantages.

This project focuses on three popular methods:

- Dapper's native support for bulk insert
- Bulk Copy
- Table-Value Parameters (TVPs)

Here are the results of my benchmark:

[image](./assets/Banchmark.png)


- **Dapper Bulk Insert**: Simple and suitable for small to medium-sized datasets. Easy to use but may not be optimal for large volumes of data.

- **Bulk Copy**: Highly efficient for large datasets. Best for high-performance bulk inserts with minimal overhead.

- **Table-Value Parameters**: Combines flexibility and efficiency. Suitable for large datasets and complex operations.
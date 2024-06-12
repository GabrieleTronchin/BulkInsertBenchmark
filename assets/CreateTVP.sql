/* Create a table type. */
CREATE TYPE SampleTableType 
   AS TABLE
      ( [id] UNIQUEIDENTIFIER
      , [Description] NVARCHAR(50) );
GO
/* Create a procedure to receive data for the table-valued parameter. */
CREATE PROCEDURE dbo. usp_SampleTableInsert
   @TVP SampleTableType READONLY
      AS
      SET NOCOUNT ON
      INSERT INTO [DapperTableInsert]
      SELECT *
      FROM @TVP;
GO

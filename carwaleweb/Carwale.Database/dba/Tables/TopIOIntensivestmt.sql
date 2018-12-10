CREATE TABLE [dba].[TopIOIntensivestmt] (
    [Average IO]       BIGINT         NULL,
    [Total IO]         BIGINT         NULL,
    [Execution count]  BIGINT         NOT NULL,
    [Individual Query] NVARCHAR (MAX) NULL,
    [Parent Query]     NVARCHAR (MAX) NULL,
    [DatabaseName]     NVARCHAR (128) NULL,
    [updateon]         DATETIME       NOT NULL,
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL
);


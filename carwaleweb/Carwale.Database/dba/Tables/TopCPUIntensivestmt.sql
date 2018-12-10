CREATE TABLE [dba].[TopCPUIntensivestmt] (
    [Average CPU used] BIGINT         NULL,
    [Total CPU used]   BIGINT         NOT NULL,
    [Execution count]  BIGINT         NOT NULL,
    [Individual Query] NVARCHAR (MAX) NULL,
    [Parent Query]     NVARCHAR (MAX) NULL,
    [DatabaseName]     NVARCHAR (128) NULL,
    [updatedon]        DATETIME       NOT NULL,
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL
);


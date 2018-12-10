CREATE TABLE [dba].[BlockingQueries] (
    [Average Time Blocked] BIGINT         NULL,
    [Total Time Blocked]   BIGINT         NULL,
    [Execution count]      BIGINT         NOT NULL,
    [Individual Query]     NVARCHAR (MAX) NULL,
    [Parent Query]         NVARCHAR (MAX) NULL,
    [DatabaseName]         NVARCHAR (128) NULL,
    [UpdatedOn]            DATETIME       NULL,
    [ID]                   BIGINT         IDENTITY (1, 1) NOT NULL
);


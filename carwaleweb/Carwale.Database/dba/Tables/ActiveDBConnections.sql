CREATE TABLE [dba].[ActiveDBConnections] (
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [DBName]              NVARCHAR (128) NULL,
    [NumberOfConnections] INT            NULL,
    [UpdatedOn]           DATETIME       NOT NULL
);


CREATE TABLE [dba].[dbfilestats] (
    [File Name]             [sysname]       NOT NULL,
    [Physical Name]         NVARCHAR (260)  NOT NULL,
    [Total Size in MB]      INT             NULL,
    [Available Space In MB] NUMERIC (18, 6) NULL,
    [UpdatedOn]             DATETIME        NULL,
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL
);


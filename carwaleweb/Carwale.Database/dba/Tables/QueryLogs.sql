CREATE TABLE [dba].[QueryLogs] (
    [Spid]                SMALLINT       NOT NULL,
    [request_id]          INT            NOT NULL,
    [command]             NVARCHAR (16)  NOT NULL,
    [Database]            NVARCHAR (128) NULL,
    [User]                NVARCHAR (128) NOT NULL,
    [blocking_session_id] SMALLINT       NULL,
    [Status]              NVARCHAR (30)  NOT NULL,
    [Wait]                NVARCHAR (60)  NULL,
    [sql_statement]       XML            NULL,
    [Parent Query]        NVARCHAR (MAX) NULL,
    [query_plan]          XML            NULL,
    [cpu_time]            INT            NOT NULL,
    [reads]               BIGINT         NOT NULL,
    [writes]              BIGINT         NOT NULL,
    [Logical_reads]       BIGINT         NOT NULL,
    [row_count]           BIGINT         NOT NULL,
    [Program]             NVARCHAR (128) NULL,
    [Host_name]           NVARCHAR (128) NULL,
    [start_time]          DATETIME       NOT NULL,
    [CreateOn]            DATETIME       NULL,
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL
);


CREATE TABLE [dbo].[DBChangesLogs] (
    [DBChangesLogsId] INT           IDENTITY (1, 1) NOT NULL,
    [EventDate]       DATETIME      NULL,
    [DBName]          VARCHAR (100) NULL,
    [SchemaName]      VARCHAR (100) NULL,
    [ObjectType]      VARCHAR (100) NULL,
    [EventType]       VARCHAR (100) NULL,
    [ObjectName]      VARCHAR (100) NULL,
    [SQLScript]       VARCHAR (MAX) NULL,
    [HostName]        VARCHAR (100) NULL,
    [IPAddress]       VARCHAR (30)  NULL,
    [LoginName]       VARCHAR (100) NULL,
    [ProgramName]     VARCHAR (250) NULL,
    CONSTRAINT [PK_DBChangesLogsId] PRIMARY KEY CLUSTERED ([DBChangesLogsId] ASC)
);


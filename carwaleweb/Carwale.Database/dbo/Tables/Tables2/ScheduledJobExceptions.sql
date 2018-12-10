CREATE TABLE [dbo].[ScheduledJobExceptions] (
    [ScheduledJobExceptionsId] INT           IDENTITY (1, 1) NOT NULL,
    [JobName]                  VARCHAR (50)  NULL,
    [SPName]                   VARCHAR (50)  NULL,
    [ErrorMsg]                 VARCHAR (MAX) NULL,
    [TableName]                VARCHAR (50)  NULL,
    [FailedId]                 INT           NULL,
    [CreatedOn]                DATETIME      NULL
);


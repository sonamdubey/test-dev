CREATE TABLE [dbo].[CarWaleMyslSyncExceptions] (
    [CarWaleWebsiteExceptionsId] INT           IDENTITY (1, 1) NOT NULL,
    [ModuleName]                 VARCHAR (50)  NULL,
    [SPName]                     VARCHAR (50)  NULL,
    [ErrorMsg]                   VARCHAR (MAX) NULL,
    [TableName]                  VARCHAR (50)  NULL,
    [FailedId]                   VARCHAR (MAX) NULL,
    [CreatedOn]                  DATETIME      NULL,
    [InputParameter]             VARCHAR (200) NULL
);


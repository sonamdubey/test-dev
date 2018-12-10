CREATE TABLE [dbo].[CarWaleMasterDataLogs] (
    [TableName]  VARCHAR (100) NULL,
    [AffectedId] INT           NULL,
    [Remarks]    VARCHAR (100) NULL,
    [ColumnName] VARCHAR (100) NULL,
    [OldValue]   VARCHAR (150) NULL,
    [NewValue]   VARCHAR (150) NULL,
    [CreatedOn]  DATETIME      NULL
);


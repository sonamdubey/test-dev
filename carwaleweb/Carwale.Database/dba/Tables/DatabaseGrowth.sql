CREATE TABLE [dba].[DatabaseGrowth] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [TableName] VARCHAR (256) NULL,
    [Rows]      INT           NULL,
    [Reserved]  VARCHAR (256) NULL,
    [Data]      VARCHAR (256) NULL,
    [IndexSize] VARCHAR (256) NULL,
    [UnUsed]    VARCHAR (256) NULL,
    [CreatedOn] DATETIME      NULL
);


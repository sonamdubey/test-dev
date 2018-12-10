CREATE TABLE [dbo].[TC_CustomerAlias] (
    [TC_CustomerAliasId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]           BIGINT        NULL,
    [UserId]             BIGINT        NULL,
    [CustomerName]       VARCHAR (100) NULL,
    [Email]              VARCHAR (100) NULL,
    [Mobile]             VARCHAR (15)  NULL,
    [AlternateNo]        VARCHAR (50)  NULL,
    [EntryDate]          DATETIME      NULL,
    [ModifiedDate]       DATETIME      NULL,
    [ModifiedBy]         BIGINT        NULL,
    [TC_CustomerId]      BIGINT        NULL,
    [IsMerged]           BIT           NULL,
    CONSTRAINT [PK_TC_CustomerAlias] PRIMARY KEY CLUSTERED ([TC_CustomerAliasId] ASC)
);


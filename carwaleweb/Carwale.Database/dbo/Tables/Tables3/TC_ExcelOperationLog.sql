CREATE TABLE [dbo].[TC_ExcelOperationLog] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [TC_UserId]      BIGINT       NULL,
    [BranchId]       BIGINT       NULL,
    [TC_ReqCategory] VARCHAR (20) NULL,
    [CreatedOn]      DATETIME     NULL,
    [Status]         BIT          CONSTRAINT [DF_TC_ExcelOperationLog_Status] DEFAULT ((0)) NULL,
    [ModifiedDate]   DATETIME     NULL,
    CONSTRAINT [PK_TC_ExcelOperationLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


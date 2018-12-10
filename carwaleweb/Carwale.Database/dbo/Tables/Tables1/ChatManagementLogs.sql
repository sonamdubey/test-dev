CREATE TABLE [dbo].[ChatManagementLogs] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [PageId]    INT           NOT NULL,
    [PageName]  VARCHAR (500) NOT NULL,
    [IsChatOn]  BIT           NOT NULL,
    [IsActive]  BIT           NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    [UpdatedBy] INT           NOT NULL,
    [Remarks]   VARCHAR (500) NULL,
    CONSTRAINT [PK_ChatManagementLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


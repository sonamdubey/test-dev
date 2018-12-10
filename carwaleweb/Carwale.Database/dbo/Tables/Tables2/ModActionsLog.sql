CREATE TABLE [dbo].[ModActionsLog] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT            NULL,
    [ThreadId]   NVARCHAR (MAX) NULL,
    [ForumId]    NVARCHAR (MAX) NULL,
    [ActionDate] DATETIME       NULL,
    [ActionType] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ModActionsLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


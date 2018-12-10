CREATE TABLE [dbo].[TC_QuickBloxUsers] (
    [Id]                INT          NOT NULL,
    [DealerId]          INT          NOT NULL,
    [TC_UserId]         INT          NOT NULL,
    [QuickBloxId]       VARCHAR (50) NOT NULL,
    [QuickBloxUniqueId] INT          NULL,
    CONSTRAINT [PK_TC_QuickBloxUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


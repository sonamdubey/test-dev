CREATE TABLE [dbo].[BlockedKeywords] (
    [Id]      INT          IDENTITY (1, 1) NOT NULL,
    [Keyword] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_BlockedKeywords] PRIMARY KEY CLUSTERED ([Id] ASC)
);


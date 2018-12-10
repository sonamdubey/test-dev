CREATE TABLE [dbo].[BlockedCwCookies] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Cookie] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BlockedCwCookies] PRIMARY KEY CLUSTERED ([Id] ASC)
);


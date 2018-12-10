CREATE TABLE [dbo].[IMG_Status] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [StatusId]     INT           NULL,
    [StatMessage]  VARCHAR (100) NULL,
    [BWMigratedId] INT           NULL,
    CONSTRAINT [PK_IMG_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);


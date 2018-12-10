CREATE TABLE [dbo].[MiscDetails] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DebitId]      NUMERIC (18) NOT NULL,
    [CustomerName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MiscDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_MiscDetails_CustomerDebits] FOREIGN KEY ([DebitId]) REFERENCES [dbo].[CustomerDebits] ([Id])
);


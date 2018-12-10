CREATE TABLE [dbo].[CustomerFavouritesUsed] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]    NUMERIC (18) NOT NULL,
    [CarProfileId]  VARCHAR (50) NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_CustomerFavouritesUsed_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CustomerFavouritesUsed] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerFavouritesUsed_CustomerId]
    ON [dbo].[CustomerFavouritesUsed]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerFavouritesUsed_CarProfileId]
    ON [dbo].[CustomerFavouritesUsed]([CarProfileId] ASC);


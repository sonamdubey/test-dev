CREATE TABLE [dbo].[CustomerFavourites] (
    [CustomerId]    NUMERIC (18) NOT NULL,
    [VersionId]     NUMERIC (18) NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_CustomerFavourites] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [VersionId] ASC) WITH (FILLFACTOR = 90)
);


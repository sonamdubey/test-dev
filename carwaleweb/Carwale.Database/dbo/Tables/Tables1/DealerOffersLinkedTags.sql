CREATE TABLE [dbo].[DealerOffersLinkedTags] (
    [OfferId] NUMERIC (18) NOT NULL,
    [TagId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DealerOffersLinkedTags] PRIMARY KEY CLUSTERED ([OfferId] ASC, [TagId] ASC) WITH (FILLFACTOR = 90)
);


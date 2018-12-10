CREATE TABLE [dbo].[DealerOffersMakeModel] (
    [OfferId] NUMERIC (18) NOT NULL,
    [MakeId]  NUMERIC (18) NOT NULL,
    [ModelId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DealerOffersMakeModel] PRIMARY KEY CLUSTERED ([OfferId] ASC, [MakeId] ASC, [ModelId] ASC) WITH (FILLFACTOR = 90)
);


CREATE TABLE [dbo].[Microsite_OfferModels] (
    [OfferId]  INT NOT NULL,
    [ModelId]  INT NOT NULL,
    [DealerId] INT NOT NULL,
    CONSTRAINT [fk_Microsite_OfferModels_Microsite_DealerOffers] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Microsite_DealerOffers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_OfferModels_DealerId]
    ON [dbo].[Microsite_OfferModels]([DealerId] ASC);


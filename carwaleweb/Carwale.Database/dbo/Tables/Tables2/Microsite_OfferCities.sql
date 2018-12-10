CREATE TABLE [dbo].[Microsite_OfferCities] (
    [OfferId]  INT NOT NULL,
    [CityId]   INT NOT NULL,
    [DealerId] INT NOT NULL,
    CONSTRAINT [fk_Microsite_OfferCities_Microsite_DealerOffers] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Microsite_DealerOffers] ([Id])
);


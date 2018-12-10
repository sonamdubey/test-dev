CREATE TABLE [dbo].[DealerOffersDealers] (
    [DealerOffersDealersId] INT          IDENTITY (1, 1) NOT NULL,
    [OfferId]               NUMERIC (18) NOT NULL,
    [DealerId]              NUMERIC (18) NOT NULL,
    [CityId]                NUMERIC (18) NULL,
    [ZoneId]                NUMERIC (18) NULL,
    [Dealer_NewCarID]       INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffersDealers_OfferId]
    ON [dbo].[DealerOffersDealers]([OfferId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffersDealers_DealerId]
    ON [dbo].[DealerOffersDealers]([DealerId] ASC);


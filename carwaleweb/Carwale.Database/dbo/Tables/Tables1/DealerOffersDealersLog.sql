CREATE TABLE [dbo].[DealerOffersDealersLog] (
    [OfferId]   NUMERIC (18) NOT NULL,
    [DealerId]  NUMERIC (18) NULL,
    [CityId]    NUMERIC (18) NULL,
    [ZoneId]    NUMERIC (18) NULL,
    [EntryDate] DATETIME     NULL,
    [EnteredBy] NUMERIC (18) NULL
);


CREATE TABLE [dbo].[BW_PQ_Offers_22022016] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT           NULL,
    [CityId]          INT           NULL,
    [ModelId]         INT           NULL,
    [OfferCategoryId] INT           NULL,
    [OfferText]       VARCHAR (500) NULL,
    [OfferValue]      INT           NULL,
    [EntryDate]       DATETIME      NULL,
    [OfferValidTill]  DATETIME      NULL,
    [IsActive]        BIT           NULL,
    [LastUpdated]     DATETIME      NULL,
    [UpdatedBy]       INT           NULL,
    [OfferId]         SMALLINT      NULL,
    [IsPriceImpact]   BIT           NOT NULL
);


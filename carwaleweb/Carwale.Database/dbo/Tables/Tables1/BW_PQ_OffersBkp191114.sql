CREATE TABLE [dbo].[BW_PQ_OffersBkp191114] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT           NULL,
    [CityId]          INT           NULL,
    [ModelId]         INT           NULL,
    [OfferCategoryId] INT           NULL,
    [OfferText]       VARCHAR (500) NULL,
    [OfferValue]      INT           NULL,
    [EntryDate]       DATETIME      NULL,
    [OfferValidTill]  DATETIME      NULL,
    [IsActive]        BIT           NULL
);


CREATE TYPE [dbo].[TC_Deals_CarStockPricesDetail] AS TABLE (
    [CityId]           INT NULL,
    [OnRoadPrice]      INT NULL,
    [OfferPrice]       INT NULL,
    [OfferValue]       INT NULL,
    [Insurance]        INT DEFAULT ((0)) NULL,
    [ExtraSavings]     INT DEFAULT ((0)) NULL,
    [ShowExtraSavings] INT DEFAULT ((0)) NULL);


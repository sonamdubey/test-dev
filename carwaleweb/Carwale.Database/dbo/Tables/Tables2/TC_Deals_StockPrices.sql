CREATE TABLE [dbo].[TC_Deals_StockPrices] (
    [TC_Deals_StockId]  INT      NOT NULL,
    [CityId]            INT      NOT NULL,
    [DiscountedPrice]   INT      NOT NULL,
    [ActualOnroadPrice] INT      NOT NULL,
    [EnteredOn]         DATETIME NOT NULL,
    [EnteredBy]         INT      NOT NULL,
    [PriceBreakupId]    INT      NULL,
    [Offer_Value]       INT      NULL,
    CONSTRAINT [PK_TC_Deals_StockPrices] PRIMARY KEY CLUSTERED ([TC_Deals_StockId] ASC, [CityId] ASC)
);


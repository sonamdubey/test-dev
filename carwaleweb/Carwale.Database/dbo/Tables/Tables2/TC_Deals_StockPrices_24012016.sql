CREATE TABLE [dbo].[TC_Deals_StockPrices_24012016] (
    [TC_Deals_StockId]  INT      NOT NULL,
    [CityId]            INT      NOT NULL,
    [DiscountedPrice]   INT      NOT NULL,
    [ActualOnroadPrice] INT      NOT NULL,
    [EnteredOn]         DATETIME NOT NULL,
    [EnteredBy]         INT      NOT NULL
);


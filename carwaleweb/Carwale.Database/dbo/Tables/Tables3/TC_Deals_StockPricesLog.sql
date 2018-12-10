CREATE TABLE [dbo].[TC_Deals_StockPricesLog] (
    [TC_Deals_StockId]  INT      NOT NULL,
    [CityId]            INT      NOT NULL,
    [DiscountedPrice]   INT      NOT NULL,
    [ActualOnroadPrice] INT      NOT NULL,
    [LoggedOn]          DATETIME NOT NULL,
    [LoggedBy]          INT      NOT NULL
);


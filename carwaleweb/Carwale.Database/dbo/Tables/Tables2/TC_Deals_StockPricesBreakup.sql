CREATE TABLE [dbo].[TC_Deals_StockPricesBreakup] (
    [TC_Deals_StockPricesBreakupId] INT      IDENTITY (1, 1) NOT NULL,
    [StockId]                       INT      NULL,
    [CityId]                        INT      NOT NULL,
    [TC_PQComponentId]              INT      NOT NULL,
    [ComponentPrice]                INT      NOT NULL,
    [CreatedOn]                     DATETIME CONSTRAINT [DF_TC_Deals_StockPricesBreakup_EnteredOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                     INT      NOT NULL
);


CREATE TABLE [dbo].[TC_Deals_StockPricesBreakupLog] (
    [Id]                            INT      IDENTITY (1, 1) NOT NULL,
    [TC_Deals_StockPricesBreakupId] INT      NULL,
    [StockId]                       INT      NULL,
    [CityId]                        INT      NULL,
    [TC_PQComponentId]              INT      NULL,
    [ComponentPrice]                INT      NULL,
    [CreatedOn]                     DATETIME NULL,
    [CreatedBy]                     INT      NULL,
    [LogCreatedOn]                  DATETIME CONSTRAINT [DF_TC_Deals_StockPricesBreakupLog_LogCreatedOn] DEFAULT (getdate()) NULL
);


CREATE TABLE [dbo].[TC_Deals_StockVIN_17022016] (
    [TC_DealsStockVINId] INT          IDENTITY (1, 1) NOT NULL,
    [TC_Deals_StockId]   INT          NOT NULL,
    [VINNo]              VARCHAR (20) NOT NULL,
    [Status]             TINYINT      NOT NULL,
    [LastRefreshedOn]    DATETIME     NOT NULL,
    [LastRefreshedBy]    INT          NOT NULL,
    [EnteredOn]          DATETIME     NOT NULL,
    [EnteredBy]          INT          NOT NULL
);


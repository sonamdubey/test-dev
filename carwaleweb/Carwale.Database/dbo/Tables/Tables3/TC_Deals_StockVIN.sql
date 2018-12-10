CREATE TABLE [dbo].[TC_Deals_StockVIN] (
    [TC_DealsStockVINId] INT          IDENTITY (1, 1) NOT NULL,
    [TC_Deals_StockId]   INT          NOT NULL,
    [VINNo]              VARCHAR (20) NOT NULL,
    [Status]             TINYINT      NOT NULL,
    [LastRefreshedOn]    DATETIME     NOT NULL,
    [LastRefreshedBy]    INT          NOT NULL,
    [EnteredOn]          DATETIME     NOT NULL,
    [EnteredBy]          INT          NOT NULL,
    CONSTRAINT [PK_TC_Deals_StockVIN] PRIMARY KEY CLUSTERED ([TC_DealsStockVINId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_StockVIN_TC_Deals_StockId]
    ON [dbo].[TC_Deals_StockVIN]([TC_Deals_StockId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_StockVIN_Status]
    ON [dbo].[TC_Deals_StockVIN]([Status] ASC);


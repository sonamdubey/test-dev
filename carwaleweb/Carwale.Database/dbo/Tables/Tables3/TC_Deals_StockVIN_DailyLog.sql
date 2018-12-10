CREATE TABLE [dbo].[TC_Deals_StockVIN_DailyLog] (
    [AsOnDate]           DATE         NULL,
    [TC_DealsStockVINId] INT          NULL,
    [TC_Deals_StockId]   INT          NULL,
    [VINNo]              VARCHAR (20) NULL,
    [Status]             TINYINT      NULL,
    [LastRefreshedOn]    DATETIME     NULL,
    [LastRefreshedBy]    INT          NULL,
    [EnteredOn]          DATETIME     NULL,
    [EnteredBy]          INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_StockVIN_DailyLog_AsOnDate]
    ON [dbo].[TC_Deals_StockVIN_DailyLog]([AsOnDate] ASC);


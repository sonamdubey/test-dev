CREATE TABLE [dbo].[DCRM_AP_RemovedDealerStocks] (
    [TC_StockId] NUMERIC (18) NOT NULL,
    [EntryDate]  DATETIME     CONSTRAINT [DF_DCRM_AP_RemovedDealerStocks_EntryDate] DEFAULT (getdate()) NOT NULL
);


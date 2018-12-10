CREATE TABLE [dbo].[DealerStockStatus] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MonthName]     DATETIME     NULL,
    [StartDate]     DATETIME     NULL,
    [EndDate]       DATETIME     NULL,
    [DealersAdded]  NUMERIC (9)  NULL,
    [StockUpdated]  NUMERIC (9)  NULL,
    [ActiveDealers] NUMERIC (9)  NULL,
    [LostDealers]   NUMERIC (9)  NULL,
    CONSTRAINT [PK_DealerStockStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


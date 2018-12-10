CREATE TABLE [dbo].[TC_StockSearch] (
    [TC_StockSearchId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [DealerId]         BIGINT   NOT NULL,
    [TC_StockId]       BIGINT   NOT NULL,
    [EntryDate]        DATETIME NOT NULL,
    CONSTRAINT [PK_TC_StockSearch] PRIMARY KEY CLUSTERED ([TC_StockSearchId] ASC)
);


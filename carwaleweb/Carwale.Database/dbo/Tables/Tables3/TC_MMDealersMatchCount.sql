CREATE TABLE [dbo].[TC_MMDealersMatchCount] (
    [DealerId]       INT      NULL,
    [StockId]        INT      NULL,
    [MatchViewCount] INT      NULL,
    [CreatedOn]      DATETIME NULL,
    [LastUpdatedOn]  DATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_MMDealersMatchCount_DealerId]
    ON [dbo].[TC_MMDealersMatchCount]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_MMDealersMatchCount_StockId]
    ON [dbo].[TC_MMDealersMatchCount]([StockId] ASC);


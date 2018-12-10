CREATE TABLE [dbo].[TC_Deals_StockPriceFlagLog] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [StockId]      INT      NOT NULL,
    [ModifiedDate] DATETIME NOT NULL,
    [ModifiedBy]   INT      NULL,
    [FlagStatus]   BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


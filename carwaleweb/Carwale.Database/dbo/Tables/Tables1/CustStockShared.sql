CREATE TABLE [dbo].[CustStockShared] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [InquiryId]       NUMERIC (18) NOT NULL,
    [CarTradeStockId] INT          NOT NULL,
    [IsLive]          BIT          NOT NULL,
    [LastUpdated]     DATETIME     NOT NULL,
    CONSTRAINT [PK_CustStockShared] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_CustStockShared] UNIQUE NONCLUSTERED ([InquiryId] ASC)
);


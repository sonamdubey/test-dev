CREATE TABLE [dbo].[TC_StockAnalysis] (
    [StockId]         NUMERIC (18) NOT NULL,
    [CWResponseCount] SMALLINT     CONSTRAINT [DF_TC_StockAnalysis_CWResponseCount] DEFAULT ((0)) NULL,
    [TCResponseCount] SMALLINT     CONSTRAINT [DF_TC_StockAnalysis_TCResponseCount] DEFAULT ((0)) NULL,
    CONSTRAINT [CK_TC_StockAnalysis] UNIQUE NONCLUSTERED ([StockId] ASC)
);


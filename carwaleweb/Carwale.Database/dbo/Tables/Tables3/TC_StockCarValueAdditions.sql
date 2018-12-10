CREATE TABLE [dbo].[TC_StockCarValueAdditions] (
    [TC_CarValueAdditionsId] SMALLINT NOT NULL,
    [TC_StockId]             BIGINT   NOT NULL,
    [IsActive]               BIT      CONSTRAINT [DF_TC_StockCarValueAdditions_IsActive] DEFAULT ((1)) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_StockCarValueAdditions]
    ON [dbo].[TC_StockCarValueAdditions]([IsActive] ASC)
    INCLUDE([TC_CarValueAdditionsId], [TC_StockId]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_StockCarValueAdditions_TC_CarValueAdditionsId]
    ON [dbo].[TC_StockCarValueAdditions]([TC_CarValueAdditionsId] ASC, [TC_StockId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_StockCarValueAdditions_TC_StockId]
    ON [dbo].[TC_StockCarValueAdditions]([TC_StockId] ASC, [IsActive] ASC)
    INCLUDE([TC_CarValueAdditionsId]);


CREATE TABLE [dbo].[TC_StockUploadedLog] (
    [SellInquiriesId] INT      NULL,
    [DealerId]        INT      NULL,
    [IsCarUploaded]   BIT      NULL,
    [CreatedOn]       DATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [TC_StockUploadedLog_SellInquiriesId]
    ON [dbo].[TC_StockUploadedLog]([SellInquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_StockUploadedLog_DealerId]
    ON [dbo].[TC_StockUploadedLog]([DealerId] ASC);


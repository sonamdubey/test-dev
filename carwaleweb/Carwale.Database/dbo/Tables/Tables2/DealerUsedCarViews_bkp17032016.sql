CREATE TABLE [dbo].[DealerUsedCarViews_bkp17032016] (
    [InquiryID]   BIGINT   NULL,
    [Sellertype]  BIT      NULL,
    [Viewcount]   INT      NULL,
    [LastUpdated] DATETIME DEFAULT (getdate()) NULL,
    [Impression]  INT      NULL
);


GO
CREATE CLUSTERED INDEX [IX_UsedCarViewCount]
    ON [dbo].[DealerUsedCarViews_bkp17032016]([InquiryID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerUsedCarViews_InquiryId]
    ON [dbo].[DealerUsedCarViews_bkp17032016]([InquiryID] ASC)
    INCLUDE([Viewcount]);


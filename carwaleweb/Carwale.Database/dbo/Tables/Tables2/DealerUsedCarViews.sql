CREATE TABLE [dbo].[DealerUsedCarViews] (
    [InquiryID]   BIGINT   NULL,
    [Sellertype]  BIT      NULL,
    [Viewcount]   INT      NULL,
    [LastUpdated] DATETIME DEFAULT (getdate()) NULL,
    [Impression]  INT      NULL
);


GO
CREATE CLUSTERED INDEX [IX_UsedCarViewCount]
    ON [dbo].[DealerUsedCarViews]([InquiryID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerUsedCarViews_InquiryId]
    ON [dbo].[DealerUsedCarViews]([InquiryID] ASC)
    INCLUDE([Viewcount]);


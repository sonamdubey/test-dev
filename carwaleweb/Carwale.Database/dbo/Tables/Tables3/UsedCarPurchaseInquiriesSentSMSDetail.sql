CREATE TABLE [dbo].[UsedCarPurchaseInquiriesSentSMSDetail] (
    [ID]            INT      IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT      NULL,
    [SellInquiryId] INT      NULL,
    [SMSSentDate]   DATETIME NULL,
    CONSTRAINT [PK_UsdCarPurInqStSMS] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UsedCarPurchaseInquiriesSentSMSDetail_Id]
    ON [dbo].[UsedCarPurchaseInquiriesSentSMSDetail]([CustomerID] ASC, [SellInquiryId] ASC);


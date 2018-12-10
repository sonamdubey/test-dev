CREATE TABLE [dbo].[ClassifiedRequestsSentSMSDetail] (
    [ID]            INT      IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT      NULL,
    [SellInquiryId] INT      NULL,
    [SMSSentDate]   DATETIME NULL,
    CONSTRAINT [PK_ClassSentSMSD] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ClassifiedRequestsSentSMSDetail_Id]
    ON [dbo].[ClassifiedRequestsSentSMSDetail]([CustomerID] ASC, [SellInquiryId] ASC);


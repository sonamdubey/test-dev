CREATE TABLE [dbo].[ForwardedNewCarPurchaseInquiries] (
    [FNP_Id]                  NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NewCarPurchaseInquiryId] NUMERIC (18) NOT NULL,
    [DealerId]                NUMERIC (18) NOT NULL,
    [ForwardDateTime]         DATETIME     NOT NULL,
    CONSTRAINT [PK_ForwardedNewCarPurchaseInquiries] PRIMARY KEY CLUSTERED ([FNP_Id] ASC) WITH (FILLFACTOR = 90)
);


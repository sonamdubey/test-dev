CREATE TABLE [dbo].[ForwardedCustomerSellInquiries] (
    [FCS_Id]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerSellInquiryId] NUMERIC (18) NOT NULL,
    [DealerId]              NUMERIC (18) NOT NULL,
    [ForwardDateTime]       DATETIME     NOT NULL,
    CONSTRAINT [PK_ForwardedCustomerSellInquiries] PRIMARY KEY CLUSTERED ([FCS_Id] ASC) WITH (FILLFACTOR = 90)
);


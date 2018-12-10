CREATE TABLE [dbo].[TC_MMCustomerDetails] (
    [DealerId]             INT          NULL,
    [CWCustomersId]        NUMERIC (18) NULL,
    [MatchedStockId]       NUMERIC (18) NULL,
    [CWInquiryId]          NUMERIC (18) NULL,
    [SellerType]           TINYINT      NULL,
    [CustomerResponseDate] DATETIME     NULL,
    [IsPurchased]          BIT          NULL,
    [CreatedOn]            DATETIME     NULL,
    [CWSellInquiryId]      NUMERIC (18) NULL,
    [IsDeleted]            BIT          CONSTRAINT [DF_TC_MMCustomerDetails_IsDeleted] DEFAULT ((0)) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_MMCustomerDetails_DealerId]
    ON [dbo].[TC_MMCustomerDetails]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_MMCustomerDetails_MatchedStockId]
    ON [dbo].[TC_MMCustomerDetails]([MatchedStockId] ASC);


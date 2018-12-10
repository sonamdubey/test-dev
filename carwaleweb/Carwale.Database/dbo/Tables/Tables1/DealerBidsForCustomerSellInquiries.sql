CREATE TABLE [dbo].[DealerBidsForCustomerSellInquiries] (
    [DealerId]      NUMERIC (18) NOT NULL,
    [SellInquiryId] NUMERIC (18) NOT NULL,
    [BidAmount]     DECIMAL (18) CONSTRAINT [DF_DealerBiddingsForCustomerSellInquiries_BidAmount] DEFAULT (0) NOT NULL,
    [LastUpdated]   DATETIME     NOT NULL,
    CONSTRAINT [PK_DeaBidCustSellInq] PRIMARY KEY CLUSTERED ([DealerId] ASC, [SellInquiryId] ASC)
);


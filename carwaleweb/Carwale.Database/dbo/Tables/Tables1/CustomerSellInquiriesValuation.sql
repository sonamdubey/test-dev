CREATE TABLE [dbo].[CustomerSellInquiriesValuation] (
    [SellInquiryId] NUMERIC (18) NOT NULL,
    [UsedCarValue]  VARCHAR (50) NOT NULL,
    [CreateDate]    DATETIME     NULL,
    CONSTRAINT [PK_CustellInqVal] PRIMARY KEY CLUSTERED ([SellInquiryId] ASC)
);


CREATE TABLE [dbo].[SellInquiriesAccessories] (
    [ID]               DECIMAL (18) NOT NULL,
    [SellInquiryId]    DECIMAL (18) NOT NULL,
    [CarAccessoriesId] DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_SellInquiriesAccessories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


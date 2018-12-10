CREATE TABLE [dbo].[TC_MissedUsedCarInquiries] (
    [id]                       INT      IDENTITY (1, 1) NOT NULL,
    [UsedCarPurchaseInquiryId] BIGINT   NULL,
    [SellInquiryId]            BIGINT   NULL,
    [CustomerId]               BIGINT   NULL,
    [RequestDate]              DATETIME NULL,
    [CreatedOn]                DATETIME DEFAULT (getdate()) NULL
);


CREATE TABLE [dbo].[TC_BookingOffersLog] (
    [TC_BookingOffersLogId] INT          IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiryId]    INT          NULL,
    [CouponCode]            VARCHAR (50) NULL,
    [CWOfferId]             INT          NULL,
    [EntryDate]             DATETIME     DEFAULT (getdate()) NOT NULL,
    [UserId]                INT          NULL,
    CONSTRAINT [PK_TC_BookingOffersLog] PRIMARY KEY CLUSTERED ([TC_BookingOffersLogId] ASC)
);


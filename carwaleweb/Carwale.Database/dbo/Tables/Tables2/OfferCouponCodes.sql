CREATE TABLE [dbo].[OfferCouponCodes] (
    [CouponId]    NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [OfferId]     NUMERIC (18) NOT NULL,
    [ReferenceId] VARCHAR (10) NOT NULL,
    [CouponCode]  VARCHAR (6)  NOT NULL,
    [OfferType]   INT          NULL,
    [GeneratedOn] DATETIME     NOT NULL
);


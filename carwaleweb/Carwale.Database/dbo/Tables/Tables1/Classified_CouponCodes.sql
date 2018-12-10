CREATE TABLE [dbo].[Classified_CouponCodes] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CouponCode]     VARCHAR (50) NOT NULL,
    [ValidUpto]      DATE         NOT NULL,
    [OfferStartDate] DATE         NOT NULL,
    [RedeemedOn]     DATETIME     NULL,
    [DiscountAmount] NUMERIC (18) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_Classified_CouponCodes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Classified_CouponCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


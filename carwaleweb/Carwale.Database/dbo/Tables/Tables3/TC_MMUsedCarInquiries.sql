CREATE TABLE [dbo].[TC_MMUsedCarInquiries] (
    [CustomerId]           INT          NULL,
    [CustomerMobile]       VARCHAR (15) NULL,
    [CustomerEmail]        VARCHAR (60) NULL,
    [CustomerResponseDate] DATETIME     NULL,
    [InquiryId]            INT          NULL,
    [SellerType]           TINYINT      NULL,
    [CarModelId]           INT          NULL,
    [CarVersionId]         INT          NULL,
    [FuelTypeId]           TINYINT      NULL,
    [Price]                INT          NULL,
    [Kms]                  INT          NULL,
    [MakeYear]             SMALLINT     NULL,
    [CityId]               INT          NULL,
    [DealerId]             INT          NULL,
    [IsUpdated]            BIT          CONSTRAINT [DF_TC_MMUsedCarInquiries_IsUpdated] DEFAULT ((0)) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is customers details updated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_MMUsedCarInquiries', @level2type = N'COLUMN', @level2name = N'IsUpdated';


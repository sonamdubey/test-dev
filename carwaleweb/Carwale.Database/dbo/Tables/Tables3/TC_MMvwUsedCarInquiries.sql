CREATE TABLE [dbo].[TC_MMvwUsedCarInquiries] (
    [CustomerId]           INT           NULL,
    [Name]                 VARCHAR (100) NULL,
    [CustomerMobile]       VARCHAR (20)  NULL,
    [CustomerEmail]        VARCHAR (100) NULL,
    [CustomerResponseDate] DATETIME      NULL,
    [InquiryId]            INT           NULL,
    [SellInquiryId]        INT           NULL,
    [SellerType]           TINYINT       NULL,
    [CarModelId]           INT           NULL,
    [CarVersionId]         INT           NULL,
    [FuelTypeId]           TINYINT       NULL,
    [Price]                INT           NULL,
    [Kms]                  INT           NULL,
    [MakeYear]             VARCHAR (8)   NULL,
    [CityId]               INT           NULL,
    [AreaName]             VARCHAR (50)  NULL,
    [DealerId]             INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [TC_MMvwUsedCarInquiries_CarModelId]
    ON [dbo].[TC_MMvwUsedCarInquiries]([CarModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [TC_MMvwUsedCarInquiries_CityId]
    ON [dbo].[TC_MMvwUsedCarInquiries]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_MMvwUsedCarInquiries_Mobile]
    ON [dbo].[TC_MMvwUsedCarInquiries]([CustomerMobile] ASC);


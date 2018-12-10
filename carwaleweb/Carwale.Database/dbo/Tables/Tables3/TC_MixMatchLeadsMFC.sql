CREATE TABLE [dbo].[TC_MixMatchLeadsMFC] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CWCustomersId]  INT           NULL,
    [CustomerName]   VARCHAR (50)  NULL,
    [CustomerMobile] VARCHAR (15)  NULL,
    [CustomerEmail]  VARCHAR (50)  NULL,
    [CarYear]        VARCHAR (10)  NULL,
    [Kms]            VARCHAR (10)  NULL,
    [Price]          VARCHAR (10)  NULL,
    [InquiryId]      INT           NULL,
    [SellerType]     SMALLINT      NULL,
    [CarName]        VARCHAR (100) NULL,
    [SellInquiryId]  INT           NULL,
    [FuelType]       VARCHAR (20)  NULL,
    [AreaNames]      VARCHAR (100) NULL,
    [ResponseDays]   VARCHAR (20)  NULL,
    [ResDate]        DATETIME      NULL,
    [MFCDealerId]    INT           NULL,
    [StockId]        INT           NULL
);


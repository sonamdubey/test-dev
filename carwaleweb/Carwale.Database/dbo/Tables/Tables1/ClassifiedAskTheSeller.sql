CREATE TABLE [dbo].[ClassifiedAskTheSeller] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (100) NULL,
    [Mobile]           VARCHAR (20)  NULL,
    [Comments]         VARCHAR (500) NULL,
    [InquiryId]        NUMERIC (18)  NULL,
    [SellerType]       VARCHAR (10)  NULL,
    [UtmaCookie]       VARCHAR (250) NULL,
    [UtmzCookie]       VARCHAR (500) NULL,
    [IsSentToCarTrade] BIT           CONSTRAINT [DF_ClassifiedAskTheSeller_IsSentToCarTrade] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


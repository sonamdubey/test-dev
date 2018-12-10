CREATE TABLE [dbo].[NewPurchaseCities01012016To30042016] (
    [NewPurchaseCities_Id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [InquiryId]            NUMERIC (18)  NOT NULL,
    [CityId]               NUMERIC (18)  NULL,
    [City]                 VARCHAR (100) NULL,
    [EmailId]              VARCHAR (100) NULL,
    [PhoneNo]              VARCHAR (100) NULL,
    [Name]                 VARCHAR (100) NULL,
    [InterestedInLoan]     BIT           CONSTRAINT [DF_NewPurchaseCitiesFrom_010012016_InterestedInLoan] DEFAULT ((0)) NULL,
    [MobileVerified]       BIT           CONSTRAINT [DF_NewPurchaseCitiesFrom_010012016_MobileVerified] DEFAULT ((0)) NULL,
    [ZoneId]               INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_NewPurchaseCitiesFrom_01012016_InquiryId]
    ON [dbo].[NewPurchaseCities01012016To30042016]([InquiryId] ASC);


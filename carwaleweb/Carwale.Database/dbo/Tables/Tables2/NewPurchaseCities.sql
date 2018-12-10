CREATE TABLE [dbo].[NewPurchaseCities] (
    [NewPurchaseCities_Id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [InquiryId]            NUMERIC (18)  NOT NULL,
    [CityId]               NUMERIC (18)  NULL,
    [City]                 VARCHAR (100) NULL,
    [EmailId]              VARCHAR (100) NULL,
    [PhoneNo]              VARCHAR (100) NULL,
    [Name]                 VARCHAR (100) NULL,
    [InterestedInLoan]     BIT           CONSTRAINT [DF_NewPurchaseCitiesFrom_070916_InterestedInLoan] DEFAULT ((0)) NULL,
    [MobileVerified]       BIT           CONSTRAINT [DF_NewPurchaseCitiesFrom_070916_MobileVerified] DEFAULT ((0)) NULL,
    [ZoneId]               INT           NULL
);


GO
CREATE CLUSTERED INDEX [IX_NewPurchaseCitiesFrom_070916_InquiryId]
    ON [dbo].[NewPurchaseCities]([InquiryId] ASC);


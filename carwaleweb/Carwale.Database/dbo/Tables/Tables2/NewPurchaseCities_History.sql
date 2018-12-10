CREATE TABLE [dbo].[NewPurchaseCities_History] (
    [InquiryId] NUMERIC (18)  NOT NULL,
    [CityId]    NUMERIC (18)  NULL,
    [City]      VARCHAR (100) NULL,
    [EmailId]   VARCHAR (100) NULL,
    [PhoneNo]   VARCHAR (100) NULL,
    [Name]      VARCHAR (100) NULL,
    CONSTRAINT [PK_NewPurchaseCities] PRIMARY KEY CLUSTERED ([InquiryId] ASC) WITH (FILLFACTOR = 90)
);


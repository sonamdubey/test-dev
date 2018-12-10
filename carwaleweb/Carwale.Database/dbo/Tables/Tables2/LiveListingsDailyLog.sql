CREATE TABLE [dbo].[LiveListingsDailyLog] (
    [AsOnDate]             DATE            NULL,
    [ProfileId]            VARCHAR (50)    NOT NULL,
    [SellerType]           SMALLINT        NULL,
    [Seller]               VARCHAR (50)    NULL,
    [Inquiryid]            NUMERIC (18)    NULL,
    [MakeId]               NUMERIC (18)    NULL,
    [MakeName]             VARCHAR (100)   NULL,
    [ModelId]              NUMERIC (18)    NULL,
    [ModelName]            VARCHAR (100)   NULL,
    [VersionId]            NUMERIC (18)    NULL,
    [VersionName]          VARCHAR (100)   NULL,
    [StateId]              NUMERIC (18)    NULL,
    [StateName]            VARCHAR (100)   NULL,
    [CityId]               NUMERIC (18)    NULL,
    [CityName]             VARCHAR (100)   NULL,
    [AreaId]               NUMERIC (18)    NULL,
    [AreaName]             VARCHAR (100)   NULL,
    [Lattitude]            DECIMAL (18, 4) NULL,
    [Longitude]            DECIMAL (18, 4) NULL,
    [MakeYear]             DATETIME        NULL,
    [Price]                NUMERIC (18)    NULL,
    [Kilometers]           NUMERIC (18)    NULL,
    [Color]                VARCHAR (100)   NULL,
    [Comments]             VARCHAR (500)   NULL,
    [EntryDate]            DATETIME        NULL,
    [LastUpdated]          DATETIME        NULL,
    [PackageType]          SMALLINT        NULL,
    [ShowDetails]          BIT             NULL,
    [Priority]             SMALLINT        NULL,
    [PhotoCount]           SMALLINT        NOT NULL,
    [FrontImagePath]       VARCHAR (500)   NULL,
    [CertificationId]      SMALLINT        NULL,
    [AdditionalFuel]       VARCHAR (50)    NULL,
    [IsReplicated]         BIT             NULL,
    [HostURL]              VARCHAR (100)   NULL,
    [CalculatedEMI]        INT             NULL,
    [Score]                FLOAT (53)      NULL,
    [Responses]            SMALLINT        NULL,
    [CertifiedLogoUrl]     VARCHAR (200)   NULL,
    [Owners]               VARCHAR (20)    NULL,
    [InsertionDate]        DATETIME        NULL,
    [ClassifiedExpiryDate] DATETIME        NULL,
    [IsPremium]            BIT             NULL,
    [VideoCount]           TINYINT         NULL,
    [SortScore]            FLOAT (53)      NULL,
    [DealerId]             INT             NULL,
    [CustomerPackageId]    INT             NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListingsDailyLogFrom01072015_Inquiryid]
    ON [dbo].[LiveListingsDailyLog]([Inquiryid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListingsDailyLogFrom01072015_SellerType]
    ON [dbo].[LiveListingsDailyLog]([SellerType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListingsDailyLogFrom01072015_AsOnDate]
    ON [dbo].[LiveListingsDailyLog]([AsOnDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListingsDailyLogFrom01072015_DealerId]
    ON [dbo].[LiveListingsDailyLog]([DealerId] ASC);


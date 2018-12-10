CREATE TABLE [dbo].[Con_NewCarNationalPrices] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]     NUMERIC (18) NOT NULL,
    [AvgPrice]      NUMERIC (18) CONSTRAINT [DF_NationalPricing_Cars_AvgPrice] DEFAULT ((0)) NOT NULL,
    [MinPrice]      NUMERIC (18) CONSTRAINT [DF_NationalPricing_Cars_MinPrice] DEFAULT ((0)) NOT NULL,
    [MaxPrice]      NUMERIC (18) CONSTRAINT [DF_NationalPricing_Cars_MaxPrice] DEFAULT ((0)) NOT NULL,
    [CityCount]     NUMERIC (18) CONSTRAINT [DF_NationalPricing_Cars_CityCount] DEFAULT ((0)) NOT NULL,
    [LastUpdatedBy] BIGINT       NOT NULL,
    [LastUpdatedOn] DATETIME     CONSTRAINT [DF_NationalPricing_Cars_LastUpdatedOn] DEFAULT (getdate()) NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_NationalPricing_Cars_IsActive] DEFAULT ((1)) NOT NULL,
    [EMI]           NUMERIC (18) NULL,
    CONSTRAINT [PK_NationalPricing_Cars] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CON_NEWCARNATIONALPRICES_VersionId]
    ON [dbo].[Con_NewCarNationalPrices]([VersionId] ASC);


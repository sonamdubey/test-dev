CREATE TABLE [dbo].[CompareFeaturedCar] (
    [CFC_Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]         INT           NOT NULL,
    [FeaturedVersionId] INT           NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [LocationId]        TINYINT       CONSTRAINT [DF_CompareFeaturedCar_LocationId] DEFAULT ((1)) NOT NULL,
    [IsCompare]         BIT           CONSTRAINT [DF_CompareFeaturedCar_IsCompare] DEFAULT ((1)) NOT NULL,
    [IsNewSearch]       BIT           CONSTRAINT [DF_CompareFeaturedCar_IsNewSearch] DEFAULT ((0)) NOT NULL,
    [IsRecommend]       BIT           CONSTRAINT [DF_CompareFeaturedCar_IsRecommend] DEFAULT ((0)) NOT NULL,
    [IsResearch]        BIT           CONSTRAINT [DF_CompareFeaturedCar_IsResearch] DEFAULT ((0)) NOT NULL,
    [SpotlightUrl]      VARCHAR (150) NULL,
    [IsPriceQuote]      BIT           CONSTRAINT [DF_CompareFeaturedCar_IsPriceQuote] DEFAULT ((0)) NULL,
    [ApplicationId]     TINYINT       CONSTRAINT [DF_CompareFeaturedCar_ApplicationId] DEFAULT ((1)) NULL,
    [CityId]            INT           NULL,
    [ZoneId]            INT           NULL,
    CONSTRAINT [PK_CompareFeaturedCar] PRIMARY KEY CLUSTERED ([CFC_Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CompareFeaturedCar_VersionId]
    ON [dbo].[CompareFeaturedCar]([VersionId] ASC);


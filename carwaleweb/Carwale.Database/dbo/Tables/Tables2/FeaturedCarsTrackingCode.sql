CREATE TABLE [dbo].[FeaturedCarsTrackingCode] (
    [Id]                 NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [VersionId]          NUMERIC (18)   NOT NULL,
    [ModelID]            INT            NOT NULL,
    [MakeId]             INT            NOT NULL,
    [ImpressionsTracker] NVARCHAR (MAX) NULL,
    [ImageTracker]       NVARCHAR (MAX) NULL,
    [KeywordTracker]     NVARCHAR (MAX) NULL,
    [OnRoadPriceTracker] NVARCHAR (MAX) NULL,
    CONSTRAINT [FK_FeaturedCarsTrackingCode_CarVersions] FOREIGN KEY ([VersionId]) REFERENCES [dbo].[CarVersions] ([ID])
);


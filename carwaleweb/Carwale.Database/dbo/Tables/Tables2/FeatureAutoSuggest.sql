CREATE TABLE [dbo].[FeatureAutoSuggest] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [FeaturedAdId]      INT           NOT NULL,
    [FeaturedModelId]   INT           NOT NULL,
    [TargetModelId]     INT           NOT NULL,
    [ImpressionTracker] VARCHAR (250) NOT NULL,
    [ClickTracker]      VARCHAR (250) NOT NULL,
    [AddedBy]           INT           NULL,
    [AddedOn]           DATETIME      NULL
);


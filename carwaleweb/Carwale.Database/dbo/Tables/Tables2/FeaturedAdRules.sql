CREATE TABLE [dbo].[FeaturedAdRules] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [FeaturedAdId]         INT           NOT NULL,
    [StateId]              INT           NOT NULL,
    [CityId]               INT           NOT NULL,
    [ZoneId]               INT           NOT NULL,
    [TargetVersion]        INT           NOT NULL,
    [FeaturedVersion]      INT           NOT NULL,
    [UpdatedOn]            DATETIME      NOT NULL,
    [UpdatedBy]            INT           NOT NULL,
    [CarImpressionTracker] VARCHAR (250) NULL,
    CONSTRAINT [PK_FeaturedAdRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_FeaturedAdRules_FeaturedAdId]
    ON [dbo].[FeaturedAdRules]([FeaturedAdId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_FeaturedAdRules_TargetVersion]
    ON [dbo].[FeaturedAdRules]([TargetVersion] ASC);


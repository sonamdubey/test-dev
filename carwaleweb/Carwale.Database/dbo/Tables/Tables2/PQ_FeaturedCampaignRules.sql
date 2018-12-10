CREATE TABLE [dbo].[PQ_FeaturedCampaignRules] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [FeaturedCampaignId] INT NOT NULL,
    [StateId]            INT NOT NULL,
    [CityId]             INT NULL,
    [TargetVersion]      INT NOT NULL,
    [FeaturedVersion]    INT NOT NULL,
    CONSTRAINT [PK_PQ_FeaturedCampaignsRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_PQ_FeaturedCampaignRules_FeaturedCampaignId]
    ON [dbo].[PQ_FeaturedCampaignRules]([FeaturedCampaignId] ASC);


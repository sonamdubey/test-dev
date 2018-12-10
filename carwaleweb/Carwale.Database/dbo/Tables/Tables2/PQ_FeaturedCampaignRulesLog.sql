CREATE TABLE [dbo].[PQ_FeaturedCampaignRulesLog] (
    [Id]                         INT      IDENTITY (1, 1) NOT NULL,
    [PQ_FeaturedCampaignRulesId] INT      NULL,
    [FeaturedCampaignId]         INT      NULL,
    [StateId]                    INT      NULL,
    [CityId]                     INT      NULL,
    [TargetVersion]              INT      NULL,
    [FeaturedVersion]            INT      NULL,
    [DeletedBy]                  INT      NULL,
    [DeletedOn]                  DATETIME NULL,
    CONSTRAINT [PK_PQ_FeaturedCampaignRulesLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


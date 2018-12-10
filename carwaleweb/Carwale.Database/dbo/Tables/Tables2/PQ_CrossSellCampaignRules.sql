CREATE TABLE [dbo].[PQ_CrossSellCampaignRules] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [CrossSellCampaignId] INT      NOT NULL,
    [CityId]              INT      NULL,
    [TargetVersion]       INT      NOT NULL,
    [CrossSellVersion]    INT      NOT NULL,
    [ZoneId]              INT      DEFAULT ((0)) NULL,
    [TemplateId]          INT      NULL,
    [StateId]             INT      NULL,
    [UpdatedOn]           DATETIME NULL,
    [AddedOn]             DATETIME NULL,
    [UpdatedBy]           INT      NULL,
    CONSTRAINT [PK_PQ_CrossSellCampaignsRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_CrossSellCampaignRules_CrossSellCampaignId]
    ON [dbo].[PQ_CrossSellCampaignRules]([CrossSellCampaignId] ASC);


CREATE TABLE [dbo].[BW_PQ_CampaignRules] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [CampaignId] INT      NOT NULL,
    [CityId]     INT      NOT NULL,
    [MakeId]     INT      NOT NULL,
    [ModelId]    INT      NOT NULL,
    [IsActive]   BIT      NOT NULL,
    [EntryDate]  DATETIME DEFAULT (getdate()) NULL,
    [EnteredBy]  INT      NULL,
    [UpdatedBy]  INT      NULL,
    [UpdatedOn]  DATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_BW_PQ_CampaignRules_CampaignId]
    ON [dbo].[BW_PQ_CampaignRules]([CampaignId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BW_PQ_CampaignRules_modelid]
    ON [dbo].[BW_PQ_CampaignRules]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BW_PQ_CampaignRules_CityId]
    ON [dbo].[BW_PQ_CampaignRules]([CityId] ASC);


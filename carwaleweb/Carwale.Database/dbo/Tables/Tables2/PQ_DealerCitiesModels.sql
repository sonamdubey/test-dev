CREATE TABLE [dbo].[PQ_DealerCitiesModels] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [PqId]       INT NOT NULL,
    [CityId]     INT NOT NULL,
    [ZoneId]     INT NULL,
    [DealerId]   INT NOT NULL,
    [ModelId]    INT NOT NULL,
    [StateId]    INT NOT NULL,
    [MakeId]     INT NOT NULL,
    [CampaignId] INT NULL,
    CONSTRAINT [PK_PQ_DealerCitiesModels_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerCitiesModels_PqId]
    ON [dbo].[PQ_DealerCitiesModels]([PqId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerCitiesModels_CampaignId]
    ON [dbo].[PQ_DealerCitiesModels]([CampaignId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerCitiesModels_ModelId_MakeId]
    ON [dbo].[PQ_DealerCitiesModels]([ModelId] ASC, [MakeId] ASC)
    INCLUDE([CityId], [ZoneId], [StateId], [CampaignId]);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerCitiesModels_MakeId]
    ON [dbo].[PQ_DealerCitiesModels]([MakeId] ASC);


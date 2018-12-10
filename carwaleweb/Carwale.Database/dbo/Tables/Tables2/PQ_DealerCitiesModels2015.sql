CREATE TABLE [dbo].[PQ_DealerCitiesModels2015] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [PqId]       INT NOT NULL,
    [CityId]     INT NOT NULL,
    [ZoneId]     INT NULL,
    [DealerId]   INT NOT NULL,
    [ModelId]    INT NOT NULL,
    [StateId]    INT NOT NULL,
    [MakeId]     INT NOT NULL,
    [CampaignId] INT NULL
);


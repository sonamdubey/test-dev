CREATE TABLE [dbo].[PQ_DealerCitiesModelsLog2015] (
    [PQ_DealerCitiesModelsId] INT      NOT NULL,
    [CampaignId]              INT      NOT NULL,
    [CityId]                  INT      NOT NULL,
    [ZoneId]                  INT      NULL,
    [DealerId]                INT      NOT NULL,
    [ModelId]                 INT      NOT NULL,
    [StateId]                 INT      NOT NULL,
    [MakeId]                  INT      NOT NULL,
    [DeletedBy]               INT      NOT NULL,
    [DeletedOn]               DATETIME NOT NULL
);


CREATE TABLE [dbo].[PQ_PriceAvailabilityCityRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [StateId]             INT NOT NULL,
    [CityId]              INT NOT NULL,
    [ZoneId]              INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_PriceAvailabilityCityRules_CityId]
    ON [dbo].[PQ_PriceAvailabilityCityRules]([CityId] ASC);


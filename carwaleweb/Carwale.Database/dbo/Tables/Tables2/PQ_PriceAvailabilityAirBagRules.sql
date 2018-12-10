CREATE TABLE [dbo].[PQ_PriceAvailabilityAirBagRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [AirBagId]            INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_AirBagRules_PriceAvailabilityId]
    ON [dbo].[PQ_PriceAvailabilityAirBagRules]([PriceAvailabilityId] ASC);


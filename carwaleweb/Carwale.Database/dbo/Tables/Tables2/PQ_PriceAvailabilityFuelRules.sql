CREATE TABLE [dbo].[PQ_PriceAvailabilityFuelRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [FuelTypeId]          INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_FuelRules_PriceAvailabilityId]
    ON [dbo].[PQ_PriceAvailabilityFuelRules]([PriceAvailabilityId] ASC);


CREATE TABLE [dbo].[PQ_PriceAvailabilityModelRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [MakeId]              INT NOT NULL,
    [ModelId]             INT NOT NULL,
    [VersionId]           INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_ModelRules_PriceAvailabilityId]
    ON [dbo].[PQ_PriceAvailabilityModelRules]([PriceAvailabilityId] ASC);


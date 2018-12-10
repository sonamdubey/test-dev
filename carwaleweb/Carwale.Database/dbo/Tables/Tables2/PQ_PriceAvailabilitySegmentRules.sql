CREATE TABLE [dbo].[PQ_PriceAvailabilitySegmentRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [SegmentId]           INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_SegmentRules_PriceAvailabilityId]
    ON [dbo].[PQ_PriceAvailabilitySegmentRules]([PriceAvailabilityId] ASC);


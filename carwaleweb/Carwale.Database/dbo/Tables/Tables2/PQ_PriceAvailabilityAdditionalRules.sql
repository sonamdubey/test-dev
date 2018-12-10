CREATE TABLE [dbo].[PQ_PriceAvailabilityAdditionalRules] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT NOT NULL,
    [DisplacementMin]     INT NULL,
    [DisplacementMax]     INT NULL,
    [LengthMin]           INT NULL,
    [LengthMax]           INT NULL,
    [GroundClearanceMin]  INT NULL,
    [GroundClearanceMax]  INT NULL,
    [ExShowroomMin]       INT NULL,
    [ExShowroomMax]       INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_AdditionalRules_PriceAvailabilityId]
    ON [dbo].[PQ_PriceAvailabilityAdditionalRules]([PriceAvailabilityId] ASC);


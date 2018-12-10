CREATE TABLE [dbo].[PQ_CategoryItemsAdditionalRules] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId]     INT NOT NULL,
    [DisplacementMin]    INT NULL,
    [DisplacementMax]    INT NULL,
    [LengthMin]          INT NULL,
    [LengthMax]          INT NULL,
    [GroundClearanceMin] INT NULL,
    [GroundClearanceMax] INT NULL,
    [ExShowroomMin]      INT NULL,
    [ExShowroomMax]      INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


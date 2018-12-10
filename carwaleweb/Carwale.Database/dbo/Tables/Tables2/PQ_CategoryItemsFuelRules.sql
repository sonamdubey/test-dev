CREATE TABLE [dbo].[PQ_CategoryItemsFuelRules] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT NOT NULL,
    [FuelTypeId]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[PQ_CategoryItemsModelRules] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT NOT NULL,
    [MakeId]         INT NOT NULL,
    [ModelId]        INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


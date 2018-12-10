CREATE TABLE [dbo].[PQ_CategoryItemsCityRules] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT NOT NULL,
    [StateId]        INT NOT NULL,
    [CityId]         INT NOT NULL,
    [ZoneId]         INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


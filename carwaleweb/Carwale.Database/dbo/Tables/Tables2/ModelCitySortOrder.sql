CREATE TABLE [dbo].[ModelCitySortOrder] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [ModelId]    INT NOT NULL,
    [CityId]     INT NOT NULL,
    [ToggleFlag] BIT NOT NULL,
    CONSTRAINT [PK_ModelCitySortOrder] PRIMARY KEY CLUSTERED ([Id] ASC)
);


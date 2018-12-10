CREATE TABLE [dbo].[DLS_ModelCities] (
    [ModelId] NUMERIC (18) NOT NULL,
    [CityId]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DLS_ModelCities] PRIMARY KEY CLUSTERED ([ModelId] ASC, [CityId] ASC) WITH (FILLFACTOR = 90)
);


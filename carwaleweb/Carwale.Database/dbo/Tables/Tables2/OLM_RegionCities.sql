CREATE TABLE [dbo].[OLM_RegionCities] (
    [RegionId] NUMERIC (18) NOT NULL,
    [CityId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OLM_RegionCities] PRIMARY KEY CLUSTERED ([RegionId] ASC, [CityId] ASC)
);


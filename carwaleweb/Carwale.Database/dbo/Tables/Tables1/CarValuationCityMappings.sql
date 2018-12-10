CREATE TABLE [dbo].[CarValuationCityMappings] (
    [CityId]                 NUMERIC (18) NOT NULL,
    [NearestValuationCityId] NUMERIC (18) CONSTRAINT [DF_ValuationCityMappings_NearestValuationCityId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ValuationCityMappings] PRIMARY KEY CLUSTERED ([CityId] ASC) WITH (FILLFACTOR = 90)
);

